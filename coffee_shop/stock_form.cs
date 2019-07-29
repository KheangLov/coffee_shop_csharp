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
    public partial class stock_form : Form
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
        string uRole;
        public stock_form(string com_id, string role)
        {
            comId = com_id;
            uRole = role;
            InitializeComponent();
            m_aeroEnabled = false;
            this.FormBorderStyle = FormBorderStyle.None;
        }
        Stock my_stock = new Stock();
        StringCapitalize sc = new StringCapitalize();
        MyInter inter;
        int stockId;
        int cId;

        private bool alreadyExist(string _text, ref char KeyChar)
        {
            if (_text.IndexOf('.') > -1)
            {
                KeyChar = '.';
                return true;
            }
            if (_text.IndexOf(',') > -1)
            {
                KeyChar = ',';
                return true;
            }
            return false;
        }

        private void Querystocks()
        {
            DataConn.Connection.Open();
            string sql = @"SELECT stocks.*, stock_categories.name AS stock_name , companies.name AS company_name, branches.name AS branch_name
                        FROM stocks
                        INNER JOIN stock_categories ON stocks.stockcate_id = stock_categories.id
                        INNER JOIN companies ON stocks.company_id = companies.id
                        INNER JOIN branches ON stocks.branch_id = branches.id
                        WHERE stocks.company_id IN (" + comId + ")" +
                        "AND LOWER(companies.status) = 'active' AND LOWER(branches.status) = 'active';";
            SqlCommand com = new SqlCommand(sql, DataConn.Connection);
            SqlDataReader sqlr = com.ExecuteReader();
            while (sqlr.Read())
            {
                string[] stock_info = { sc.ToCapitalize(sqlr["name"].ToString()), sqlr["expired_date"].ToString(), sqlr["qty"].ToString(), sc.ToCapitalize(sqlr["price"].ToString()), sqlr["selling_price"].ToString(), sqlr["alert_qty"].ToString(), sqlr["stock_name"].ToString(), sc.ToCapitalize(sqlr["company_name"].ToString()), sc.ToCapitalize(sqlr["branch_name"].ToString()) };
                ListViewItem item = new ListViewItem(stock_info);
                lvStocks.Items.Add(item);
            }
            com.Dispose();
            sqlr.Close();
            DataConn.Connection.Close();
        }

        private void loadComboStockCate()
        {
            DataConn.Connection.Open();
            string sql = "SELECT * FROM stock_categories;";
            SqlCommand sqld = new SqlCommand(sql, DataConn.Connection);
            SqlDataReader sqlr = sqld.ExecuteReader();
            while (sqlr.Read())
            {
                cbstkcate.Items.Add(sc.ToCapitalize(sqlr["name"].ToString()));
            }
            sqlr.Close();
            sqld.Dispose();
            DataConn.Connection.Close();
        }

        private void loadComboCompany()
        {
            DataConn.Connection.Open();
            string sql = "SELECT * FROM companies WHERE id IN (" + comId + ");";
            SqlCommand sqld = new SqlCommand(sql, DataConn.Connection);
            SqlDataReader sqlr = sqld.ExecuteReader();
            while (sqlr.Read())
            {
                cbCompany.Items.Add(sc.ToCapitalize(sqlr["name"].ToString()));
                cbByCompany.Items.Add(sc.ToCapitalize(sqlr["name"].ToString()));
            }
            sqlr.Close();
            sqld.Dispose();
            DataConn.Connection.Close();
        }

        private void loadComboBranch()
        {
            DataConn.Connection.Open();
            string sql = "SELECT * FROM branches WHERE company_id IN (" + comId + ");";
            SqlCommand sqld = new SqlCommand(sql, DataConn.Connection);
            SqlDataReader sqlr = sqld.ExecuteReader();
            while (sqlr.Read())
            {
                cbBranch.Items.Add(sc.ToCapitalize(sqlr["name"].ToString()));
            }
            sqlr.Close();
            sqld.Dispose();
            DataConn.Connection.Close();
        }

        private void loadComboByBranch()
        {
            DataConn.Connection.Open();
            string sql = "";
            if (cbByCompany.SelectedItem.ToString().ToLower() == "all")
                sql = "SELECT * FROM branches WHERE company_id IN (" + comId + ");";
            else
                sql = "SELECT * FROM branches WHERE company_id = " + cId + ";";
            SqlCommand sqld = new SqlCommand(sql, DataConn.Connection);
            SqlDataReader sqlr = sqld.ExecuteReader();
            while (sqlr.Read())
            {
                cbByBranch.Items.Add(sc.ToCapitalize(sqlr["name"].ToString()));
            }
            sqlr.Close();
            sqld.Dispose();
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

        private void addCombostockcat()
        {
            DataConn.Connection.Open();
            string sql = "SELECT * FROM stock_categories WHERE LOWER(name) = '" + cbstkcate.Text.ToLower() + "';";
            SqlCommand sqld = new SqlCommand(sql, DataConn.Connection);
            SqlDataReader sqlr = sqld.ExecuteReader();
            if (sqlr.Read())
            {
                my_stock.StockCateId = int.Parse(sqlr["id"].ToString());
            }
            else
            {
                MessageBox.Show("Nothing found!");
            }
            sqlr.Close();
            sqld.Dispose();
            DataConn.Connection.Close();
        }

        private void addComboCompany()
        {
            DataConn.Connection.Open();
            string sql = "SELECT * FROM companies WHERE LOWER(name) = '" + cbCompany.Text.ToLower() + "';";
            SqlCommand sqld = new SqlCommand(sql, DataConn.Connection);
            SqlDataReader sqlr = sqld.ExecuteReader();
            if (sqlr.Read())
            {
                my_stock.CompanyId = int.Parse(sqlr["id"].ToString());
            }
            else
            {
                MessageBox.Show("Nothing found!");
            }
            sqlr.Close();
            sqld.Dispose();
            DataConn.Connection.Close();
        }

        private void addComboBranch()
        {
            DataConn.Connection.Open();
            string sql = "SELECT * FROM branches WHERE LOWER(name) = '" + cbBranch.Text.ToLower() + "';";
            SqlCommand sqld = new SqlCommand(sql, DataConn.Connection);
            SqlDataReader sqlr = sqld.ExecuteReader();
            if (sqlr.Read())
            {
                my_stock.BranchId = int.Parse(sqlr["id"].ToString());
            }
            else
            {
                MessageBox.Show("Nothing found!");
            }
            sqlr.Close();
            sqld.Dispose();
            DataConn.Connection.Close();
        }

        private void getByCompany()
        {
            DataConn.Connection.Open();
            string sql = "SELECT * FROM companies WHERE LOWER(name) = '" + cbByCompany.Text.ToLower() + "';";
            SqlCommand sqld = new SqlCommand(sql, DataConn.Connection);
            SqlDataReader sqlr = sqld.ExecuteReader();
            if (sqlr.Read())
            {
                cId = int.Parse(sqlr["id"].ToString());
            }
            sqld.Dispose();
            sqlr.Close();
            DataConn.Connection.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtname.Text != "")
                {
                    DataConn.Connection.Open();
                    string query_name = "SELECT COUNT(*) FROM stocks WHERE LOWER(name) = '" + txtname.Text.ToLower() + "';";
                    SqlCommand check_name = new SqlCommand(query_name, DataConn.Connection);
                    int nName = Convert.ToInt16(check_name.ExecuteScalar());
                    if (nName != 0)
                    {
                        MessageBox.Show("Stock already exist!");
                        DataConn.Connection.Close();
                        txtname.Text = "";
                        txtname.Focus();
                    }
                    else
                    {
                        DataConn.Connection.Close();
                        my_stock.Name = txtname.Text.Trim();
                        my_stock.ImportedDate = DateTime.Now.ToString("yyyy-MM-dd");
                        my_stock.ExpiredDate = DateTime.Parse(dtpExp.Text).ToString("yyyy-MM-dd");
                        my_stock.Quantity = decimal.Parse(txtqty.Text);
                        my_stock.Price = decimal.Parse(txtprice.Text);
                        my_stock.SellingPrice = decimal.Parse(txtsellingprice.Text);
                        my_stock.AlertedQuantity = decimal.Parse(txtaltqty.Text);
                        addCombostockcat();
                        addComboCompany();
                        addComboBranch();
                        if (my_stock.AlertedQuantity >= my_stock.Quantity)
                        {
                            my_stock.Alerted = 1;
                        }
                        else
                        {
                            my_stock.Alerted = 0;
                        }
                        DataConn.Connection.Open();
                        inter.insert();
                        DataConn.Connection.Close();
                        MessageBox.Show("Insert successfully!");
                        ClearTextBoxes(groupBox1);
                        txtname.Focus();
                        lvStocks.Items.Clear();
                        Querystocks();
                    }
                    DataConn.Connection.Close();
                }
                else
                {
                    MessageBox.Show("Name can't be blank!");
                    txtname.Focus();
                }
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

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (lvStocks.SelectedItems.Count != 0)
            {
                btnDel.Enabled = false;
                if (btnEdit.Text.ToLower() == "edit")
                {
                    DataConn.Connection.Open();
                    ListViewItem list_item = lvStocks.SelectedItems[0];
                    string name = list_item.SubItems[0].Text;
                    string sql = @"SELECT stocks.*, stock_categories.name AS stock_name, companies.name AS company_name, branches.name AS branch_name FROM stocks 
                        INNER JOIN stock_categories ON stocks.stockcate_id = stock_categories.id 
                        INNER JOIN companies ON stocks.company_id = companies.id
                        INNER JOIN branches ON stocks.branch_id = branches.id
                        WHERE stocks.company_id IN (" + comId + ") AND LOWER(stocks.name) = '" + name.ToLower() + "';";
                    SqlCommand upd_cmd = new SqlCommand(sql, DataConn.Connection);
                    SqlDataReader upd_rd = upd_cmd.ExecuteReader();
                    while (upd_rd.Read())
                    {
                        stockId = int.Parse(upd_rd["id"].ToString());
                        txtname.Text = upd_rd["name"].ToString();
                        dtpExp.Text = upd_rd["expired_date"].ToString();
                        txtqty.Text = upd_rd["qty"].ToString();
                        txtprice.Text = upd_rd["price"].ToString();
                        txtsellingprice.Text = upd_rd["selling_price"].ToString();
                        txtaltqty.Text = upd_rd["alert_qty"].ToString();
                        cbstkcate.SelectedItem = upd_rd["stock_name"].ToString();
                        cbCompany.SelectedItem = upd_rd["company_name"].ToString();
                        cbBranch.SelectedItem = upd_rd["branch_name"].ToString();
                    }
                    upd_cmd.Dispose();
                    upd_rd.Close();
                    DataConn.Connection.Close();
                    btnEdit.Text = "Update";
                }
                else if (btnEdit.Text.ToLower() == "update")
                {
                    my_stock.Name = txtname.Text;
                    my_stock.ImportedDate = DateTime.Now.ToString("yyyy-MM-dd");
                    my_stock.ExpiredDate = DateTime.Parse(dtpExp.Text).ToString("yyyy-MM-dd");
                    my_stock.Quantity = decimal.Parse(txtqty.Text);
                    my_stock.Price = decimal.Parse(txtprice.Text);
                    my_stock.SellingPrice = decimal.Parse(txtsellingprice.Text);
                    my_stock.AlertedQuantity = decimal.Parse(txtaltqty.Text);
                    addCombostockcat();
                    addComboCompany();
                    addComboBranch();
                    if (my_stock.AlertedQuantity >= my_stock.Quantity)
                    {
                        my_stock.Alerted = 1;
                    }
                    else
                    {
                        my_stock.Alerted = 0;
                    }
                    DataConn.Connection.Open();
                    inter.update(stockId);
                    DataConn.Connection.Close();
                    MessageBox.Show("Stock has been updated!");
                    ClearTextBoxes(groupBox1);
                    lvStocks.Items.Clear();
                    Querystocks();
                    btnEdit.Text = "Edit";
                    btnEdit.Enabled = false;
                }
            }
        }

        private void lvStocks_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvStocks.SelectedItems.Count != 0 && uRole.ToLower() != "user")
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

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (lvStocks.SelectedItems.Count != 0)
            {
                btnEdit.Enabled = false;
                if (MessageBox.Show("Are you sure, you want to delete this stocks?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    ListViewItem del_item = lvStocks.SelectedItems[0];
                    int val = 0;
                    string name = del_item.SubItems[0].Text;
                    DataConn.Connection.Open();
                    string del_sql = "DELETE FROM stocks WHERE LOWER(name) = '" + name.ToLower() + "';";
                    SqlCommand del_cmd = new SqlCommand(del_sql, DataConn.Connection);
                    val = del_cmd.ExecuteNonQuery();
                    del_cmd.Dispose();
                    MessageBox.Show("Stock has been deleted!");
                    lvStocks.Items.Clear();
                    Querystocks();
                    DataConn.Connection.Close();
                    btnDel.Enabled = false;
                }
                else
                {
                    MessageBox.Show("No stock was deleted!", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnDel.Enabled = false;
                }
            }
        }

        private void stockform_load(object sender, EventArgs e)
        {
            if (uRole.ToLower() == "user")
                btnAdd.Enabled = false;
            else
                btnAdd.Enabled = true;
            btnEdit.Enabled = false;
            btnDel.Enabled = false;
            MyInter stock_inter = my_stock;
            inter = stock_inter;
            loadComboStockCate();
            if (cbstkcate.Items.Count > 0)
                cbstkcate.SelectedIndex = 0;
            
            loadComboCompany();
            
            cbCompany.SelectedIndex = 0;
            cbByCompany.SelectedIndex = 0;
            
            getByCompany();
            
            
            loadComboBranch();
            
            cbBranch.SelectedIndex = 0;
            cbByBranch.Items.Clear();
            cbByBranch.Items.Add("All");
            
            loadComboByBranch();
            
            if (cbByBranch.Items.Count > 0)
                cbByBranch.SelectedIndex = 0;
            lvStocks.Items.Clear();
            
            Querystocks();
            
        }

        private void stock_form_FormClosing(object sender, FormClosingEventArgs e)
        {
            DataConn.Connection.Close();
            this.Dispose();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            lvStocks.Items.Clear();
            DataConn.Connection.Open();
            string search_query = @"SELECT stocks.*, stock_categories.name AS stock_name , companies.name AS company_name, branches.name AS branch_name
                            FROM stocks
                            INNER JOIN stock_categories ON stocks.stockcate_id = stock_categories.id
                            INNER JOIN companies ON stocks.company_id = companies.id
                            INNER JOIN branches ON stocks.branch_id = branches.id
                            WHERE stocks.company_id IN (" + comId + ") AND stocks.name LIKE '%" + txtSearch.Text.Trim().ToLower() + "%';";
            SqlCommand srh_cmd = new SqlCommand(search_query, DataConn.Connection);
            SqlDataReader srh_rd = srh_cmd.ExecuteReader();
            while (srh_rd.Read())
            {
                string[] stock_info = { sc.ToCapitalize(srh_rd["name"].ToString()), srh_rd["expired_date"].ToString(), srh_rd["qty"].ToString(), sc.ToCapitalize(srh_rd["price"].ToString()), srh_rd["selling_price"].ToString(), srh_rd["alert_qty"].ToString(), srh_rd["stock_name"].ToString(), sc.ToCapitalize(srh_rd["company_name"].ToString()), sc.ToCapitalize(srh_rd["branch_name"].ToString()) };
                ListViewItem item = new ListViewItem(stock_info);
                lvStocks.Items.Add(item);
            }
            srh_cmd.Dispose();
            srh_rd.Close();
            DataConn.Connection.Close();
        }

        private void cbByCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            getByCompany();
            cbByBranch.Items.Clear();
            cbByBranch.Items.Add("All");
            loadComboByBranch();
            if (cbByBranch.Items.Count > 0)
                cbByBranch.SelectedIndex = 0;
            else
                cbByBranch.Text = "";
            string sql = "";
            if(cbByCompany.SelectedItem.ToString().ToLower() == "all" && cbByBranch.SelectedItem.ToString().ToLower() == "all")
            {
                sql = @"SELECT stocks.*, stock_categories.name AS stock_name , companies.name AS company_name, branches.name AS branch_name
                        FROM stocks
                        INNER JOIN stock_categories ON stocks.stockcate_id = stock_categories.id
                        INNER JOIN companies ON stocks.company_id = companies.id
                        INNER JOIN branches ON stocks.branch_id = branches.id
                        WHERE stocks.company_id IN (" + comId + ");";
            }
            else if(cbByCompany.SelectedItem.ToString().ToLower() != "all" && cbByBranch.SelectedItem.ToString().ToLower() == "all")
            {
                sql = @"SELECT stocks.*, stock_categories.name AS stock_name , companies.name AS company_name, branches.name AS branch_name
                        FROM stocks
                        INNER JOIN stock_categories ON stocks.stockcate_id = stock_categories.id
                        INNER JOIN companies ON stocks.company_id = companies.id
                        INNER JOIN branches ON stocks.branch_id = branches.id
                        WHERE LOWER(companies.name) = '" + cbByCompany.Text.ToLower() + "';";
            }
            else
            {
                sql = @"SELECT stocks.*, stock_categories.name AS stock_name , companies.name AS company_name, branches.name AS branch_name
                        FROM stocks
                        INNER JOIN stock_categories ON stocks.stockcate_id = stock_categories.id
                        INNER JOIN companies ON stocks.company_id = companies.id
                        INNER JOIN branches ON stocks.branch_id = branches.id
                        WHERE LOWER(companies.name) = '" + cbByCompany.Text.ToLower() + "' AND LOWER(branches.name) = '" + cbByBranch.Text.ToLower() + "';";
            }
            DataConn.Connection.Open();
            SqlCommand com = new SqlCommand(sql, DataConn.Connection);
            SqlDataReader sqlr = com.ExecuteReader();
            lvStocks.Items.Clear();
            while (sqlr.Read())
            {
                string[] stock_info = { sc.ToCapitalize(sqlr["name"].ToString()), sqlr["expired_date"].ToString(), sqlr["qty"].ToString(), sc.ToCapitalize(sqlr["price"].ToString()), sqlr["selling_price"].ToString(), sqlr["alert_qty"].ToString(), sqlr["stock_name"].ToString(), sc.ToCapitalize(sqlr["company_name"].ToString()), sc.ToCapitalize(sqlr["branch_name"].ToString()) };
                ListViewItem item = new ListViewItem(stock_info);
                lvStocks.Items.Add(item);
            }
            com.Dispose();
            sqlr.Close();
            DataConn.Connection.Close();
        }

        private void cbByBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataConn.Connection.Open();
            string sql = "";
            if (cbByCompany.SelectedItem.ToString().ToLower() == "all" && cbByBranch.SelectedItem.ToString().ToLower() == "all")
            {
                sql = @"SELECT stocks.*, stock_categories.name AS stock_name , companies.name AS company_name, branches.name AS branch_name
                        FROM stocks
                        INNER JOIN stock_categories ON stocks.stockcate_id = stock_categories.id
                        INNER JOIN companies ON stocks.company_id = companies.id
                        INNER JOIN branches ON stocks.branch_id = branches.id
                        WHERE stocks.company_id IN (" + comId + ");";
            }
            else if (cbByCompany.SelectedItem.ToString().ToLower() != "all" && cbByBranch.SelectedItem.ToString().ToLower() == "all")
            {
                sql = @"SELECT stocks.*, stock_categories.name AS stock_name , companies.name AS company_name, branches.name AS branch_name
                        FROM stocks
                        INNER JOIN stock_categories ON stocks.stockcate_id = stock_categories.id
                        INNER JOIN companies ON stocks.company_id = companies.id
                        INNER JOIN branches ON stocks.branch_id = branches.id
                        WHERE LOWER(companies.name) = '" + cbByCompany.Text.ToLower() + "';";
            }
            else
            {
                sql = @"SELECT stocks.*, stock_categories.name AS stock_name , companies.name AS company_name, branches.name AS branch_name
                        FROM stocks
                        INNER JOIN stock_categories ON stocks.stockcate_id = stock_categories.id
                        INNER JOIN companies ON stocks.company_id = companies.id
                        INNER JOIN branches ON stocks.branch_id = branches.id
                        WHERE LOWER(companies.name) = '" + cbByCompany.Text.ToLower() + "' AND LOWER(branches.name) = '" + cbByBranch.Text.ToLower() + "';";
            }
            SqlCommand com = new SqlCommand(sql, DataConn.Connection);
            SqlDataReader sqlr = com.ExecuteReader();
            lvStocks.Items.Clear();
            while (sqlr.Read())
            {
                string[] stock_info = { sc.ToCapitalize(sqlr["name"].ToString()), sqlr["expired_date"].ToString(), sqlr["qty"].ToString(), sc.ToCapitalize(sqlr["price"].ToString()), sqlr["selling_price"].ToString(), sqlr["alert_qty"].ToString(), sqlr["stock_name"].ToString(), sc.ToCapitalize(sqlr["company_name"].ToString()), sc.ToCapitalize(sqlr["branch_name"].ToString()) };
                ListViewItem item = new ListViewItem(stock_info);
                lvStocks.Items.Add(item);
            }
            com.Dispose();
            sqlr.Close();
            DataConn.Connection.Close();
        }

        private void txtqty_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.' && e.KeyChar != ',')
            {
                e.Handled = true;
            }

            //check if '.' , ',' pressed
            char sepratorChar = 's';
            if (e.KeyChar == '.' || e.KeyChar == ',')
            {
                // check if it's in the beginning of text not accept
                if (txtqty.Text.Length == 0) e.Handled = true;
                // check if it's in the beginning of text not accept
                if (txtqty.SelectionStart == 0) e.Handled = true;
                // check if there is already exist a '.' , ','
                if (alreadyExist(txtqty.Text, ref sepratorChar)) e.Handled = true;
                //check if '.' or ',' is in middle of a number and after it is not a number greater than 99
                if (txtqty.SelectionStart != txtqty.Text.Length && e.Handled == false)
                {
                    // '.' or ',' is in the middle
                    string AfterDotString = txtqty.Text.Substring(txtqty.SelectionStart);

                    if (AfterDotString.Length > 2)
                    {
                        e.Handled = true;
                    }
                }
            }
            //check if a number pressed

            if (Char.IsDigit(e.KeyChar))
            {
                //check if a coma or dot exist
                if (alreadyExist(txtqty.Text, ref sepratorChar))
                {
                    int sepratorPosition = txtqty.Text.IndexOf(sepratorChar);
                    string afterSepratorString = txtqty.Text.Substring(sepratorPosition + 1);
                    if (txtqty.SelectionStart > sepratorPosition && afterSepratorString.Length > 1)
                    {
                        e.Handled = true;
                    }

                }
            }
        }

        private void txtprice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.' && e.KeyChar != ',')
            {
                e.Handled = true;
            }

            //check if '.' , ',' pressed
            char sepratorChar = 's';
            if (e.KeyChar == '.' || e.KeyChar == ',')
            {
                // check if it's in the beginning of text not accept
                if (txtprice.Text.Length == 0) e.Handled = true;
                // check if it's in the beginning of text not accept
                if (txtprice.SelectionStart == 0) e.Handled = true;
                // check if there is already exist a '.' , ','
                if (alreadyExist(txtprice.Text, ref sepratorChar)) e.Handled = true;
                //check if '.' or ',' is in middle of a number and after it is not a number greater than 99
                if (txtprice.SelectionStart != txtprice.Text.Length && e.Handled == false)
                {
                    // '.' or ',' is in the middle
                    string AfterDotString = txtprice.Text.Substring(txtprice.SelectionStart);

                    if (AfterDotString.Length > 2)
                    {
                        e.Handled = true;
                    }
                }
            }
            //check if a number pressed

            if (Char.IsDigit(e.KeyChar))
            {
                //check if a coma or dot exist
                if (alreadyExist(txtprice.Text, ref sepratorChar))
                {
                    int sepratorPosition = txtprice.Text.IndexOf(sepratorChar);
                    string afterSepratorString = txtprice.Text.Substring(sepratorPosition + 1);
                    if (txtprice.SelectionStart > sepratorPosition && afterSepratorString.Length > 1)
                    {
                        e.Handled = true;
                    }

                }
            }
        }

        private void txtsellingprice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar)
                    && !char.IsDigit(e.KeyChar)
                    && e.KeyChar != '.' && e.KeyChar != ',')
            {
                e.Handled = true;
            }

            //check if '.' , ',' pressed
            char sepratorChar = 's';
            if (e.KeyChar == '.' || e.KeyChar == ',')
            {
                // check if it's in the beginning of text not accept
                if (txtsellingprice.Text.Length == 0) e.Handled = true;
                // check if it's in the beginning of text not accept
                if (txtsellingprice.SelectionStart == 0) e.Handled = true;
                // check if there is already exist a '.' , ','
                if (alreadyExist(txtsellingprice.Text, ref sepratorChar)) e.Handled = true;
                //check if '.' or ',' is in middle of a number and after it is not a number greater than 99
                if (txtsellingprice.SelectionStart != txtsellingprice.Text.Length && e.Handled == false)
                {
                    // '.' or ',' is in the middle
                    string AfterDotString = txtsellingprice.Text.Substring(txtsellingprice.SelectionStart);

                    if (AfterDotString.Length > 2)
                    {
                        e.Handled = true;
                    }
                }
            }
            //check if a number pressed

            if (Char.IsDigit(e.KeyChar))
            {
                //check if a coma or dot exist
                if (alreadyExist(txtsellingprice.Text, ref sepratorChar))
                {
                    int sepratorPosition = txtsellingprice.Text.IndexOf(sepratorChar);
                    string afterSepratorString = txtsellingprice.Text.Substring(sepratorPosition + 1);
                    if (txtsellingprice.SelectionStart > sepratorPosition && afterSepratorString.Length > 1)
                    {
                        e.Handled = true;
                    }

                }
            }
        }

        private void txtaltqty_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.' && e.KeyChar != ',')
            {
                e.Handled = true;
            }

            //check if '.' , ',' pressed
            char sepratorChar = 's';
            if (e.KeyChar == '.' || e.KeyChar == ',')
            {
                // check if it's in the beginning of text not accept
                if (txtaltqty.Text.Length == 0) e.Handled = true;
                // check if it's in the beginning of text not accept
                if (txtaltqty.SelectionStart == 0) e.Handled = true;
                // check if there is already exist a '.' , ','
                if (alreadyExist(txtaltqty.Text, ref sepratorChar)) e.Handled = true;
                //check if '.' or ',' is in middle of a number and after it is not a number greater than 99
                if (txtaltqty.SelectionStart != txtaltqty.Text.Length && e.Handled == false)
                {
                    // '.' or ',' is in the middle
                    string AfterDotString = txtaltqty.Text.Substring(txtaltqty.SelectionStart);

                    if (AfterDotString.Length > 2)
                    {
                        e.Handled = true;
                    }
                }
            }
            //check if a number pressed

            if (Char.IsDigit(e.KeyChar))
            {
                //check if a coma or dot exist
                if (alreadyExist(txtaltqty.Text, ref sepratorChar))
                {
                    int sepratorPosition = txtaltqty.Text.IndexOf(sepratorChar);
                    string afterSepratorString = txtaltqty.Text.Substring(sepratorPosition + 1);
                    if (txtaltqty.SelectionStart > sepratorPosition && afterSepratorString.Length > 1)
                    {
                        e.Handled = true;
                    }
                }
            }
        }
    }
}
