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
                string sql = "SELECT COUNT(*) FROM users WHERE username = '" + txtUser.Text + "' AND password = '" + hc.PassHash(txtPassword.Text) + "';";
                SqlCommand sqld = new SqlCommand(sql, DataConn.Connection);
                int count_data = Convert.ToInt16(sqld.ExecuteScalar());
                if (count_data != 0)
                {
                    this.Hide();
                    new Main().Show();
                }
                else
                {
                    MessageBox.Show("Wrong username or password!");
                }
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
                DataConn.ConnectionDB();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnReg_Click(object sender, EventArgs e)
        {
        }

        private void login_closing(object sender, FormClosingEventArgs e)
        {
            DataConn.Connection.Close();
            Application.Exit();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DataConn.Connection.Close();
            Application.Exit();
        }
    }
}
