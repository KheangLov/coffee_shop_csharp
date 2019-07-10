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
    public partial class product_category_form : Form
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

        string uRole;
        public product_category_form(string role)
        {
            uRole = role;
            InitializeComponent();
            m_aeroEnabled = false;
            this.FormBorderStyle = FormBorderStyle.None;
        }
        ProductCategory procate = new ProductCategory();
        MyInter inter;
        StringCapitalize sc = new StringCapitalize();
        int proCateId;

        private void QueryProCate()
        {
            DataConn.Connection.Open();
            string sql = @"SELECT * FROM product_categories;";
            SqlCommand com = new SqlCommand(sql, DataConn.Connection);
            SqlDataReader sqlr = com.ExecuteReader();
            while (sqlr.Read())
            {
                string[] pro_cate_info = { sc.ToCapitalize(sqlr["name"].ToString()), sqlr["descriptions"].ToString() };
                ListViewItem item = new ListViewItem(pro_cate_info);
                lvProCate.Items.Add(item);
            }
            com.Dispose();
            sqlr.Close();
            DataConn.Connection.Close();
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

        private void add_product_category_form_Load(object sender, EventArgs e)
        {
            if (uRole.ToLower() == "user")
                btnAdd.Enabled = false;
            else
                btnAdd.Enabled = true;
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
            MyInter procate_inter = procate;
            inter = procate_inter;
            QueryProCate();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtName.Text != "")
                {
                    DataConn.Connection.Open();
                    string query_name = "SELECT COUNT(*) FROM product_categories WHERE LOWER(name) = '" + txtName.Text.ToLower() + "';";
                    SqlCommand check_name = new SqlCommand(query_name, DataConn.Connection);
                    int nName = Convert.ToInt16(check_name.ExecuteScalar());
                    check_name.Dispose();
                    DataConn.Connection.Close();
                    if (nName != 0)
                    {
                        MessageBox.Show("Product Category already exist!");
                        txtName.Text = "";
                        txtName.Focus();
                    }
                    else
                    {
                        procate.Name = txtName.Text.Trim();
                        procate.Descriptions = txtDescriptions.Text.Trim();
                        DataConn.Connection.Open();
                        inter.insert();
                        DataConn.Connection.Close();
                        MessageBox.Show("Insert succcessfully");
                        ClearTextBoxes(gpAddproductCategory);
                        txtName.Focus();
                        lvProCate.Items.Clear();
                        QueryProCate();
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void lvProCate_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvProCate.SelectedItems.Count != 0 && uRole.ToLower() == "user")
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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lvProCate.SelectedItems.Count != 0)
            {
                btnEdit.Enabled = false;
                if (MessageBox.Show("Are you sure, you want to delete this product category?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    DataConn.Connection.Open();
                    ListViewItem del_item = lvProCate.SelectedItems[0];
                    int val = 0;
                    string name = del_item.SubItems[0].Text;
                    string del_sql = "DELETE FROM product_categories WHERE LOWER(name) = '" + name.ToLower() + "';";
                    SqlCommand del_cmd = new SqlCommand(del_sql, DataConn.Connection);
                    val = del_cmd.ExecuteNonQuery();
                    del_cmd.Dispose();
                    DataConn.Connection.Close();
                    MessageBox.Show("Product category has been deleted!");
                    lvProCate.Items.Clear();
                    QueryProCate();
                    btnDelete.Enabled = false;
                }
                else
                {
                    MessageBox.Show("No product category was deleted!", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnDelete.Enabled = false;
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (lvProCate.SelectedItems.Count != 0)
            {
                btnDelete.Enabled = false;
                if (btnEdit.Text.ToLower() == "edit")
                {
                    DataConn.Connection.Open();
                    ListViewItem list_item = lvProCate.SelectedItems[0];
                    string name = list_item.SubItems[0].Text;
                    string sql = @"SELECT * FROM product_categories;";
                    SqlCommand upd_cmd = new SqlCommand(sql, DataConn.Connection);
                    SqlDataReader upd_rd = upd_cmd.ExecuteReader();
                    while (upd_rd.Read())
                    {
                        proCateId = int.Parse(upd_rd["id"].ToString());
                        txtName.Text = upd_rd["name"].ToString();
                        txtDescriptions.Text = upd_rd["descriptions"].ToString();
                    }
                    upd_cmd.Dispose();
                    upd_rd.Close();
                    DataConn.Connection.Close();
                    btnEdit.Text = "Update";
                }
                else if (btnEdit.Text.ToLower() == "update")
                {
                    procate.Name = txtName.Text.Trim();
                    procate.Descriptions = txtDescriptions.Text.Trim();
                    DataConn.Connection.Open();
                    inter.update(proCateId);
                    DataConn.Connection.Close();
                    MessageBox.Show("Product Categories has been updated!");
                    ClearTextBoxes(gpAddproductCategory);
                    lvProCate.Items.Clear();
                    QueryProCate();
                    btnEdit.Text = "Edit";
                    btnEdit.Enabled = false;
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
