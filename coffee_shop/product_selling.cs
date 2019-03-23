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
               
            }
        }

        private void product_selling_Load(object sender, EventArgs e)
        {
            DataConn.Connection.Open();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {

        }
    }
}
