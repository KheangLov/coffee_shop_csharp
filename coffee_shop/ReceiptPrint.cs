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
    public partial class ReceiptPrint : Form
    {
        StaticPrintReceipt _spr;
        List<PrintReceipt> _list;
        public ReceiptPrint(StaticPrintReceipt sprs, List<PrintReceipt> lists)
        {
            InitializeComponent();
            _spr = sprs;
            _list = lists;
        }

        private void ReceiptPrint_Load(object sender, EventArgs e)
        {
            recipt1.SetDataSource(_list);
            recipt1.SetParameterValue("CompanyName", _spr.CompanyName);
            recipt1.SetParameterValue("CompanyBranch", _spr.BranchName);
            crystalReportViewer1.ReportSource = recipt1;
        }
    }
}
