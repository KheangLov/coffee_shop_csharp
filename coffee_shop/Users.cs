using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace coffee_shop
{
    class Users : Db_Insert
    {
        SqlCommand sqld;
        private string firstname;
        private string lastname;
        private string username;
        private string email;
        private string password;
        private string gender;
        private string phone;
        private string address;
        private DateTime created_date;
        private int role_id;

        public Users()
        {

        }
        public string Firstname
        {
            get
            {
                return firstname;
            }
            set
            {
                firstname = value;
            }
        }

        public string Lastname
        {
            get
            {
                return lastname;
            }
            set
            {
                lastname = value;
            }
        }

        public string Username
        {
            get
            {
                return username;
            }
            set
            {
                username = value;
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

        public string Gender
        {
            get
            {
                return gender;
            }
            set
            {
                gender = value;
            }
        }

        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                password = value;
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

        public int Role_Id
        {
            get
            {
                return role_id;
            }
            set
            {
                role_id = value;
            }
        }

        public DateTime Created_Date
        {
            get
            {
                return created_date;
            }
            set
            {
                created_date = value;
            }
        }

        public override void insert()
        {
            string query = @"INSERT INTO [coffee_shop].[dbo].[users](firstname, lastname, username, email, password, created_date, gender, phone, address, role_id) 
                values('" + Firstname + "', '" + Lastname + "', '" + Username + "', '" + Email + "', '" + Password + "', '"+ Created_Date + "', '" + Gender + "', '" + Phone + "', '" + Address + "', " + Role_Id + ");";
            sqld = new SqlCommand(query, DataConn.Connection);
            sqld.ExecuteNonQuery();
            sqld.Dispose();
        }

        public override void update(int id)
        {
            string query = @"UPDATE [coffee_shop].[dbo].[users] 
                SET firstname = '" + Firstname + "', lastname ='" + Lastname + "', username = '" + Username + "', email = '" + Email + "', gender = '" + Gender + "', phone = '" + Phone + "', address = '" + Address + "', role_id = '" + Role_Id + "' WHERE id = " + id + ";";
            sqld = new SqlCommand(query, DataConn.Connection);
            sqld.ExecuteNonQuery();
            sqld.Dispose();
        }
    }
}
