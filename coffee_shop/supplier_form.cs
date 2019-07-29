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
    public partial class supplier_form : Form
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

        string comId;
        public supplier_form(string com_id)
        {
            comId = com_id;
            InitializeComponent();
            m_aeroEnabled = false;
            this.FormBorderStyle = FormBorderStyle.None;
        }
        Supplier my_supplier = new Supplier();
        MyInter inter;
        StringCapitalize sc = new StringCapitalize();
        int supplierId;

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

        private void QuerySupplier()
        {
            string sql = @"SELECT suppliers.*, companies.name AS company_name, branches.name AS branch_name
                FROM suppliers
                INNER JOIN companies ON suppliers.company_id = companies.id
                INNER JOIN branches ON suppliers.branch_id = branches.id
                WHERE companies.id IN (" + comId + ")" +
                "AND LOWER(companies.status) = 'active' AND LOWER(branches.status) = 'active';";
            SqlCommand com = new SqlCommand(sql, DataConn.Connection);
            SqlDataReader sqlr = com.ExecuteReader();
            while (sqlr.Read())
            {
                string[] supplier_info = { sqlr["name"].ToString(), sqlr["address"].ToString(), sqlr["phone"].ToString(), sqlr["email"].ToString(), sqlr["company_name"].ToString(), sqlr["branch_name"].ToString() };
                ListViewItem item = new ListViewItem(supplier_info);
                lvSupplier.Items.Add(item);
            }
            com.Dispose();
            sqlr.Close();
        }

        private void loadCombos(string tblName)
        {
            string sql = "";
            if (tblName.ToLower() == "companies")
                sql = "SELECT * FROM " + tblName.ToLower() + " WHERE id IN (" + comId + ");";
            else
                sql = "SELECT * FROM " + tblName.ToLower() + " WHERE company_id IN (" + comId + ");";
            SqlCommand sqld = new SqlCommand(sql, DataConn.Connection);
            SqlDataReader sqlr = sqld.ExecuteReader();
            while (sqlr.Read())
            {
                if (tblName.ToLower() == "companies")
                    cbCompany.Items.Add(sc.ToCapitalize(sqlr["name"].ToString()));
                else if (tblName.ToLower() == "branches")
                    cbBranch.Items.Add(sc.ToCapitalize(sqlr["name"].ToString()));
            }
            sqlr.Close();
            sqld.Dispose();
        }

        private void addCombos(string tblName)
        {
            string sql = "";
            if (tblName.ToLower() == "companies")
                sql = "SELECT * FROM " + tblName.ToLower() + " WHERE LOWER(name) = '" + cbCompany.Text.Trim() + "';";
            else if (tblName.ToLower() == "branches")
                sql = "SELECT * FROM " + tblName.ToLower() + " WHERE LOWER(name) = '" + cbBranch.Text.Trim() + "';";
            SqlCommand sqld = new SqlCommand(sql, DataConn.Connection);
            SqlDataReader sqlr = sqld.ExecuteReader();
            if (sqlr.Read())
            {
                if (tblName.ToLower() == "companies")
                    my_supplier.CompanyId = int.Parse(sqlr["id"].ToString());
                else if (tblName.ToLower() == "branches")
                    my_supplier.BranchId = int.Parse(sqlr["id"].ToString());
            }
            sqlr.Close();
            sqld.Dispose();
        }

        private void supplier_form_Load(object sender, EventArgs e)
        {
            try
            {
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
                DataConn.Connection.Open();
                MyInter supplier_inter = my_supplier;
                inter = supplier_inter;
                QuerySupplier();
                loadCombos("companies");
                loadCombos("branches");
                cbCompany.SelectedIndex = 0;
                cbBranch.SelectedIndex = 0;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DataConn.Connection.Close();
            this.Dispose();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            lvSupplier.Items.Clear();
            string search_query = @"SELECT suppliers.*, companies.name AS company_name, branches.name AS branch_name
                            FROM suppliers
                            INNER JOIN companies ON suppliers.company_id = companies.id
                            INNER JOIN branches ON suppliers.branch_id = branches.id
                            WHERE companies.id IN (" + comId + ") AND suppliers.name LIKE '%" + txtSearch.Text.Trim().ToLower() + "%';";
            SqlCommand srh_cmd = new SqlCommand(search_query, DataConn.Connection);
            SqlDataReader srh_rd = srh_cmd.ExecuteReader();
            while (srh_rd.Read())
            {
                string[] supplier_info = { srh_rd["name"].ToString(), srh_rd["address"].ToString(), srh_rd["phone"].ToString(), srh_rd["email"].ToString(), srh_rd["company_name"].ToString(), srh_rd["branch_name"].ToString() };
                ListViewItem item = new ListViewItem(supplier_info);
                lvSupplier.Items.Add(item);
            }
            srh_cmd.Dispose();
            srh_rd.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtName.Text != "" && txtAddress.Text != "" && txtPhone.Text != "")
                {
                    string query_name = "SELECT COUNT(*) FROM suppliers WHERE LOWER(name) = '" + txtName.Text.ToLower() + "';";
                    SqlCommand check_name = new SqlCommand(query_name, DataConn.Connection);
                    int nName = Convert.ToInt16(check_name.ExecuteScalar());
                    if (nName != 0)
                    {
                        MessageBox.Show("Supplier already exist!");
                        txtName.Text = "";
                        txtName.Focus();
                    }
                    else
                    {
                        my_supplier.Name = txtName.Text.Trim();
                        my_supplier.Address = txtAddress.Text.Trim();
                        my_supplier.Phone = txtPhone.Text.Trim();
                        my_supplier.Email = txtEmail.Text.Trim();
                        addCombos("companies");
                        addCombos("branches");
                        inter.insert();
                        MessageBox.Show("Insert successfully!");
                        ClearTextBoxes(groupBox1);
                        txtName.Focus();
                        lvSupplier.Items.Clear();
                        QuerySupplier();
                    }
                }
                else
                {
                    MessageBox.Show("Name, Address or Phone can't be blank!");
                    txtName.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void lvSupplier_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvSupplier.SelectedItems.Count != 0)
            {
                btnEdit.Enabled = true;
                btnDelete.Enabled = true;
            }
            else
            {
                btnDelete.Enabled = false;
                btnEdit.Enabled = false;
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if(lvSupplier.SelectedItems.Count > 0)
            {
                btnDelete.Enabled = false;
                if (btnEdit.Text.ToLower() == "edit")
                {
                    ListViewItem list_item = lvSupplier.SelectedItems[0];
                    string name = list_item.SubItems[0].Text;
                    string sql = @"SELECT suppliers.*, companies.name AS company_name, branches.name AS branch_name
                                FROM suppliers
                                INNER JOIN companies ON suppliers.company_id = companies.id
                                INNER JOIN branches ON suppliers.branch_id = branches.id
                                WHERE companies.id IN (" + comId + ") AND LOWER(suppliers.name) = '" + name.ToLower() + "';";
                    SqlCommand upd_cmd = new SqlCommand(sql, DataConn.Connection);
                    SqlDataReader upd_rd = upd_cmd.ExecuteReader();
                    if (upd_rd.Read())
                    {
                        supplierId = int.Parse(upd_rd["id"].ToString());
                        txtName.Text = upd_rd["name"].ToString();
                        txtAddress.Text = upd_rd["address"].ToString();
                        txtPhone.Text = upd_rd["phone"].ToString();
                        txtEmail.Text = upd_rd["email"].ToString();
                        cbCompany.SelectedItem = upd_rd["company_name"].ToString();
                        cbBranch.SelectedItem = upd_rd["branch_name"].ToString();
                    }
                    upd_cmd.Dispose();
                    upd_rd.Close();
                    btnEdit.Text = "Update";
                }
                else if (btnEdit.Text.ToLower() == "update")
                {
                    my_supplier.Name = txtName.Text.Trim();
                    my_supplier.Address = txtAddress.Text.Trim();
                    my_supplier.Phone = txtPhone.Text.Trim();
                    my_supplier.Email = txtEmail.Text.Trim();
                    addCombos("companies");
                    addCombos("branches");
                    inter.update(supplierId);
                    MessageBox.Show("Supplier has been updated!");
                    ClearTextBoxes(groupBox1);
                    lvSupplier.Items.Clear();
                    QuerySupplier();
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lvSupplier.SelectedItems.Count != 0)
            {
                btnEdit.Enabled = false;
                if (MessageBox.Show("Are you sure, you want to delete this supplier?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    ListViewItem del_item = lvSupplier.SelectedItems[0];
                    int val = 0;
                    string name = del_item.SubItems[0].Text;
                    string del_sql = "DELETE FROM suppliers WHERE LOWER(name) = '" + name.ToLower() + "';";
                    SqlCommand del_cmd = new SqlCommand(del_sql, DataConn.Connection);
                    val = del_cmd.ExecuteNonQuery();
                    del_cmd.Dispose();
                    MessageBox.Show("Supplier has been deleted!");
                    lvSupplier.Items.Clear();
                    QuerySupplier();
                    btnDelete.Enabled = false;
                }
                else
                {
                    MessageBox.Show("No supplier was deleted!", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnDelete.Enabled = false;
                }
            }
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

        private void btnExit_Click_1(object sender, EventArgs e)
        {
            DataConn.Connection.Close();
            this.Dispose();
        }
    }
}
