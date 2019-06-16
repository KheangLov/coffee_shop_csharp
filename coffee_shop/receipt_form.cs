﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace coffee_shop
{
    public partial class receipt_form : Form
    {
        private bool Drag;
        private int MouseX;
        private int MouseY;

        private const int WM_NCHITTEST = 0x84;
        private const int HTCLIENT = 0x1;
        private const int HTCAPTION = 0x2;

        private bool m_aeroEnabled;

        private const int CS_DROPSHADOW = 0x00020000;
        private const int WM_NCPAINT = 0x0085;
        private const int WM_ACTIVATEAPP = 0x001C;

        [System.Runtime.InteropServices.DllImport("dwmapi.dll")]
        public static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS pMarInset);
        [System.Runtime.InteropServices.DllImport("dwmapi.dll")]
        public static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);
        [System.Runtime.InteropServices.DllImport("dwmapi.dll")]

        public static extern int DwmIsCompositionEnabled(ref int pfEnabled);
        [System.Runtime.InteropServices.DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect,
            int nTopRect,
            int nRightRect,
            int nBottomRect,
            int nWidthEllipse,
            int nHeightEllipse
            );

        public struct MARGINS
        {
            public int leftWidth;
            public int rightWidth;
            public int topHeight;
            public int bottomHeight;
        }
        protected override CreateParams CreateParams
        {
            get
            {
                m_aeroEnabled = CheckAeroEnabled();
                CreateParams cp = base.CreateParams;
                if (!m_aeroEnabled)
                    cp.ClassStyle |= CS_DROPSHADOW; return cp;
            }
        }
        private bool CheckAeroEnabled()
        {
            if (Environment.OSVersion.Version.Major >= 6)
            {
                int enabled = 0; DwmIsCompositionEnabled(ref enabled);
                return (enabled == 1) ? true : false;
            }
            return false;
        }
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_NCPAINT:
                    if (m_aeroEnabled)
                    {
                        var v = 2;
                        DwmSetWindowAttribute(this.Handle, 2, ref v, 4);
                        MARGINS margins = new MARGINS()
                        {
                            bottomHeight = 1,
                            leftWidth = 0,
                            rightWidth = 0,
                            topHeight = 0
                        }; DwmExtendFrameIntoClientArea(this.Handle, ref margins);
                    }
                    break;
                default: break;
            }
            base.WndProc(ref m);
            if (m.Msg == WM_NCHITTEST && (int)m.Result == HTCLIENT) m.Result = (IntPtr)HTCAPTION;
        }
        private void PanelMove_MouseDown(object sender, MouseEventArgs e)
        {
            Drag = true;
            MouseX = Cursor.Position.X - this.Left;
            MouseY = Cursor.Position.Y - this.Top;
        }
        private void PanelMove_MouseMove(object sender, MouseEventArgs e)
        {
            if (Drag)
            {
                this.Top = Cursor.Position.Y - MouseY;
                this.Left = Cursor.Position.X - MouseX;
            }
        }
        private void PanelMove_MouseUp(object sender, MouseEventArgs e) { Drag = false; }

        public receipt_form()
        {
            InitializeComponent();
            m_aeroEnabled = false;
        }

        StringCapitalize sc = new StringCapitalize();

        public void QueryReceipt()
        {
            DataConn.Connection.Open();
            string sql = "SELECT number, COUNT(number) AS count_number FROM receipts GROUP BY number;";
            SqlCommand sqld = new SqlCommand(sql, DataConn.Connection);
            SqlDataReader sqlr = sqld.ExecuteReader();
            while(sqlr.Read())
            {
                string[] receipt_info = { sqlr["number"].ToString(), sqlr["count_number"].ToString() };
                ListViewItem item = new ListViewItem(receipt_info);
                lvReceipt.Items.Add(item);
            }
            sqld.Dispose();
            sqlr.Close();
            DataConn.Connection.Close();
        }

        private void receipt_form_Load(object sender, EventArgs e)
        {
            btnPrint.Enabled = false;
            QueryReceipt();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if(lvReceipt.SelectedItems.Count > 0)
            {
                List<PrintReceipt> printRec = new List<PrintReceipt>();
                StaticPrintReceipt spr = new StaticPrintReceipt();
                ListViewItem item = lvReceipt.SelectedItems[0];
                string recNum = item.SubItems[0].Text;
                DataConn.Connection.Open();
                string sql = @"SELECT receipts.*, companies.name AS company_name, branches.name AS branch_name, 
                            products.name AS product_name, employees.fullname AS emp_name
                            FROM receipts
                            INNER JOIN companies ON receipts.company_id = companies.id
                            INNER JOIN branches ON receipts.branch_id = branches.id
                            INNER JOIN products ON receipts.product_id = products.id
                            INNER JOIN employees ON receipts.employee_id = employees.id WHERE receipts.number = '" + recNum + "';";
                SqlCommand sqld = new SqlCommand(sql, DataConn.Connection);
                SqlDataReader sqlr = sqld.ExecuteReader();
                while(sqlr.Read())
                {
                    spr.Number = sqlr["number"].ToString();
                    spr.WaitingNumber = int.Parse(sqlr["waiting_number"].ToString());
                    spr.CompanyName = sqlr["company_name"].ToString();
                    spr.BranchName = sqlr["branch_name"].ToString();
                    spr.EmployeeName = sqlr["emp_name"].ToString();
                    spr.Date = sqlr["date"].ToString();
                    spr.Discount = double.Parse(sqlr["discount"].ToString());
                    PrintReceipt pr = new PrintReceipt();
                    pr.ProductName = sqlr["product_name"].ToString().ToUpper();
                    pr.Qty = double.Parse(sqlr["qty"].ToString());
                    pr.Price = double.Parse(sqlr["price"].ToString());
                    pr.Discount = double.Parse(sqlr["discount"].ToString());
                    printRec.Add(pr);
                }
                sqld.Dispose();
                sqlr.Close();
                DataConn.Connection.Close();
                new ReceiptPrint(spr, printRec).ShowDialog();
            }
        }

        private void lvReceipt_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(lvReceipt.SelectedItems.Count > 0)
            {
                btnPrint.Enabled = true;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            lvReceipt.Items.Clear();
            DataConn.Connection.Open();
            string sql_search = "SELECT number, COUNT(number) AS count_num FROM receipts WHERE number LIKE '%" + txtSearch.Text + "%' GROUP BY number;";
            SqlCommand srch_cmd = new SqlCommand(sql_search, DataConn.Connection);
            SqlDataReader srch_read = srch_cmd.ExecuteReader();
            while(srch_read.Read())
            {
                string[] recItem = { srch_read["number"].ToString(), srch_read["count_num"].ToString() };
                ListViewItem item = new ListViewItem(recItem);
                lvReceipt.Items.Add(item);
            }
            srch_cmd.Dispose();
            srch_read.Close();
            DataConn.Connection.Close();
        }
    }
}