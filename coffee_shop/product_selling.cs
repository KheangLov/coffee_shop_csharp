using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace coffee_shop
{
    public partial class product_selling : Form
    {
        string proImage = "";
        public product_selling()
        {
            InitializeComponent();
        }

        private void QueryProducts()
        {
            string sql = "SELECT * FROM products";
            SqlCommand sqld = new SqlCommand(sql, DataConn.Connection);
            SqlDataReader sqlr = sqld.ExecuteReader();
            while(sqlr.Read())
            {
                string[] product_info = { sqlr["name"].ToString(), sqlr["selling_price"].ToString(), sqlr["type"].ToString() };
                ListViewItem item = new ListViewItem(product_info);
                lvProducts.Items.Add(item);
            }
            sqld.Dispose();
            sqlr.Close();
        }

        private void QueryPicture()
        {
            ListViewItem item = lvProducts.SelectedItems[0];
            string itemName = item.SubItems[0].Text;
            string sql = "SELECT images FROM products WHERE LOWER(name) = '" + itemName.ToLower() + "';";
            SqlCommand sqld = new SqlCommand(sql, DataConn.Connection);
            SqlDataReader sqlr = sqld.ExecuteReader();
            if (sqlr.Read())
            {
                proImage = sqlr["images"].ToString();
            }
            sqld.Dispose();
            sqlr.Close();
        }

        private void QueryToCart(string proName = "")
        {
            string sql = "SELECT * FROM products WHERE LOWER(name) = '" + proName.ToLower() + "';";
            SqlCommand sqld = new SqlCommand(sql, DataConn.Connection);
            SqlDataReader sqlr = sqld.ExecuteReader();
            if (sqlr.Read())
            {
                string[] product_info = { sqlr["name"].ToString(), sqlr["selling_price"].ToString(), 1.ToString(), sqlr["type"].ToString() };
                ListViewItem item = new ListViewItem(product_info);
                lvCart.Items.Add(item);
            }
            sqld.Dispose();
            sqlr.Close();
        }

        private void product_selling_Load(object sender, EventArgs e)
        {
            txtTotal.Enabled = false;
            txtSubTotal.Enabled = false;
            txtDisPrice.Enabled = false;
            txtReceive.Enabled = false;
            txtChange.Enabled = false;
            txtDiscount.Enabled = false;
            cbCurrency.Enabled = false;
            try
            {
                DataConn.Connection.Open();
                QueryProducts();
                cbType.SelectedIndex = 0;
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

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void lvProducts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(lvProducts.SelectedItems.Count != 0)
            {
                QueryPicture();
                pbProduct.ImageLocation = proImage;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if(lvProducts.SelectedItems.Count != 0)
            {
                ListViewItem item = lvProducts.SelectedItems[0];
                string proName = item.SubItems[0].Text;
                string proPrice = item.SubItems[1].Text;
                if (lvCart.Items.Count == 0)
                {
                    QueryToCart(proName);
                    txtTotal.Text = proPrice;
                }
                else
                {
                    int index = lvCart.Items.IndexOf(lvCart.FindItemWithText(proName));
                    if(index >= 0)
                    {
                        lvCart.Items[index].SubItems[2].Text = (int.Parse(lvCart.Items[index].SubItems[2].Text) + 1).ToString();
                        txtTotal.Text = (double.Parse(txtTotal.Text) + double.Parse(proPrice)).ToString();
                    }
                    else
                    {
                        QueryToCart(proName);
                        txtTotal.Text = (double.Parse(txtTotal.Text) + double.Parse(proPrice)).ToString();
                    }
                }
            }
        }

        private void txtSubTotal_TextChanged(object sender, EventArgs e)
        {

        }

        private void cbCurrency_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cbCurrency.Text.ToLower() == "dollar")
            {
                txtSubTotal.Text = (double.Parse(txtTotal.Text) - double.Parse(txtDisPrice.Text)).ToString();
            }
            else
            {
                txtSubTotal.Text = ((double.Parse(txtTotal.Text) - double.Parse(txtDisPrice.Text)) * 4100).ToString();
            }
        }

        private void txtDiscount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 13)
            {
                txtDisPrice.Text = (double.Parse(txtTotal.Text) * double.Parse(txtDiscount.Text) / 100).ToString();
                txtSubTotal.Text = (double.Parse(txtTotal.Text) - double.Parse(txtDisPrice.Text)).ToString();
                cbCurrency.Enabled = true;
                cbCurrency.SelectedIndex = 0;
                txtReceive.Enabled = true;
            }
        }

        private void txtReceive_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 13)
            {
                double subTotal = double.Parse(txtSubTotal.Text);
                double receive = double.Parse(txtReceive.Text);
                if(receive > subTotal)
                {
                    txtChange.Text = (receive - subTotal).ToString();
                }
                else
                {
                    MessageBox.Show("Please insert the right recievment!");
                    txtReceive.Text = "";
                    txtReceive.Focus();
                }
            }
        }

        private void txtTotal_TextChanged(object sender, EventArgs e)
        {
            if(txtTotal.Text != "")
            {
                txtDiscount.Enabled = true;
            }
        }
    }
}
