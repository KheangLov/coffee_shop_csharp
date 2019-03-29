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
    public partial class branch_form : Form
    {
        public branch_form()
        {
            InitializeComponent();
        }
        Branch my_branch = new Branch();
        MyInter inter;
        StringCapitalize sc = new StringCapitalize();
        int branchId;

        private void QueryBranches()
        {
            string query = @"SELECT branches.*, companies.name AS company_name FROM branches
                            INNER JOIN companies ON branches.company_id = companies.id;";
            SqlCommand sqld = new SqlCommand(query, DataConn.Connection);
            SqlDataReader sqlr = sqld.ExecuteReader();
            while (sqlr.Read())
            {
                string[] branch_info = { sc.ToCapitalize(sqlr["name"].ToString()), sqlr["email"].ToString(), sqlr["phone"].ToString(), sqlr["address"].ToString(), sc.ToCapitalize(sqlr["company_name"].ToString()) };
                ListViewItem item = new ListViewItem(branch_info);
                lvBranch.Items.Add(item);
            }
            sqld.Dispose();
            sqlr.Close();
        }

        private void loadComboCompany()
        {
            string sql = "SELECT id, name FROM companies;";
            SqlCommand sqld = new SqlCommand(sql, DataConn.Connection);
            SqlDataReader sqlr = sqld.ExecuteReader();
            while (sqlr.Read())
            {
                cbCompany.Items.Add(sc.ToCapitalize(sqlr["name"].ToString()));
            }
            sqlr.Close();
            sqld.Dispose();
        }

        private void addCompany()
        {
            string sql = "SELECT * FROM companies WHERE LOWER(name) = '" + cbCompany.Text.ToLower() + "';";
            SqlCommand sqld = new SqlCommand(sql, DataConn.Connection);
            SqlDataReader sqlr = sqld.ExecuteReader();
            if (sqlr.Read())
            {
                my_branch.CompanyId = int.Parse(sqlr["id"].ToString());
            }
            else
            {
                MessageBox.Show("Nothing found!");
            }
            sqlr.Close();
            sqld.Dispose();
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

        private void branch_form_Load(object sender, EventArgs e)
        {
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
            DataConn.Connection.Open();
            MyInter branch_inter = my_branch;
            inter = branch_inter;
            loadComboCompany();
            cbCompany.SelectedIndex = 0;
            QueryBranches();
        }

        private void txtName_Leave(object sender, EventArgs e)
        {
            my_branch.Name = txtName.Text.Trim();
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            my_branch.Name = txtName.Text.Trim();
        }

        private void txtEmail_Leave(object sender, EventArgs e)
        {
            my_branch.Email = txtEmail.Text.Trim();
        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {
            my_branch.Email = txtEmail.Text.Trim();
        }

        private void txtPhone_Leave(object sender, EventArgs e)
        {
            my_branch.Phone = txtPhone.Text.Trim();
        }

        private void txtPhone_TextChanged(object sender, EventArgs e)
        {
            my_branch.Phone = txtPhone.Text.Trim();
        }

        private void txtAddress_Leave(object sender, EventArgs e)
        {
            my_branch.Address = txtAddress.Text.Trim();
        }

        private void txtAddress_TextChanged(object sender, EventArgs e)
        {
            my_branch.Address = txtAddress.Text.Trim();
        }

        private void cbCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            addCompany();
        }

        private void lvBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvBranch.SelectedItems.Count != 0)
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
            lvBranch.Items.Clear();
            string sql = @"SELECT branches.*, companies.name AS company_name FROM branches
                        INNER JOIN companies ON branches.company_id = companies.id
                        WHERE branches.name LIKE '%" + txtSearch.Text.Trim() + "%'; ";
            SqlCommand sqld = new SqlCommand(sql, DataConn.Connection);
            SqlDataReader sqlr = sqld.ExecuteReader();
            while(sqlr.Read())
            {
                string[] branch_info = { sc.ToCapitalize(sqlr["name"].ToString()), sqlr["email"].ToString(), sqlr["phone"].ToString(), sqlr["address"].ToString(), sc.ToCapitalize(sqlr["company_name"].ToString()) };
                ListViewItem item = new ListViewItem(branch_info);
                lvBranch.Items.Add(item);
            }
            sqld.Dispose();
            sqlr.Close();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DataConn.Connection.Close();
            this.Dispose();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lvBranch.SelectedItems.Count != 0)
            {
                if (MessageBox.Show("Are you sure, you want to delete this branch?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    ListViewItem del_item = lvBranch.SelectedItems[0];
                    int val = 0;
                    string name = del_item.SubItems[0].Text;
                    string del_sql = "DELETE FROM branches WHERE name = '" + name + "';";
                    SqlCommand del_cmd = new SqlCommand(del_sql, DataConn.Connection);
                    val = del_cmd.ExecuteNonQuery();
                    del_cmd.Dispose();
                    MessageBox.Show("Branch has been deleted!");
                    lvBranch.Items.Clear();
                    QueryBranches();
                }
                else
                {
                    MessageBox.Show("No branch was deleted!", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtName.Text != "" && txtAddress.Text != "")
                { 
                    inter.insert();
                    MessageBox.Show("Insert Successfully!");
                    ClearTextBoxes(groupBox1);
                    txtName.Focus();
                    lvBranch.Clear();
                    QueryBranches();
                }
                else
                {
                    MessageBox.Show("Name and Address can't be blank!");
                    txtName.Focus();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if(lvBranch.SelectedItems.Count != 0)
            {
                if (btnEdit.Text.ToLower() == "edit")
                {
                    ListViewItem list_item = lvBranch.SelectedItems[0];
                    string name = list_item.SubItems[0].Text;
                    string sql = "SELECT branches.*, companies.name AS company_name FROM branches INNER JOIN companies ON branches.company_id = companies.id WHERE LOWER(branches.name) = '" + name.ToLower() + "';";
                    SqlCommand upd_cmd = new SqlCommand(sql, DataConn.Connection);
                    SqlDataReader upd_rd = upd_cmd.ExecuteReader();
                    while (upd_rd.Read())
                    {
                        branchId = int.Parse(upd_rd["id"].ToString());
                        txtName.Text = upd_rd["name"].ToString();
                        txtAddress.Text = upd_rd["address"].ToString();
                        txtEmail.Text = upd_rd["email"].ToString();
                        txtPhone.Text = upd_rd["phone"].ToString();
                        cbCompany.Text = sc.ToCapitalize(upd_rd["company_name"].ToString());
                    }
                    upd_cmd.Dispose();
                    upd_rd.Close();
                    btnEdit.Text = "Update";
                }
                else if (btnEdit.Text.ToLower() == "update")
                {
                    inter.update(branchId);
                    MessageBox.Show("Branch has been updated!");
                    ClearTextBoxes(groupBox1);
                    lvBranch.Items.Clear();
                    QueryBranches();
                }
            }
        }
    }
}
