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

        private void loadComboUser()
        {
            string get_users = "SELECT users.username, roles.name FROM users INNER JOIN roles ON users.role_id = roles.id WHERE roles.name = 'admin';";
            SqlCommand sqld = new SqlCommand(get_users, DataConn.Connection);
            SqlDataReader sqlr = sqld.ExecuteReader();
            while (sqlr.Read())
            {
                comboBoxProductProcateID.Items.Add(sc.ToCapitalize(sqlr["username"].ToString()));
            }
            sqld.Dispose();
            sqlr.Close();
        }

        private void addProductCategoryID()
        {
            string products_user = "SELECT id, name FROM product_categories;";
            SqlCommand sqld = new SqlCommand(products_user, DataConn.Connection);
            SqlDataReader sqlr = sqld.ExecuteReader();
            if (sqlr.Read())
            {
                comboBoxProductProcateID.Items.Add(sqlr["name"].ToString());
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
            string query = "SELECT * FROM products;";
            SqlCommand sqld = new SqlCommand(query, DataConn.Connection);
            SqlDataReader sqlr = sqld.ExecuteReader();
            while (sqlr.Read())
            {
                string[] products_info = { sc.ToCapitalize(sqlr["name"].ToString()), sqlr["price"].ToString(), sqlr["selling_price"].ToString(), sqlr["sale"].ToString(), sqlr["type"].ToString(), sqlr["procate_id"].ToString() };
                ListViewItem item = new ListViewItem(products_info);
                listViewAllProducts.Items.Add(item);
            }
            sqld.Dispose();
            sqlr.Close();
        }

        private void products_form_Load(object sender, EventArgs e)
        {
            DataConn.Connection.Open();
            addProductCategoryID();
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
            //new add_product_category_form().ShowDialog();
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

        }
    }
}
