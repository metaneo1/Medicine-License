using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace MvcBaseApp
{
    public static class SqlHelper
    {
        public static DataTable ExecuteQuery(string query)
        {

            string connStr = WebConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
            SqlConnection conn = new SqlConnection(connStr);
            var table = new DataTable();
            conn.Open();
            using (var adapter = new SqlDataAdapter())
            {
                var s = query.Replace("\\t", "");
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandTimeout = 1200;
                cmd.CommandText = s;
                adapter.SelectCommand = cmd;
                adapter.Fill(table);

            }
            conn.Close();
            return table;
        }
    }
}