using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace coffee_shop
{
    public partial class Login : Form
    {

        public Login()
        {
            InitializeComponent();
        }
        HashCode hc = new HashCode();

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtUser.Text != "" && txtPassword.Text != "")
            {
                string sql = "SELECT * FROM users WHERE username = '" + txtUser.Text + "' AND password = '" + hc.PassHash(txtPassword.Text) + "';";
                SqlCommand sqld = new SqlCommand(sql, DataConn.Connection);
                SqlDataReader sqlr = sqld.ExecuteReader();
                if (sqlr.Read())
                {
                    //string my_un = sqlr["username"].ToString();
                    //if (txtUser.Text == my_un && hc.PassHash(txtPassword.Text) == hc.PassHash(sqlr["password"].ToString()))
                    //{
                    //    this.Hide();
                    //    new Main().Show();
                    //}
                    if(txtUser.Text.Equals(sqlr["username"].ToString()) && hc.PassHash(txtPassword.Text).Equals(sqlr["password"].ToString()))
                    {
                        this.Hide();
                        new Main().Show();
                    }
                    else if (txtUser.Text.Equals(sqlr["username"].ToString()))
                    {
                        MessageBox.Show("Wrong username!");
                    }
                    else if (hc.PassHash(txtPassword.Text).Equals(sqlr["password"].ToString()))
                    {
                        MessageBox.Show("Wrong password!");
                    }
                    else
                    {
                        MessageBox.Show("Wrong username & password!");
                    }
                }
                sqlr.Close();
                sqld.Dispose();
            }
            else
            {
                MessageBox.Show("Please input user and password first!");
            }
        }

        private void login_load(object sender, EventArgs e)
        {
            try
            {
                string ipServer = @"KHEANG-PC";
                string dbName = "coffee_shop";
                string user = "coffee";
                string password = "not4you";
                DataConn.ConnectionDB(ipServer, dbName, user, password);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnReg_Click(object sender, EventArgs e)
        {
            this.Hide();
            new new_user().Show();
        }

        private void login_closing(object sender, FormClosingEventArgs e)
        {
            DataConn.Connection.Close();
            Application.Exit();
        }
    }
}
