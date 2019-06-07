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
        private string number;
        private string address;
        private string product_name;
        private double qty;
        private double price;
        private double total;
        private double discount;
        private string employee_name;
        private string company_name;
        private string branch_name;
        private string type;
        private int waiting_number;
        private string date;

        public Receipt()
        {

        }

        public string Number
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

        public string ProductName
        {
            get
            {
                return product_name;
            }
            set
            {
                product_name = value;
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

        public string EmployeeName
        {
            get
            {
                return employee_name;
            }
            set
            {
                employee_name = value;
            }
        }

        public string CompanyName
        {
            get
            {
                return company_name;
            }
            set
            {
                company_name = value;
            }
        }

        public string BranchName
        {
            get
            {
                return branch_name;
            }
            set
            {
                branch_name = value;
            }
        }

        public string Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
            }
        }

        public int WaitingNumber
        {
            get
            {
                return waiting_number;
            }
            set
            {
                waiting_number = value;
            }
        }

        public string Date
        {
            get
            {
                return date;
            }
            set
            {
                date = value;
            }
        }

        public override void insert()
        {
            string query = @"INSERT INTO [coffee_shop].[dbo].[receipts](number, product_name, qty, price, total, discount, employee_name, company_name, branch_name, waiting_number, date)
                values('" + Number + "', '" + ProductName + "', " + Qty + ", " + Price + ", " + Total + ", " + Discount + ", '" + EmployeeName + "', '" + CompanyName + "', '" + BranchName + "', " + WaitingNumber + ", '" + Date + "');";
            sqld = new SqlCommand(query, DataConn.Connection);
            sqld.ExecuteNonQuery();
            sqld.Dispose();
        }

        public override void update(int id)
        {
            string query = @"UPDATE [coffee_shop].[dbo].[receipts] 
                SET number = '" + Number + "', product_name = '" + ProductName + "', qty = " + Qty + ", price = " + Price + ", total = " + Total + ", discount = " + Discount + ", employee_name = '" + EmployeeName + "', company_name = '" + CompanyName + "', branch_name = '" + BranchName + "', waiting_number = " + WaitingNumber + ", date = '" + Date + "' WHERE id = " + id + ";";
            sqld = new SqlCommand(query, DataConn.Connection);
            sqld.ExecuteNonQuery();
            sqld.Dispose();
        }
    }
}
