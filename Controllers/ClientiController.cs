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
    public class ClientiController : Controller
    {
        public string connString = ConfigurationManager.ConnectionStrings["DBconn"].ConnectionString;
        
        public ActionResult Index()
        {
            var conn = new SqlConnection(connString);
            conn.Open();
            var cmd = new SqlCommand("SELECT * FROM Clienti", conn);
            var reader = cmd.ExecuteReader();

            var clienti = new List<Clienti>();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    var cliente = new Clienti()
                    {
                        IdCliente = (int)reader["IdCliente"],
                        Cognome = reader["Cognome"].ToString(),
                        Nome = reader["Nome"].ToString(),
                        Citta = reader["Citta"].ToString(),
                        Provincia = reader["Provincia"].ToString(),
                        Email = reader["Email"].ToString(),
                        Telefono = (int)reader["Telefono"],
                        Cellulare = (int)reader["Cellulate"],
                        CodiceFiscale = reader["CodiceFiscale"].ToString(),
                    };
                    clienti.Add(cliente);
                }
            }
            reader.Close();

            ViewBag.Clienti = clienti;

            return View();
        }
        
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

                TempData["Cliente"] = true;
                return RedirectToAction("Index");
            }

            return View();
        }

    }
}