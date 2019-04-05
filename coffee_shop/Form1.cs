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
        string uRole;
        int uId;
        public Main(string role, int id)
        {
            uRole = role;
            uId = id;
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

        private void myUsersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (uRole.ToLower() == "superadmin" || uRole.ToLower() == "admin")
                new users_form(uRole).ShowDialog();
            else
                MessageBox.Show("You don't have permission!");
        }

        private void myCompanyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (uRole.ToLower() == "superadmin" || uRole.ToLower() == "admin")
                new company_form(uId).ShowDialog();
            else
                MessageBox.Show("You don't have permission!");
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

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnDrinks_Click(object sender, EventArgs e)
        {
            new product_selling().ShowDialog();
        }

        private void btnFood_Click(object sender, EventArgs e)
        {

        }

        private void myEmloyeesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new employees_form().ShowDialog();
        }
    }
}
