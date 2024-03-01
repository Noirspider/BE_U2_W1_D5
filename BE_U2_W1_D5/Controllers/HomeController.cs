using BE_U2_W1_D5.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace BE_U2_W1_D5.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        // PARTIAL PAGES
        public ActionResult _ContravvenzioniPerTrasgressore()
        {
            List<Contravvenzione> Contravvenzione = new List<Contravvenzione>();
            string connectionString = ConfigurationManager.ConnectionStrings["DB1"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    Contravvenzione.Clear();
                    conn.Open();
                    string tsql = "SELECT Cognome, Nome, Sum(Importo) AS TotaleImporto, count(*) AS NumeroVerbali " +
                                  "FROM Anagrafica AS A join Verbale AS V  ON A.ID_Anagrafica = V.ID_Anagrafica " +
                                  "GROUP BY Cognome, Nome ORDER BY NumeroVerbali DESC";
                    SqlDataReader reader = Shared.getReader(tsql, conn);

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Contravvenzione c = new Contravvenzione();
                            c.T_Cognome = reader["Cognome"].ToString();
                            c.T_Nome = reader["Nome"].ToString();
                            c.V_TotaleImporto = Convert.ToDecimal(reader["TotaleImporto"]);
                            c.V_NumeroVerbali = Convert.ToInt32(reader["NumeroVerbali"]);

                            Contravvenzione.Add(c);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Response.Write("errore: " + ex.Message);
                }
            }

            return PartialView("_ContravvenzioniPerTrasgressore", Contravvenzione);
        }

        public ActionResult _ContravvenzioniSuperano10Punti()
        {
            List<Contravvenzione> Contravvenzione = new List<Contravvenzione>();
            string connectionString = ConfigurationManager.ConnectionStrings["DB1"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                Contravvenzione.Clear();
                conn.Open();
                string tsql = "SELECT Cognome, Nome, Importo, DataViolazione, DecurtamentoPunti " +
                    "FROM Anagrafica AS A join Verbale AS V  ON A.ID_Anagrafica = V.ID_Anagrafica  WHERE DecurtamentoPunti > 9";
                SqlDataReader reader = Shared.getReader(tsql, conn);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Contravvenzione c = new Contravvenzione();
                        c.T_Cognome = reader["Cognome"].ToString();
                        c.T_Nome = reader["Nome"].ToString();
                        c.V_Importo = Convert.ToDecimal(reader["Importo"]);
                        c.V_DataViolazione = Convert.ToDateTime(reader["DataViolazione"]);
                        c.V_DecurtamentoPunti = Convert.ToInt32(reader["DecurtamentoPunti"]);

                        Contravvenzione.Add(c);
                    }
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                Response.Write("errore: " + ex.Message);
            }

            return PartialView("_ContravvenzioniSuperano10Punti", Contravvenzione);
        }

        public ActionResult _ContravvenzioniImportoMaggiore400()
        {
            List<Contravvenzione> Contravvenzione = new List<Contravvenzione>();
            string connectionString = ConfigurationManager.ConnectionStrings["DB1"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                Contravvenzione.Clear();
                conn.Open();
                string tsql = "SELECT Cognome, Nome, DataViolazione, DecurtamentoPunti, Importo FROM Anagrafica AS A join Verbale AS V  ON A.ID_Anagrafica = V.ID_Anagrafica  WHERE Importo > 400";
                SqlDataReader reader = Shared.getReader(tsql, conn);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Contravvenzione c = new Contravvenzione();
                        c.T_Cognome = reader["Cognome"].ToString();
                        c.T_Nome = reader["Nome"].ToString();
                        c.V_Importo = Convert.ToDecimal(reader["Importo"]);
                        c.V_DataViolazione = Convert.ToDateTime(reader["DataViolazione"]);
                        c.V_DecurtamentoPunti = Convert.ToInt32(reader["DecurtamentoPunti"]);

                        Contravvenzione.Add(c);
                    }
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                Response.Write("errore: " + ex.Message);
            }

            return PartialView("_ContravvenzioniImportoMaggiore400", Contravvenzione);
        }

        public ActionResult _PuntiDecurtatiPerOgniTrasgressore()
        {
            List<Contravvenzione> Contravvenzione = new List<Contravvenzione>();
            string connectionString = ConfigurationManager.ConnectionStrings["DB1"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                Contravvenzione.Clear();
                conn.Open();
                string tsql = "SELECT Cognome, Nome, Sum(DecurtamentoPunti) AS DecurtamentoPunti" +
                    " FROM Anagrafica AS A  join Verbale AS V  ON A.ID_Anagrafica = V.ID_Anagrafica GROUP BY Cognome, Nome";
                SqlDataReader reader = Shared.getReader(tsql, conn);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Contravvenzione c = new Contravvenzione();
                        c.T_Cognome = reader["Cognome"].ToString();
                        c.T_Nome = reader["Nome"].ToString();
                        c.V_DecurtamentoPunti = Convert.ToInt32(reader["DecurtamentoPunti"]);

                        Contravvenzione.Add(c);
                    }
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                Response.Write("errore: " + ex.Message);
            }

            return PartialView("_PuntiDecurtatiPerOgniTrasgressore", Contravvenzione);
        }
    }
}