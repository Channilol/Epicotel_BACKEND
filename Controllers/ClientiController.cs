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
    public class ClientiController : Controller
    {
        public string connString = ConfigurationManager.ConnectionStrings["DBconn"].ConnectionString;

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Clienti cliente)
        {
            if(ModelState.IsValid)
            {
                SqlConnection conn = new SqlConnection(connString);
                conn.Open();
                string query = "INSERT INTO Clienti (Cognome, Nome, Citta, Provincia, Email, Telefono, Cellulate, CodiceFiscale) VALUES (@Cognome, @Nome, @Citta, @Provincia, @Email, @Telefono, @Cellulate, @CodiceFiscale)";
                SqlCommand register = new SqlCommand(query, conn);
                register.Parameters.AddWithValue("@Cognome", cliente.Cognome);
                register.Parameters.AddWithValue("@Nome", cliente.Nome);
                register.Parameters.AddWithValue("@Citta", cliente.Citta);
                register.Parameters.AddWithValue("@Provincia", cliente.Provincia);
                register.Parameters.AddWithValue("@Email", cliente.Email);
                register.Parameters.AddWithValue("@Telefono", cliente.Telefono);
                register.Parameters.AddWithValue("@Cellulate", cliente.Cellulare);
                register.Parameters.AddWithValue("@CodiceFiscale", cliente.CodiceFiscale);
                register.ExecuteNonQuery();

                return RedirectToAction("Index", "Home");

            }

            return View();
        }

    }
}