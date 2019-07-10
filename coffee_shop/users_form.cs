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
using System.IO;
using System.Runtime.InteropServices;

namespace coffee_shop
{
    public partial class users_form : Form
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
                    (
                        int nLeftRect, // x-coordinate of upper-left corner
                        int nTopRect, // y-coordinate of upper-left corner
                        int nRightRect, // x-coordinate of lower-right corner
                        int nBottomRect, // y-coordinate of lower-right corner
                        int nWidthEllipse, // height of ellipse
                        int nHeightEllipse // width of ellipse
                     );

        [DllImport("dwmapi.dll")]
        public static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS pMarInset);

        [DllImport("dwmapi.dll")]
        public static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        [DllImport("dwmapi.dll")]
        public static extern int DwmIsCompositionEnabled(ref int pfEnabled);

        private bool m_aeroEnabled;                     // variables for box shadow
        private const int CS_DROPSHADOW = 0x00020000;
        private const int WM_NCPAINT = 0x0085;
        private const int WM_ACTIVATEAPP = 0x001C;

        public struct MARGINS                           // struct for box shadow
        {
            public int leftWidth;
            public int rightWidth;
            public int topHeight;
            public int bottomHeight;
        }

        private const int WM_NCHITTEST = 0x84;          // variables for dragging the form
        private const int HTCLIENT = 0x1;
        private const int HTCAPTION = 0x2;

        protected override CreateParams CreateParams
        {
            get
            {
                m_aeroEnabled = CheckAeroEnabled();

                CreateParams cp = base.CreateParams;
                if (!m_aeroEnabled)
                    cp.ClassStyle |= CS_DROPSHADOW;

                return cp;
            }
        }

        private bool CheckAeroEnabled()
        {
            if (Environment.OSVersion.Version.Major >= 6)
            {
                int enabled = 0;
                DwmIsCompositionEnabled(ref enabled);
                return (enabled == 1) ? true : false;
            }
            return false;
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_NCPAINT:                        // box shadow
                    if (m_aeroEnabled)
                    {
                        var v = 2;
                        DwmSetWindowAttribute(this.Handle, 2, ref v, 4);
                        MARGINS margins = new MARGINS()
                        {
                            bottomHeight = 1,
                            leftWidth = 1,
                            rightWidth = 1,
                            topHeight = 1
                        };
                        DwmExtendFrameIntoClientArea(this.Handle, ref margins);

                    }
                    break;
                default:
                    break;
            }
            base.WndProc(ref m);

            //if (m.Msg == WM_NCHITTEST && (int)m.Result == HTCLIENT)     // drag the form
            //    m.Result = (IntPtr)HTCAPTION;
            //m_aeroEnabled = false;
            //this.FormBorderStyle = FormBorderStyle.None;

        }

        string uRole;
        public users_form(string role)
        {
            uRole = role;
            InitializeComponent();
            m_aeroEnabled = false;
            this.FormBorderStyle = FormBorderStyle.None;
        }

        Users my_user = new Users();
        HashCode hc = new HashCode();
        StringCapitalize sc = new StringCapitalize();
        MyInter inter;
        int userId;
        string userPass;
        bool deleted = false;
        bool banned = false;
        string proImg = "";
        string path = "";

        private void lvUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvUsers.SelectedItems.Count != 0)
            {
                btnEdit.Enabled = true;
                btnDel.Enabled = true;
                btnPassword.Enabled = true;
                btnBan.Enabled = true;
            }
            else
            {
                btnDel.Enabled = false;
                btnEdit.Enabled = false;
                btnPassword.Enabled = false;
                btnBan.Enabled = false;
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
                string[] user_info = { sc.ToCapitalize(sqlr["username"].ToString()), sqlr["email"].ToString(), sqlr["gender"].ToString(), sc.ToCapitalize(sqlr["name"].ToString()), sqlr["phone"].ToString(), sqlr["address"].ToString(), sc.ToCapitalize(sqlr["status"].ToString()) };
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
                    if (type.ToLower() == "delete")
                    {
                        MessageBox.Show("You can't delete application owner!");
                        btnDel.Enabled = false;
                    }
                    else if (type.ToLower() == "edit")
                    {
                        MessageBox.Show("You can't edit application owner!");
                        btnEdit.Enabled = false;
                    }
                    else if (type.ToLower() == "ban")
                    {
                        MessageBox.Show("You can't ban application owner!");
                        btnBan.Enabled = false;
                    }
                }
                else
                {
                    if (type.ToLower() == "delete")
                    {
                        deleted = true;
                    }
                    else if (type.ToLower() == "edit")
                    {
                        userId = int.Parse(reader["id"].ToString());
                        txtFirstname.Text = reader["firstname"].ToString();
                        txtLastname.Text = reader["lastname"].ToString();
                        txtEmail.Text = reader["email"].ToString();
                        cbGender.Text = reader["gender"].ToString();
                        txtPhone.Text = reader["phone"].ToString();
                        txtAddress.Text = reader["address"].ToString();
                        cbRole.Text = sc.ToCapitalize(reader["name"].ToString());
                        pbProfile.ImageLocation = reader["image"].ToString();
                        cbStatus.Text = sc.ToCapitalize(reader["status"].ToString());
                        btnEdit.Text = "Update";
                    }
                    else if (type.ToLower() == "ban")
                    {
                        banned = true;
                    }
                }
            }
            command.Dispose();
            reader.Close();
            if (deleted == true)
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
                    btnBan.Enabled = false;
                }
                else
                {
                    MessageBox.Show("No user was deleted!", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnDel.Enabled = false;
                }
                deleted = false;
            }
            else if (banned == true)
            {
                if (MessageBox.Show("Are you sure, you want to ban this user?", "Ban", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    ListViewItem lvi = lvUsers.SelectedItems[0];
                    int val = 0;
                    string userName = lvi.SubItems[0].Text;
                    string del_que = "UPDATE users SET status = 'ban' WHERE LOWER(username) = '" + userName.ToLower() + "';";
                    SqlCommand del_com = new SqlCommand(del_que, DataConn.Connection);
                    val = del_com.ExecuteNonQuery();
                    del_com.Dispose();
                    MessageBox.Show("User has been banned!");
                    lvUsers.Items.Clear();
                    QueryUsers();
                    btnDel.Enabled = false;
                    btnEdit.Enabled = false;
                    btnPassword.Enabled = false;
                    btnBan.Enabled = false;
                }
                else
                {
                    MessageBox.Show("No user was banned!", "Ban", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnBan.Enabled = false;
                }
            }
        }

        private void users_form_load(object sender, EventArgs e)
        {
            btnEdit.Enabled = false;
            btnDel.Enabled = false;
            btnPassword.Enabled = false;
            btnBan.Enabled = false;
            MyInter user_inter = my_user;
            inter = user_inter;
            DataConn.Connection.Open();
            loadComboRole();
            DataConn.Connection.Close();
            cbGender.SelectedIndex = 0;
            cbStatus.SelectedIndex = 0;
            if(cbRole.Items.Count > 0)
                cbRole.SelectedIndex = 0;
            try
            {
                DataConn.Connection.Open();
                QueryUsers();
                DataConn.Connection.Close();
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
                    DataConn.Connection.Open();
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
                        my_user.Created_Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        my_user.Updated_Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        if(cbStatus.Text.ToLower() == "active" || cbStatus.Text.ToLower() == "inactive" || cbStatus.Text.ToLower() == "ban")
                        {
                            my_user.Status = cbStatus.Text.ToLower().Trim();
                        }
                        my_user.Image = path;
                        addComboRole();
                        inter.insert();
                        MessageBox.Show("Insert Successfully!");
                        ClearTextBoxes(groupBox1);
                        lvUsers.Items.Clear();
                        QueryUsers();
                        cbGender.SelectedIndex = 0;
                        cbRole.SelectedIndex = 0;
                        cbStatus.SelectedIndex = 0;
                    }
                    check_com.Dispose();
                    DataConn.Connection.Close();
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

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if(lvUsers.SelectedItems.Count != 0)
            {
                
                if (btnEdit.Text.ToLower() == "edit")
                {
                    btnDel.Enabled = false;
                    btnPassword.Enabled = false;
                    txtPassword.Enabled = false;
                    btnBan.Enabled = false;
                    txtConfirmPass.Enabled = false;
                    DataConn.Connection.Open();
                    CheckUserRole("edit");
                    DataConn.Connection.Close();
                }
                else if(btnEdit.Text.ToLower() == "update")
                {
                    try
                    {
                        my_user.Firstname = txtFirstname.Text.Trim();
                        my_user.Lastname = txtLastname.Text.Trim();
                        my_user.Username = my_user.Firstname + my_user.Lastname;
                        DataConn.Connection.Open();
                        string sql_dup_user = "SELECT COUNT(*) FROM users WHERE LOWER(username) = '" + my_user.Username + "' AND id != " + userId + ";";
                        SqlCommand dup_sqld = new SqlCommand(sql_dup_user, DataConn.Connection);
                        int count_dup_user = Convert.ToInt16(dup_sqld.ExecuteScalar());
                        dup_sqld.Dispose();
                        DataConn.Connection.Close();
                        if (count_dup_user != 0)
                        {
                            MessageBox.Show("User already existed");
                            ClearTextBoxes(groupBox1);
                            txtFirstname.Focus();
                            btnEdit.Text = "Edit";
                            btnEdit.Enabled = false;
                            txtPassword.Enabled = true;
                            txtConfirmPass.Enabled = true;
                            cbGender.SelectedIndex = 0;
                            cbRole.SelectedIndex = 0;
                            cbStatus.SelectedIndex = 0;
                        }
                        else
                        {
                            my_user.Email = txtEmail.Text.Trim();
                            my_user.Gender = cbGender.Text.Trim();
                            my_user.Password = hc.PassHash(txtPassword.Text.Trim());
                            my_user.Phone = txtPhone.Text.Trim();
                            my_user.Address = txtAddress.Text.Trim();
                            my_user.Updated_Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            if (cbStatus.Text.ToLower() == "active" || cbStatus.Text.ToLower() == "inactive" || cbStatus.Text.ToLower() == "ban")
                            {
                                my_user.Status = cbStatus.Text.ToLower().Trim();
                            }
                            my_user.Image = path;
                            DataConn.Connection.Open();
                            addComboRole();
                            inter.update(userId);
                            MessageBox.Show("Update Successfully!");
                            ClearTextBoxes(groupBox1);
                            lvUsers.Items.Clear();
                            QueryUsers();
                            DataConn.Connection.Close();
                            cbGender.SelectedIndex = 0;
                            cbRole.SelectedIndex = 0;
                            cbStatus.SelectedIndex = 0;
                            btnEdit.Text = "Edit";
                            btnEdit.Enabled = false;
                            txtPassword.Enabled = true;
                            txtConfirmPass.Enabled = true;
                        }
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
                DataConn.Connection.Open();
                CheckUserRole("delete");
                DataConn.Connection.Close();
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            lvUsers.Items.Clear();
            DataConn.Connection.Open();
            string search_query = @"SELECT * FROM users 
                INNER JOIN roles ON users.role_id = roles.id 
                WHERE users.role_id = roles.id AND LOWER(username) LIKE '%" + txtSearch.Text.Trim().ToLower() + "%' ORDER BY roles.id;";
            SqlCommand srh_cmd = new SqlCommand(search_query, DataConn.Connection);
            SqlDataReader srh_rd = srh_cmd.ExecuteReader();
            while (srh_rd.Read())
            {
                string[] user_info = { sc.ToCapitalize(srh_rd["username"].ToString()), srh_rd["email"].ToString(), srh_rd["gender"].ToString(), sc.ToCapitalize(srh_rd["name"].ToString()), srh_rd["phone"].ToString(), srh_rd["address"].ToString(), sc.ToCapitalize(srh_rd["status"].ToString()) };
                ListViewItem item = new ListViewItem(user_info);
                lvUsers.Items.Add(item);
            }
            srh_cmd.Dispose();
            srh_rd.Close();
            DataConn.Connection.Close();
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
                    DataConn.Connection.Open();
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
                    DataConn.Connection.Close();
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

        private void txtFirstname_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((Char.IsControl(e.KeyChar) != true && Char.IsNumber(e.KeyChar) == true) || Char.IsPunctuation(e.KeyChar) == true)
            {
                e.Handled = true;
            }
        }

        private void txtLastname_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((Char.IsControl(e.KeyChar) != true && Char.IsNumber(e.KeyChar) == true) || Char.IsPunctuation(e.KeyChar) == true)
            {
                e.Handled = true;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DataConn.Connection.Close();
            this.Dispose();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                var parent = Directory.GetParent(Directory.GetCurrentDirectory()).Parent;
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.Filter = "All Files (*.*)|*.*|JPG Files (*.jpg)|*.jpg|PNG Files (*.png)|*.png";
                dlg.Title = "Select Product Picture.";
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    string file = Path.GetFileName(dlg.FileName);
                    path = parent.FullName + @"\pictures\profiles\" + file;
                    proImg = dlg.FileName.ToString();
                    MoveImage(proImg, path);
                    pbProfile.ImageLocation = proImg;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void MoveImage(string source, string path)
        {
            try
            {
                File.Copy(source, path);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnBan_Click(object sender, EventArgs e)
        {
            if (lvUsers.SelectedItems.Count != 0)
            {
                DataConn.Connection.Open();
                CheckUserRole("ban");
                DataConn.Connection.Close();
            }
        }

        private void lvUsers_DoubleClick(object sender, EventArgs e)
        {
            ListViewItem userDetail = lvUsers.SelectedItems[0];
            string name = userDetail.SubItems[0].Text;
            DataConn.Connection.Close();
            new user_detail(name).ShowDialog();
        }
    }
}
