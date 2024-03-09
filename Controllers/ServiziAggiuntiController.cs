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
    public class ServiziAggiuntiController : Controller
    {
        public string connString = ConfigurationManager.ConnectionStrings["DBconn"].ConnectionString;

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Add()
        {
            var conn = new SqlConnection(connString);
            conn.Open();
            var selectPrenotazioni = new SqlCommand("SELECT * FROM Prenotazioni", conn);
            var readerPrenotazioni = selectPrenotazioni.ExecuteReader();

            var prenotazioni = new List<Prenotazioni>();
            if(readerPrenotazioni.HasRows)
            {
                while (readerPrenotazioni.Read())
                {
                    var prenotazione = new Prenotazioni()
                    {
                        IdPrenotazione = (int)readerPrenotazioni["IdPrenotazione"],
                        DataPrenotazione = "Id Prenotazione: " + readerPrenotazioni["IdPrenotazione"].ToString() + " | Pensione: " + readerPrenotazioni["IdPensione"].ToString() + " | Cliente: " + readerPrenotazioni["IdCliente"].ToString() + " | Camera: " + readerPrenotazioni["IdCamera"].ToString()
                    };
                    prenotazioni.Add(prenotazione);
                }
            }

            readerPrenotazioni.Close();

            var selectServizi = new SqlCommand("SELECT * FROM ServiziAggiuntivi", conn);
            var readerServizi = selectServizi.ExecuteReader();

            var servizi = new List<ServiziAggiunti>();
            if(readerServizi.HasRows)
            {
                while (readerServizi.Read())
                {
                    var servizio = new ServiziAggiunti()
                    {
                        IdServizio = (int)readerServizi["IdServizio"],
                        TipoServizio = readerServizi["TipoServizio"].ToString() + " " + readerServizi["PrezzoServizio"].ToString(),
                    };
                    servizi.Add(servizio);
                }
            }

            readerServizi.Close();
            
            //var selectStorico = new SqlCommand("SELECT IdPrenotazione, sum(PrezzoTotale) as PrezzoTotale FROM StoricoServiziAggiuntivi GROUP BY IdPrenotazione", conn);
            //var readerStorico = selectStorico.ExecuteReader();

            ViewBag.Prenotazioni = prenotazioni;
            ViewBag.Servizi = servizi;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(StoricoServiziAggiuntivi storico)
        {
            if (ModelState.IsValid)
            {
                SqlConnection conn = new SqlConnection(connString);
                conn.Open();
                //SELECT PREZZO TOTALE FINO AD ORA
                //decimal prezzoTot = 0;
                //string selectPrezzoTot = "SELECT PrezzoTotale FROM StoricoServiziAggiuntivi WHERE IdPrenotazione = @IdPrenotazione GROUP BY IdPrenotazione";
                //SqlCommand cmdSelect = new SqlCommand(selectPrezzoTot, conn);
                //cmdSelect.Parameters.AddWithValue("@IdPrenotazione", storico.IdPrenotazione);
                //var reader = cmdSelect.ExecuteReader();
                //if (reader.HasRows)
                //{
                //    if (reader.Read())
                //    {
                //        prezzoTot = (decimal)reader["prezzoTotale"];
                //    }
                //}
                //reader.Close();

                //SELECT PREZZO SERVIZIO
                string selectServizio = "SELECT PrezzoServizio FROM ServiziAggiuntivi WHERE IdServizio = @IdServizio";
                SqlCommand cmdServizio = new SqlCommand(selectServizio, conn);
                cmdServizio.Parameters.AddWithValue("@IdServizio", storico.IdServizio);
                var reader2 = cmdServizio.ExecuteReader();
                decimal prezzoServizio = 0;
                if (reader2.HasRows)
                {
                    if (reader2.Read())
                    {
                        prezzoServizio = (decimal)reader2["PrezzoServizio"];
                    }
                }
                reader2.Close();

                //prezzoTot += prezzoServizio;

                //INSERT NUOVO STORICO CON IL PREZZO AGGIORNATO
                string insertServizio = "INSERT INTO StoricoServiziAggiuntivi (IdPrenotazione, IdServizio, DataServizio, PrezzoTotale) VALUES (@IdPrenotazione, @IdServizio, @DataServizio, @PrezzoTotale)";
                SqlCommand cmdInsert = new SqlCommand(insertServizio, conn);
                cmdInsert.Parameters.AddWithValue("@IdPrenotazione", storico.IdPrenotazione);
                cmdInsert.Parameters.AddWithValue("@IdServizio", storico.IdServizio);
                cmdInsert.Parameters.AddWithValue("@DataServizio", storico.DataServizio);
                cmdInsert.Parameters.AddWithValue("@PrezzoTotale", prezzoServizio);
                cmdInsert.ExecuteNonQuery();

                TempData["Servizio"] = true;

                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
            
        }
    }
}