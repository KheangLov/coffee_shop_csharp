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
    public partial class product_category_form : Form
    {
        public product_category_form()
        {
            InitializeComponent();
        }
        ProductCategory procate = new ProductCategory();
        MyInter inter;

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

        private void add_product_category_form_Load(object sender, EventArgs e)
        {
            MyInter procate_inter = procate;
            inter = procate_inter;
        }

        private void txtName_Leave(object sender, EventArgs e)
        {
            procate.Name = txtName.Text.Trim();
        }

        private void txtDescriptions_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtDescriptions_Leave(object sender, EventArgs e)
        {
            procate.Descriptions = txtDescriptions.Text.Trim();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                inter.insert();
                MessageBox.Show("insert succcessfully");
                ClearTextBoxes(gpAddproductCategory);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void product_category_form_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Dispose();
        }
    }
}
