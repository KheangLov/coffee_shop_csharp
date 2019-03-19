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

        public static void ConnectionDB()
        {
            string connectionString = "Server = Zen;Database = coffee_shop;User ID = coffee;Password = not4you;";
            Connection = new SqlConnection(connectionString);
            Connection.Open();
        }
    }
}
