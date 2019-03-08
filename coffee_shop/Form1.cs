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

        }

        private void main_closing(object sender, FormClosingEventArgs e)
        {
            DataConn.Connection.Close();
            Application.Exit();
        }
    }
}
