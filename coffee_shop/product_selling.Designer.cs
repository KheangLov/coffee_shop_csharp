﻿namespace coffee_shop
{
    partial class product_selling
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lvProducts = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pbProduct = new System.Windows.Forms.PictureBox();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbBranch = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.cbCom = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.cbEmp = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSubTotal = new System.Windows.Forms.TextBox();
            this.cbCurrency = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtDisPrice = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtTotal = new System.Windows.Forms.TextBox();
            this.txtChange = new System.Windows.Forms.TextBox();
            this.txtDiscount = new System.Windows.Forms.TextBox();
            this.cbType = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtReceive = new System.Windows.Forms.TextBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnPay = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.lvCart = new System.Windows.Forms.ListView();
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnDel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbProduct)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvProducts
            // 
            this.lvProducts.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.lvProducts.Font = new System.Drawing.Font("Montserrat", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvProducts.FullRowSelect = true;
            this.lvProducts.GridLines = true;
            this.lvProducts.Location = new System.Drawing.Point(12, 311);
            this.lvProducts.Name = "lvProducts";
            this.lvProducts.Size = new System.Drawing.Size(299, 247);
            this.lvProducts.TabIndex = 0;
            this.lvProducts.UseCompatibleStateImageBehavior = false;
            this.lvProducts.View = System.Windows.Forms.View.Details;
            this.lvProducts.SelectedIndexChanged += new System.EventHandler(this.lvProducts_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 92;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Price";
            this.columnHeader2.Width = 118;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Type";
            this.columnHeader3.Width = 82;
            // 
            // pbProduct
            // 
            this.pbProduct.Location = new System.Drawing.Point(675, 73);
            this.pbProduct.Name = "pbProduct";
            this.pbProduct.Size = new System.Drawing.Size(312, 485);
            this.pbProduct.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbProduct.TabIndex = 1;
            this.pbProduct.TabStop = false;
            // 
            // txtSearch
            // 
            this.txtSearch.Font = new System.Drawing.Font("Montserrat", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearch.Location = new System.Drawing.Point(676, 24);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(311, 37);
            this.txtSearch.TabIndex = 2;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbBranch);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.cbCom);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.cbEmp);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtSubTotal);
            this.groupBox1.Controls.Add(this.cbCurrency);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtDisPrice);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.txtTotal);
            this.groupBox1.Controls.Add(this.txtChange);
            this.groupBox1.Controls.Add(this.txtDiscount);
            this.groupBox1.Controls.Add(this.cbType);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtReceive);
            this.groupBox1.Font = new System.Drawing.Font("Montserrat", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(657, 251);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Product Selling";
            // 
            // cbBranch
            // 
            this.cbBranch.Font = new System.Drawing.Font("Montserrat", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbBranch.FormattingEnabled = true;
            this.cbBranch.Location = new System.Drawing.Point(449, 163);
            this.cbBranch.Name = "cbBranch";
            this.cbBranch.Size = new System.Drawing.Size(185, 26);
            this.cbBranch.TabIndex = 36;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Montserrat", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(447, 141);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(59, 18);
            this.label11.TabIndex = 35;
            this.label11.Text = "Branch:";
            // 
            // cbCom
            // 
            this.cbCom.Font = new System.Drawing.Font("Montserrat", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbCom.FormattingEnabled = true;
            this.cbCom.Location = new System.Drawing.Point(449, 112);
            this.cbCom.Name = "cbCom";
            this.cbCom.Size = new System.Drawing.Size(185, 26);
            this.cbCom.TabIndex = 34;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Montserrat", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(447, 90);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(75, 18);
            this.label10.TabIndex = 33;
            this.label10.Text = "Company:";
            // 
            // cbEmp
            // 
            this.cbEmp.Font = new System.Drawing.Font("Montserrat", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbEmp.FormattingEnabled = true;
            this.cbEmp.Location = new System.Drawing.Point(450, 61);
            this.cbEmp.Name = "cbEmp";
            this.cbEmp.Size = new System.Drawing.Size(185, 26);
            this.cbEmp.TabIndex = 32;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Montserrat", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(448, 39);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(78, 18);
            this.label9.TabIndex = 31;
            this.label9.Text = "Employee:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Montserrat", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(242, 90);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 18);
            this.label2.TabIndex = 30;
            this.label2.Text = "Sub Total:";
            // 
            // txtSubTotal
            // 
            this.txtSubTotal.Location = new System.Drawing.Point(245, 112);
            this.txtSubTotal.Name = "txtSubTotal";
            this.txtSubTotal.Size = new System.Drawing.Size(185, 27);
            this.txtSubTotal.TabIndex = 29;
            // 
            // cbCurrency
            // 
            this.cbCurrency.Font = new System.Drawing.Font("Montserrat", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbCurrency.FormattingEnabled = true;
            this.cbCurrency.Items.AddRange(new object[] {
            "Dollar",
            "Riel"});
            this.cbCurrency.Location = new System.Drawing.Point(245, 60);
            this.cbCurrency.Name = "cbCurrency";
            this.cbCurrency.Size = new System.Drawing.Size(185, 26);
            this.cbCurrency.TabIndex = 28;
            this.cbCurrency.SelectedIndexChanged += new System.EventHandler(this.cbCurrency_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Montserrat", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(18, 192);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 18);
            this.label1.TabIndex = 27;
            this.label1.Text = "Dicount Price:";
            // 
            // txtDisPrice
            // 
            this.txtDisPrice.Location = new System.Drawing.Point(21, 213);
            this.txtDisPrice.Name = "txtDisPrice";
            this.txtDisPrice.Size = new System.Drawing.Size(185, 27);
            this.txtDisPrice.TabIndex = 26;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Montserrat", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(242, 39);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 18);
            this.label3.TabIndex = 25;
            this.label3.Text = "Currency:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Montserrat", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(242, 192);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(63, 18);
            this.label8.TabIndex = 19;
            this.label8.Text = "Change:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Montserrat", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(242, 141);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(62, 18);
            this.label7.TabIndex = 18;
            this.label7.Text = "Receive:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Montserrat", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(18, 39);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(42, 18);
            this.label6.TabIndex = 17;
            this.label6.Text = "Total:";
            // 
            // txtTotal
            // 
            this.txtTotal.Location = new System.Drawing.Point(20, 60);
            this.txtTotal.Name = "txtTotal";
            this.txtTotal.Size = new System.Drawing.Size(185, 27);
            this.txtTotal.TabIndex = 16;
            this.txtTotal.TextChanged += new System.EventHandler(this.txtTotal_TextChanged);
            // 
            // txtChange
            // 
            this.txtChange.Location = new System.Drawing.Point(245, 213);
            this.txtChange.Name = "txtChange";
            this.txtChange.Size = new System.Drawing.Size(185, 27);
            this.txtChange.TabIndex = 15;
            // 
            // txtDiscount
            // 
            this.txtDiscount.Location = new System.Drawing.Point(21, 162);
            this.txtDiscount.Name = "txtDiscount";
            this.txtDiscount.Size = new System.Drawing.Size(185, 27);
            this.txtDiscount.TabIndex = 14;
            this.txtDiscount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtDiscount_KeyPress);
            // 
            // cbType
            // 
            this.cbType.Font = new System.Drawing.Font("Montserrat", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbType.FormattingEnabled = true;
            this.cbType.Items.AddRange(new object[] {
            "In place",
            "Take away"});
            this.cbType.Location = new System.Drawing.Point(20, 112);
            this.cbType.Name = "cbType";
            this.cbType.Size = new System.Drawing.Size(185, 26);
            this.cbType.TabIndex = 13;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Montserrat", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(18, 90);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(42, 18);
            this.label5.TabIndex = 12;
            this.label5.Text = "Type:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Montserrat", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(18, 141);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 18);
            this.label4.TabIndex = 9;
            this.label4.Text = "Discount:";
            // 
            // txtReceive
            // 
            this.txtReceive.Location = new System.Drawing.Point(245, 162);
            this.txtReceive.Name = "txtReceive";
            this.txtReceive.Size = new System.Drawing.Size(185, 27);
            this.txtReceive.TabIndex = 8;
            this.txtReceive.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtReceive_KeyPress);
            // 
            // btnAdd
            // 
            this.btnAdd.Font = new System.Drawing.Font("Montserrat", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdd.Location = new System.Drawing.Point(12, 270);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(100, 35);
            this.btnAdd.TabIndex = 4;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnPay
            // 
            this.btnPay.Font = new System.Drawing.Font("Montserrat", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPay.Location = new System.Drawing.Point(118, 270);
            this.btnPay.Name = "btnPay";
            this.btnPay.Size = new System.Drawing.Size(100, 35);
            this.btnPay.TabIndex = 5;
            this.btnPay.Text = "Pay";
            this.btnPay.UseVisualStyleBackColor = true;
            this.btnPay.Click += new System.EventHandler(this.btnPay_Click);
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("Montserrat", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.Location = new System.Drawing.Point(330, 270);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(100, 35);
            this.btnExit.TabIndex = 6;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // lvCart
            // 
            this.lvCart.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7});
            this.lvCart.Font = new System.Drawing.Font("Montserrat", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvCart.FullRowSelect = true;
            this.lvCart.GridLines = true;
            this.lvCart.Location = new System.Drawing.Point(317, 311);
            this.lvCart.Name = "lvCart";
            this.lvCart.Size = new System.Drawing.Size(352, 247);
            this.lvCart.TabIndex = 7;
            this.lvCart.UseCompatibleStateImageBehavior = false;
            this.lvCart.View = System.Windows.Forms.View.Details;
            this.lvCart.SelectedIndexChanged += new System.EventHandler(this.lvCart_SelectedIndexChanged);
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Name";
            this.columnHeader4.Width = 79;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Price";
            this.columnHeader5.Width = 95;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Quantity";
            this.columnHeader6.Width = 74;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Type";
            this.columnHeader7.Width = 95;
            // 
            // btnDel
            // 
            this.btnDel.Font = new System.Drawing.Font("Montserrat", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDel.Location = new System.Drawing.Point(224, 270);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(100, 35);
            this.btnDel.TabIndex = 8;
            this.btnDel.Text = "Delete";
            this.btnDel.UseVisualStyleBackColor = true;
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // product_selling
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1001, 574);
            this.Controls.Add(this.btnDel);
            this.Controls.Add(this.lvCart);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnPay);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.pbProduct);
            this.Controls.Add(this.lvProducts);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "product_selling";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Product Selling";
            this.Load += new System.EventHandler(this.product_selling_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbProduct)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lvProducts;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.PictureBox pbProduct;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cbType;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtReceive;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtTotal;
        private System.Windows.Forms.TextBox txtChange;
        private System.Windows.Forms.TextBox txtDiscount;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnPay;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.ListView lvCart;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSubTotal;
        private System.Windows.Forms.ComboBox cbCurrency;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDisPrice;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbBranch;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox cbCom;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cbEmp;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnDel;
    }
}