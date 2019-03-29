﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coffee_shop
{
    class StockCategory : Db_Insert
    {
        SqlCommand sqld;
        private string name;
        private string descriptions;
        private int branch_id;

        public StockCategory()
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
            string query = @"INSERT INTO [coffee_shop].[dbo].[stock_categories](name, descriptions, branch_id) 
                values('" + Name + "', '" + Descriptions + "', " + BranchId + ");";
            sqld = new SqlCommand(query, DataConn.Connection);
            sqld.ExecuteNonQuery();
            sqld.Dispose();
        }

        public override void update(int id)
        {
            string query = @"UPDATE [coffee_shop].[dbo].[stocks] 
                SET name = '" + Name + "', descriptions ='" + Descriptions + "', branch_id = " + BranchId + ";";
            sqld = new SqlCommand(query, DataConn.Connection);
            sqld.ExecuteNonQuery();
            sqld.Dispose();
        }
    }
}