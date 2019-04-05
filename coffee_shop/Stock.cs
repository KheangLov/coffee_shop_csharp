using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coffee_shop
{
    class Stock : Db_Insert
    {
        SqlCommand sqld;
        private string name;
        private string imported_date;
        private string expired_date;
        private decimal quantity;
        private decimal price;
        private decimal selling_price;
        private decimal alert_quantity;
        private byte alerted;
        private int stockcate_id;
        private int company_id;
        private int branch_id;

        public Stock()
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

        public string ImportedDate
        {
            get
            {
                return imported_date;
            }
            set
            {
                imported_date = value;
            }
        }

        public string ExpiredDate
        {
            get
            {
                return expired_date;
            }
            set
            {
                expired_date = value;
            }
        }

        public decimal Quantity
        {
            get
            {
                return quantity;
            }
            set
            {
                quantity = value;
            }
        }

        public decimal Price
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

        public decimal SellingPrice
        {
            get
            {
                return selling_price;
            }
            set
            {
                selling_price = value;
            }
        }

        public decimal AlertedQuantity
        {
            get
            {
                return alert_quantity;
            }
            set
            {
                alert_quantity = value;
            }
        }

        public byte Alerted
        {
            get
            {
                return alerted;
            }
            set
            {
                alerted = value;
            }
        }

        public int StockCateId
        {
            get
            {
                return stockcate_id;
            }
            set
            {
                stockcate_id = value;
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
            string query = @"INSERT INTO [coffee_shop].[dbo].[stocks](name, imported_date, expired_date, qty,price, selling_price, alert_qty, alerted, stockcate_id, company_id, branch_id) 
                values('" + Name + "', '" + ImportedDate + "', '" + ExpiredDate + "', " + Quantity + ", " + Price + ", " + SellingPrice + ", " + AlertedQuantity + ", " + Alerted + ", " + StockCateId + ", " + CompanyId + ", " + BranchId + ");";
            sqld = new SqlCommand(query, DataConn.Connection);
            sqld.ExecuteNonQuery();
            sqld.Dispose();
        }

        public override void update(int id)
        {
            string query = @"UPDATE [coffee_shop].[dbo].[stocks] 
                SET name = '" + Name + "', imported_date ='" + ImportedDate + "', expired_date = '" + ExpiredDate + "', qty = " + Quantity + ", price = " + Price + ", selling_price = " + SellingPrice + ", alert_qty = " + AlertedQuantity + ", alerted = " + Alerted + ", stockcate_id = " + StockCateId + ", company_id = " + CompanyId + ", branch_id = " + BranchId + " WHERE id = " + id + ";";
            sqld = new SqlCommand(query, DataConn.Connection);
            sqld.ExecuteNonQuery();
            sqld.Dispose();
        }
    }
}
