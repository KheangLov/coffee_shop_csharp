using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace coffee_shop
{
    public partial class Main : Form
    {
        string uRole;
        int uId;
        string uName;
        string com_id = "";
        public Main(string role, int id, string name)
        {
            uRole = role;
            uId = id;
            uName = name;
            InitializeComponent();
        }
        string cid = "";
        string bId = "";

        private void GetUserCompany()
        {
            DataConn.Connection.Open();
            string sql = "SELECT * FROM companies WHERE user_id = " + uId + ";";
            SqlCommand sqld = new SqlCommand(sql, DataConn.Connection);
            SqlDataReader sqlr = sqld.ExecuteReader();
            int i = 0;
            while (sqlr.Read())
            {
                if(i == 0)
                    com_id += sqlr["id"].ToString();
                else
                    com_id += ", " + sqlr["id"].ToString();
                i++;
            }
            sqld.Dispose();
            sqlr.Close();
            DataConn.Connection.Close();
        }

        private void CheckStock()
        {
            if(com_id != "")
            {
                DataConn.Connection.Open();
                string sql = "SELECT COUNT(*) FROM stocks WHERE alerted = 1 AND company_id IN(" + com_id + ");";
                SqlCommand sqld = new SqlCommand(sql, DataConn.Connection);
                int count_alert = Convert.ToInt16(sqld.ExecuteScalar());
                sqld.Dispose();
                DataConn.Connection.Close();
                if (count_alert > 0)
                    lbAlert.Text = "Please update your stock!";
                else
                    lbAlert.Text = "";
            }
            else
            {
                MessageBox.Show("You don't have company yet!");
            }
        }

        private int CheckBranch()
        {
            DataConn.Connection.Open();
            string sql = "SELECT COUNT(*) FROM branches WHERE company_id IN (" + com_id + ");";
            SqlCommand sqld = new SqlCommand(sql, DataConn.Connection);
            int count = Convert.ToInt16(sqld.ExecuteScalar());
            sqld.Dispose();
            DataConn.Connection.Close();
            return count;
        }

        private int CheckCompany()
        {
            DataConn.Connection.Open();
            string sql = "SELECT COUNT(*) FROM companies WHERE user_id = " + uId + ";";
            SqlCommand sqld = new SqlCommand(sql, DataConn.Connection);
            int count = Convert.ToInt16(sqld.ExecuteScalar());
            sqld.Dispose();
            DataConn.Connection.Close();
            return count;
        }

        private int CheckUser()
        {
            DataConn.Connection.Open();
            string sql = @"SELECT COUNT(*) FROM users 
                        INNER JOIN roles ON users.role_id = roles.id
                        WHERE roles.name IN ('editor', 'user');";
            SqlCommand sqld = new SqlCommand(sql, DataConn.Connection);
            int count = Convert.ToInt16(sqld.ExecuteScalar());
            sqld.Dispose();
            DataConn.Connection.Close();
            return count;
        }

        private void CheckMember()
        {
            DataConn.Connection.Open();
            string sql = "SELECT * FROM members WHERE LOWER(name) = '" + uName.ToLower() + "';";
            SqlCommand sqld = new SqlCommand(sql, DataConn.Connection);
            SqlDataReader sqlr = sqld.ExecuteReader();
            if (sqlr.Read())
            {
                cid = sqlr["company_id"].ToString();
                bId = sqlr["branch_id"].ToString();
            }
            DataConn.Connection.Close();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            if (uRole.ToLower() == "admin")
            {
                GetUserCompany();
                timer1.Start();
                if (com_id != "")
                    CheckStock();
            }
        }

        private void main_closing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Dispose();
            new Login().Show();
        }

        private void myUsersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (uRole.ToLower() == "superadmin" || uRole.ToLower() == "admin")
            {
                new users_form(uRole).ShowDialog();
            }
            else
                MessageBox.Show("You don't have permission!");
        }

        private void myCompanyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (uRole.ToLower() == "superadmin" || uRole.ToLower() == "admin")
                new company_form(uId, uRole, uName).ShowDialog();
            else
                MessageBox.Show("You don't have permission!");
        }

        private void allProductsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (uRole.ToLower() == "admin")
            {
                if(com_id != "")
                {
                    if (CheckCompany() > 0 && CheckBranch() > 0)
                        new products_form(com_id, uRole).ShowDialog();
                    else
                        MessageBox.Show("Please create Company or Branch first!");
                }
            }
            else
            {
                CheckMember();
                if (cid != "")
                    new products_form(cid, uRole).ShowDialog();
                else
                    MessageBox.Show("You're not a member of a company!");
            }
        }

        private void myStockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(uRole.ToLower() == "admin")
            {
                if (CheckCompany() > 0 && CheckBranch() > 0)
                    new stock_form(com_id, uRole).ShowDialog();
                else
                    MessageBox.Show("Please create Company or Branch first!");
            }
            else
            {
                CheckMember();
                if (cid != "")
                    new stock_form(cid, uRole).ShowDialog();
                else
                    MessageBox.Show("You're not a member of a company!");
            }
        }

        private void stockCateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (uRole.ToLower() == "admin")
            {
                if (CheckCompany() > 0 && CheckBranch() > 0)
                    new stock_categories_form(uRole).ShowDialog();
                else
                    MessageBox.Show("Please create Company or Branch first!");
            }
            else
            {
                CheckMember();
                if (cid != "")
                    new stock_categories_form(uRole).ShowDialog();
                else
                    MessageBox.Show("You're not a member of a company!");
            }
        }

        private void branchesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (uRole.ToLower() == "admin")
            {
                if (CheckCompany() > 0)
                    new branch_form(uId, uRole).ShowDialog();
                else
                    MessageBox.Show("Please create company first!");
            }
            else
                MessageBox.Show("You don't have permission!");
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnDrinks_Click(object sender, EventArgs e)
        {
            if(uRole.ToLower() == "admin")
            {
                if (CheckCompany() > 0 && CheckBranch() > 0)
                    new product_selling("drinks", com_id, bId, uRole.ToLower()).ShowDialog();
                else
                    MessageBox.Show("Please create Company or Branch first!");
            }
            else
            {
                CheckMember();
                if (cid != "")
                    new product_selling("drinks", cid, bId, uRole.ToLower()).ShowDialog();
                else
                    MessageBox.Show("You're not a member of a company!");
            }
        }

        private void btnFood_Click(object sender, EventArgs e)
        {
            if (uRole.ToLower() == "admin")
            {
                if (CheckCompany() > 0 && CheckBranch() > 0)
                    new product_selling("foods", com_id, bId, com_id).ShowDialog();
                else
                    MessageBox.Show("Please create Company or Branch first!");
            }
            else
            {
                CheckMember();
                if (cid != "")
                    new product_selling("foods", com_id, bId, cid).ShowDialog();
                else
                    MessageBox.Show("You're not a member of a company!");
            }
        }

        private void newProductToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (uRole.ToLower() == "admin")
            {
                if (CheckCompany() > 0 && CheckBranch() > 0)
                    new product_category_form(uRole).ShowDialog();
                else
                    MessageBox.Show("Please create Company or Branch first!");
            }
            else
            {
                CheckMember();
                if (cid != "")
                    new product_category_form(uRole).ShowDialog();
                else
                    MessageBox.Show("Please create Company or Branch first!");
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AboutCoffeeShop().ShowDialog();
        }

        private void membersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (uRole.ToLower() == "admin" && CheckUser() > 0)
                new member_form(uId, uName).ShowDialog();
            else if (CheckUser() <= 0)
                MessageBox.Show("Please create editor or user first!");
            else
                MessageBox.Show("You don't have permission!");
        }

        private void employeesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (uRole.ToLower() == "admin")
            {
                if (CheckCompany() > 0 && CheckBranch() > 0)
                    new employees_form(com_id).ShowDialog();
                else
                    MessageBox.Show("Please create Company or Branch first!");
            }
            else
                MessageBox.Show("You don't have permission!");
        }

        private void suppliersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (uRole.ToLower() == "admin")
            {
                if (CheckCompany() > 0 && CheckBranch() > 0)
                    new supplier_form(com_id).ShowDialog();
                else
                    MessageBox.Show("Please create Company or Branch first!");
            }
            else
                MessageBox.Show("You don't have permission!");
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            CheckStock();
        }
    }
}
