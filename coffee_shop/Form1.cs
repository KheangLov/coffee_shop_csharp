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
            new new_user().ShowDialog();
        }

        private void allUsersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new all_users().ShowDialog();
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
            new all_company().ShowDialog();
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
    }
}
