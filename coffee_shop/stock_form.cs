﻿using System;
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
            string sql = "SELECT stocks.*, stock_categories.name AS stock_name FROM stocks INNER JOIN stock_categories ON stocks.stockcate_id = stock_categories.id;";
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

        private void loadCombostockcate()
        {
            string sql = "SELECT * FROM stock_categories;";
            SqlCommand sqld = new SqlCommand(sql, DataConn.Connection);
            SqlDataReader sqlr = sqld.ExecuteReader();
            while (sqlr.Read())
            {
                cbstkcate.Items.Add(sqlr["name"].ToString());
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

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void stock_form_Load(object sender, EventArgs e)
        {
            try
            {
                btnDel.Enabled = false;
                btnEdit.Enabled = false;
                DataConn.Connection.Open();
                loadCombostockcate();
                cbstkcate.SelectedIndex = 0;
                MyInter stock_inter = my_stock;
                inter = stock_inter;
                Querystocks();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }

        private void txtname_Leave(object sender, EventArgs e)
        {
            my_stock.Name = txtname.Text.Trim();
        }

        private void dtpExp_ValueChanged(object sender, EventArgs e)
        {
            my_stock.ExpiredDate = DateTime.Parse(dtpExp.Text.Trim());
        }

        private void txtqty_TextChanged(object sender, EventArgs e)
        {
            my_stock.Quantity = decimal.Parse(txtqty.Text.Trim());

        }

        private void txtname_TextChanged(object sender, EventArgs e)
        {
            my_stock.Name = txtname.Text.Trim();
        }

        private void txtprice_TextChanged(object sender, EventArgs e)
        {
            my_stock.Price = decimal.Parse(txtprice.Text.Trim());
        }

        private void txtsellingprice_TextChanged(object sender, EventArgs e)
        {
            my_stock.SellingPrice = decimal.Parse(txtsellingprice.Text.Trim());

        }

        private void txtaltqty_TextChanged(object sender, EventArgs e)
        {
            my_stock.AlertedQuantity = decimal.Parse(txtaltqty.Text.Trim());
        }

        private void cbstkcate_SelectedIndexChanged(object sender, EventArgs e)
        {
            addCombostockcat();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                my_stock.ImportedDate = DateTime.Now.ToString("yyyy-MM-dd");
                inter.insert();
                MessageBox.Show("Insert successfully!");
                ClearTextBoxes(groupBox1);
                txtname.Focus();
                lvStocks.Clear();
                Querystocks();
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
                if (MessageBox.Show("Are you sure, you want to delete this stocks?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    ListViewItem del_item = lvStocks.SelectedItems[0];
                    int val = 0;
                    string name = del_item.SubItems[0].Text;
                    string del_sql = "DELETE FROM stocks WHERE name = '" + name + "';";
                    SqlCommand del_cmd = new SqlCommand(del_sql, DataConn.Connection);
                    val = del_cmd.ExecuteNonQuery();
                    del_cmd.Dispose();
                    MessageBox.Show("Stock has been deleted!");
                    lvStocks.Items.Clear();
                    Querystocks();
                }
                else
                {
                    MessageBox.Show("No stock was deleted!", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnstockcat_Click(object sender, EventArgs e)
        {
            new stock_categories_form().ShowDialog();
        }
    }
}