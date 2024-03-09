using Epicotel.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Epicotel.Controllers
{
    [Authorize]
    public class PrenotazioniController : Controller
    {
        public string connString = ConfigurationManager.ConnectionStrings["DBconn"].ConnectionString;
        public ActionResult Index()
        {
            var conn = new SqlConnection(connString);
            conn.Open();
            var cmd = new SqlCommand("SELECT pre.IdPrenotazione, pre.DataPrenotazione, pre.SoggiornoDa, pre.SoggiornoA, pre.Tariffa, pen.TipoPensione, cli.Nome, cli.Cognome, pre.IdCamera FROM Prenotazioni pre LEFT JOIN Pensioni pen ON pre.IdPensione = pen.IdPensione LEFT JOIN Clienti cli ON pre.IdCliente = cli.IdCliente", conn);
            var reader = cmd.ExecuteReader();

            var prenotazioni = new List<Prenotazioni>();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var prenotazione = new Prenotazioni()
                    {
                        IdPrenotazione = (int)reader["IdPrenotazione"],
                        DataPrenotazione = reader["DataPrenotazione"].ToString(),
                        SoggiornoDa = reader["SoggiornoDa"].ToString(),
                        SoggiornoA = reader["SoggiornoA"].ToString(),
                        Tariffa = (decimal)reader["Tariffa"],
                        TipoPensione = reader["TipoPensione"].ToString(),
                        NomeCliente = reader["Nome"].ToString() + " " + reader["Cognome"].ToString(),
                        IdCamera = (int)reader["IdCamera"],
                    };
                    prenotazioni.Add(prenotazione);
                }
            }
            reader.Close();

            ViewBag.Prenotazioni = prenotazioni;

            return View();
        }

        public ActionResult Add()
        {
            var conn = new SqlConnection(connString);
            conn.Open();
            var cmdClienti = new SqlCommand("SELECT * FROM Clienti", conn);
            var reader = cmdClienti.ExecuteReader();

            var clienti = new List<Clienti>();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var cliente = new Clienti()
                    {
                        IdCliente = (int)reader["IdCliente"],
                        Nome = reader["Nome"].ToString() + " " + reader["Cognome"].ToString() + " | " + reader["Email"].ToString(),
                    };
                    clienti.Add(cliente);
                }
            }

            reader.Close();

            var cmdCamere = new SqlCommand("SELECT * FROM Camere", conn);
            var reader2 = cmdCamere.ExecuteReader();

            var camere = new List<Camere>();
            if (reader2.HasRows)
            {
                while (reader2.Read())
                {
                    bool type = (bool)reader2["Tipologia"];
                    var camera = new Camere()
                    {
                        IdCamera = (int)reader2["IdCamera"],
                        Descrizione = (type ? "Camera Doppia" : "Camera Singola") + " | " + reader2["Descrizione"].ToString()
                    };
                    camere.Add(camera);
                }
            }

            reader2.Close();

            var cmdPensioni = new SqlCommand("SELECT * FROM Pensioni", conn);
            var reader3 = cmdPensioni.ExecuteReader();

            var pensioni = new List<Pensioni>();
            if (reader3.HasRows)
            {
                while (reader3.Read())
                {
                    var pensione = new Pensioni()
                    {
                        IdPensione = (int)reader3["IdPensione"],
                        TipoPensione = reader3["TipoPensione"].ToString() + " | " + reader3["Prezzo"].ToString() + "€"
                    };
                    pensioni.Add(pensione);
                }
            }

            reader3.Close();

            ViewBag.Clienti = clienti;
            ViewBag.Camere = camere;
            ViewBag.Pensioni = pensioni;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Prenotazioni prenotazione)
        {
            // IMPORTANT: Non so perchè ma con ModelState.IsValid mi dice che il value delle option Cliente (ovviamente selezionate) è nullo. Però senza il controllo il value delle option Cliente lo prende tranquillamente.
            //if (ModelState.IsValid)
            //{
                SqlConnection conn = new SqlConnection(connString);
                conn.Open();
                string insertPrenotazione = "INSERT INTO Prenotazioni (DataPrenotazione, SoggiornoDa, SoggiornoA, Tariffa, IdPensione, IdCliente, IdCamera) VALUES (@DataPrenotazione, @SoggiornoDa, @SoggiornoA, @Tariffa, @IdPensione, @IdCliente, @IdCamera)";
                string selectPrenotazione = "SELECT TOP (1) IdPrenotazione, cam.Caparra, pen.Prezzo FROM Prenotazioni pre LEFT JOIN Camere cam ON pre.IdCamera = cam.IdCamera LEFT JOIN Pensioni pen ON pre.IdPensione = pen.IdPensione ORDER BY IdPrenotazione desc";
                string insertStorico = "INSERT INTO StoricoServiziAggiuntivi (IdPrenotazione, IdServizio, DataServizio, PrezzoTotale) VALUES (@IdPrenotazione, @IdServizio, @DataServizio, @PrezzoTotale)";
                string IdUltimaPrenotazione = "";
                decimal prezzoTot = 0;

                //INSERT PRENOTAZIONE
                SqlCommand insrtPrenotazione = new SqlCommand(insertPrenotazione, conn);
                insrtPrenotazione.Parameters.AddWithValue("@IdCliente", prenotazione.IdCliente);
                insrtPrenotazione.Parameters.AddWithValue("@IdCamera", prenotazione.IdCamera);
                insrtPrenotazione.Parameters.AddWithValue("@IdPensione", prenotazione.IdPensione);
                insrtPrenotazione.Parameters.AddWithValue("@DataPrenotazione", DateTime.Now.ToString());
                insrtPrenotazione.Parameters.AddWithValue("@SoggiornoDa", prenotazione.SoggiornoDa.ToString());
                insrtPrenotazione.Parameters.AddWithValue("@SoggiornoA", prenotazione.SoggiornoA.ToString());
                insrtPrenotazione.Parameters.AddWithValue("@Tariffa", prenotazione.Tariffa);
                insrtPrenotazione.ExecuteNonQuery();

                //SELECT DELLA PRENOTAZIONE APPENA INSERITA
                SqlCommand slctPrenotazione = new SqlCommand(selectPrenotazione, conn);
                var reader = slctPrenotazione.ExecuteReader();
                if (reader.HasRows)
                {
                    if(reader.Read())
                    {
                        IdUltimaPrenotazione = reader["IdPrenotazione"].ToString();
                        prezzoTot = prenotazione.Tariffa + (decimal)reader["Caparra"] + (decimal)reader["Prezzo"];
                    }
                }
                reader.Close();

                //INSERT STORICO PRENOTAZIONE
                SqlCommand insrtStorico = new SqlCommand(insertStorico, conn);
                insrtStorico.Parameters.AddWithValue("@IdPrenotazione", IdUltimaPrenotazione);
                insrtStorico.Parameters.AddWithValue("@IdServizio", 1);
                insrtStorico.Parameters.AddWithValue("@DataServizio", DateTime.Now.ToString());
                insrtStorico.Parameters.AddWithValue("@PrezzoTotale", prezzoTot);
                insrtStorico.ExecuteNonQuery();

                TempData["Prenotazione"] = true;
                return RedirectToAction("Index");
            //}
        }
    }
}