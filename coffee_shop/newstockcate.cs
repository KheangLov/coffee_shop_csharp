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
    public partial class newstockcate : Form
    {
        public newstockcate()
        {
            InitializeComponent();
        }
        Stock_Category st_cate = new Stock_Category();
        MyInter inter;

        private void newstockcate_Load(object sender, EventArgs e)
        {
            MyInter stcate_inter = st_cate;
            inter = stcate_inter;
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

        private void txtname_leave(object sender, EventArgs e)
        {
            st_cate.Name = txtname.Text.Trim();
        }

        private void txtDesc_leave(object sender, EventArgs e)
        {
            st_cate.Description = txtDesc.Text.Trim();
        }

        private void txtBranchID_leave(object sender, EventArgs e)
        {
            st_cate.Branch_ID = int.Parse(txtBranchID.Text.Trim());
        }
    }
}
