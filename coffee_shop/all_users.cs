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
    public partial class all_users : Form
    {
        public all_users()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
            new Main().Show();
        }

        private void all_users_closing(object sender, FormClosingEventArgs e)
        {
            DataConn.Connection.Close();
            Application.Exit();
        }

        private void all_users_load(object sender, EventArgs e)
        {
            try
            {
                string sql = "SELECT * FROM users FULL OUTER JOIN roles ON users.role_id = roles.id;";
                SqlCommand com = new SqlCommand(sql, DataConn.Connection);
                SqlDataReader sqlr = com.ExecuteReader();
                while(sqlr.Read())
                {
                    string[] user_info = { sqlr["username"].ToString(), sqlr["email"].ToString(), sqlr["gender"].ToString(), sqlr["name"].ToString() };
                    ListViewItem item = new ListViewItem(user_info);
                    lvUsers.Items.Add(item);
                }
                sqlr.Close();
                com.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
