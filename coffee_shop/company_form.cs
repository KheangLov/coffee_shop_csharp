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
using System.Runtime.InteropServices;

namespace coffee_shop
{
    public partial class company_form : Form
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

        }

        int uId;
        string uRole;
        string uName;
        public company_form(int id, string role, string name)
        {
            uId = id;
            uRole = role;
            uName = name;
            InitializeComponent();
            m_aeroEnabled = false;
            this.FormBorderStyle = FormBorderStyle.None;
        }
        Company my_company = new Company();
        StringCapitalize sc = new StringCapitalize();
        MyInter inter;
        int companyId;

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

        private void loadComboUser()
        {
            string get_users = @"SELECT users.username, roles.name FROM users 
                INNER JOIN roles ON users.role_id = roles.id 
                WHERE LOWER(roles.name) = 'admin' AND users.id = " + uId + " AND LOWER(users.status) = 'active';";
            SqlCommand sqld = new SqlCommand(get_users, DataConn.Connection);
            SqlDataReader sqlr = sqld.ExecuteReader();
            while (sqlr.Read())
            {
                cbUser.Items.Add(sc.ToCapitalize(sqlr["username"].ToString()));
            }
            sqld.Dispose();
            sqlr.Close();
        }

        private void addCompanyUser()
        {
            string company_user = "SELECT id, username FROM users WHERE LOWER(username) = '" + cbUser.Text.Trim().ToLower() + "';";
            SqlCommand sqld = new SqlCommand(company_user, DataConn.Connection);
            SqlDataReader sqlr = sqld.ExecuteReader();
            if(sqlr.Read())
            {
                my_company.UserID = int.Parse(sqlr["id"].ToString());
            }
            else
            {
                MessageBox.Show("Nothing found!");
            }
            sqld.Dispose();
            sqlr.Close();
        }

        private void QueryCompanies()
        {
            string query = "";
            if (uRole.ToLower() == "superadmin")
            {
                query = @"SELECT companies.*, users.username FROM companies 
                    INNER JOIN users ON companies.user_id = users.id 
                    ORDER BY name;";
            }
            else
            {
                query = @"SELECT companies.*, users.username FROM companies 
                    INNER JOIN users ON companies.user_id = users.id 
                    WHERE companies.user_id = " + uId + " AND LOWER(companies.status) = 'active' ORDER BY name;";
            }
            SqlCommand sqld = new SqlCommand(query, DataConn.Connection);
            SqlDataReader sqlr = sqld.ExecuteReader();
            while (sqlr.Read())
            {
                string[] company_info = { sc.ToCapitalize(sqlr["name"].ToString()), sqlr["address"].ToString(), sqlr["email"].ToString(), sqlr["phone"].ToString(), sc.ToCapitalize(sqlr["username"].ToString()), sc.ToCapitalize(sqlr["status"].ToString()) };
                ListViewItem item = new ListViewItem(company_info);
                lvCompany.Items.Add(item);
            }
            sqld.Dispose();
            sqlr.Close();
        }

        private void company_form_Load(object sender, EventArgs e)
        {
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
            btnBan.Enabled = false;
            txtName.Focus();
            MyInter company_inter = my_company;
            inter = company_inter;
            cbStatus.SelectedIndex = 0;
            try
            {
                DataConn.Connection.Open();
                QueryCompanies();
                DataConn.Connection.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            DataConn.Connection.Open();
            loadComboUser();
            DataConn.Connection.Close();
            cbUser.SelectedItem = sc.ToCapitalize(uName);
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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if(txtName.Text != "" && txtPhone.Text != "")
                {
                    DataConn.Connection.Open();
                    string query_cpn = "SELECT COUNT(*) FROM companies WHERE name = '" + txtName.Text.Trim() + "';";
                    SqlCommand cpn_ex = new SqlCommand(query_cpn, DataConn.Connection);
                    int find_cpn = Convert.ToInt16(cpn_ex.ExecuteScalar());
                    if(find_cpn != 0)
                    {
                        MessageBox.Show("Company already existed!");
                        txtName.Focus();
                    }
                    else
                    {
                        my_company.Name = txtName.Text.Trim();
                        my_company.Address = txtAddress.Text.Trim();
                        my_company.Phone = txtPhone.Text.Trim();
                        my_company.Email = txtEmail.Text.Trim();
                        addCompanyUser();
                        my_company.Status = cbStatus.Text.Trim();
                        inter.insert();
                        MessageBox.Show("Insert successful!");
                        ClearTextBoxes(groupBox1);
                        lvCompany.Items.Clear();
                        QueryCompanies();
                    }
                    cpn_ex.Dispose();
                    DataConn.Connection.Close();
                }
                else
                {
                    MessageBox.Show("Name and Phone can't be blank!");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if(lvCompany.SelectedItems.Count != 0)
            {
                if(btnEdit.Text.ToLower() == "edit")
                {
                    DataConn.Connection.Open();
                    ListViewItem list_item = lvCompany.SelectedItems[0];
                    string name = list_item.SubItems[0].Text;
                    string sql = @"SELECT * FROM companies 
                        INNER JOIN users ON companies.user_id = users.id 
                        WHERE LOWER(companies.name) = '" + name.ToLower() + "';";
                    SqlCommand upd_cmd = new SqlCommand(sql, DataConn.Connection);
                    SqlDataReader upd_rd = upd_cmd.ExecuteReader();
                    while(upd_rd.Read())
                    {
                        companyId = int.Parse(upd_rd["id"].ToString());
                        txtName.Text = upd_rd["name"].ToString();
                        txtAddress.Text = upd_rd["address"].ToString();
                        txtEmail.Text = upd_rd["email"].ToString();
                        txtPhone.Text = upd_rd["phone"].ToString();
                        cbUser.Text = sc.ToCapitalize(upd_rd["username"].ToString());
                        cbStatus.Text = sc.ToCapitalize(upd_rd["status"].ToString());
                    }
                    upd_cmd.Dispose();
                    upd_rd.Close();
                    DataConn.Connection.Close();
                    btnEdit.Text = "Update";
                }
                else if(btnEdit.Text.ToLower() == "update")
                {
                    DataConn.Connection.Open();
                    my_company.Name = txtName.Text.Trim();
                    my_company.Address = txtAddress.Text.Trim();
                    my_company.Phone = txtPhone.Text.Trim();
                    my_company.Email = txtEmail.Text.Trim();
                    addCompanyUser();
                    my_company.Status = cbStatus.Text.Trim();
                    inter.update(companyId);
                    MessageBox.Show("Company has been updated!");
                    ClearTextBoxes(groupBox1);
                    lvCompany.Items.Clear();
                    QueryCompanies();
                    DataConn.Connection.Close();
                    cbStatus.SelectedIndex = 0;
                    btnEdit.Text = "Edit";
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if(lvCompany.SelectedItems.Count != 0)
            {
                if (MessageBox.Show("Are you sure, you want to delete this company?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    ListViewItem del_item = lvCompany.SelectedItems[0];
                    int val = 0;
                    string cpn_name = del_item.SubItems[0].Text;
                    DataConn.Connection.Open();
                    string del_sql = "DELETE FROM companies WHERE name = '" + cpn_name + "';";
                    SqlCommand del_cmd = new SqlCommand(del_sql, DataConn.Connection);
                    val = del_cmd.ExecuteNonQuery();
                    del_cmd.Dispose();
                    DataConn.Connection.Close();
                    MessageBox.Show("Company has been deleted!");
                    lvCompany.Items.Clear();
                    QueryCompanies();
                    DataConn.Connection.Close();
                    btnDelete.Enabled = false;
                    btnEdit.Enabled = false;
                    btnBan.Enabled = false;
                }
                else
                {
                    MessageBox.Show("No company was deleted!", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void lvCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvCompany.SelectedItems.Count != 0)
            {
                btnEdit.Enabled = true;
                btnDelete.Enabled = true;
                if(uRole.ToLower() == "superadmin")
                {
                    btnBan.Enabled = true;
                }
            }
            else
            {
                btnDelete.Enabled = false;
                btnEdit.Enabled = false;
                btnBan.Enabled = false;
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            lvCompany.Items.Clear();
            DataConn.Connection.Open();
            string query = @"SELECT * FROM companies 
                INNER JOIN users ON companies.user_id = users.id WHERE 
                LOWER(companies.name) LIKE '%" + txtSearch.Text.Trim() + "%' ORDER BY name;";
            SqlCommand sqld = new SqlCommand(query, DataConn.Connection);
            SqlDataReader sqlr = sqld.ExecuteReader();
            while (sqlr.Read())
            {
                string[] company_info = { sc.ToCapitalize(sqlr["name"].ToString()), sqlr["address"].ToString(), sqlr["email"].ToString(), sqlr["phone"].ToString(), sc.ToCapitalize(sqlr["username"].ToString()), sc.ToCapitalize(sqlr["status"].ToString()) };
                ListViewItem item = new ListViewItem(company_info);
                lvCompany.Items.Add(item);
            }
            sqld.Dispose();
            sqlr.Close();
            DataConn.Connection.Close();
        }

        private void company_form_FormClosing(object sender, FormClosingEventArgs e)
        {
            DataConn.Connection.Close();
            this.Dispose();
        }

        private void btnBan_Click(object sender, EventArgs e)
        {
            if(lvCompany.SelectedItems.Count > 0)
            {
                if (MessageBox.Show("Are you sure, you want to ban this company?", "Ban", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    ListViewItem del_item = lvCompany.SelectedItems[0];
                    int val = 0;
                    string cpn_name = del_item.SubItems[0].Text;
                    DataConn.Connection.Open();
                    string del_sql = "UPDATE companies SET status = 'ban' WHERE LOWER(name) = '" + cpn_name + "';";
                    SqlCommand del_cmd = new SqlCommand(del_sql, DataConn.Connection);
                    val = del_cmd.ExecuteNonQuery();
                    del_cmd.Dispose();
                    DataConn.Connection.Close();
                    MessageBox.Show("Company has been banned!");
                    lvCompany.Items.Clear();
                    DataConn.Connection.Open();
                    QueryCompanies();
                    DataConn.Connection.Close();
                    btnDelete.Enabled = false;
                    btnEdit.Enabled = false;
                    btnBan.Enabled = false;
                }
                else
                {
                    MessageBox.Show("No company was banned!", "Ban", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnExit_Click_1(object sender, EventArgs e)
        {
            DataConn.Connection.Close();
            this.Dispose();
        }
    }
}
