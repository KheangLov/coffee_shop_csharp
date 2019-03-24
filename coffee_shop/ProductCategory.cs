using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace coffee_shop
{
    class ProductCategory : Db_Insert
    {
        SqlCommand sqld;
        private string name;
        private string descriptions;
        public ProductCategory()
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

        public string Descriptions
        {
            get
            {
                return descriptions;
            }
            set
            {
                descriptions = value;
            }
        }


        public override void insert()
        {
            string query = @"INSERT INTO [coffee_shop].[dbo].[product_categories](name, descriptions) 
                values('" + Name + "', '" + Descriptions + "');";
            sqld = new SqlCommand(query, DataConn.Connection);
            sqld.ExecuteNonQuery();
            sqld.Dispose();
        }
        public override void update(int id)
        {

        }
    }
}
