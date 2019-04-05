using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace coffee_shop
{
    class Products : Db_Insert
    {
        SqlCommand sqld;
        private string name;
        private double price;
        private double selling_price;
        private string type;
        private int sale;
        private int stock_id;
        private int procate_id;
        private string image;

        public Products()
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

        public double Selling_Price
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

        public int Sale
        {
            get
            {
                return sale;
            }
            set
            {
                sale = value;
            }
        }

        public int Stock_id
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

        public int Procate_id
        {
            get
            {
                return procate_id;
            }
            set
            {
                procate_id = value;
            }
        }

        public string Image
        {
            get
            {
                return image;
            }
            set
            {
                image = value;
            }
        }
        
        public override void insert()
        {
            string query = @"INSERT INTO [coffee_shop].[dbo].[products](name, price, selling_price, type, sale, stock_id, procate_id, images) 
                values('" + Name + "', " + Price + ", " + Selling_Price + ", '" + Type + "', " + Sale + ", " + Stock_id + ", " + Procate_id + ", '" + Image +"');";
            sqld = new SqlCommand(query, DataConn.Connection);
            sqld.ExecuteNonQuery();
            sqld.Dispose();
        }
        public override void update(int id)
        {
            string query = @"UPDATE [coffee_shop].[dbo].[products] 
                SET name = '" + Name + "', price = " + Price + ", selling_price = " + Selling_Price + ", type = '" + Type + "', sale = " + Sale + ", stock_id = " + Stock_id + ", procate_id = " + Procate_id + ", images = '" + Image + "' WHERE id = " + id + ";";
            sqld = new SqlCommand(query, DataConn.Connection);
            sqld.ExecuteNonQuery();
            sqld.Dispose();
        }
    }
}
