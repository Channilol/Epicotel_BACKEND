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
    public class StoricoServiziAggiuntiviController : Controller
    {
        public string connString = ConfigurationManager.ConnectionStrings["DBconn"].ConnectionString;
        public ActionResult Index()
        {
            var conn = new SqlConnection(connString);
            conn.Open();
            var selectPrenotazioni = new SqlCommand("SELECT * FROM Prenotazioni", conn);
            var readerPrenotazioni = selectPrenotazioni.ExecuteReader();

            var prenotazioni = new List<Prenotazioni>();
            if (readerPrenotazioni.HasRows)
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

            ViewBag.Prenotazioni = prenotazioni;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(StoricoServiziAggiuntivi checkout)
        {
            int IdPrenotazione = checkout.IdPrenotazione;
            return Redirect($"/StoricoServiziAggiuntivi/Checkout/{IdPrenotazione}");
        }


        public ActionResult Checkout(int id)
        {
            var conn = new SqlConnection(connString);
            conn.Open();
            var scontrino = new SqlCommand("SELECT IdPrenotazione, sum(PrezzoTotale) as PrezzoCheckout FROM StoricoServiziAggiuntivi WHERE IdPrenotazione = @IdPrenotazione GROUP BY IdPrenotazione", conn);
            var reader = scontrino.ExecuteReader();

            var checkout = new List<Checkout>();
            if (reader.HasRows)
            {
                if(reader.Read())
                {
                    var checkout1 = new Checkout()
                    {
                        IdPrenotazione = id,
                        PrezzoCheckout = (decimal)reader["PrezzoCheckout"]
                    };
                    checkout.Add(checkout1);
                }
            }
            reader.Close();

            //TODO: MOSTRARE LO SCONTRINO CHECKOUT

            ViewBag.Checkout = checkout;

            return View();
        }
    }
}