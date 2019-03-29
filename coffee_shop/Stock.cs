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
        private DateTime expired_date;
        private decimal quantity;
        private decimal price;
        private decimal selling_price;
        private decimal alert_quantity;
        private int stockcate_id;

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

        public DateTime ExpiredDate
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

        public override void insert()
        {
            string query = @"INSERT INTO [coffee_shop].[dbo].[stocks](name, imported_date, expired_date, qty,price, selling_price, alert_qty, alerted, stockcate_id) 
                values('" + Name + "', '" + ImportedDate + "', '" + ExpiredDate.ToString("yyyy-MM-dd") + "', " + Quantity + ", " + price + ", " + SellingPrice + ", " + AlertedQuantity + ", " + 0 + ", " + StockCateId + ");";
            sqld = new SqlCommand(query, DataConn.Connection);
            sqld.ExecuteNonQuery();
            sqld.Dispose();
        }

        public override void update(int id)
        {
            string query = @"UPDATE [coffee_shop].[dbo].[stocks] 
                SET name = '" + Name + "', imported_date ='" + ImportedDate + "', expired_date = '" + ExpiredDate + "', qty = " + Quantity + ", price = " + Price + ", selling_price = " + SellingPrice + ", alert_qty = " + AlertedQuantity + ", alerted = " + 0 + ", stockcate_id = " + StockCateId + ";";
            sqld = new SqlCommand(query, DataConn.Connection);
            sqld.ExecuteNonQuery();
            sqld.Dispose();
        }
    }
}
