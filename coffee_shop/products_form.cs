using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace coffee_shop
{
    public partial class products_form : Form
    {
        public products_form()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void products_form_Load(object sender, EventArgs e)
        {
            DataConn.Connection.Open();
        }

        private void labelProductImage_Click(object sender, EventArgs e)
        {

        }

        private void groupBoxProductForm_Enter(object sender, EventArgs e)
        {

        }

        private void comboBoxProductProcateID_Click(object sender, EventArgs e)
        {
            new add_product_category_form().ShowDialog();
        }
    }
}
