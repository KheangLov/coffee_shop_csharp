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
    public partial class users_form : Form
    {
        public users_form()
        {
            InitializeComponent();
        }

        Users my_user = new Users();
        HashCode hc = new HashCode();
        MyInter inter;

        private void lvUsers_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        void ClearTextBoxes(Control parent)
        {
            foreach (Control child in parent.Controls)
            {
                TextBox textBox = child as TextBox;
                if (textBox == null)
                    ClearTextBoxes(child);
                else
                    textBox.Text = string.Empty;
            }
        }

        private void users_form_load(object sender, EventArgs e)
        {
            DataConn.Connection.Open();
            MyInter user_inter = my_user;
            inter = user_inter;
            cbGender.SelectedIndex = 0;
            cbRole.SelectedIndex = 0;
            try
            {
                string sql = "SELECT * FROM users FULL OUTER JOIN roles ON users.role_id = roles.id;";
                SqlCommand com = new SqlCommand(sql, DataConn.Connection);
                SqlDataReader sqlr = com.ExecuteReader();
                while (sqlr.Read())
                {
                    string[] user_info = { sqlr["username"].ToString(), sqlr["email"].ToString(), sqlr["gender"].ToString(), sqlr["name"].ToString(), sqlr["address"].ToString() };
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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                my_user.Created_Date = DateTime.Now.ToString();
                my_user.Username = my_user.Firstname + my_user.Lastname;
                inter.insert();
                MessageBox.Show("Insert successfull!");
                ClearTextBoxes(groupBox1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cbRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbRole.Text.ToLower() == "superadmin")
            {
                my_user.Role_Id = cbRole.SelectedIndex + 1;
            }
            else if (cbRole.Text.ToLower() == "admin")
            {
                my_user.Role_Id = cbRole.SelectedIndex + 1;
            }
            else if (cbRole.Text.ToLower() == "editor")
            {
                my_user.Role_Id = cbRole.SelectedIndex + 1;
            }
            else if (cbRole.Text.ToLower() == "user")
            {
                my_user.Role_Id = cbRole.SelectedIndex + 1;
            }
            else
            {
                MessageBox.Show("Nothing found!");
            }
        }

        private void cbGender_SelectedIndexChanged(object sender, EventArgs e)
        {
            my_user.Gender = cbGender.Text.Trim();
        }

        private void txtEmail_leave(object sender, EventArgs e)
        {
            if (IsValidEmail(txtEmail.Text.Trim()))
                my_user.Email = txtEmail.Text.Trim();
            else
                MessageBox.Show("Invalid Email!");
        }

        private void users_form_closing(object sender, FormClosingEventArgs e)
        {
            DataConn.Connection.Close();
            this.Dispose();
        }

        private void txtFirstname_leave(object sender, EventArgs e)
        {
            my_user.Firstname = txtFirstname.Text.Trim();
        }

        private void txtLastname_leave(object sender, EventArgs e)
        {
            my_user.Lastname = txtLastname.Text.Trim();
        }

        private void txtPassword_leave(object sender, EventArgs e)
        {

        }

        private void txtConfirmPass_leave(object sender, EventArgs e)
        {
            if (txtPassword.Text != "")
            {
                string pass = txtPassword.Text.Trim();
                string pass_con = txtConfirmPass.Text.Trim();
                if (pass == pass_con)
                {
                    my_user.Password = hc.PassHash(pass);
                }
                else
                {
                    MessageBox.Show("Wrong confirmation password!");
                }
            }
            else
            {
                MessageBox.Show("Please insert password first!");
                txtPassword.Focus();
            }
        }

        private void txtPhone_leave(object sender, EventArgs e)
        {
            my_user.Phone = txtPhone.Text.Trim();
        }

        private void txtAddress_leave(object sender, EventArgs e)
        {
            my_user.Address = txtAddress.Text.Trim();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtConfirmPass_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
