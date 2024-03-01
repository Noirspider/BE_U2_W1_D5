using BE_U2_W1_D5.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BE_U2_W1_D5.Controllers
{
    public class TrasgressoreController : Controller
    {
        // GET: Trasgressore
        public ActionResult Index()
        {
            List<Trasgressore> Trasgressore = new List<Trasgressore>();

            string connectionString = ConfigurationManager.ConnectionStrings["DB1"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                Trasgressore.Clear();
                conn.Open();
                string tsql = "SELECT * FROM Anagrafica";
                SqlDataReader reader = Shared.getReader(tsql, conn);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Trasgressore t = new Trasgressore();
                        t.ID_Anagrafica = Convert.ToInt32(reader["ID_Anagrafica"]);
                        t.Cognome = reader["Cognome"].ToString();
                        t.Nome = reader["Nome"].ToString();
                        t.Indirizzo = reader["Indirizzo"].ToString();
                        t.Citta = reader["Citta"].ToString();
                        t.CAP = reader["Cap"].ToString();
                        t.CodiceFiscale = reader["CodiceFiscale"].ToString();

                        Trasgressore.Add(t);

                    }
                }
                conn.Close();
            }
            catch (Exception ex)
            {

                Response.Write("errore: " + ex.Message);
            }
            return View(Trasgressore);
        }
        // POST: Trasgressore/Create

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Trasgressore trasgressore)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DB1"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                if (ModelState.IsValid)
                {
                    conn.Open();
                    string tsql = "INSERT INTO Anagrafica (Cognome, Nome, Indirizzo, Citta, Cap, CodiceFiscale) VALUES (@Cognome, @Nome, @Indirizzo, @Citta, @Cap, @CodiceFiscale)";
                    SqlCommand cmd = new SqlCommand(tsql, conn);
                    cmd.Parameters.AddWithValue("@Cognome", trasgressore.Cognome);
                    cmd.Parameters.AddWithValue("@Nome", trasgressore.Nome);
                    cmd.Parameters.AddWithValue("@Indirizzo", trasgressore.Indirizzo);
                    cmd.Parameters.AddWithValue("@Citta", trasgressore.Citta);
                    cmd.Parameters.AddWithValue("@Cap", trasgressore.CAP);
                    cmd.Parameters.AddWithValue("@CodiceFiscale", trasgressore.CodiceFiscale);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Response.Write("errore: " + ex.Message);

            }
            finally
            {
                conn.Close();
            }
            return RedirectToAction("Index");
        }
    }
}

