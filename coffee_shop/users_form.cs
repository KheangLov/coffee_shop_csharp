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
using System.Net.Mail;

namespace coffee_shop
{
    public partial class users_form : Form
    {
        string uRole;
        public users_form(string role)
        {
            uRole = role;
            InitializeComponent();
        }

        Users my_user = new Users();
        HashCode hc = new HashCode();
        StringCapitalize sc = new StringCapitalize();
        MyInter inter;
        int userId;
        string userPass;
        bool deleted = false;

        private void lvUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvUsers.SelectedItems.Count != 0)
            {
                btnEdit.Enabled = true;
                btnDel.Enabled = true;
                btnPassword.Enabled = true;
            }
            else
            {
                btnDel.Enabled = false;
                btnEdit.Enabled = false;
                btnPassword.Enabled = false;
            }
        }

        bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private void QueryUsers()
        {
            string sql = "SELECT * FROM users INNER JOIN roles ON users.role_id = roles.id WHERE users.role_id = roles.id ORDER BY roles.id;";
            SqlCommand com = new SqlCommand(sql, DataConn.Connection);
            SqlDataReader sqlr = com.ExecuteReader();
            while (sqlr.Read())
            {
                string[] user_info = { sc.ToCapitalize(sqlr["username"].ToString()), sqlr["email"].ToString(), sqlr["gender"].ToString(), sc.ToCapitalize(sqlr["name"].ToString()), sqlr["phone"].ToString(), sqlr["address"].ToString() };
                ListViewItem item = new ListViewItem(user_info);
                lvUsers.Items.Add(item);
            }
            com.Dispose();
            sqlr.Close();
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

        private void loadComboRole()
        {
            string sql = "";
            if (uRole.ToLower() == "superadmin")
                sql = "SELECT * FROM roles WHERE LOWER(name) IN ('admin', 'editor', 'user');";
            else
                sql = "SELECT * FROM roles WHERE LOWER(name) IN ('editor', 'user');";
            SqlCommand sqld = new SqlCommand(sql, DataConn.Connection);
            SqlDataReader sqlr = sqld.ExecuteReader();
            while(sqlr.Read())
            {
                cbRole.Items.Add(sc.ToCapitalize(sqlr["name"].ToString()));
            }
            sqld.Dispose();
            sqlr.Close();
        }

        private void CheckUserRole(string type = "")
        {
            ListViewItem item = lvUsers.SelectedItems[0];
            string lv_username = item.SubItems[0].Text;
            string sql = "SELECT * FROM users INNER JOIN roles ON users.role_id = roles.id WHERE LOWER(username) = '" + lv_username.ToLower() + "';";
            SqlCommand command = new SqlCommand(sql, DataConn.Connection);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                if (reader["name"].ToString().ToLower() == "superadmin")
                {
                    if(type.ToLower() == "delete")
                    {
                        MessageBox.Show("You can't delete application owner!");
                        btnDel.Enabled = false;
                    }
                    else if (type.ToLower() == "edit")
                    {
                        MessageBox.Show("You can't edit application owner!");
                        btnEdit.Enabled = false;
                    }
                }
                else
                {
                    if (type.ToLower() == "delete")
                    {
                        deleted = true;
                    }
                    else if(type.ToLower() == "edit")
                    {
                        userId = int.Parse(reader["id"].ToString());
                        txtFirstname.Text = reader["firstname"].ToString();
                        txtLastname.Text = reader["lastname"].ToString();
                        txtEmail.Text = reader["email"].ToString();
                        cbGender.Text = reader["gender"].ToString();
                        txtPhone.Text = reader["phone"].ToString();
                        txtAddress.Text = reader["address"].ToString();
                        cbRole.Text = sc.ToCapitalize(reader["name"].ToString());
                        btnEdit.Text = "Update";
                    }
                }
            }
            command.Dispose();
            reader.Close();
            if(deleted == true)
            {
                if (MessageBox.Show("Are you sure, you want to delete this user?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    ListViewItem lvi = lvUsers.SelectedItems[0];
                    int val = 0;
                    string userName = lvi.SubItems[0].Text;
                    string del_que = "DELETE FROM users WHERE username = '" + userName + "';";
                    SqlCommand del_com = new SqlCommand(del_que, DataConn.Connection);
                    val = del_com.ExecuteNonQuery();
                    del_com.Dispose();
                    MessageBox.Show("User has been deleted!");
                    lvUsers.Items.Clear();
                    QueryUsers();
                    btnDel.Enabled = false;
                    btnEdit.Enabled = false;
                    btnPassword.Enabled = false;
                }
                else
                {
                    MessageBox.Show("No user was deleted!", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnDel.Enabled = false;
                }
                deleted = false;
            }

        }

        private void users_form_load(object sender, EventArgs e)
        {
            btnEdit.Enabled = false;
            btnDel.Enabled = false;
            btnPassword.Enabled = false;
            DataConn.Connection.Open();
            MyInter user_inter = my_user;
            inter = user_inter;
            loadComboRole();
            cbGender.SelectedIndex = 0;
            if(cbRole.Items.Count > 0)
                cbRole.SelectedIndex = 0;
            try
            {
                QueryUsers();
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
                if (txtFirstname.Text != "" && txtLastname.Text != "" && txtEmail.Text != "" && txtPassword.Text != "")
                {
                    string my_user_name = txtFirstname.Text + txtLastname.Text;
                    string check_name = "SELECT COUNT(*) FROM users WHERE LOWER(username) = '" + my_user_name.ToLower() + "';";
                    SqlCommand check_com = new SqlCommand(check_name, DataConn.Connection);
                    int find_user = Convert.ToInt16(check_com.ExecuteScalar());
                    if (find_user != 0)
                    {
                        MessageBox.Show("User already exist!");
                        txtFirstname.Focus();
                    }
                    else
                    {
                        my_user.Firstname = txtFirstname.Text.Trim();
                        my_user.Lastname = txtLastname.Text.Trim();
                        my_user.Username = my_user.Firstname + my_user.Lastname;
                        my_user.Email = txtEmail.Text.Trim();
                        my_user.Gender = cbGender.Text.Trim();
                        my_user.Password = hc.PassHash(txtPassword.Text.Trim());
                        my_user.Phone = txtPhone.Text.Trim();
                        my_user.Address = txtAddress.Text.Trim();
                        my_user.Created_Date = DateTime.Now.ToString("yyyy-MM-dd");
                        addComboRole();
                        inter.insert();
                        MessageBox.Show("Insert Successfully!");
                        ClearTextBoxes(groupBox1);
                        lvUsers.Items.Clear();
                        QueryUsers();
                    }
                    check_com.Dispose();
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

        private void addComboRole()
        {
            if (cbRole.Text.ToLower() != "superadmin")
            {
                string sql = "SELECT * FROM roles WHERE LOWER(name) = '" + cbRole.Text.ToLower() + "';";
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
            else
            {
                MessageBox.Show("Wrong role selection!");
            }
        }

        private void txtEmail_leave(object sender, EventArgs e)
        {
            if (!IsValidEmail(txtEmail.Text.Trim()))
            {
                MessageBox.Show("Invalid Email!");
                txtEmail.Text = "";
                txtEmail.Focus();
            }
        }

        private void users_form_closing(object sender, FormClosingEventArgs e)
        {
            DataConn.Connection.Close();
            this.Dispose();
        }

        private void txtConfirmPass_leave(object sender, EventArgs e)
        {
            if (txtPassword.Text != "")
            {
                string pass = txtPassword.Text.Trim();
                string pass_con = txtConfirmPass.Text.Trim();
                if (pass != pass_con)
                {
                    MessageBox.Show("Wrong confirmation password!");
                    txtConfirmPass.Focus();
                }
            }
            else
            {
                MessageBox.Show("Please insert password first!");
                txtPassword.Focus();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DataConn.Connection.Close();
            this.Dispose();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if(lvUsers.SelectedItems.Count != 0)
            {
                if(btnEdit.Text.ToLower() == "edit")
                {
                    btnDel.Enabled = false;
                    btnPassword.Enabled = false;
                    txtPassword.Enabled = false;
                    txtConfirmPass.Enabled = false;
                    CheckUserRole("edit");
                }
                else if(btnEdit.Text.ToLower() == "update")
                {
                    try
                    {
                        my_user.Firstname = txtFirstname.Text.Trim();
                        my_user.Lastname = txtLastname.Text.Trim();
                        my_user.Username = my_user.Firstname + my_user.Lastname;
                        my_user.Email = txtEmail.Text.Trim();
                        my_user.Gender = cbGender.Text.Trim();
                        my_user.Password = hc.PassHash(txtPassword.Text.Trim());
                        my_user.Phone = txtPhone.Text.Trim();
                        my_user.Address = txtAddress.Text.Trim();
                        my_user.Created_Date = DateTime.Now.ToString("yyyy-MM-dd");
                        addComboRole();
                        inter.update(userId);
                        MessageBox.Show("Update Successfully!");
                        ClearTextBoxes(groupBox1);
                        lvUsers.Items.Clear();
                        QueryUsers();
                        btnEdit.Text = "Edit";
                        btnEdit.Enabled = false;
                        txtPassword.Enabled = true;
                        txtConfirmPass.Enabled = true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if(lvUsers.SelectedItems.Count != 0)
            {
                CheckUserRole("delete");
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            lvUsers.Items.Clear();
            string search_query = "SELECT * FROM users INNER JOIN roles ON users.role_id = roles.id WHERE users.role_id = roles.id AND LOWER(username) LIKE '%" + txtSearch.Text.Trim().ToLower() + "%' ORDER BY roles.id;";
            SqlCommand srh_cmd = new SqlCommand(search_query, DataConn.Connection);
            SqlDataReader srh_rd = srh_cmd.ExecuteReader();
            while (srh_rd.Read())
            {
                string[] user_info = { sc.ToCapitalize(srh_rd["username"].ToString()), srh_rd["email"].ToString(), srh_rd["gender"].ToString(), sc.ToCapitalize(srh_rd["name"].ToString()), srh_rd["phone"].ToString(), srh_rd["address"].ToString() };
                ListViewItem item = new ListViewItem(user_info);
                lvUsers.Items.Add(item);
            }
            srh_cmd.Dispose();
            srh_rd.Close();
        }

        private void btnPassword_Click(object sender, EventArgs e)
        {
            if(lvUsers.SelectedItems.Count != 0)
            {
                btnDel.Enabled = false;
                btnEdit.Enabled = false;
                ListViewItem list_item = lvUsers.SelectedItems[0];
                string name = list_item.SubItems[0].Text;
                string role = list_item.SubItems[3].Text;
                if(role.ToLower() != "superadmin")
                {
                    string pass_que = "SELECT id, password FROM users WHERE username = '" + name + "';";
                    SqlCommand pas_cmd = new SqlCommand(pass_que, DataConn.Connection);
                    SqlDataReader pas_rd = pas_cmd.ExecuteReader();
                    if (pas_rd.Read())
                    {
                        userId = int.Parse(pas_rd["id"].ToString());
                        userPass = pas_rd["password"].ToString();
                    }
                    pas_cmd.Dispose();
                    pas_rd.Close();
                    btnPassword.Enabled = false;
                    new change_password(userPass, userId).ShowDialog();
                }
                else
                {
                    MessageBox.Show("You can't change application owner's password!");
                    btnPassword.Enabled = false;
                }
            }
        }

        private void txtPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }
    }
}
