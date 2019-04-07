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
    public partial class stock_form : Form
    {
        public stock_form()
        {
            InitializeComponent();
        }
        Stock my_stock = new Stock();
        StringCapitalize sc = new StringCapitalize();
        MyInter inter;
        int stockId;

        private void Querystocks()
        {
            string sql = @"SELECT stocks.*, stock_categories.name AS stock_name 
                        FROM stocks
                        INNER JOIN stock_categories ON stocks.stockcate_id = stock_categories.id; ";
            SqlCommand com = new SqlCommand(sql, DataConn.Connection);
            SqlDataReader sqlr = com.ExecuteReader();
            while (sqlr.Read())
            {
                string[] stock_info = { sc.ToCapitalize(sqlr["name"].ToString()), sqlr["expired_date"].ToString(), sqlr["qty"].ToString(), sc.ToCapitalize(sqlr["price"].ToString()), sqlr["selling_price"].ToString(), sqlr["alert_qty"].ToString(), sqlr["stock_name"].ToString() };
                ListViewItem item = new ListViewItem(stock_info);
                lvStocks.Items.Add(item);
            }
            com.Dispose();
            sqlr.Close();
        }

        private void loadComboBoxes(string tblName)
        {
            string sql = "SELECT * FROM " + tblName.ToLower() + ";";
            SqlCommand sqld = new SqlCommand(sql, DataConn.Connection);
            SqlDataReader sqlr = sqld.ExecuteReader();
            while (sqlr.Read())
            {
                switch(tblName.ToLower())
                {
                    case "stock_categories":
                        cbstkcate.Items.Add(sqlr["name"].ToString());
                        break;
                    case "companies":
                        cbCompany.Items.Add(sqlr["name"].ToString());
                        break;
                    case "branches":
                        cbBranch.Items.Add(sqlr["name"].ToString());
                        break;
                    default:
                        break;
                }
            }
            sqlr.Close();
            sqld.Dispose();
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
        }

        private void addComboCompany()
        {
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
        }

        private void addComboBranch()
        {
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
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtname.Text != "")
                {
                    string query_name = "SELECT COUNT(*) FROM stocks WHERE LOWER(name) = '" + txtname.Text.ToLower() + "';";
                    SqlCommand check_name = new SqlCommand(query_name, DataConn.Connection);
                    int nName = Convert.ToInt16(check_name.ExecuteScalar());
                    if (nName != 0)
                    {
                        MessageBox.Show("Stock already exist!");
                        txtname.Text = "";
                        txtname.Focus();
                    }
                    else
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
                        inter.insert();
                        MessageBox.Show("Insert successfully!");
                        ClearTextBoxes(groupBox1);
                        txtname.Focus();
                        lvStocks.Clear();
                        Querystocks();
                    }
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
                if (btnEdit.Text.ToLower() == "edit")
                {
                    ListViewItem list_item = lvStocks.SelectedItems[0];
                    string name = list_item.SubItems[0].Text;
                    string sql = @"SELECT stocks.*, stock_categories.name AS stock_name FROM stocks 
                        INNER JOIN stock_categories ON stocks.stockcate_id = stock_categories.id 
                        WHERE LOWER(stocks.name) = '" + name.ToLower() + "';";
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
                        cbstkcate.SelectedText = upd_rd["stock_name"].ToString();
                    }
                    upd_cmd.Dispose();
                    upd_rd.Close();
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
                    inter.update(stockId);
                    MessageBox.Show("Stock has been updated!");
                    ClearTextBoxes(groupBox1);
                    lvStocks.Items.Clear();
                    Querystocks();
                }
            }
        }

        private void lvStocks_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvStocks.SelectedItems.Count != 0)
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
                    string del_sql = "DELETE FROM stocks WHERE LOWER(name) = '" + name.ToLower() + "';";
                    SqlCommand del_cmd = new SqlCommand(del_sql, DataConn.Connection);
                    val = del_cmd.ExecuteNonQuery();
                    del_cmd.Dispose();
                    MessageBox.Show("Stock has been deleted!");
                    lvStocks.Items.Clear();
                    Querystocks();
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
            btnEdit.Enabled = false;
            btnDel.Enabled = false;
            DataConn.Connection.Open();
            MyInter stock_inter = my_stock;
            inter = stock_inter;
            loadComboBoxes("stock_categories");
            loadComboBoxes("companies");
            loadComboBoxes("branches");
            cbCompany.SelectedIndex = 0;
            Querystocks();
        }

        private void stock_form_FormClosing(object sender, FormClosingEventArgs e)
        {
            DataConn.Connection.Close();
            this.Dispose();
        }
    }
}
