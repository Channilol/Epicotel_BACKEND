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
            var checkoutDetails = new SqlCommand("SELECT sto.IdPrenotazione, cam.IdCamera, pre.SoggiornoDa, pre.SoggiornoA, sum(PrezzoTotale) as PrezzoCheckout, pre.Tariffa, cam.Caparra, pen.TipoPensione, pen.Prezzo FROM StoricoServiziAggiuntivi sto LEFT JOIN Prenotazioni pre ON sto.IdPrenotazione = pre.IdPrenotazione LEFT JOIN Camere cam ON pre.IdCamera = cam.IdCamera LEFT JOIN Pensioni pen ON pre.IdPensione = pen.IdPensione WHERE sto.IdPrenotazione = @IdPrenotazione GROUP BY sto.IdPrenotazione, cam.IdCamera, pre.SoggiornoDa, pre.SoggiornoA, pre.Tariffa, cam.Caparra, pen.TipoPensione, pen.Prezzo", conn);
            checkoutDetails.Parameters.AddWithValue("@IdPrenotazione", id);
            var readerCheckout = checkoutDetails.ExecuteReader();

            var checkout = new Checkout();
            if (readerCheckout.HasRows)
            {
                if(readerCheckout.Read())
                {
                    checkout.IdPrenotazione = id;
                    checkout.IdCamera = (int)readerCheckout["IdCamera"];
                    checkout.Periodo = "Dal " + readerCheckout["SoggiornoDa"].ToString() + " al " + readerCheckout["SoggiornoA"].ToString();
                    checkout.Tariffa = (decimal)readerCheckout["Tariffa"];
                    checkout.PrezzoCheckout = (decimal)readerCheckout["PrezzoCheckout"] - (decimal)readerCheckout["Caparra"];
                    checkout.TipoPensione = readerCheckout["TipoPensione"].ToString();
                    checkout.PrezzoPensione = (decimal)readerCheckout["Prezzo"];
                    checkout.Caparra = readerCheckout["Caparra"].ToString();
                }
            }
            readerCheckout.Close();

            var serviziDetails = new SqlCommand("SELECT sto.IdPrenotazione, ser.TipoServizio, ser.PrezzoServizio, sto.DataServizio FROM StoricoServiziAggiuntivi sto LEFT JOIN Prenotazioni pre ON sto.IdPrenotazione = pre.IdPrenotazione LEFT JOIN ServiziAggiuntivi ser ON sto.IdServizio = ser.IdServizio WHERE sto.IdPrenotazione = @IdPrenotazione", conn);
            serviziDetails.Parameters.AddWithValue("@IdPrenotazione", id);
            var readerServizi = serviziDetails.ExecuteReader();

            var serviziList = new List<Checkout>();
            if(readerServizi.HasRows)
            {
                while (readerServizi.Read())
                {
                    var servizio = new Checkout()
                    {
                        TipoServizio = readerServizi["TipoServizio"].ToString(),
                        PrezzoServizio = (decimal)readerServizi["PrezzoServizio"],
                        DataServizio = readerServizi["DataServizio"].ToString(),
                    };
                    serviziList.Add(servizio);
                }
            }

            readerServizi.Close();

            if (serviziList.Count > 0)
            {
                serviziList.RemoveAt(0);
            }

            ViewBag.Checkout = checkout;
            ViewBag.Servizi = serviziList;

            return View();
        }
    }
}