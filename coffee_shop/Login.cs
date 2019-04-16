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
        string user_role = "";
        string user_name = "";
        int user_id;

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtUser.Text != "" && txtPassword.Text != "")
            {
                string sql = @"SELECT users.*, roles.name AS role_name FROM users
                    INNER JOIN roles ON users.role_id = roles.id
                    WHERE LOWER(username) = '" + txtUser.Text.ToLower() + "' AND password = '" + hc.PassHash(txtPassword.Text) + "';";
                SqlCommand sqld = new SqlCommand(sql, DataConn.Connection);
                SqlDataReader sqlr = sqld.ExecuteReader();
                while(sqlr.Read())
                {
                    user_id = int.Parse(sqlr["id"].ToString());
                    user_name = sqlr["username"].ToString();
                    user_role = sqlr["role_name"].ToString();
                }
                sqld.Dispose();
                sqlr.Close();
                string count = "SELECT COUNT(*) FROM users WHERE LOWER(username) = '" + txtUser.Text.ToLower() + "' AND password = '" + hc.PassHash(txtPassword.Text) + "';";
                SqlCommand count_cmd = new SqlCommand(count, DataConn.Connection);
                int count_data = Convert.ToInt16(count_cmd.ExecuteScalar());
                count_cmd.Dispose();
                if (count_data != 0)
                {
                    this.Hide();
                    DataConn.Connection.Close();
                    if (user_role.ToLower() == "superadmin")
                        new Mainsa_Form(user_role, user_id, user_name).Show();
                    else
                        new Main(user_role, user_id, user_name).Show();
                }
                else
                {
                    MessageBox.Show("Wrong username or password!");
                }
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

        private void btnExit_Click(object sender, EventArgs e)
        {
            DataConn.Connection.Close();
            Application.Exit();
        }
    }
}
