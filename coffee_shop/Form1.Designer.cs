﻿namespace coffee_shop
{
    partial class Main
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.usersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.myUsersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.membersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logoutToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.companiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.myCompanyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.branchesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.employeesToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.suppliersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stocksToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stockCateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.myStockToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.productsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.btnDrinks = new System.Windows.Forms.Button();
            this.btnFood = new System.Windows.Forms.Button();
            this.lbAlert = new System.Windows.Forms.Label();
            this.btnCheckStock = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.White;
            this.menuStrip1.Font = new System.Drawing.Font("Montserrat", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.usersToolStripMenuItem,
            this.companiesToolStripMenuItem,
            this.stocksToolStripMenuItem,
            this.productsToolStripMenuItem,
            this.aboutToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(8, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(1114, 35);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // usersToolStripMenuItem
            // 
            this.usersToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.myUsersToolStripMenuItem,
            this.membersToolStripMenuItem,
            this.logoutToolStripMenuItem1});
            this.usersToolStripMenuItem.Name = "usersToolStripMenuItem";
            this.usersToolStripMenuItem.Size = new System.Drawing.Size(80, 31);
            this.usersToolStripMenuItem.Text = "Users";
            // 
            // myUsersToolStripMenuItem
            // 
            this.myUsersToolStripMenuItem.Name = "myUsersToolStripMenuItem";
            this.myUsersToolStripMenuItem.Size = new System.Drawing.Size(186, 32);
            this.myUsersToolStripMenuItem.Text = "My Users";
            this.myUsersToolStripMenuItem.Click += new System.EventHandler(this.myUsersToolStripMenuItem_Click);
            // 
            // membersToolStripMenuItem
            // 
            this.membersToolStripMenuItem.Name = "membersToolStripMenuItem";
            this.membersToolStripMenuItem.Size = new System.Drawing.Size(186, 32);
            this.membersToolStripMenuItem.Text = "Members";
            this.membersToolStripMenuItem.Click += new System.EventHandler(this.membersToolStripMenuItem_Click);
            // 
            // logoutToolStripMenuItem1
            // 
            this.logoutToolStripMenuItem1.Name = "logoutToolStripMenuItem1";
            this.logoutToolStripMenuItem1.Size = new System.Drawing.Size(186, 32);
            this.logoutToolStripMenuItem1.Text = "Logout";
            this.logoutToolStripMenuItem1.Click += new System.EventHandler(this.logoutToolStripMenuItem_Click);
            // 
            // companiesToolStripMenuItem
            // 
            this.companiesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.myCompanyToolStripMenuItem,
            this.branchesToolStripMenuItem,
            this.employeesToolStripMenuItem1,
            this.suppliersToolStripMenuItem});
            this.companiesToolStripMenuItem.Name = "companiesToolStripMenuItem";
            this.companiesToolStripMenuItem.Size = new System.Drawing.Size(139, 31);
            this.companiesToolStripMenuItem.Text = "Companies";
            // 
            // myCompanyToolStripMenuItem
            // 
            this.myCompanyToolStripMenuItem.Name = "myCompanyToolStripMenuItem";
            this.myCompanyToolStripMenuItem.Size = new System.Drawing.Size(240, 32);
            this.myCompanyToolStripMenuItem.Text = "My Companies";
            this.myCompanyToolStripMenuItem.Click += new System.EventHandler(this.myCompanyToolStripMenuItem_Click);
            // 
            // branchesToolStripMenuItem
            // 
            this.branchesToolStripMenuItem.Name = "branchesToolStripMenuItem";
            this.branchesToolStripMenuItem.Size = new System.Drawing.Size(240, 32);
            this.branchesToolStripMenuItem.Text = "Branches";
            this.branchesToolStripMenuItem.Click += new System.EventHandler(this.branchesToolStripMenuItem_Click);
            // 
            // employeesToolStripMenuItem1
            // 
            this.employeesToolStripMenuItem1.Name = "employeesToolStripMenuItem1";
            this.employeesToolStripMenuItem1.Size = new System.Drawing.Size(240, 32);
            this.employeesToolStripMenuItem1.Text = "Employees";
            this.employeesToolStripMenuItem1.Click += new System.EventHandler(this.employeesToolStripMenuItem1_Click);
            // 
            // suppliersToolStripMenuItem
            // 
            this.suppliersToolStripMenuItem.Name = "suppliersToolStripMenuItem";
            this.suppliersToolStripMenuItem.Size = new System.Drawing.Size(240, 32);
            this.suppliersToolStripMenuItem.Text = "Suppliers";
            this.suppliersToolStripMenuItem.Click += new System.EventHandler(this.suppliersToolStripMenuItem_Click);
            // 
            // stocksToolStripMenuItem
            // 
            this.stocksToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.stockCateToolStripMenuItem,
            this.myStockToolStripMenuItem});
            this.stocksToolStripMenuItem.Name = "stocksToolStripMenuItem";
            this.stocksToolStripMenuItem.Size = new System.Drawing.Size(90, 31);
            this.stocksToolStripMenuItem.Text = "Stocks";
            // 
            // stockCateToolStripMenuItem
            // 
            this.stockCateToolStripMenuItem.Name = "stockCateToolStripMenuItem";
            this.stockCateToolStripMenuItem.Size = new System.Drawing.Size(259, 32);
            this.stockCateToolStripMenuItem.Text = "Stock Categories";
            this.stockCateToolStripMenuItem.Click += new System.EventHandler(this.stockCateToolStripMenuItem_Click);
            // 
            // myStockToolStripMenuItem
            // 
            this.myStockToolStripMenuItem.Name = "myStockToolStripMenuItem";
            this.myStockToolStripMenuItem.Size = new System.Drawing.Size(259, 32);
            this.myStockToolStripMenuItem.Text = "My Stocks";
            this.myStockToolStripMenuItem.Click += new System.EventHandler(this.myStockToolStripMenuItem_Click);
            // 
            // productsToolStripMenuItem
            // 
            this.productsToolStripMenuItem.Name = "productsToolStripMenuItem";
            this.productsToolStripMenuItem.Size = new System.Drawing.Size(115, 31);
            this.productsToolStripMenuItem.Text = "Products";
            this.productsToolStripMenuItem.Click += new System.EventHandler(this.productsToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(86, 31);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(61, 31);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "coffee-coffee-machine-coffee-maker-cup-wallpaper-ea114aeb4ad68a4485a91afaffff1624" +
        ".png");
            // 
            // btnDrinks
            // 
            this.btnDrinks.BackColor = System.Drawing.Color.White;
            this.btnDrinks.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnDrinks.FlatAppearance.BorderSize = 0;
            this.btnDrinks.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDrinks.Font = new System.Drawing.Font("Montserrat", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDrinks.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnDrinks.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDrinks.Location = new System.Drawing.Point(61, 303);
            this.btnDrinks.Margin = new System.Windows.Forms.Padding(16, 15, 16, 15);
            this.btnDrinks.Name = "btnDrinks";
            this.btnDrinks.Size = new System.Drawing.Size(299, 92);
            this.btnDrinks.TabIndex = 1;
            this.btnDrinks.Text = "DRINKS";
            this.btnDrinks.UseVisualStyleBackColor = false;
            this.btnDrinks.Click += new System.EventHandler(this.btnDrinks_Click);
            // 
            // btnFood
            // 
            this.btnFood.BackColor = System.Drawing.Color.White;
            this.btnFood.FlatAppearance.BorderSize = 0;
            this.btnFood.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFood.Font = new System.Drawing.Font("Montserrat", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFood.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.btnFood.Location = new System.Drawing.Point(61, 425);
            this.btnFood.Margin = new System.Windows.Forms.Padding(16, 15, 16, 15);
            this.btnFood.Name = "btnFood";
            this.btnFood.Size = new System.Drawing.Size(299, 92);
            this.btnFood.TabIndex = 2;
            this.btnFood.Text = "FOODS";
            this.btnFood.UseVisualStyleBackColor = false;
            this.btnFood.Click += new System.EventHandler(this.btnFood_Click);
            // 
            // lbAlert
            // 
            this.lbAlert.AutoSize = true;
            this.lbAlert.BackColor = System.Drawing.Color.Transparent;
            this.lbAlert.Font = new System.Drawing.Font("Montserrat", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbAlert.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(69)))), ((int)(((byte)(1)))));
            this.lbAlert.Location = new System.Drawing.Point(56, 217);
            this.lbAlert.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbAlert.Name = "lbAlert";
            this.lbAlert.Size = new System.Drawing.Size(0, 27);
            this.lbAlert.TabIndex = 3;
            // 
            // btnCheckStock
            // 
            this.btnCheckStock.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(69)))), ((int)(((byte)(1)))));
            this.btnCheckStock.FlatAppearance.BorderSize = 0;
            this.btnCheckStock.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCheckStock.Font = new System.Drawing.Font("Montserrat", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCheckStock.ForeColor = System.Drawing.Color.White;
            this.btnCheckStock.Location = new System.Drawing.Point(61, 154);
            this.btnCheckStock.Margin = new System.Windows.Forms.Padding(16, 15, 16, 15);
            this.btnCheckStock.Name = "btnCheckStock";
            this.btnCheckStock.Size = new System.Drawing.Size(299, 48);
            this.btnCheckStock.TabIndex = 4;
            this.btnCheckStock.Text = "Check Stock";
            this.btnCheckStock.UseVisualStyleBackColor = false;
            this.btnCheckStock.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Montserrat", 28.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(50, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(333, 66);
            this.label1.TabIndex = 6;
            this.label1.Text = "Coffee Shop";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1114, 618);
            this.Controls.Add(this.btnDrinks);
            this.Controls.Add(this.btnFood);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCheckStock);
            this.Controls.Add(this.lbAlert);
            this.Controls.Add(this.menuStrip1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.main_closing);
            this.Load += new System.EventHandler(this.Main_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem usersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem companiesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stocksToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem productsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stockCateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem myStockToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem myUsersToolStripMenuItem;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolStripMenuItem myCompanyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem logoutToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem branchesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Button btnDrinks;
        private System.Windows.Forms.Button btnFood;
        private System.Windows.Forms.Label lbAlert;
        private System.Windows.Forms.ToolStripMenuItem membersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem employeesToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem suppliersToolStripMenuItem;
        private System.Windows.Forms.Button btnCheckStock;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label1;
    }
}

