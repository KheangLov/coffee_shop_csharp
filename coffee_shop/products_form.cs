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

namespace coffee_shop
{
    public partial class products_form : Form
    {
        public products_form()
        {
            InitializeComponent();
        }
        Products my_products = new Products();
        StringCapitalize sc = new StringCapitalize();
        MyInter inter;
        int proId;
        string proImg = "";
        string path = "";

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
            string get_stocks = "SELECT * FROM stocks;";
            SqlCommand stock_cmd = new SqlCommand(get_stocks, DataConn.Connection);
            SqlDataReader stock_reader = stock_cmd.ExecuteReader();
            while (stock_reader.Read())
            {
                comboBoxProductStockID.Items.Add(sc.ToCapitalize(stock_reader["name"].ToString()));
            }
            stock_cmd.Dispose();
            stock_reader.Close();
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

        private void QueryProducts()
        {
            string query = @"SELECT products.*, product_categories.name AS procate_name , stocks.name AS stocks_name FROM products
                            INNER JOIN product_categories ON products.procate_id = product_categories.id
                            INNER JOIN stocks ON products.stock_id = stocks.id;";
            SqlCommand sqld = new SqlCommand(query, DataConn.Connection);
            SqlDataReader sqlr = sqld.ExecuteReader();
            while (sqlr.Read())
            {
                string[] products_info = { sc.ToCapitalize(sqlr["name"].ToString()), sqlr["price"].ToString(), sqlr["selling_price"].ToString(), sqlr["sale"].ToString(), sqlr["type"].ToString(), sqlr["stocks_name"].ToString(), sqlr["procate_name"].ToString() };
                ListViewItem item = new ListViewItem(products_info);
                listViewAllProducts.Items.Add(item);
            }
            sqld.Dispose();
            sqlr.Close();
        }

        private void products_form_Load(object sender, EventArgs e)
        {
            btnProductDelete.Enabled = false;
            btnProductEdit.Enabled = false;
            DataConn.Connection.Open();
            loadComboProCate();
            loadStocks();
            comboBoxProductProcateID.SelectedIndex = 0;
            comboBoxProductStockID.SelectedIndex = 0;
            MyInter product_inter = my_products;
            inter = product_inter;
            QueryProducts();
        }

        private void listViewAllProducts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewAllProducts.SelectedItems.Count != 0)
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
                    string query_name = "SELECT COUNT(*) FROM products WHERE LOWER(name) = '" + txtProductName.Text.ToLower() + "';";
                    SqlCommand check_name = new SqlCommand(query_name, DataConn.Connection);
                    int nName = Convert.ToInt16(check_name.ExecuteScalar());
                    
                    if(nName != 0)
                    {
                        MessageBox.Show("Product already exist!");
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
                        my_products.Image = path;
                        my_products.Sale = 0;
                        inter.insert();
                        MessageBox.Show("Insert successful!");
                        ClearTextBoxes(groupBoxProductForm);
                        listViewAllProducts.Items.Clear();
                        QueryProducts();
                    }
                    check_name.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "");
            }
        }

        private void btnAddProcate_Click(object sender, EventArgs e)
        {
            new product_category_form().ShowDialog();
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
                if(btnProductEdit.Text.ToLower() == "edit")
                {
                    try
                    {
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
                            btnProductEdit.Text = "Update";
                        }
                        command.Dispose();
                        reader.Close();
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
                        addProductCategoryID();
                        addStocks();
                        my_products.Sale = 0;
                        inter.update(proId);
                        MessageBox.Show("Update Successfully!");
                        ClearTextBoxes(groupBoxProductForm);
                        listViewAllProducts.Items.Clear();
                        QueryProducts();
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
                    ListViewItem lvi = listViewAllProducts.SelectedItems[0];
                    int val = 0;
                    string proName = lvi.SubItems[0].Text;
                    string del_que = "DELETE FROM products WHERE name = '" + proName + "';";
                    SqlCommand del_com = new SqlCommand(del_que, DataConn.Connection);
                    val = del_com.ExecuteNonQuery();
                    del_com.Dispose();
                    MessageBox.Show("Product has been deleted!");
                    listViewAllProducts.Items.Clear();
                    QueryProducts();
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
            listViewAllProducts.Items.Clear();
            string search_query = @"SELECT products.*, product_categories.name AS procate_name , stocks.name AS stocks_name FROM products
                            INNER JOIN product_categories ON products.procate_id = product_categories.id
                            INNER JOIN stocks ON products.stock_id = stocks.id WHERE LOWER(products.name) LIKE '%" + txtProductSearch.Text.Trim().ToLower() + "%';";
            SqlCommand srh_cmd = new SqlCommand(search_query, DataConn.Connection);
            SqlDataReader srh_rd = srh_cmd.ExecuteReader();
            while (srh_rd.Read())
            {
                string[] products_info = { sc.ToCapitalize(srh_rd["name"].ToString()), srh_rd["price"].ToString(), srh_rd["selling_price"].ToString(), srh_rd["sale"].ToString(), srh_rd["type"].ToString(), srh_rd["stocks_name"].ToString(), srh_rd["procate_name"].ToString() };
                ListViewItem item = new ListViewItem(products_info);
                listViewAllProducts.Items.Add(item);
            }
            srh_cmd.Dispose();
            srh_rd.Close();
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
    }
}
