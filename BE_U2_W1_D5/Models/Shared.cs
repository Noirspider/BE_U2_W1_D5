using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace BE_U2_W1_D5.Models
{
    public class Shared
    {
        public static SqlConnection getConToDB()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["DB1"].ToString();
            return con;
        }

        public static SqlDataReader getReader(string tsql, SqlConnection conn)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = tsql;
            return cmd.ExecuteReader();
        }
    }
}