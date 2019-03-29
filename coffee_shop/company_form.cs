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
    public partial class company_form : Form
    {
        public company_form()
        {
            InitializeComponent();
        }
        Company my_company = new Company();
        StringCapitalize sc = new StringCapitalize();
        MyInter inter;
        int companyId;

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

        private void loadComboUser()
        {
            string get_users = "SELECT users.username, roles.name FROM users INNER JOIN roles ON users.role_id = roles.id WHERE roles.name = 'admin';";
            SqlCommand sqld = new SqlCommand(get_users, DataConn.Connection);
            SqlDataReader sqlr = sqld.ExecuteReader();
            while (sqlr.Read())
            {
                cbUser.Items.Add(sc.ToCapitalize(sqlr["username"].ToString()));
            }
            sqld.Dispose();
            sqlr.Close();
        }

        private void addCompanyUser()
        {
            string company_user = "SELECT id, username FROM users WHERE username = '" + cbUser.Text.Trim() + "';";
            SqlCommand sqld = new SqlCommand(company_user, DataConn.Connection);
            SqlDataReader sqlr = sqld.ExecuteReader();
            if(sqlr.Read())
            {
                my_company.UserID = int.Parse(sqlr["id"].ToString());
            }
            else
            {
                MessageBox.Show("Nothing found!");
            }
            sqld.Dispose();
            sqlr.Close();
        }

        private void QueryCompanies()
        {
            string query = "SELECT * FROM companies INNER JOIN users ON companies.user_id = users.id ORDER BY name;";
            SqlCommand sqld = new SqlCommand(query, DataConn.Connection);
            SqlDataReader sqlr = sqld.ExecuteReader();
            while (sqlr.Read())
            {
                string[] company_info = { sc.ToCapitalize(sqlr["name"].ToString()), sqlr["address"].ToString(), sqlr["email"].ToString(), sqlr["phone"].ToString(), sc.ToCapitalize(sqlr["username"].ToString()) };
                ListViewItem item = new ListViewItem(company_info);
                lvCompany.Items.Add(item);
            }
            sqld.Dispose();
            sqlr.Close();
        }

        private void company_form_Load(object sender, EventArgs e)
        {
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
            txtName.Focus();
            DataConn.Connection.Open();
            MyInter company_inter = my_company;
            inter = company_inter;
            try
            {
                QueryCompanies();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            loadComboUser();
            cbUser.SelectedIndex = 0;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DataConn.Connection.Close();
            this.Dispose();
        }

        private void txtName_leave(object sender, EventArgs e)
        {
            my_company.Name = txtName.Text.Trim();
        }

        private void txtEmail_leave(object sender, EventArgs e)
        {
            if (IsValidEmail(txtEmail.Text.Trim()))
                my_company.Email = txtEmail.Text.Trim();
            else
                MessageBox.Show("Invalid Email!");
        }

        private void txtAddress_leave(object sender, EventArgs e)
        {
            my_company.Address = txtAddress.Text.Trim();
        }

        private void txtPhone_leave(object sender, EventArgs e)
        {
            my_company.Phone = txtPhone.Text.Trim();
        }

        private void cbUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            addCompanyUser();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if(txtName.Text != "" && txtPhone.Text != "")
                {
                    string query_cpn = "SELECT COUNT(*) FROM companies WHERE name = '" + txtName.Text.Trim() + "';";
                    SqlCommand cpn_ex = new SqlCommand(query_cpn, DataConn.Connection);
                    int find_cpn = Convert.ToInt16(cpn_ex.ExecuteScalar());
                    if(find_cpn != 0)
                    {
                        MessageBox.Show("Company already existed!");
                        txtName.Focus();
                    }
                    else
                    {
                        inter.insert();
                        MessageBox.Show("Insert successful!");
                        ClearTextBoxes(groupBox1);
                        lvCompany.Items.Clear();
                        QueryCompanies();
                    }
                    cpn_ex.Dispose();
                }
                else
                {
                    MessageBox.Show("Name and Phone can't be blank!");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if(lvCompany.SelectedItems.Count != 0)
            {
                if(btnEdit.Text.ToLower() == "edit")
                {
                    ListViewItem list_item = lvCompany.SelectedItems[0];
                    string name = list_item.SubItems[0].Text;
                    string sql = "SELECT * FROM companies INNER JOIN users ON companies.user_id = users.id WHERE LOWER(name) = '" + name.ToLower() + "';";
                    SqlCommand upd_cmd = new SqlCommand(sql, DataConn.Connection);
                    SqlDataReader upd_rd = upd_cmd.ExecuteReader();
                    while(upd_rd.Read())
                    {
                        companyId = int.Parse(upd_rd["id"].ToString());
                        txtName.Text = upd_rd["name"].ToString();
                        txtAddress.Text = upd_rd["address"].ToString();
                        txtEmail.Text = upd_rd["email"].ToString();
                        txtPhone.Text = upd_rd["phone"].ToString();
                        cbUser.Text = sc.ToCapitalize(upd_rd["username"].ToString());
                    }
                    upd_cmd.Dispose();
                    upd_rd.Close();
                    btnEdit.Text = "Update";
                }
                else if(btnEdit.Text.ToLower() == "update")
                {
                    inter.update(companyId);
                    MessageBox.Show("Company has been updated!");
                    ClearTextBoxes(groupBox1);
                    lvCompany.Items.Clear();
                    QueryCompanies();
                    btnEdit.Text = "Edit";
                }
            }
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            my_company.Name = txtName.Text.Trim();
        }

        private void txtAddress_TextChanged(object sender, EventArgs e)
        {
            my_company.Address = txtAddress.Text.Trim();
        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {
            my_company.Email = txtEmail.Text.Trim();
        }

        private void txtPhone_TextChanged(object sender, EventArgs e)
        {
            my_company.Phone = txtPhone.Text.Trim();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if(lvCompany.SelectedItems.Count != 0)
            {
                if (MessageBox.Show("Are you sure, you want to delete this company?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    ListViewItem del_item = lvCompany.SelectedItems[0];
                    int val = 0;
                    string cpn_name = del_item.SubItems[0].Text;
                    string del_sql = "DELETE FROM companies WHERE name = '" + cpn_name + "';";
                    SqlCommand del_cmd = new SqlCommand(del_sql, DataConn.Connection);
                    val = del_cmd.ExecuteNonQuery();
                    del_cmd.Dispose();
                    MessageBox.Show("Company has been deleted!");
                    lvCompany.Items.Clear();
                    QueryCompanies();
                }
                else
                {
                    MessageBox.Show("No company was deleted!", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void lvCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvCompany.SelectedItems.Count != 0)
            {
                btnEdit.Enabled = true;
                btnDelete.Enabled = true;
            }
            else
            {
                btnDelete.Enabled = false;
                btnEdit.Enabled = false;
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            lvCompany.Items.Clear();
            string query = "SELECT * FROM companies INNER JOIN users ON companies.user_id = users.id AND name LIKE '%" + txtSearch.Text.Trim() + "%' ORDER BY name;";
            SqlCommand sqld = new SqlCommand(query, DataConn.Connection);
            SqlDataReader sqlr = sqld.ExecuteReader();
            while (sqlr.Read())
            {
                string[] company_info = { sc.ToCapitalize(sqlr["name"].ToString()), sqlr["address"].ToString(), sqlr["email"].ToString(), sqlr["phone"].ToString(), sc.ToCapitalize(sqlr["username"].ToString()) };
                ListViewItem item = new ListViewItem(company_info);
                lvCompany.Items.Add(item);
            }
            sqld.Dispose();
            sqlr.Close();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void company_form_FormClosing(object sender, FormClosingEventArgs e)
        {
            DataConn.Connection.Close();
            this.Dispose();
        }
    }
}
