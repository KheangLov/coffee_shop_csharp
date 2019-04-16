using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coffee_shop
{
    class Member : Db_Insert
    {
        SqlCommand sqld;
        private string name;
        private int user_id;
        private int company_id;
        private int branch_id;

        public Member()
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

        public int UserId
        {
            get
            {
                return user_id;
            }
            set
            {
                user_id = value;
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
            string query = @"INSERT INTO [coffee_shop].[dbo].[members](name, user_id, company_id, branch_id) 
                values('" + Name + "', " + UserId + ", " + CompanyId + ", " + BranchId + ");";
            sqld = new SqlCommand(query, DataConn.Connection);
            sqld.ExecuteNonQuery();
            sqld.Dispose();
        }

        public override void update(int id)
        {
            string query = @"UPDATE [coffee_shop].[dbo].[members] 
                SET name = '" + Name + "', user_id = " + UserId + ", company_id = " + CompanyId + ", branch_id = " + BranchId + " WHERE id = " + id + ";";
            sqld = new SqlCommand(query, DataConn.Connection);
            sqld.ExecuteNonQuery();
            sqld.Dispose();
        }
    }
}
