using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coffee_shop
{
    class Branch : Db_Insert
    {
        SqlCommand sqld;
        private string name;
        private string email;
        private string phone;
        private string address;
        private int company_id;

        public Branch()
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

        public int CompanyId
        {
            get
            {
                return company_id;
            }
            set
            {
                company_id = value;
            }
        }

        public override void insert()
        {
            string query = @"INSERT INTO [coffee_shop].[dbo].[branches](name, email, phone, address, company_id) 
                values('" + Name + "', '" + Email + "', '" + Phone + "', '" + Address + "', " + CompanyId + ");";
            sqld = new SqlCommand(query, DataConn.Connection);
            sqld.ExecuteNonQuery();
            sqld.Dispose();
        }

        public override void update(int id)
        {
            string query = @"UPDATE [coffee_shop].[dbo].[branches] 
                SET name = '" + Name + "', email ='" + Email + "', phone = '" + Phone + "', address = '" + Address + "', company_id = '" + CompanyId + "' WHERE id = " + id + ";";
            sqld = new SqlCommand(query, DataConn.Connection);
            sqld.ExecuteNonQuery();
            sqld.Dispose();
        }
    }
}
