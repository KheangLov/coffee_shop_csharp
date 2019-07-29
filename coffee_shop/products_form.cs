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
using System.IO;
using System.Runtime.InteropServices;

namespace coffee_shop
{
    public partial class products_form : Form
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
        public products_form(string com_id, string role)
        {
            comId = com_id;
            uRole = role;
            InitializeComponent();
            m_aeroEnabled = false;
            this.FormBorderStyle = FormBorderStyle.None;
        }
        Products my_products = new Products();
        StringCapitalize sc = new StringCapitalize();
        MyInter inter;
        int proId;
        string proImg = "";
        string path = "";
        int cId;

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

        private void MoveImage(string source, string path)
        {
            try
            {
                File.Copy(source, path);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void loadComboProCate()
        {
            string get_products = "SELECT * FROM product_categories;";
            SqlCommand sqld = new SqlCommand(get_products, DataConn.Connection);
            SqlDataReader sqlr = sqld.ExecuteReader();
            while (sqlr.Read())
            {
                comboBoxProductProcateID.Items.Add(sc.ToCapitalize(sqlr["name"].ToString()));
            }
            sqld.Dispose();
            sqlr.Close();
        }

        private void loadStocks()
        {
            string get_stocks = "SELECT * FROM stocks WHERE company_id IN(" + comId + ");";
            SqlCommand stock_cmd = new SqlCommand(get_stocks, DataConn.Connection);
            SqlDataReader stock_reader = stock_cmd.ExecuteReader();
            while (stock_reader.Read())
            {
                comboBoxProductStockID.Items.Add(sc.ToCapitalize(stock_reader["name"].ToString()));
            }
            stock_cmd.Dispose();
            stock_reader.Close();
        }

        private void loadCompanies()
        {
            string sql = "SELECT * FROM companies WHERE id IN(" + comId + ");";
            SqlCommand sqld = new SqlCommand(sql, DataConn.Connection);
            SqlDataReader sqlr = sqld.ExecuteReader();
            while (sqlr.Read())
            {
                cbCompany.Items.Add(sc.ToCapitalize(sqlr["name"].ToString()));
                cbByCompany.Items.Add(sc.ToCapitalize(sqlr["name"].ToString()));
            }
            sqld.Dispose();
            sqlr.Close();
        }

        private void loadBranches(int com = 0)
        {
            if(com > 0)
            {
                string sql = "SELECT * FROM branches WHERE company_id = " + com + ";";
                SqlCommand sqld = new SqlCommand(sql, DataConn.Connection);
                SqlDataReader sqlr = sqld.ExecuteReader();
                while (sqlr.Read())
                {
                    cbBranch.Items.Add(sc.ToCapitalize(sqlr["name"].ToString()));
                }
                sqld.Dispose();
                sqlr.Close();
            }
        }

        private void getByCompany()
        {
            string sql = "SELECT * FROM companies WHERE LOWER(name) = '" + cbByCompany.Text.ToLower() + "';";
            SqlCommand sqld = new SqlCommand(sql, DataConn.Connection);
            SqlDataReader sqlr = sqld.ExecuteReader();
            if(sqlr.Read())
            {
                cId = int.Parse(sqlr["id"].ToString());
            }
            sqld.Dispose();
            sqlr.Close();
        }

        private void loadByBranch()
        {
            string sql = "";
            if(cbByCompany.SelectedItem.ToString().ToLower() == "all")
                sql = "SELECT * FROM branches WHERE company_id IN(" + comId + ");";
            else
                sql = "SELECT * FROM branches WHERE company_id = " + cId + ";";
            SqlCommand sqld = new SqlCommand(sql, DataConn.Connection);
            SqlDataReader sqlr = sqld.ExecuteReader();
            while (sqlr.Read())
            {
                cbByBranch.Items.Add(sc.ToCapitalize(sqlr["name"].ToString()));
            }
            sqld.Dispose();
            sqlr.Close();
        }

        private void addStocks()
        {
            string get_stock = "SELECT id, name FROM stocks WHERE LOWER(name) = '" + comboBoxProductStockID.Text.Trim().ToLower() + "';";
            SqlCommand stocks_cmd = new SqlCommand(get_stock, DataConn.Connection);
            SqlDataReader stocks_reader = stocks_cmd.ExecuteReader();
            if (stocks_reader.Read())
            {
                my_products.Stock_id = int.Parse(stocks_reader["id"].ToString());
            }
            else
            {
                MessageBox.Show("Nothing found!");
            }
            stocks_cmd.Dispose();
            stocks_reader.Close();
        }

        private void addProductCategoryID()
        {
            string get_procate = "SELECT id, name FROM product_categories WHERE LOWER(name) = '" + comboBoxProductProcateID.Text.Trim().ToLower() + "';";
            SqlCommand procate_cmd = new SqlCommand(get_procate, DataConn.Connection);
            SqlDataReader procate_reader = procate_cmd.ExecuteReader();
            if (procate_reader.Read())
            {
                my_products.Procate_id = int.Parse(procate_reader["id"].ToString());
            }
            else
            {
                MessageBox.Show("Nothing found!");
            }
            procate_cmd.Dispose();
            procate_reader.Close();
        }

        private void addCompany()
        {
            string sql = "SELECT * FROM companies WHERE LOWER(name) = '" + cbCompany.Text.Trim().ToLower() + "';";
            SqlCommand sqld = new SqlCommand(sql, DataConn.Connection);
            SqlDataReader sqlr = sqld.ExecuteReader();
            if(sqlr.Read())
            {
                my_products.CompanyId = int.Parse(sqlr["id"].ToString());
            }
            sqld.Dispose();
            sqlr.Close();
        }

        private void addBranch()
        {
            string sql = "SELECT * FROM branches WHERE LOWER(name) = '" + cbBranch.Text.Trim().ToLower() + "';";
            SqlCommand sqld = new SqlCommand(sql, DataConn.Connection);
            SqlDataReader sqlr = sqld.ExecuteReader();
            if (sqlr.Read())
            {
                my_products.BranchId = int.Parse(sqlr["id"].ToString());
            }
            sqld.Dispose();
            sqlr.Close();
        }

        private void QueryProducts()
        {
            string query = @"SELECT products.*, product_categories.name AS procate_name , stocks.name AS stocks_name,
                            companies.name AS company_name, branches.name AS branch_name
                            FROM products
                            INNER JOIN product_categories ON products.procate_id = product_categories.id
                            INNER JOIN stocks ON products.stock_id = stocks.id 
                            INNER JOIN companies ON products.company_id = companies.id
                            INNER JOIN branches ON products.branch_id = branches.id
                            WHERE products.company_id IN(" + comId + ")" +
                            "AND LOWER(companies.status) = 'active' AND LOWER(branches.status) = 'active';";
            SqlCommand sqld = new SqlCommand(query, DataConn.Connection);
            SqlDataReader sqlr = sqld.ExecuteReader();
            while (sqlr.Read())
            {
                string[] products_info = { sc.ToCapitalize(sqlr["name"].ToString()), sqlr["price"].ToString(), sqlr["selling_price"].ToString(), sqlr["type"].ToString(), sqlr["stocks_name"].ToString(), sqlr["procate_name"].ToString(), sqlr["company_name"].ToString(), sqlr["branch_name"].ToString() };
                ListViewItem item = new ListViewItem(products_info);
                listViewAllProducts.Items.Add(item);
            }
            sqld.Dispose();
            sqlr.Close();
        }

        private void products_form_Load(object sender, EventArgs e)
        {
            if (uRole.ToLower() == "user")
                btnProductAdd.Enabled = false;
            else
                btnProductAdd.Enabled = true;
            btnProductDelete.Enabled = false;
            btnProductEdit.Enabled = false;
            cbBranch.Enabled = false;
            MyInter product_inter = my_products;
            inter = product_inter;
            DataConn.Connection.Open();
            loadComboProCate();
            if(comboBoxProductProcateID.Items.Count > 0)
                comboBoxProductProcateID.SelectedIndex = 0;
            loadStocks();
            if(comboBoxProductStockID.Items.Count > 0)
                comboBoxProductStockID.SelectedIndex = 0;
            loadCompanies();
            DataConn.Connection.Close();
            if(cbCompany.Items.Count > 0)
                cbCompany.SelectedIndex = 0;
            if(cbCompany.Text != "")
            {
                int com = 0;
                DataConn.Connection.Open();
                string sql = "SELECT * FROM companies WHERE LOWER(name) = '" + cbCompany.Text.Trim().ToLower() + "';";
                SqlCommand sqld = new SqlCommand(sql, DataConn.Connection);
                SqlDataReader sqlr = sqld.ExecuteReader();
                if (sqlr.Read())
                {
                    com = int.Parse(sqlr["id"].ToString());
                }
                sqld.Dispose();
                sqlr.Close();
                cbBranch.Items.Clear();
                loadBranches(com);
                DataConn.Connection.Close();
                if (cbBranch.Items.Count > 0)
                    cbBranch.SelectedIndex = 0;
                cbBranch.Enabled = true;
            }
            cbByCompany.SelectedIndex = 0;
            DataConn.Connection.Open();
            getByCompany();
            cbByBranch.Items.Clear();
            cbByBranch.Items.Add("All");
            loadByBranch();
            DataConn.Connection.Close();
            if (cbByBranch.Items.Count > 0)
                cbByBranch.SelectedIndex = 0;
            listViewAllProducts.Items.Clear();
            DataConn.Connection.Open();
            QueryProducts();
            DataConn.Connection.Close();
        }

        private void listViewAllProducts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewAllProducts.SelectedItems.Count != 0 && uRole != "user")
            {
                btnProductEdit.Enabled = true;
                btnProductDelete.Enabled = true;
            }
            else
            {
                btnProductDelete.Enabled = false;
                btnProductEdit.Enabled = false;
            }
        }

