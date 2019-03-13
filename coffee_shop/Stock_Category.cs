using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace coffee_shop
{
    class Stock_Category : Db_Insert
    {
        SqlCommand sqld;
        private string name;
        private string description;
        private int branch_id;

        public Stock_Category()
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

        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
            }
        }

        public int Branch_ID
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
            string query = @"INSERT INTO [coffee_shop].[dbo].[stock_categories](name, description, branch_id) 
                VALUES('" + Name + "', '" + Description + "', " + Branch_ID + ");";
            sqld = new SqlCommand(query, DataConn.Connection);
            sqld.ExecuteNonQuery();
            sqld.Dispose();
        }
    }
}
