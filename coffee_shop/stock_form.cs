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
                DataConn.ConnectionDB();
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

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
