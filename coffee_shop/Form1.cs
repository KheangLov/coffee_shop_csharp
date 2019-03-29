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
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void insertUsersToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void allUsersToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void Main_Load(object sender, EventArgs e)
        {
            DataConn.Connection.Close();
        }

        private void main_closing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Dispose();
            new Login().Show();
        }
        private void newCompanyToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void allCompaniesToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void myUsersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new users_form().ShowDialog();
        }

        private void myCompanyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new company_form().ShowDialog();
        }

        private void logoutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Dispose();
            new Login().Show();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void allProductsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new products_form().ShowDialog();
        }

        private void saleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new product_selling().ShowDialog();
        }

        private void myStockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new stock_form().ShowDialog();
        }

        private void stockCateToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void branchesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new branch_form().ShowDialog();
        }
    }
}
