using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coffee_shop
{
    class Receipt : Db_Insert
    {
        SqlCommand sqld;
        private int number;
        private string address;
        private int product_id;
        private double qty;
        private double price;
        private double total;
        private double discount;
        private int employee_id;
        private int company_id;
        private int branch_id;

        public Receipt()
        {

        }

        public int Number
        {
            get
            {
                return number;
            }
            set
            {
                number = value;
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

        public int ProductId
        {
            get
            {
                return product_id;
            }
            set
            {
                product_id = value;
            }
        }

        public double Qty
        {
            get
            {
                return qty;
            }
            set
            {
                qty = value;
            }
        }

        public double Price
        {
            get
            {
                return price;
            }
            set
            {
                price = value;
            }
        }

        public double Total
        {
            get
            {
                return total;
            }
            set
            {
                total = value;
            }
        }

        public double Discount
        {
            get
            {
                return discount;
            }
            set
            {
                discount = value;
            }
        }

        public int EmployeeId
        {
            get
            {
                return employee_id;
            }
            set
            {
                employee_id = value;
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

        public override void insert()
        {
            string query = @"INSERT INTO [coffee_shop].[dbo].[receipts](number, address, product_id, qty, price, total, discount, employee_id, company_id, branch_id) 
                values(" + Number + ", '" + Address + "', " + ProductId + ", " + Qty + ", " + Price + ", " + Total + ", " + Discount + ", " + EmployeeId + ", " + CompanyId + ", " + BranchId + ");";
            sqld = new SqlCommand(query, DataConn.Connection);
            sqld.ExecuteNonQuery();
            sqld.Dispose();
        }

        public override void update(int id)
        {
            string query = @"UPDATE [coffee_shop].[dbo].[receipts] 
                SET number = " + Number + ", address = '" + Address + "', product_id = " + ProductId + ", qty = " + Qty + ", price = " + Price + ", total = " + Total + ", discount = " + Discount + ", employee_id = " + EmployeeId + ", company_id = " + CompanyId + ", branch_id = " + BranchId + " WHERE id = " + id + ";";
            sqld = new SqlCommand(query, DataConn.Connection);
            sqld.ExecuteNonQuery();
            sqld.Dispose();
        }
    }
}