        private void btnProductAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if(txtProductName.Text != "")
                {
                    DataConn.Connection.Open();
                    string query_name = "SELECT COUNT(*) FROM products WHERE LOWER(name) = '" + txtProductName.Text.ToLower() + "' AND id IN(" + comId + ");";
                    SqlCommand check_name = new SqlCommand(query_name, DataConn.Connection);
                    int nName = Convert.ToInt16(check_name.ExecuteScalar());
                    check_name.Dispose();
                    if (nName != 0)
                    {
                        MessageBox.Show("Product already exist!");
                        DataConn.Connection.Close();
                        txtProductName.Text = "";
                        txtProductName.Focus();
                    }
                    else
                    {
                        my_products.Name = txtProductName.Text.Trim();
                        my_products.Price = double.Parse(txtProductPrice.Text.Trim());
                        my_products.Selling_Price = double.Parse(txtProductSellingPrice.Text.Trim());
                        my_products.Type = txtProductType.Text.Trim();
                        addStocks();
                        addProductCategoryID();
                        addCompany();
                        addBranch();
                        my_products.Image = path;
                        my_products.Sale = 0;
                        my_products.CutFromStock = double.Parse(txtStockCut.Text.Trim());
                        inter.insert();
                        MessageBox.Show("Insert successful!");
                        ClearTextBoxes(groupBoxProductForm);
                        listViewAllProducts.Items.Clear();
                        QueryProducts();
                        DataConn.Connection.Close();
                    }
                    DataConn.Connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "");
            }
        }

        private void products_form_FormClosing(object sender, FormClosingEventArgs e)
        {
            DataConn.Connection.Close();
            this.Dispose();
        }

        private void btnProductExit_Click(object sender, EventArgs e)
        {
            DataConn.Connection.Close();
            this.Dispose();
        }

        private void btnProductImage_Click(object sender, EventArgs e)
        {
            try
            {
                var parent = Directory.GetParent(Directory.GetCurrentDirectory()).Parent;
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.Filter = "All Files (*.*)|*.*|JPG Files (*.jpg)|*.jpg|PNG Files (*.png)|*.png";
                dlg.Title = "Select Product Picture.";
                if(dlg.ShowDialog() == DialogResult.OK)
                {
                    string file = Path.GetFileName(dlg.FileName);
                    path = parent.FullName + @"\pictures\" + file;
                    proImg = dlg.FileName.ToString();
                    MoveImage(proImg, path);
                    pictureBoxProductImage.ImageLocation = proImg;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnProductEdit_Click(object sender, EventArgs e)
        {
            if(listViewAllProducts.SelectedItems.Count != 0)
            {
                btnProductDelete.Enabled = false;
                if(btnProductEdit.Text.ToLower() == "edit")
                {
                    try
                    {
                        DataConn.Connection.Open();
                        ListViewItem item = listViewAllProducts.SelectedItems[0];
                        string lv_products = item.SubItems[0].Text;
                        string sql = @"SELECT products.*, product_categories.name AS procate_name , stocks.name AS stocks_name FROM products
                            INNER JOIN product_categories ON products.procate_id = product_categories.id
                            INNER JOIN stocks ON products.stock_id = stocks.id WHERE LOWER(products.name) = '" + lv_products.ToLower() + "';";
                        SqlCommand command = new SqlCommand(sql, DataConn.Connection);
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            proId = int.Parse(reader["id"].ToString());
                            txtProductName.Text = reader["name"].ToString();
                            txtProductPrice.Text = reader["price"].ToString();
                            txtProductSellingPrice.Text = reader["selling_price"].ToString();
                            txtProductType.Text = reader["type"].ToString();
                            comboBoxProductStockID.SelectedItem = reader["stocks_name"].ToString();
                            comboBoxProductProcateID.SelectedItem = reader["procate_id"].ToString();
                            pictureBoxProductImage.ImageLocation = reader["images"].ToString();
                            txtStockCut.Text = reader["cut_from_stock"].ToString();
                            btnProductEdit.Text = "Update";
                        }
                        command.Dispose();
                        reader.Close();
                        DataConn.Connection.Close();
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else if (btnProductEdit.Text.ToLower() == "update")
                {
                    try
                    {
                        DataConn.Connection.Open();
                        my_products.Name = txtProductName.Text.Trim();
                        my_products.Price = double.Parse(txtProductPrice.Text.Trim());
                        my_products.Selling_Price = double.Parse(txtProductSellingPrice.Text.Trim());
                        my_products.Type = txtProductType.Text.Trim();
                        addStocks();
                        addProductCategoryID();
                        addCompany();
                        addBranch();
                        my_products.Image = path;
                        my_products.Sale = 0;
                        my_products.CutFromStock = double.Parse(txtStockCut.Text.Trim());
                        inter.update(proId);
                        MessageBox.Show("Update Successfully!");
                        ClearTextBoxes(groupBoxProductForm);
                        listViewAllProducts.Items.Clear();
                        QueryProducts();
                        DataConn.Connection.Close();
                        btnProductEdit.Text = "Edit";
                        btnProductEdit.Enabled = false;
                        btnProductDelete.Enabled = false;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void btnProductDelete_Click(object sender, EventArgs e)
        {
            if(listViewAllProducts.SelectedItems.Count != 0)
            {
                if (MessageBox.Show("Are you sure, you want to delete this product?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    DataConn.Connection.Open();
                    ListViewItem lvi = listViewAllProducts.SelectedItems[0];
                    int val = 0;
                    string proName = lvi.SubItems[0].Text;
                    string del_que = "DELETE FROM products WHERE LOWER(name) = '" + proName.ToLower() + "';";
                    SqlCommand del_com = new SqlCommand(del_que, DataConn.Connection);
                    val = del_com.ExecuteNonQuery();
                    del_com.Dispose();
                    MessageBox.Show("Product has been deleted!");
                    listViewAllProducts.Items.Clear();
                    QueryProducts();
                    DataConn.Connection.Close();
                    btnProductDelete.Enabled = false;
                    btnProductEdit.Enabled = false;
                }
                else
                {
                    MessageBox.Show("No product was deleted!", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void txtProductSearch_TextChanged(object sender, EventArgs e)
        {
            DataConn.Connection.Open();
            listViewAllProducts.Items.Clear();
            string search_query = @"SELECT products.*, product_categories.name AS procate_name , stocks.name AS stocks_name,
                            companies.name AS company_name, branches.name AS branch_name
                            FROM products
                            INNER JOIN product_categories ON products.procate_id = product_categories.id
                            INNER JOIN stocks ON products.stock_id = stocks.id 
                            INNER JOIN companies ON products.company_id = companies.id
                            INNER JOIN branches ON products.branch_id = branches.id 
                            WHERE products.company_id IN(" + comId + ") AND LOWER(products.name) LIKE '%" + txtProductSearch.Text.Trim().ToLower() + "%';";
            SqlCommand srh_cmd = new SqlCommand(search_query, DataConn.Connection);
            SqlDataReader srh_rd = srh_cmd.ExecuteReader();
            while (srh_rd.Read())
            {
                string[] products_info = { sc.ToCapitalize(srh_rd["name"].ToString()), srh_rd["price"].ToString(), srh_rd["selling_price"].ToString(), srh_rd["type"].ToString(), srh_rd["stocks_name"].ToString(), srh_rd["procate_name"].ToString(), srh_rd["company_name"].ToString(), srh_rd["branch_name"].ToString() };
                ListViewItem item = new ListViewItem(products_info);
                listViewAllProducts.Items.Add(item);
            }
            srh_cmd.Dispose();
            srh_rd.Close();
            DataConn.Connection.Close();
        }

        private void txtProductSellingPrice_KeyPress(object sender, KeyPressEventArgs e)
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
                if (txtProductSellingPrice.Text.Length == 0) e.Handled = true;
                // check if it's in the beginning of text not accept
                if (txtProductSellingPrice.SelectionStart == 0) e.Handled = true;
                // check if there is already exist a '.' , ','
                if (alreadyExist(txtProductSellingPrice.Text, ref sepratorChar)) e.Handled = true;
                //check if '.' or ',' is in middle of a number and after it is not a number greater than 99
                if (txtProductSellingPrice.SelectionStart != txtProductSellingPrice.Text.Length && e.Handled == false)
                {
                    // '.' or ',' is in the middle
                    string AfterDotString = txtProductSellingPrice.Text.Substring(txtProductSellingPrice.SelectionStart);

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
                if (alreadyExist(txtProductSellingPrice.Text, ref sepratorChar))
                {
                    int sepratorPosition = txtProductSellingPrice.Text.IndexOf(sepratorChar);
                    string afterSepratorString = txtProductSellingPrice.Text.Substring(sepratorPosition + 1);
                    if (txtProductSellingPrice.SelectionStart > sepratorPosition && afterSepratorString.Length > 1)
                    {
                        e.Handled = true;
                    }

                }
            }
        }

        private void txtProductPrice_KeyPress(object sender, KeyPressEventArgs e)
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
                if (txtProductPrice.Text.Length == 0) e.Handled = true;
                // check if it's in the beginning of text not accept
                if (txtProductPrice.SelectionStart == 0) e.Handled = true;
                // check if there is already exist a '.' , ','
                if (alreadyExist(txtProductPrice.Text, ref sepratorChar)) e.Handled = true;
                //check if '.' or ',' is in middle of a number and after it is not a number greater than 99
                if (txtProductPrice.SelectionStart != txtProductPrice.Text.Length && e.Handled == false)
                {
                    // '.' or ',' is in the middle
                    string AfterDotString = txtProductPrice.Text.Substring(txtProductPrice.SelectionStart);

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
                if (alreadyExist(txtProductPrice.Text, ref sepratorChar))
                {
                    int sepratorPosition = txtProductPrice.Text.IndexOf(sepratorChar);
                    string afterSepratorString = txtProductPrice.Text.Substring(sepratorPosition + 1);
                    if (txtProductPrice.SelectionStart > sepratorPosition && afterSepratorString.Length > 1)
                    {
                        e.Handled = true;
                    }
                }
            }
        }

        private void cbByCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataConn.Connection.Open();
            getByCompany();
            DataConn.Connection.Close();
            cbByBranch.Items.Clear();
            cbByBranch.Items.Add("All");
            DataConn.Connection.Open();
            loadByBranch();
            DataConn.Connection.Close();
            if (cbByBranch.Items.Count > 0)
                cbByBranch.SelectedIndex = 0;
            else
                cbByBranch.Text = "";
            string sql = "";
            if (cbByCompany.SelectedItem.ToString().ToLower() == "all" && cbByBranch.SelectedItem.ToString().ToLower() == "all")
            {
                sql = @"SELECT products.*, product_categories.name AS procate_name , stocks.name AS stocks_name,
                        companies.name AS company_name, branches.name AS branch_name
                        FROM products
                        INNER JOIN product_categories ON products.procate_id = product_categories.id
                        INNER JOIN stocks ON products.stock_id = stocks.id 
                        INNER JOIN companies ON products.company_id = companies.id
                        INNER JOIN branches ON products.branch_id = branches.id 
                        WHERE products.company_id IN(" + comId + ");";
            }
            else if (cbByCompany.SelectedItem.ToString().ToLower() != "all" && cbByBranch.SelectedItem.ToString().ToLower() == "all")
            {
                sql = @"SELECT products.*, product_categories.name AS procate_name , stocks.name AS stocks_name,
                        companies.name AS company_name, branches.name AS branch_name
                        FROM products
                        INNER JOIN product_categories ON products.procate_id = product_categories.id
                        INNER JOIN stocks ON products.stock_id = stocks.id
                        INNER JOIN companies ON products.company_id = companies.id
                        INNER JOIN branches ON products.branch_id = branches.id
                        WHERE LOWER(companies.name) = '" + cbByCompany.Text.ToLower() + "';";
            }
            else
            {
                sql = @"SELECT products.*, product_categories.name AS procate_name , stocks.name AS stocks_name,
                        companies.name AS company_name, branches.name AS branch_name
                        FROM products
                        INNER JOIN product_categories ON products.procate_id = product_categories.id
                        INNER JOIN stocks ON products.stock_id = stocks.id
                        INNER JOIN companies ON products.company_id = companies.id
                        INNER JOIN branches ON products.branch_id = branches.id
                        WHERE LOWER(companies.name) = '" + cbByCompany.Text.ToLower() + "' AND LOWER(branches.name) = '" + cbByBranch.Text.ToLower() + "';";
            }
            DataConn.Connection.Open();
            SqlCommand com = new SqlCommand(sql, DataConn.Connection);
            SqlDataReader sqlr = com.ExecuteReader();
            listViewAllProducts.Items.Clear();
            while (sqlr.Read())
            {
                string[] products_info = { sc.ToCapitalize(sqlr["name"].ToString()), sqlr["price"].ToString(), sqlr["selling_price"].ToString(), sqlr["type"].ToString(), sqlr["stocks_name"].ToString(), sqlr["procate_name"].ToString(), sqlr["company_name"].ToString(), sqlr["branch_name"].ToString() };
                ListViewItem item = new ListViewItem(products_info);
                listViewAllProducts.Items.Add(item);
            }
            com.Dispose();
            sqlr.Close();
            DataConn.Connection.Close();
        }

        private void cbByBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sql = "";
            if (cbByCompany.SelectedItem.ToString().ToLower() == "all" && cbByBranch.SelectedItem.ToString().ToLower() == "all")
            {
                sql = @"SELECT products.*, product_categories.name AS procate_name , stocks.name AS stocks_name,
                        companies.name AS company_name, branches.name AS branch_name
                        FROM products
                        INNER JOIN product_categories ON products.procate_id = product_categories.id
                        INNER JOIN stocks ON products.stock_id = stocks.id 
                        INNER JOIN companies ON products.company_id = companies.id
                        INNER JOIN branches ON products.branch_id = branches.id 
                        WHERE products.company_id IN(" + comId + ");";
            }
            else if (cbByCompany.SelectedItem.ToString().ToLower() != "all" && cbByBranch.SelectedItem.ToString().ToLower() == "all")
            {
                sql = @"SELECT products.*, product_categories.name AS procate_name , stocks.name AS stocks_name,
                        companies.name AS company_name, branches.name AS branch_name
                        FROM products
                        INNER JOIN product_categories ON products.procate_id = product_categories.id
                        INNER JOIN stocks ON products.stock_id = stocks.id
                        INNER JOIN companies ON products.company_id = companies.id
                        INNER JOIN branches ON products.branch_id = branches.id
                        WHERE LOWER(companies.name) = '" + cbByCompany.Text.ToLower() + "';";
            }
            else
            {
                sql = @"SELECT products.*, product_categories.name AS procate_name , stocks.name AS stocks_name,
                        companies.name AS company_name, branches.name AS branch_name
                        FROM products
                        INNER JOIN product_categories ON products.procate_id = product_categories.id
                        INNER JOIN stocks ON products.stock_id = stocks.id
                        INNER JOIN companies ON products.company_id = companies.id
                        INNER JOIN branches ON products.branch_id = branches.id
                        WHERE LOWER(companies.name) = '" + cbByCompany.Text.ToLower() + "' AND LOWER(branches.name) = '" + cbByBranch.Text.ToLower() + "';";
            }
            DataConn.Connection.Open();
            SqlCommand com = new SqlCommand(sql, DataConn.Connection);
            SqlDataReader sqlr = com.ExecuteReader();
            listViewAllProducts.Items.Clear();
            while (sqlr.Read())
            {
                string[] products_info = { sc.ToCapitalize(sqlr["name"].ToString()), sqlr["price"].ToString(), sqlr["selling_price"].ToString(), sqlr["sale"].ToString(), sqlr["type"].ToString(), sqlr["stocks_name"].ToString(), sqlr["procate_name"].ToString(), sqlr["company_name"].ToString(), sqlr["branch_name"].ToString() };
                ListViewItem item = new ListViewItem(products_info);
                listViewAllProducts.Items.Add(item);
            }
            com.Dispose();
            sqlr.Close();
            DataConn.Connection.Close();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DataConn.Connection.Close();
            this.Dispose();
        }

        private void cbCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbBranch.Items.Clear();
            DataConn.Connection.Open();
            int com = 0;
            string sql = "SELECT * FROM companies WHERE LOWER(name) = '" + cbCompany.Text.Trim().ToLower() + "';";
            SqlCommand sqld = new SqlCommand(sql, DataConn.Connection);
            SqlDataReader sqlr = sqld.ExecuteReader();
            if (sqlr.Read())
            {
                com = int.Parse(sqlr["id"].ToString());
            }
            sqld.Dispose();
            sqlr.Close();
            loadBranches(com);
            if(cbBranch.Items.Count > 0)
            {
                cbBranch.SelectedIndex = 0;
            }
            DataConn.Connection.Close();
            cbBranch.Enabled = true;
        }
    }
}
