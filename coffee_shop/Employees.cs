using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace coffee_shop
{
    class Employees : Db_Insert
    {
        SqlCommand sqld;
        private string firstname;
        private string lastname;
        private string fullname;
        private string gender;
        private string dob;
        private string address;
        private string phone;
        private string email;
        private string positions;
        private string work_times;
        private decimal salary;
        private int company_id;
        private int branch_id;

        public Employees()
        {

        }
        public string Firstname
        {
            get
            {
                return firstname;
            }
            set
            {
                firstname = value;
            }
        }

        public string Lastname
        {
            get
            {
                return lastname;
            }
            set
            {
                lastname = value;
            }
        }

        public string Fullname
        {
            get
            {
                return fullname;
            }
            set
            {
                fullname = value;
            }
        }

        public string Dob
        {
            get
            {
                return dob;
            }
            set
            {
                dob = value;
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

        public string Positions
        {
            get
            {
                return positions;
            }
            set
            {
                positions = value;
            }
        }

        public string WorkTimes
        {
            get
            {
                return work_times;
            }
            set
            {
                work_times = value;
            }
        }

        public decimal Salary
        {
            get
            {
                return salary;
            }
            set
            {
                salary = value;
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

        public string Gender
        {
            get
            {
                return gender;
            }
            set
            {
                gender = value;
            }
        }

        public override void insert()
        {
            string query = @"INSERT INTO [coffee_shop].[dbo].[employees]
                (firstname, lastname, fullname, dob, address, phone, email, positions, work_times, salary, company_id, branch_id, gender) 
                VALUES('" + Firstname + "', '" + Lastname + "', '" + Fullname + "', '" + Dob + "', '" + Address + "', '" + Phone + "', '" + Email + "', '" + Positions + "', '" + WorkTimes + "', " + Salary + ", " + CompanyId + ", " + BranchId + ", '" + Gender + "');";
            sqld = new SqlCommand(query, DataConn.Connection);
            sqld.ExecuteNonQuery();
            sqld.Dispose();
        }

        public override void update(int id)
        {
            string query = @"UPDATE [coffee_shop].[dbo].[employees] 
                SET firstname = '" + Firstname + "', lastname = '" + Lastname + "', fullname = '" + Fullname + "', dob = '" + Dob + "', address = '" + Address + "', phone = '" + Phone + "', email = '" + Email + "', positions = '" + Positions + "', work_times = '" + WorkTimes + "', salary = " + Salary + ", company_id = " + CompanyId + ", branch_id = " + BranchId + ", gender = " + Gender + ";";
            sqld = new SqlCommand(query, DataConn.Connection);
            sqld.ExecuteNonQuery();
            sqld.Dispose();
        }
    }
}
