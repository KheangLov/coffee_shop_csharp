using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace coffee_shop
{
    class Company : Db_Insert
    {
        SqlCommand sqld;
        private string name;
        private string address;
        private string email;
        private string phone;
        private string user_id;
        public Company()
        {

        }
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }
        public string Address
        {
            get
            {
                return address;
            }
            set
            {
                address = value;
            }
        }
        public string Email
        {
            get
            {
                return email;
            }
            set
            {
                email = value;
            }
        }
        public string Phone
        {
            get
            {
                return phone;
            }
            set
            {
                phone = value;
            }
        }
        public string UserID
        {
            get
            {
                return user_id;
            }
            set
            {
                user_id = value;
            }
        }
        public override void insert()
        {
            string query = @"INSERT INTO [coffee_shop].[dbo].[companies](name, address, email, phone,user_id) 
                values('" + Name + "', '" + Address + "', '" + Email + "', '" + Phone + "', " + user_id + ");";
            sqld = new SqlCommand(query, DataConn.Connection);
            sqld.ExecuteNonQuery();
            sqld.Dispose();
        }
    }
}
