using System;
using System.Collections;
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
        string type;
        string comId;
        public product_selling(string item, string com_id)
        {
            type = item;
            comId = com_id;
            InitializeComponent();
        }
        StringCapitalize sc = new StringCapitalize();

        private void QueryProducts()
        {
            string sql = @"SELECT * FROM products
                INNER JOIN product_categories ON products.procate_id = product_categories.id
                WHERE LOWER(product_categories.name) = '" + type.ToLower() + "' AND products.company_id IN(" + comId + ");";
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

        private void QueryCombos(string tblName)
        {
            string query = "";
            if (tblName.ToLower() != "companies")
                query = "SELECT * FROM " + tblName.ToLower() + " WHERE company_id IN(" + comId + ");";
            else
                query = "SELECT * FROM companies WHERE id IN(" + comId + ");";
            SqlCommand sqld = new SqlCommand(query, DataConn.Connection);
            SqlDataReader sqlr = sqld.ExecuteReader();
            while(sqlr.Read())
            {
                switch(tblName.ToLower())
                {
                    case "employees":
                        cbEmp.Items.Add(sc.ToCapitalize(sqlr["fullname"].ToString()));
                        break;
                    case "companies":
                        cbCom.Items.Add(sc.ToCapitalize(sqlr["name"].ToString()));
                        break;
                    case "branches":
                        cbBranch.Items.Add(sc.ToCapitalize(sqlr["name"].ToString()));
                        break;
                    default:
                        break;
                }
            }
            sqld.Dispose();
            sqlr.Close();
        }

        private void product_selling_Load(object sender, EventArgs e)
        {
            btnPay.Enabled = false;
            btnDel.Enabled = false;
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
                QueryCombos("employees");
                QueryCombos("companies");
                QueryCombos("branches");
                if(cbEmp.Items.Count > 0)
                    cbEmp.SelectedIndex = 0;
                if (cbCom.Items.Count > 0)
                    cbCom.SelectedIndex = 0;
                if (cbBranch.Items.Count > 0)
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
                if(cbCurrency.Items.Count > 0)
                    cbCurrency.SelectedIndex = 0;
                txtReceive.Enabled = true;
                btnAdd.Enabled = false;
            }
        }

        private void txtReceive_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 13)
            {
                btnPay.Enabled = true;
                double subTotal = double.Parse(txtSubTotal.Text);
                double receive = double.Parse(txtReceive.Text);
                if(receive >= subTotal)
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

        private void btnPay_Click(object sender, EventArgs e)
        {
            if (lvCart.Items.Count != 0)
            {
                Dictionary<string, int> dict = new Dictionary<string, int>();
                for (int i = 0; i < lvCart.Items.Count; i++)
                {
                    ListViewItem item = lvCart.Items[i];
                    dict.Add(item.SubItems[0].Text, int.Parse(item.SubItems[2].Text));
                }
                foreach (KeyValuePair<string, int> item in dict)
                {
                    double cutStock = 0;
                    int stockId = 0;
                    string sale_sql = "UPDATE products SET sale = " + item.Value + "WHERE LOWER(name) = '" + item.Key.ToLower() + "';";
                    SqlCommand sqld = new SqlCommand(sale_sql, DataConn.Connection);
                    sqld.ExecuteNonQuery();
                    sqld.Dispose();
                    Console.WriteLine("Key: {0}, Value: {1}", item.Key, item.Value);
                    string query_sale = "SELECT stock_id, cut_from_stock FROM products WHERE LOWER(name) = '" + item.Key.ToLower() + "';";
                    SqlCommand qsd = new SqlCommand(query_sale, DataConn.Connection);
                    SqlDataReader qsr = qsd.ExecuteReader();
                    if (qsr.Read())
                    {
                        stockId = int.Parse(qsr["stock_id"].ToString());
                        cutStock = double.Parse(qsr["cut_from_stock"].ToString());
                    }
                    qsd.Dispose();
                    qsr.Close();
                    if(cutStock > 0 && stockId > 0)
                    {
                        double checkStockQty = 0;
                        string sqty_sql = "SELECT * FROM stocks WHERE id = " + stockId + ";";
                        SqlCommand sqd = new SqlCommand(sqty_sql, DataConn.Connection);
                        SqlDataReader sqr = sqd.ExecuteReader();
                        if (sqr.Read())
                        {
                            checkStockQty = double.Parse(sqr["qty"].ToString());
                        }
                        sqd.Dispose();
                        sqr.Close();
                        if(checkStockQty - (cutStock * item.Value) < 0)
                        {
                            MessageBox.Show("You don't have enough item in your stock!");
                        }
                        else
                        {
                            string upd_stock = "UPDATE stocks SET qty = qty - " + (cutStock * item.Value) + " WHERE id = " + stockId + ";";
                            SqlCommand ups = new SqlCommand(upd_stock, DataConn.Connection);
                            int val = ups.ExecuteNonQuery();
                            ups.Dispose();
                            double stockQty = 0;
                            double stockAlertQty = 0;
                            string check_stock = "SELECT * FROM stocks WHERE id = " + stockId + ";";
                            SqlCommand csd = new SqlCommand(check_stock, DataConn.Connection);
                            SqlDataReader csr = csd.ExecuteReader();
                            if (csr.Read())
                            {
                                stockQty = double.Parse(csr["qty"].ToString());
                                stockAlertQty = double.Parse(csr["alert_qty"].ToString());
                            }
                            csd.Dispose();
                            csr.Close();
                            if (stockAlertQty >= stockQty)
                            {
                                string upd_alerted = "UPDATE stocks SET alerted = 1 WHERE id = " + stockId + ";";
                                SqlCommand sad = new SqlCommand(upd_alerted, DataConn.Connection);
                                int sar = sad.ExecuteNonQuery();
                                sad.Dispose();
                            }
                        }
                    }
                }
                MessageBox.Show("Pay Success!");
                ClearTextBoxes(groupBox1);
                txtReceive.Enabled = false;
                cbType.SelectedIndex = 0;
                if (cbCom.Items.Count > 0)
                    cbCom.SelectedIndex = 0;
                if (cbBranch.Items.Count > 0)
                    cbBranch.SelectedIndex = 0;
                if (cbEmp.Items.Count > 0)
                    cbEmp.SelectedIndex = 0;
                lvCart.Items.Clear();
                cbCurrency.Text = "";
                cbCurrency.Enabled = false;
                btnPay.Enabled = false;
                btnAdd.Enabled = true;
            }
        }

        private void lvCart_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(lvCart.SelectedItems.Count != 0)
            {
                btnDel.Enabled = true;
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {   
            if(lvCart.SelectedItems.Count != 0)
            {
                ListViewItem item = lvCart.SelectedItems[0];
                lvCart.Items.Remove(item);
                txtTotal.Text = (double.Parse(txtTotal.Text) - (double.Parse(item.SubItems[1].Text) * double.Parse(item.SubItems[2].Text))).ToString();
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            lvProducts.Items.Clear();
            string sql = @"SELECT * FROM products
                INNER JOIN product_categories ON products.procate_id = product_categories.id
                WHERE LOWER(product_categories.name) = '" + type.ToLower() + "' AND products.company_id IN(" + comId + ") AND LOWER(products.name) LIKE '%" + txtSearch.Text.ToLower() + "%';";
            SqlCommand sqld = new SqlCommand(sql, DataConn.Connection);
            SqlDataReader sqlr = sqld.ExecuteReader();
            while (sqlr.Read())
            {
                string[] product_info = { sqlr["name"].ToString(), sqlr["selling_price"].ToString(), sqlr["type"].ToString() };
                ListViewItem item = new ListViewItem(product_info);
                lvProducts.Items.Add(item);
            }
            sqld.Dispose();
            sqlr.Close();
        }
    }
}
