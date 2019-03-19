namespace coffee_shop
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
            this.logoutToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.companiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.myCompanyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.branchesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stocksToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.allStocksToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newStockToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.productsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.allProductsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newProductToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.usersToolStripMenuItem,
            this.companiesToolStripMenuItem,
            this.stocksToolStripMenuItem,
            this.productsToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(601, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // usersToolStripMenuItem
            // 
            this.usersToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.myUsersToolStripMenuItem,
            this.logoutToolStripMenuItem1});
            this.usersToolStripMenuItem.Name = "usersToolStripMenuItem";
            this.usersToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.usersToolStripMenuItem.Text = "Users";
            // 
            // myUsersToolStripMenuItem
            // 
            this.myUsersToolStripMenuItem.Name = "myUsersToolStripMenuItem";
            this.myUsersToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.myUsersToolStripMenuItem.Text = "My Users";
            this.myUsersToolStripMenuItem.Click += new System.EventHandler(this.myUsersToolStripMenuItem_Click);
            // 
            // logoutToolStripMenuItem1
            // 
            this.logoutToolStripMenuItem1.Name = "logoutToolStripMenuItem1";
            this.logoutToolStripMenuItem1.Size = new System.Drawing.Size(122, 22);
            this.logoutToolStripMenuItem1.Text = "Logout";
            this.logoutToolStripMenuItem1.Click += new System.EventHandler(this.logoutToolStripMenuItem1_Click);
            // 
            // companiesToolStripMenuItem
            // 
            this.companiesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.myCompanyToolStripMenuItem,
            this.branchesToolStripMenuItem});
            this.companiesToolStripMenuItem.Name = "companiesToolStripMenuItem";
            this.companiesToolStripMenuItem.Size = new System.Drawing.Size(79, 20);
            this.companiesToolStripMenuItem.Text = "Companies";
            // 
            // myCompanyToolStripMenuItem
            // 
            this.myCompanyToolStripMenuItem.Name = "myCompanyToolStripMenuItem";
            this.myCompanyToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.myCompanyToolStripMenuItem.Text = "My Companies";
            this.myCompanyToolStripMenuItem.Click += new System.EventHandler(this.myCompanyToolStripMenuItem_Click);
            // 
            // branchesToolStripMenuItem
            // 
            this.branchesToolStripMenuItem.Name = "branchesToolStripMenuItem";
            this.branchesToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.branchesToolStripMenuItem.Text = "Branches";
            // 
            // stocksToolStripMenuItem
            // 
            this.stocksToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.allStocksToolStripMenuItem,
            this.newStockToolStripMenuItem});
            this.stocksToolStripMenuItem.Name = "stocksToolStripMenuItem";
            this.stocksToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
            this.stocksToolStripMenuItem.Text = "Stocks";
            // 
            // allStocksToolStripMenuItem
            // 
            this.allStocksToolStripMenuItem.Name = "allStocksToolStripMenuItem";
            this.allStocksToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.allStocksToolStripMenuItem.Text = "All Stocks";
            // 
            // newStockToolStripMenuItem
            // 
            this.newStockToolStripMenuItem.Name = "newStockToolStripMenuItem";
            this.newStockToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.newStockToolStripMenuItem.Text = "New Stock";
            // 
            // productsToolStripMenuItem
            // 
            this.productsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.allProductsToolStripMenuItem,
            this.newProductToolStripMenuItem});
            this.productsToolStripMenuItem.Name = "productsToolStripMenuItem";
            this.productsToolStripMenuItem.Size = new System.Drawing.Size(66, 20);
            this.productsToolStripMenuItem.Text = "Products";
            // 
            // allProductsToolStripMenuItem
            // 
            this.allProductsToolStripMenuItem.Name = "allProductsToolStripMenuItem";
            this.allProductsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.allProductsToolStripMenuItem.Text = "All Products";
            this.allProductsToolStripMenuItem.Click += new System.EventHandler(this.allProductsToolStripMenuItem_Click);
            // 
            // newProductToolStripMenuItem
            // 
            this.newProductToolStripMenuItem.Name = "newProductToolStripMenuItem";
            this.newProductToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.newProductToolStripMenuItem.Text = "New Product";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.aboutToolStripMenuItem.Text = "About";
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(0, 27);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(601, 720);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(601, 749);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Main";
            this.Text = "Main";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.main_closing);
            this.Load += new System.EventHandler(this.Main_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem usersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem companiesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stocksToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem productsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem allStocksToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newStockToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem allProductsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newProductToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem myUsersToolStripMenuItem;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ToolStripMenuItem myCompanyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem logoutToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem branchesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
    }
}

