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
    public partial class Mainsa_Form : Form
    {
        string uRole;
        int uId;
        string uName;
        public Mainsa_Form(string role, int id, string name)
        {
            uRole = role;
            uId = id;
            uName = name;
            InitializeComponent();
        }

        private void myUsersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new users_form(uRole).ShowDialog();
        }

        private void logoutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Dispose();
            new Login().Show();
        }

        private void myCompanyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new company_form(uId, uRole, uName).ShowDialog();
        }

        private void branchesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new branch_form(uId, uRole).ShowDialog();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AboutCoffeeShop().ShowDialog();
        }

        private void Mainsa_Form_Load(object sender, EventArgs e)
        {

        }
    }
}
