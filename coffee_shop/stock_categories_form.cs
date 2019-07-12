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
    public partial class stock_categories_form : Form
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
        public stock_categories_form(string role)
        {
            uRole = role;
            InitializeComponent();
            m_aeroEnabled = false;
            this.FormBorderStyle = FormBorderStyle.None;
        }
        StockCategory my_stockcate = new StockCategory();
        MyInter inter;
        StringCapitalize sc = new StringCapitalize();
        int stockCateId;

        private void QueryStockCate()
        {
            DataConn.Connection.Open();
            string sql = "SELECT * FROM stock_categories;";
            SqlCommand com = new SqlCommand(sql, DataConn.Connection);
            SqlDataReader sqlr = com.ExecuteReader();
            while (sqlr.Read())
            {
                string[] stock_cate_info = { sc.ToCapitalize(sqlr["name"].ToString()), sqlr["descriptions"].ToString() };
                ListViewItem item = new ListViewItem(stock_cate_info);
                lvStockCate.Items.Add(item);
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

        private void stock_categories_form_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Dispose();
        }

        private void stock_categories_form_Load(object sender, EventArgs e)
        {
            if (uRole.ToLower() == "user")
                btnAdd.Enabled = false;
            else
                btnAdd.Enabled = true;
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
            MyInter stockcat_inter = my_stockcate;
            inter = stockcat_inter;
            QueryStockCate();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtName.Text != "")
                {
                    DataConn.Connection.Open();
                    string query_name = "SELECT COUNT(*) FROM stock_categories WHERE LOWER(name) = '" + txtName.Text.ToLower() + "';";
                    SqlCommand check_name = new SqlCommand(query_name, DataConn.Connection);
                    int nName = Convert.ToInt16(check_name.ExecuteScalar());
                    check_name.Dispose();
                    DataConn.Connection.Close();
                    if (nName != 0)
                    {
                        MessageBox.Show("Stock category already exist!");
                        txtName.Text = "";
                        txtName.Focus();
                    }
                    else
                    {
                        my_stockcate.Name = txtName.Text.Trim();
                        my_stockcate.Descriptions = txtDesc.Text.Trim();
                        DataConn.Connection.Open();
                        inter.insert();
                        DataConn.Connection.Close();
                        MessageBox.Show("Insert successful!");
                        ClearTextBoxes(groupBox1);
                        lvStockCate.Items.Clear();
                        QueryStockCate();
                    }
                }
                else
                {
                    MessageBox.Show("Name can't be blank!");
                    txtName.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void lvStockCate_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvStockCate.SelectedItems.Count != 0 && uRole.ToLower() != "user")
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
            if (lvStockCate.SelectedItems.Count != 0)
            {
                btnDelete.Enabled = false;
                if (btnEdit.Text.ToLower() == "edit")
                {
                    DataConn.Connection.Open();
                    ListViewItem list_item = lvStockCate.SelectedItems[0];
                    string name = list_item.SubItems[0].Text;
                    string sql = @"SELECT * FROM stock_categories WHERE LOWER(name) = '" + name.ToLower() + "';";
                    SqlCommand upd_cmd = new SqlCommand(sql, DataConn.Connection);
                    SqlDataReader upd_rd = upd_cmd.ExecuteReader();
                    while (upd_rd.Read())
                    {
                        stockCateId = int.Parse(upd_rd["id"].ToString());
                        txtName.Text = upd_rd["name"].ToString();
                        txtDesc.Text = upd_rd["descriptions"].ToString();
                    }
                    upd_cmd.Dispose();
                    upd_rd.Close();
                    DataConn.Connection.Close();
                    btnEdit.Text = "Update";
                }
                else if (btnEdit.Text.ToLower() == "update")
                {
                    my_stockcate.Name = txtName.Text.Trim();
                    my_stockcate.Descriptions = txtDesc.Text.Trim();
                    DataConn.Connection.Open();
                    inter.update(stockCateId);
                    DataConn.Connection.Close();
                    MessageBox.Show("Stock Categories has been updated!");
                    ClearTextBoxes(groupBox1);
                    lvStockCate.Items.Clear();
                    QueryStockCate();
                    btnEdit.Text = "Edit";
                    btnEdit.Enabled = false;
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lvStockCate.SelectedItems.Count != 0)
            {
                btnEdit.Enabled = false;
                if (MessageBox.Show("Are you sure, you want to delete this stock category?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    DataConn.Connection.Open();
                    ListViewItem del_item = lvStockCate.SelectedItems[0];
                    int val = 0;
                    string name = del_item.SubItems[0].Text;
                    string del_sql = "DELETE FROM stock_categories WHERE LOWER(name) = '" + name.ToLower() + "';";
                    SqlCommand del_cmd = new SqlCommand(del_sql, DataConn.Connection);
                    val = del_cmd.ExecuteNonQuery();
                    del_cmd.Dispose();
                    DataConn.Connection.Close();
                    MessageBox.Show("Stock Categories has been deleted!");
                    lvStockCate.Items.Clear();
                    QueryStockCate();
                    btnDelete.Enabled = false;
                }
                else
                {
                    MessageBox.Show("No stock category was deleted!", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnDelete.Enabled = false;
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DataConn.Connection.Close();
            this.Dispose();
        }
    }
}
