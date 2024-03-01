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
    public class VerbaleController : Controller
    {
        // GET: Verbale
        public ActionResult Index()
        {
            List<Verbale> listaVerbali = new List<Verbale>(); // Cambiato il nome della lista per coerenza
            string connectionString = ConfigurationManager.ConnectionStrings["DB1"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string tsql = "SELECT * FROM Verbale AS V JOIN TipoViolazione AS tV " +
                                  "ON tV.ID_Violazione = V.ID_Violazione JOIN Anagrafica AS A ON A.ID_Anagrafica = V.ID_Anagrafica " +
                                  "ORDER BY DataTrascrizioneVerbale DESC";
                    using (SqlCommand cmd = new SqlCommand(tsql, conn))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            Verbale v = new Verbale();
                            v.ID_Verbale = Convert.ToInt32(reader["ID_Verbale"]);
                            v.NominativoAgente = reader["NominativoAgente"].ToString(); // Corretta la conversione a stringa
                            v.DataViolazione = Convert.ToDateTime(reader["DataViolazione"]);
                            v.IndirizzoViolazione = reader["IndirizzoViolazione"].ToString(); // Corretta la conversione a stringa
                            v.DataTrascrizioneVerbale = Convert.ToDateTime(reader["DataTrascrizioneVerbale"]);
                            v.Importo = Convert.ToDecimal(reader["importo"]);
                            v.DecurtamentoPunti = Convert.ToInt32(reader["DecurtamentoPunti"]);
                            v.ID_Violazione = Convert.ToInt32(reader["ID_Violazione"]);
                            v.ID_Anagrafica = Convert.ToInt32(reader["ID_Anagrafica"]);
                            v.TipoViolazione = reader["Descrizione"].ToString(); // Corretta la conversione a stringa
                            v.NomeCognomeTrasgressore = reader["Nome"] + " " + reader["Cognome"]; // Corretta la concatenazione
                            listaVerbali.Add(v);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Utilizzo del metodo di logging per gestire gli errori
                    System.Diagnostics.Trace.WriteLine("Errore: " + ex.Message);
                    return View("Error");
                }
            }
            return View(listaVerbali);
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Verbale verbale)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DB1"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string tsql = "INSERT INTO Verbale (NominativoAgente, DataViolazione, IndirizzoViolazione, DataTrascrizioneVerbale, Importo, DecurtamentoPunti, ID_Violazione, ID_Anagrafica) " +
                                  "VALUES (@NominativoAgente, @DataViolazione, @IndirizzoViolazione, @DataTrascrizioneVerbale, @Importo, @DecurtamentoPunti, @ID_Violazione, @ID_Anagrafica)";
                    using (SqlCommand cmd = new SqlCommand(tsql, conn))
                    {
                        cmd.Parameters.AddWithValue("@NominativoAgente", verbale.NominativoAgente);
                        cmd.Parameters.AddWithValue("@DataViolazione", verbale.DataViolazione);
                        cmd.Parameters.AddWithValue("@IndirizzoViolazione", verbale.IndirizzoViolazione);
                        cmd.Parameters.AddWithValue("@DataTrascrizioneVerbale", verbale.DataTrascrizioneVerbale);
                        cmd.Parameters.AddWithValue("@Importo", verbale.Importo);
                        cmd.Parameters.AddWithValue("@DecurtamentoPunti", verbale.DecurtamentoPunti);
                        cmd.Parameters.AddWithValue("@ID_Violazione", verbale.ID_Violazione);
                        cmd.Parameters.AddWithValue("@ID_Anagrafica", verbale.ID_Anagrafica);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    // Utilizzo del metodo di logging per gestire gli errori
                    System.Diagnostics.Trace.WriteLine("Errore: " + ex.Message);
                    return View("Error");
                }
            }
            return RedirectToAction("Index");
        }


    }
}
