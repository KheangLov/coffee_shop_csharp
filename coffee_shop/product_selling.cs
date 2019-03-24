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
                proImage = sqlr["images"].ToString();
            }
            sqld.Dispose();
            sqlr.Close();
        }

        private void product_selling_Load(object sender, EventArgs e)
        {
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
                pbProduct.ImageLocation = proImage;
        }
    }
}
