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
        int productID;
        string proImg = "";

        private void button1_Click(object sender, EventArgs e)
        {

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
            SqlCommand sqld = new SqlCommand(get_stocks, DataConn.Connection);
            SqlDataReader sqlr = sqld.ExecuteReader();
            while (sqlr.Read())
            {
                comboBoxProductStockID.Items.Add(sc.ToCapitalize(sqlr["name"].ToString()));
            }
            sqld.Dispose();
            sqlr.Close();
        }

        private void addStocks()
        {
            string sql = "SELECT id, name FROM stocks WHERE name = '" + comboBoxProductStockID.Text.Trim() + "';";
            SqlCommand sqld = new SqlCommand(sql, DataConn.Connection);
            SqlDataReader sqlr = sqld.ExecuteReader();
            if (sqlr.Read())
            {
                my_products.Stock_id = int.Parse(sqlr["id"].ToString());
            }
            else
            {
                MessageBox.Show("Nothing found!");
            }
            sqld.Dispose();
            sqlr.Close();
        }

        private void addProductCategoryID()
        {
            string sql = "SELECT id, name FROM product_categories WHERE name = '" + comboBoxProductProcateID.Text.Trim() + "';";
            SqlCommand sqld = new SqlCommand(sql, DataConn.Connection);
            SqlDataReader sqlr = sqld.ExecuteReader();
            if (sqlr.Read())
            {
                my_products.Procate_id = int.Parse(sqlr["id"].ToString());
            }
            else
            {
                MessageBox.Show("Nothing found!");
            }
            sqld.Dispose();
            sqlr.Close();
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
            DataConn.Connection.Open();
            loadComboProCate();
            loadStocks();
            comboBoxProductProcateID.SelectedIndex = 0;
            comboBoxProductStockID.SelectedIndex = 0;
            MyInter product_inter = my_products;
            inter = product_inter;
            QueryProducts();
        }

        private void labelProductImage_Click(object sender, EventArgs e)
        {

        }

        private void groupBoxProductForm_Enter(object sender, EventArgs e)
        {

        }

        private void comboBoxProductProcateID_Click(object sender, EventArgs e)
        {

        }

        private void txtProductName_TextChanged(object sender, EventArgs e)
        {
            my_products.Name = txtProductName.Text.Trim();
        }

        private void txtProductName_Leave(object sender, EventArgs e)
        {

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
                inter.insert();
                MessageBox.Show("Insert successful!");
                ClearTextBoxes(groupBoxProductForm);
                listViewAllProducts.Items.Clear();
                QueryProducts();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtProductPrice_TextChanged(object sender, EventArgs e)
        {
            my_products.Price = decimal.Parse(txtProductPrice.Text.Trim());
        }

        private void txtProductSellingPrice_TextChanged(object sender, EventArgs e)
        {
            my_products.Selling_Price = decimal.Parse(txtProductSellingPrice.Text.Trim());
        }

        private void txtProductSale_TextChanged(object sender, EventArgs e)
        {
            my_products.Sale = int.Parse(txtProductSale.Text.Trim());
        }

        private void txtProductType_TextChanged(object sender, EventArgs e)
        {
            my_products.Type = txtProductType.Text.Trim();
        }

        private void comboBoxProductProcateID_SelectedIndexChanged(object sender, EventArgs e)
        {
            addProductCategoryID();
        }

        private void comboBoxProductStockID_SelectedIndexChanged(object sender, EventArgs e)
        {
            addStocks();
        }

        private void btnAddProcate_Click(object sender, EventArgs e)
        {
            new add_product_category_form().ShowDialog();
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
                    string path = parent.FullName + @"\pictures\" + file;
                    proImg = dlg.FileName.ToString();
                    MoveImage(proImg, path);
                    pictureBoxProductImage.ImageLocation = proImg;
                    my_products.Image = path;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
