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
        SqlCommand com = new SqlCommand();
        public Login()
        {
            InitializeComponent();
        }
        HashCode hc = new HashCode();

        private void btnLogin_Click(object sender, EventArgs e)
        {
            com.Connection = DataConn.Connection;
            com.CommandText = "SELECT * FROM users;";
            SqlDataReader dr = com.ExecuteReader();
            if(dr.Read())
            {
                if(txtUser.Text.Equals(dr["username"].ToString()) && hc.PassHash(txtPassword.Text).Equals(dr["password"].ToString()))
                {
                    this.Hide();
                    new Main().Show();
                }
                else
                {
                    MessageBox.Show("Wrong user or password!");
                }
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
    }
}
