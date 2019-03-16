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
    public partial class new_company : Form
    {
        public new_company()
        {
            InitializeComponent();
        }

        Company new_com = new Company();
        MyInter inter;
         
        bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                inter.insert();
                MessageBox.Show("Insert successfull!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtName_Leave(object sender, EventArgs e)
        {
            new_com.Name = txtName.Text.Trim();
        }

        private void new_company_Load(object sender, EventArgs e)
        {
            DataConn.Connection.Open();
            MyInter com_inter = new_com;
            inter = com_inter;
        }

        private void txtAddress_Leave(object sender, EventArgs e)
        {
            new_com.Address = txtAddress.Text.Trim();
        }

        private void txtEmail_Leave(object sender, EventArgs e)
        {
            if (IsValidEmail(txtEmail.Text.Trim()))
                new_com.Email = txtEmail.Text.Trim();
            else
                MessageBox.Show("Invalid Email!");
        }

        private void txtPhone_Leave(object sender, EventArgs e)
        {
            new_com.Phone = txtPhone.Text.Trim();
        }

        private void txtUserID_Leave(object sender, EventArgs e)
        {
            new_com.UserID = txtUserID.Text.Trim();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
