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
    public partial class member_form : Form
    {
        int uId;
        string uName;
        public member_form(int id, string name)
        {
            uId = id;
            uName = name;
            InitializeComponent();
        }

        Member my_member = new Member();
        StringCapitalize sc = new StringCapitalize();
        MyInter inter;
        int user_id;
        int company_id;
        int memId;

        private void QueryMembers()
        {
            string sql = @"SELECT members.*, users.username AS user_name, companies.name AS company_name,
                branches.name AS branch_name FROM members 
                INNER JOIN users ON members.user_id = users.id 
                INNER JOIN companies ON members.company_id = companies.id
                INNER JOIN branches ON members.branch_id = branches.id
                WHERE members.user_id = " + uId + ";";
            SqlCommand com = new SqlCommand(sql, DataConn.Connection);
            SqlDataReader sqlr = com.ExecuteReader();
            while (sqlr.Read())
            {
                string[] member_info = { sc.ToCapitalize(sqlr["name"].ToString()), sc.ToCapitalize(sqlr["user_name"].ToString()), sc.ToCapitalize(sqlr["company_name"].ToString()), sc.ToCapitalize(sqlr["branch_name"].ToString()) };
                ListViewItem item = new ListViewItem(member_info);
                lvMember.Items.Add(item);
            }
            com.Dispose();
            sqlr.Close();
        }

        private void loadComboName()
        {
            string mName = "";
            DataConn.Connection.Open();
            string sql_member = "SELECT * FROM members;";
            SqlCommand com_mem = new SqlCommand(sql_member, DataConn.Connection);
            SqlDataReader read_mem = com_mem.ExecuteReader();
            int i = 0;
            while (read_mem.Read())
            {
                if (i == 0)
                    mName += "'" + read_mem["name"].ToString().ToLower() + "'";
                else
                    mName += ", '" + read_mem["name"].ToString().ToLower() + "'";
                i++;
            }
            com_mem.Dispose();
            read_mem.Close();
            Console.Write(mName);
            string sql = @"SELECT users.*, roles.name AS role_name FROM users 
                        INNER JOIN roles ON users.role_id = roles.id
                        WHERE LOWER(roles.name) IN('editor', 'user')
                        AND LOWER(users.username) NOT IN(" + mName + ");";
            SqlCommand sqld = new SqlCommand(sql, DataConn.Connection);
            SqlDataReader sqlr = sqld.ExecuteReader();
            while(sqlr.Read())
            {
                cbName.Items.Add(sc.ToCapitalize(sqlr["username"].ToString()));
            }
            sqld.Dispose();
            sqlr.Close();
            DataConn.Connection.Close();
        }

        private void loadComboUser()
        {
            DataConn.Connection.Open();
            string sql = @"SELECT users.*, roles.name AS role_name FROM users 
                        INNER JOIN roles ON users.role_id = roles.id
                        WHERE LOWER(roles.name) = 'admin';";
            SqlCommand sqld = new SqlCommand(sql, DataConn.Connection);
            SqlDataReader sqlr = sqld.ExecuteReader();
            while (sqlr.Read())
            {
                cbUser.Items.Add(sc.ToCapitalize(sqlr["username"].ToString()));
            }
            sqld.Dispose();
            sqlr.Close();
            DataConn.Connection.Close();
        }

        private void addUser()
        {
            if (cbUser.Text != "")
            {
                DataConn.Connection.Open();
                string sql = "SELECT * FROM users WHERE LOWER(username) = '" + cbUser.Text.ToLower() + "';";
                SqlCommand sqld = new SqlCommand(sql, DataConn.Connection);
                SqlDataReader sqlr = sqld.ExecuteReader();
                if (sqlr.Read())
                {
                    user_id = int.Parse(sqlr["id"].ToString());
                }
                else
                {
                    MessageBox.Show("Nothing found!");
                }
                sqlr.Close();
                sqld.Dispose();
                DataConn.Connection.Close();
            }
        }

        private void loadComboCompany()
        {
            DataConn.Connection.Open();
            string sql = @"SELECT * FROM companies WHERE user_id = " + user_id + ";";
            SqlCommand sqld = new SqlCommand(sql, DataConn.Connection);
            SqlDataReader sqlr = sqld.ExecuteReader();
            while (sqlr.Read())
            {
                cbCompany.Items.Add(sc.ToCapitalize(sqlr["name"].ToString()));
            }
            sqld.Dispose();
            sqlr.Close();
            DataConn.Connection.Close();
        }

        private void addCompany()
        {
            if (cbCompany.Text != "")
            {
                DataConn.Connection.Open();
                string sql = "SELECT * FROM companies WHERE LOWER(name) = '" + cbCompany.Text.ToLower() + "';";
                SqlCommand sqld = new SqlCommand(sql, DataConn.Connection);
                SqlDataReader sqlr = sqld.ExecuteReader();
                if (sqlr.Read())
                {
                    company_id = int.Parse(sqlr["id"].ToString());
                }
                else
                {
                    MessageBox.Show("Nothing found!");
                }
                sqlr.Close();
                sqld.Dispose();
                DataConn.Connection.Close();
            }
        }

        private void loadComboBranch()
        {
            DataConn.Connection.Open();
            string sql = @"SELECT * FROM branches WHERE company_id = " + company_id + ";";
            SqlCommand sqld = new SqlCommand(sql, DataConn.Connection);
            SqlDataReader sqlr = sqld.ExecuteReader();
            while (sqlr.Read())
            {
                cbBranch.Items.Add(sc.ToCapitalize(sqlr["name"].ToString()));
            }
            sqld.Dispose();
            sqlr.Close();
            DataConn.Connection.Close();
        }

        private void addBranch()
        {
            DataConn.Connection.Open();
            string sql = @"SELECT * FROM branches WHERE LOWER(name) = '" + cbBranch.SelectedItem.ToString().ToLower() + "';";
            SqlCommand sqld = new SqlCommand(sql, DataConn.Connection);
            SqlDataReader sqlr = sqld.ExecuteReader();
            if (sqlr.Read())
            {
                my_member.BranchId = int.Parse(sqlr["id"].ToString());
            }
            sqld.Dispose();
            sqlr.Close();
            DataConn.Connection.Close();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void member_form_Load(object sender, EventArgs e)
        {
            btnEdit.Enabled = false;
            btnDelete.Enabled = false;
            MyInter member_inter = my_member;
            inter = member_inter;
            loadComboName();
            if (cbName.Items.Count > 0)
                cbName.SelectedIndex = 0;
            loadComboUser();
            cbUser.SelectedItem = sc.ToCapitalize(uName);
            addUser();
            loadComboCompany();
            if (cbCompany.Items.Count > 0)
                cbCompany.SelectedIndex = 0;
            addCompany();
            loadComboBranch();
            if (cbBranch.Items.Count > 0)
                cbBranch.SelectedIndex = 0;
            DataConn.Connection.Open();
            QueryMembers();
            DataConn.Connection.Close();
        }

        private void cbUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            addUser();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                my_member.Name = cbName.SelectedItem.ToString().Trim();
                my_member.UserId = user_id;
                my_member.CompanyId = company_id;
                addBranch();
                DataConn.Connection.Open();
                inter.insert();
                MessageBox.Show("Insert successfully!");
                lvMember.Items.Clear();
                QueryMembers();
                DataConn.Connection.Close();
                cbName.Items.Clear();
                loadComboName();
                if (cbName.Items.Count > 0)
                    cbName.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void lvMember_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvMember.SelectedItems.Count != 0)
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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lvMember.SelectedItems.Count != 0)
            {
                btnEdit.Enabled = false;
                if (MessageBox.Show("Are you sure, you want to delete this member?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    ListViewItem lvi = lvMember.SelectedItems[0];
                    int val = 0;
                    string proName = lvi.SubItems[0].Text;
                    string del_que = "DELETE FROM members WHERE LOWER(name) = '" + proName.ToLower() + "';";
                    SqlCommand del_com = new SqlCommand(del_que, DataConn.Connection);
                    val = del_com.ExecuteNonQuery();
                    del_com.Dispose();
                    MessageBox.Show("Member has been deleted!");
                    lvMember.Items.Clear();
                    QueryMembers();
                    btnDelete.Enabled = false;
                    btnEdit.Enabled = false;
                }
                else
                {
                    MessageBox.Show("No Member was deleted!", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (lvMember.SelectedItems.Count != 0)
            {
                btnDelete.Enabled = false;
                if (btnEdit.Text.ToLower() == "edit")
                {
                    try
                    {
                        ListViewItem item = lvMember.SelectedItems[0];
                        string lv_members = item.SubItems[0].Text;
                        string sql = @"SELECT members.*, users.name AS user_name, companies.name AS company_name,
                                branches.name AS branch_name FROM members 
                                INNER JOIN users ON members.user_id = users.id 
                                INNER JOIN companies ON members.company_id = companies.id
                                INNER JOIN branches ON members.branch_id = branches.id
                                WHERE LOWER(members.name) = '" + lv_members.ToLower() + "';";
                        SqlCommand command = new SqlCommand(sql, DataConn.Connection);
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            memId = int.Parse(reader["id"].ToString());
                            cbName.SelectedItem = reader["name"].ToString();
                            cbUser.SelectedItem = reader["user_name"].ToString();
                            cbCompany.SelectedItem = reader["company_name"].ToString();
                            cbBranch.SelectedItem = reader["branch_name"].ToString();
                            btnEdit.Text = "Update";
                        }
                        command.Dispose();
                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else if (btnEdit.Text.ToLower() == "update")
                {
                    try
                    {
                        my_member.Name = cbName.SelectedItem.ToString().Trim();
                        my_member.UserId = user_id;
                        my_member.CompanyId = company_id;
                        addBranch();
                        inter.update(memId);
                        MessageBox.Show("Update Successfully!");
                        lvMember.Items.Clear();
                        QueryMembers();
                        btnEdit.Text = "Edit";
                        btnEdit.Enabled = false;
                        btnDelete.Enabled = false;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void cbCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            addCompany();
        }

        private void cbBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            addBranch();
        }
    }
}
