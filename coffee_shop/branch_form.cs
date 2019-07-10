using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace coffee_shop
{
    public partial class branch_form : Form
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
        public branch_form(int id, string role)
        {
            uId = id;
            uRole = role;
            InitializeComponent();
            m_aeroEnabled = false;
            this.FormBorderStyle = FormBorderStyle.None;
        }
        Branch my_branch = new Branch();
        MyInter inter;
        StringCapitalize sc = new StringCapitalize();
        int branchId;

        private void QueryBranches(string byCompany)
        {
            string query = "";
            if (byCompany == "all")
            {
                string company_names = "";
                string get_ucompany = "";
                if(uRole.ToLower() == "superadmin")
                    get_ucompany = "SELECT name FROM companies WHERE LOWER(status) = 'active';";
                else
                    get_ucompany = "SELECT name FROM companies WHERE LOWER(status) = 'active' AND user_id = " + uId + ";";
                SqlCommand ucomd = new SqlCommand(get_ucompany, DataConn.Connection);
                SqlDataReader ucomr = ucomd.ExecuteReader();
                int i = 0;
                while (ucomr.Read())
                {
                    if (i == 0)
                        company_names += "'" + ucomr["name"].ToString().ToLower() + "'";
                    else
                        company_names += ", '" + ucomr["name"].ToString().ToLower() + "'";
                    i++;
                }
                ucomd.Dispose();
                ucomr.Close();
                query = @"SELECT branches.*, companies.name AS company_name FROM branches
                        INNER JOIN companies ON branches.company_id = companies.id
                        WHERE LOWER(companies.name) IN (" + company_names + ");";
            }
            else
            {
                query = @"SELECT branches.*, companies.name AS company_name FROM branches
                        INNER JOIN companies ON branches.company_id = companies.id
                        WHERE LOWER(companies.name) = '" + byCompany + "';";
            }
            SqlCommand sqld = new SqlCommand(query, DataConn.Connection);
            SqlDataReader sqlr = sqld.ExecuteReader();
            while (sqlr.Read())
            {
                string[] branch_info = {
                    sc.ToCapitalize(sqlr["name"].ToString()),
                    sqlr["email"].ToString(),
                    sqlr["phone"].ToString(),
                    sqlr["address"].ToString(),
                    sc.ToCapitalize(sqlr["company_name"].ToString()),
                    sc.ToCapitalize(sqlr["status"].ToString())
                };
                ListViewItem item = new ListViewItem(branch_info);
                lvBranch.Items.Add(item);
            }
            sqld.Dispose();
            sqlr.Close();
        }

        private void loadComboCompany()
        {
            string sql = "";
            if (uRole.ToLower() == "superadmin")
                sql = "SELECT id, name FROM companies WHERE LOWER(status) = 'active';";
            else
                sql = "SELECT id, name FROM companies WHERE LOWER(status) = 'active' AND user_id = " + uId + ";";
            SqlCommand sqld = new SqlCommand(sql, DataConn.Connection);
            SqlDataReader sqlr = sqld.ExecuteReader();
            while (sqlr.Read())
            {
                cbByCompany.Items.Add(sc.ToCapitalize(sqlr["name"].ToString()));
                cbCompany.Items.Add(sc.ToCapitalize(sqlr["name"].ToString()));
            }
            sqlr.Close();
            sqld.Dispose();
        }

        private void addCompany()
        {
            string sql = "SELECT * FROM companies WHERE LOWER(name) = '" + cbCompany.Text.ToLower() + "';";
            SqlCommand sqld = new SqlCommand(sql, DataConn.Connection);
            SqlDataReader sqlr = sqld.ExecuteReader();
            if (sqlr.Read())
            {
                my_branch.CompanyId = int.Parse(sqlr["id"].ToString());
            }
            else
            {
                MessageBox.Show("Nothing found!");
            }
            sqlr.Close();
            sqld.Dispose();
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

        private void branch_form_Load(object sender, EventArgs e)
        {
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
            btnBan.Enabled = false;
            MyInter branch_inter = my_branch;
            inter = branch_inter;
            DataConn.Connection.Open();
            loadComboCompany();
            DataConn.Connection.Close();
            cbCompany.SelectedIndex = 0;
            cbByCompany.SelectedIndex = 0;
            cbStaus.SelectedIndex = 0;
            lvBranch.Items.Clear();
            DataConn.Connection.Open();
            QueryBranches(cbByCompany.Text.ToLower());
            DataConn.Connection.Close();
        }

        private void txtEmail_Leave(object sender, EventArgs e)
        {
            if (!IsValidEmail(txtEmail.Text.Trim()))
            {
                MessageBox.Show("Invalid Email!");
                txtEmail.Text = "";
                txtEmail.Focus();
            }
        }

        private void lvBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvBranch.SelectedItems.Count != 0)
            {
                btnEdit.Enabled = true;
                btnDelete.Enabled = true;
                btnBan.Enabled = true;
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
            lvBranch.Items.Clear();
            DataConn.Connection.Open();
            string sql = "";
            if (cbByCompany.Text.ToLower() == "all")
            {
                sql = @"SELECT branches.*, companies.name AS company_name FROM branches
                        INNER JOIN companies ON branches.company_id = companies.id
                        WHERE branches.name LIKE '%" + txtSearch.Text.Trim() + "%'; ";
            }
            else
            {
                sql = @"SELECT branches.*, companies.name AS company_name FROM branches
                        INNER JOIN companies ON branches.company_id = companies.id
                        WHERE LOWER(companies.name) = '" + cbByCompany.Text.Trim().ToLower() + "' " +
                        "AND branches.name LIKE '%" + txtSearch.Text.Trim() + "%'; ";
            }
            SqlCommand sqld = new SqlCommand(sql, DataConn.Connection);
            SqlDataReader sqlr = sqld.ExecuteReader();
            while(sqlr.Read())
            {
                string[] branch_info = {
                    sc.ToCapitalize(sqlr["name"].ToString()),
                    sqlr["email"].ToString(),
                    sqlr["phone"].ToString(),
                    sqlr["address"].ToString(),
                    sc.ToCapitalize(sqlr["company_name"].ToString()),
                    sc.ToCapitalize(sqlr["status"].ToString()) };
                ListViewItem item = new ListViewItem(branch_info);
                lvBranch.Items.Add(item);
            }
            sqld.Dispose();
            sqlr.Close();
            DataConn.Connection.Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lvBranch.SelectedItems.Count != 0)
            {
                if (MessageBox.Show("Are you sure, you want to delete this branch?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    ListViewItem del_item = lvBranch.SelectedItems[0];
                    int val = 0;
                    string name = del_item.SubItems[0].Text;
                    DataConn.Connection.Open();
                    string del_sql = "DELETE FROM branches WHERE LOWER(name) = '" + name.ToLower() + "';";
                    SqlCommand del_cmd = new SqlCommand(del_sql, DataConn.Connection);
                    val = del_cmd.ExecuteNonQuery();
                    del_cmd.Dispose();
                    MessageBox.Show("Branch has been deleted!");
                    lvBranch.Items.Clear();
                    QueryBranches(cbByCompany.Text.ToLower());
                    DataConn.Connection.Close();
                }
                else
                {
                    MessageBox.Show("No branch was deleted!", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtName.Text != "" && txtAddress.Text != "")
                {
                    DataConn.Connection.Open();
                    my_branch.Name = txtName.Text.Trim();
                    my_branch.Address = txtAddress.Text.Trim();
                    my_branch.Phone = txtPhone.Text.Trim();
                    my_branch.Status = cbStaus.Text.Trim();
                    my_branch.Email = txtEmail.Text.Trim();
                    addCompany();
                    inter.insert();
                    MessageBox.Show("Insert Successfully!");
                    ClearTextBoxes(groupBox1);
                    txtName.Focus();
                    lvBranch.Items.Clear();
                    QueryBranches(cbByCompany.Text.ToLower());
                    DataConn.Connection.Close();
                    cbCompany.SelectedIndex = 0;
                    cbByCompany.SelectedIndex = 0;
                    cbStaus.SelectedIndex = 0;
                }
                else
                {
                    MessageBox.Show("Name and Address can't be blank!");
                    txtName.Focus();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if(lvBranch.SelectedItems.Count != 0)
            {
                if (btnEdit.Text.ToLower() == "edit")
                {
                    ListViewItem list_item = lvBranch.SelectedItems[0];
                    string name = list_item.SubItems[0].Text;
                    DataConn.Connection.Open();
                    string sql = @"SELECT branches.*, companies.name AS company_name FROM branches 
                        INNER JOIN companies ON branches.company_id = companies.id 
                        WHERE LOWER(branches.name) = '" + name.ToLower() + "';";
                    SqlCommand upd_cmd = new SqlCommand(sql, DataConn.Connection);
                    SqlDataReader upd_rd = upd_cmd.ExecuteReader();
                    while (upd_rd.Read())
                    {
                        branchId = int.Parse(upd_rd["id"].ToString());
                        txtName.Text = upd_rd["name"].ToString();
                        txtAddress.Text = upd_rd["address"].ToString();
                        txtEmail.Text = upd_rd["email"].ToString();
                        txtPhone.Text = upd_rd["phone"].ToString();
                        cbCompany.Text = sc.ToCapitalize(upd_rd["company_name"].ToString());
                        cbStaus.Text = sc.ToCapitalize(upd_rd["status"].ToString());
                    }
                    upd_cmd.Dispose();
                    upd_rd.Close();
                    DataConn.Connection.Close();
                    btnEdit.Text = "Update";
                }
                else if (btnEdit.Text.ToLower() == "update")
                {
                    DataConn.Connection.Open();
                    my_branch.Name = txtName.Text.Trim();
                    my_branch.Address = txtAddress.Text.Trim();
                    my_branch.Phone = txtPhone.Text.Trim();
                    my_branch.Status = cbStaus.Text.Trim();
                    my_branch.Email = txtEmail.Text.Trim();
                    addCompany();
                    inter.update(branchId);
                    MessageBox.Show("Branch has been updated!");
                    ClearTextBoxes(groupBox1);
                    lvBranch.Items.Clear();
                    QueryBranches(cbByCompany.Text.ToLower());
                    DataConn.Connection.Close();
                    cbCompany.SelectedIndex = 0;
                    cbByCompany.SelectedIndex = 0;
                    cbStaus.SelectedIndex = 0;
                }
            }
        }

        private void cbByCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            lvBranch.Items.Clear();
            DataConn.Connection.Open();
            QueryBranches(cbByCompany.Text.ToLower());
            DataConn.Connection.Close();
        }

        private void branch_form_FormClosing(object sender, FormClosingEventArgs e)
        {
            DataConn.Connection.Close();
            this.Dispose();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DataConn.Connection.Close();
            this.Dispose();
        }
    }
}
