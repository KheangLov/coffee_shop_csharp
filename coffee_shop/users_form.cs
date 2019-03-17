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
        StringCapitalize sc = new StringCapitalize();
        MyInter inter;
        int userId;

        private void lvUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(lvUsers.SelectedItems.Count != 0)
            {
                btnEdit.Enabled = true;
                btnDel.Enabled = true;
            }
            else
            {
                btnDel.Enabled = false;
                btnEdit.Enabled = false;
            }
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
            btnEdit.Enabled = false;
            btnDel.Enabled = false;
            DataConn.Connection.Open();
            MyInter user_inter = my_user;
            inter = user_inter;
            cbGender.SelectedIndex = 0;
            cbRole.SelectedIndex = 0;
            try
            {
                string sql = "SELECT * FROM users INNER JOIN roles ON users.role_id = roles.id WHERE users.role_id = roles.id;";
                SqlCommand com = new SqlCommand(sql, DataConn.Connection);
                SqlDataReader sqlr = com.ExecuteReader();
                while (sqlr.Read())
                {
                    string[] user_info = { sc.ToCapitalize(sqlr["username"].ToString()), sqlr["email"].ToString(), sqlr["gender"].ToString(), sc.ToCapitalize(sqlr["name"].ToString()), sqlr["phone"].ToString(), sqlr["address"].ToString() };
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
            loadComboRole();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtFirstname.Text != "" && txtLastname.Text != "" && txtEmail.Text != "" && txtPassword.Text != "")
                {
                    loadComboRole();
                    my_user.Created_Date = DateTime.Now.ToString();
                    my_user.Username = my_user.Firstname + my_user.Lastname;
                    string check_name = "SELECT COUNT(*) FROM users WHERE username = '" + my_user.Username + "';";
                    SqlCommand check_com = new SqlCommand(check_name, DataConn.Connection);
                    int find_user = Convert.ToInt16(check_com.ExecuteScalar());
                    if (find_user != 0)
                    {
                        MessageBox.Show("User already exist!");
                        txtFirstname.Focus();
                    }
                    else
                    {
                        inter.insert();
                        MessageBox.Show("Insert Successfully!");
                        ClearTextBoxes(groupBox1);
                        lvUsers.Items.Clear();
                        string sql = "SELECT * FROM users INNER JOIN roles ON users.role_id = roles.id WHERE users.role_id = roles.id;";
                        SqlCommand com = new SqlCommand(sql, DataConn.Connection);
                        SqlDataReader sqlr = com.ExecuteReader();
                        while (sqlr.Read())
                        {
                            string[] user_info = { sc.ToCapitalize(sqlr["username"].ToString()), sqlr["email"].ToString(), sqlr["gender"].ToString(), sc.ToCapitalize(sqlr["name"].ToString()), sqlr["phone"].ToString(), sqlr["address"].ToString() };
                            ListViewItem item = new ListViewItem(user_info);
                            lvUsers.Items.Add(item);
                        }
                        sqlr.Close();
                        com.Dispose();
                    }
                }
                else
                {
                    MessageBox.Show("Firstname, Lastname, Email and Password can't be blank!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void loadComboRole()
        {
            string sql = "SELECT * FROM roles WHERE name = '" + cbRole.Text.ToLower() + "';";
            SqlCommand sqld = new SqlCommand(sql, DataConn.Connection);
            SqlDataReader sqlr = sqld.ExecuteReader();
            if (sqlr.Read())
            {
                my_user.Role_Id = int.Parse(sqlr["id"].ToString());
            }
            else
            {
                MessageBox.Show("Nothing found!");
            }
            sqlr.Close();
            sqld.Dispose();
        }

        private void cbRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            
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
            if(txtFirstname.Text != "")
                my_user.Firstname = txtFirstname.Text.Trim();
        }

        private void txtLastname_leave(object sender, EventArgs e)
        {
            if (txtLastname.Text != "")
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
            DataConn.Connection.Close();
            this.Dispose();
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtConfirmPass_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if(lvUsers.SelectedItems.Count != 0)
            {
                if(btnEdit.Text.ToLower() == "edit")
                {
                    ListViewItem item = lvUsers.SelectedItems[0];
                    string lv_username = item.SubItems[0].Text;
                    string sql = "SELECT * FROM users INNER JOIN roles ON users.role_id = roles.id WHERE username = '" + lv_username + "';";
                    SqlCommand command = new SqlCommand(sql, DataConn.Connection);
                    SqlDataReader reader = command.ExecuteReader();
                    while(reader.Read())
                    {
                        if(reader["name"].ToString().ToLower() == "superadmin")
                        {
                            MessageBox.Show("You can't edit application owner!");
                        }
                        else
                        {
                            userId = int.Parse(reader["id"].ToString());
                            txtFirstname.Text = reader["firstname"].ToString();
                            txtLastname.Text = reader["lastname"].ToString();
                            txtEmail.Text = reader["email"].ToString();
                            cbGender.Text = reader["gender"].ToString();
                            txtPhone.Text = reader["phone"].ToString();
                            txtAddress.Text = reader["address"].ToString();

                            if (reader["name"].ToString().ToLower() == "admin")
                            {
                                cbRole.SelectedIndex = 0;
                            }
                            else if (reader["name"].ToString().ToLower() == "editor")
                            {
                                cbRole.SelectedIndex = 1;
                            }
                            else if (reader["name"].ToString().ToLower() == "user")
                            {
                                cbRole.SelectedIndex = 2;
                            }
                            else
                            {
                                MessageBox.Show("No role found!");
                            }
                        }
                    }
                    command.Dispose();
                    reader.Close();
                    btnEdit.Text = "Update";
                }
                else if(btnEdit.Text.ToLower() == "update")
                {
                    try
                    {
                        loadComboRole();
                        my_user.Username = my_user.Firstname + my_user.Lastname;
                        inter.update(userId);
                        MessageBox.Show("Update Successfully!");
                        ClearTextBoxes(groupBox1);
                        lvUsers.Items.Clear();
                        string sql = "SELECT * FROM users INNER JOIN roles ON users.role_id = roles.id WHERE users.role_id = roles.id;";
                        SqlCommand com = new SqlCommand(sql, DataConn.Connection);
                        SqlDataReader sqlr = com.ExecuteReader();
                        while (sqlr.Read())
                        {
                            string[] user_info = { sc.ToCapitalize(sqlr["username"].ToString()), sqlr["email"].ToString(), sqlr["gender"].ToString(), sc.ToCapitalize(sqlr["name"].ToString()), sqlr["phone"].ToString(), sqlr["address"].ToString() };
                            ListViewItem item = new ListViewItem(user_info);
                            lvUsers.Items.Add(item);
                        }
                        sqlr.Close();
                        com.Dispose();
                        btnEdit.Text = "Edit";
                        btnEdit.Enabled = false;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void txtFirstname_TextChanged(object sender, EventArgs e)
        {
            my_user.Firstname = txtFirstname.Text.Trim();
        }

        private void txtLastname_TextChanged(object sender, EventArgs e)
        {
            my_user.Lastname = txtLastname.Text.Trim();
        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {
            my_user.Email = txtEmail.Text.Trim();
        }

        private void txtPhone_TextChanged(object sender, EventArgs e)
        {
            my_user.Phone = txtPhone.Text.Trim();
        }

        private void txtAddress_TextChanged(object sender, EventArgs e)
        {
           my_user.Address = txtAddress.Text.Trim();
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if(lvUsers.SelectedItems.Count != 0)
            {
                if(MessageBox.Show("Are you sure, you want to delete this user?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    ListViewItem lvi = lvUsers.SelectedItems[0];
                    int val = 0;
                    string userName = lvi.SubItems[0].Text;
                    string del_que = "DELETE FROM users WHERE username = '" + userName + "';";
                    SqlCommand del_com = new SqlCommand(del_que, DataConn.Connection);
                    val = del_com.ExecuteNonQuery();
                    MessageBox.Show("User has been deleted!");
                    lvUsers.Items.Clear();
                    string sql = "SELECT * FROM users INNER JOIN roles ON users.role_id = roles.id WHERE users.role_id = roles.id;";
                    SqlCommand com = new SqlCommand(sql, DataConn.Connection);
                    SqlDataReader sqlr = com.ExecuteReader();
                    while (sqlr.Read())
                    {
                        string[] user_info = { sc.ToCapitalize(sqlr["username"].ToString()), sqlr["email"].ToString(), sqlr["gender"].ToString(), sc.ToCapitalize(sqlr["name"].ToString()), sqlr["phone"].ToString(), sqlr["address"].ToString() };
                        ListViewItem item = new ListViewItem(user_info);
                        lvUsers.Items.Add(item);
                    }
                    sqlr.Close();
                    com.Dispose();
                    btnDel.Enabled = false;
                }
                else
                {
                    MessageBox.Show("No user was deleted!", "Delete", MessageBoxButtons.OK,MessageBoxIcon.Information);
                }
            }
        }
    }
}
