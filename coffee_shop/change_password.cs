using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace coffee_shop
{
    public partial class change_password : Form
    {
        string oldPass;
        int uId;
        string newPass;
        public change_password(string frt_item, int sec_item)
        {
            oldPass = frt_item;
            uId = sec_item;
            InitializeComponent();
        }
        HashCode hc = new HashCode();

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

        private void btnBack_Click(object sender, EventArgs e)
        {

        }

        private void txtOldPass_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtOldPass_leave(object sender, EventArgs e)
        {
            if(hc.PassHash(txtOldPass.Text.Trim()) != oldPass)
            {
                MessageBox.Show("Wrong old password!");
                ClearTextBoxes(groupBox1);
                txtOldPass.Focus();
            }
            else
            {
                txtNewPass.Enabled = true;
                txtConPass.Enabled = true;
            }
        }

        private void change_password_Load(object sender, EventArgs e)
        {
            txtNewPass.Enabled = false;
            txtConPass.Enabled = false;
        }

        private void txtConPass_leave(object sender, EventArgs e)
        {
            if(txtNewPass.Text.Trim() == txtConPass.Text.Trim())
            {
                newPass = hc.PassHash(txtNewPass.Text.Trim());
            }
            else
            {
                MessageBox.Show("Wrong confirmation password!");
                newPass = "";
                txtNewPass.Text = "";
                txtConPass.Text = "";
            }
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            if(newPass != "")
            {
                try
                {
                    string upp_que = "UPDATE users SET password = '" + newPass + "' WHERE id = " + uId + ";";
                    SqlCommand upp_cmd = new SqlCommand(upp_que, DataConn.Connection);
                    upp_cmd.ExecuteNonQuery();
                    upp_cmd.Dispose();
                    MessageBox.Show("User's password has been updated!");
                    this.Dispose();
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Please insert you new password!");
            }
        }

        private void change_password_closing(object sender, FormClosingEventArgs e)
        {
            this.Dispose();
        }

        private void txtNewPass_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
