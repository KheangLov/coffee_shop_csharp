using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coffee_shop
{
    class Supplier : Db_Insert
    {
        SqlCommand sqld;
        private string name;
        private string address;
        private string phone;
        private string email;
        private int company_id;
        private int branch_id;
        private int stock_id;
        

        public Supplier()
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

        public int BranchId
        {
            get
            {
                return branch_id;
            }
            set
            {
                branch_id = value;
            }
        }

        public int StockId
        {
            get
            {
                return stock_id;
            }
            set
            {
                stock_id = value;
            }
        }

        public override void insert()
        {
            string query = @"INSERT INTO [coffee_shop].[dbo].[suppliers](name, address, phone, email, company_id, branch_id, stock_id,) 
                values('" + Name + "', '" + Address + "', '" + Phone + "', '" + Email + "', " + CompanyId + ", " + BranchId + ", " + StockId + ");";
            sqld = new SqlCommand(query, DataConn.Connection);
            sqld.ExecuteNonQuery();
            sqld.Dispose();
        }

        public override void update(int id)
        {
            string query = @"UPDATE [coffee_shop].[dbo].[suppliers] 
                SET name = '" + Name + "', address ='" + Address + "', phone = '" + Phone + "', email = '" + Email + "', company_id = " + CompanyId + ", branch_id = " + BranchId + ", stock_id = " + StockId + " WHERE id = " + id + ";";
            sqld = new SqlCommand(query, DataConn.Connection);
            sqld.ExecuteNonQuery();
            sqld.Dispose();
        }
    }
}
