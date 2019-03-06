using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace coffee_shop
{
    class DataConn
    {
        private static SqlConnection connection;

        public static SqlConnection Connection
        {
            get
            {
                return connection;
            }

            set
            {
                connection = value;
            }
        }

        public static void ConnectionDB(string ipServer, string dbName, string user, string pass)
        {
            string connectionString = "Server = " + ipServer + ";Database = " + dbName + ";User ID = " + user + ";Password = " + pass + ";";
            Connection = new SqlConnection(connectionString);
            Connection.Open();
        }
    }
}
