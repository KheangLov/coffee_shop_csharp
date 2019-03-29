namespace coffee_shop
{
    partial class products_form
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
            this.groupBoxProductForm = new System.Windows.Forms.GroupBox();
            this.btnAddProcate = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnProductImage = new System.Windows.Forms.Button();
            this.pictureBoxProductImage = new System.Windows.Forms.PictureBox();
            this.txtProductType = new System.Windows.Forms.TextBox();
            this.comboBoxProductProcateID = new System.Windows.Forms.ComboBox();
            this.comboBoxProductStockID = new System.Windows.Forms.ComboBox();
            this.txtProductSellingPrice = new System.Windows.Forms.TextBox();
            this.txtProductPrice = new System.Windows.Forms.TextBox();
            this.txtProductName = new System.Windows.Forms.TextBox();
            this.labelProductProcateID = new System.Windows.Forms.Label();
            this.labelProductStockID = new System.Windows.Forms.Label();
            this.labelProductType = new System.Windows.Forms.Label();
            this.labelProductSellingPrice = new System.Windows.Forms.Label();
            this.labelProductPrice = new System.Windows.Forms.Label();
            this.labelProductName = new System.Windows.Forms.Label();
            this.btnProductAdd = new System.Windows.Forms.Button();
            this.btnProductEdit = new System.Windows.Forms.Button();
            this.btnProductDelete = new System.Windows.Forms.Button();
            this.btnProductExit = new System.Windows.Forms.Button();
            this.labelAllProduct = new System.Windows.Forms.Label();
            this.txtProductSearch = new System.Windows.Forms.TextBox();
            this.listViewAllProducts = new System.Windows.Forms.ListView();
            this.columnHeaderName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderPrice = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderSellingPrice = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderSale = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderProcateID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBoxProductForm.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxProductImage)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBoxProductForm
            // 
            this.groupBoxProductForm.Controls.Add(this.btnAddProcate);
            this.groupBoxProductForm.Controls.Add(this.label1);
            this.groupBoxProductForm.Controls.Add(this.btnProductImage);
            this.groupBoxProductForm.Controls.Add(this.pictureBoxProductImage);
            this.groupBoxProductForm.Controls.Add(this.txtProductType);
            this.groupBoxProductForm.Controls.Add(this.comboBoxProductProcateID);
            this.groupBoxProductForm.Controls.Add(this.comboBoxProductStockID);
            this.groupBoxProductForm.Controls.Add(this.txtProductSellingPrice);
            this.groupBoxProductForm.Controls.Add(this.txtProductPrice);
            this.groupBoxProductForm.Controls.Add(this.txtProductName);
            this.groupBoxProductForm.Controls.Add(this.labelProductProcateID);
            this.groupBoxProductForm.Controls.Add(this.labelProductStockID);
            this.groupBoxProductForm.Controls.Add(this.labelProductType);
            this.groupBoxProductForm.Controls.Add(this.labelProductSellingPrice);
            this.groupBoxProductForm.Controls.Add(this.labelProductPrice);
            this.groupBoxProductForm.Controls.Add(this.labelProductName);
            this.groupBoxProductForm.Font = new System.Drawing.Font("Montserrat", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxProductForm.Location = new System.Drawing.Point(12, 12);
            this.groupBoxProductForm.Name = "groupBoxProductForm";
            this.groupBoxProductForm.Size = new System.Drawing.Size(502, 436);
            this.groupBoxProductForm.TabIndex = 0;
            this.groupBoxProductForm.TabStop = false;
            this.groupBoxProductForm.Text = "Product\'s Informations";
            this.groupBoxProductForm.Enter += new System.EventHandler(this.groupBoxProductForm_Enter);
            // 
            // btnAddProcate
            // 
            this.btnAddProcate.Location = new System.Drawing.Point(401, 210);
            this.btnAddProcate.Name = "btnAddProcate";
            this.btnAddProcate.Size = new System.Drawing.Size(85, 30);
            this.btnAddProcate.TabIndex = 21;
            this.btnAddProcate.Text = "Add";
            this.btnAddProcate.UseVisualStyleBackColor = true;
            this.btnAddProcate.Click += new System.EventHandler(this.btnAddProcate_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Montserrat", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(8, 246);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 18);
            this.label1.TabIndex = 20;
            this.label1.Text = "Image :";
            // 
            // btnProductImage
            // 
            this.btnProductImage.Font = new System.Drawing.Font("Montserrat", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnProductImage.Location = new System.Drawing.Point(11, 391);
            this.btnProductImage.Name = "btnProductImage";
            this.btnProductImage.Size = new System.Drawing.Size(96, 31);
            this.btnProductImage.TabIndex = 19;
            this.btnProductImage.Text = "Browse";
            this.btnProductImage.UseVisualStyleBackColor = true;
            this.btnProductImage.Click += new System.EventHandler(this.btnProductImage_Click);
            // 
            // pictureBoxProductImage
            // 
            this.pictureBoxProductImage.Location = new System.Drawing.Point(176, 246);
            this.pictureBoxProductImage.Name = "pictureBoxProductImage";
            this.pictureBoxProductImage.Size = new System.Drawing.Size(310, 176);
            this.pictureBoxProductImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxProductImage.TabIndex = 18;
            this.pictureBoxProductImage.TabStop = false;
            // 
            // txtProductType
            // 
            this.txtProductType.Location = new System.Drawing.Point(176, 141);
            this.txtProductType.Name = "txtProductType";
            this.txtProductType.Size = new System.Drawing.Size(310, 27);
            this.txtProductType.TabIndex = 17;
            this.txtProductType.TextChanged += new System.EventHandler(this.txtProductType_TextChanged);
            this.txtProductType.Leave += new System.EventHandler(this.txtProductType_Leave);
            // 
            // comboBoxProductProcateID
            // 
            this.comboBoxProductProcateID.FormattingEnabled = true;
            this.comboBoxProductProcateID.Location = new System.Drawing.Point(176, 210);
            this.comboBoxProductProcateID.Name = "comboBoxProductProcateID";
            this.comboBoxProductProcateID.Size = new System.Drawing.Size(155, 30);
            this.comboBoxProductProcateID.TabIndex = 16;
            this.comboBoxProductProcateID.SelectedIndexChanged += new System.EventHandler(this.comboBoxProductProcateID_SelectedIndexChanged);
            this.comboBoxProductProcateID.Click += new System.EventHandler(this.comboBoxProductProcateID_Click);
            // 
            // comboBoxProductStockID
            // 
            this.comboBoxProductStockID.FormattingEnabled = true;
            this.comboBoxProductStockID.Location = new System.Drawing.Point(176, 174);
            this.comboBoxProductStockID.Name = "comboBoxProductStockID";
            this.comboBoxProductStockID.Size = new System.Drawing.Size(155, 30);
            this.comboBoxProductStockID.TabIndex = 15;
            this.comboBoxProductStockID.SelectedIndexChanged += new System.EventHandler(this.comboBoxProductStockID_SelectedIndexChanged);
            // 
            // txtProductSellingPrice
            // 
            this.txtProductSellingPrice.Location = new System.Drawing.Point(176, 106);
            this.txtProductSellingPrice.Name = "txtProductSellingPrice";
            this.txtProductSellingPrice.Size = new System.Drawing.Size(310, 27);
            this.txtProductSellingPrice.TabIndex = 12;
            this.txtProductSellingPrice.TextChanged += new System.EventHandler(this.txtProductSellingPrice_TextChanged);
            this.txtProductSellingPrice.Leave += new System.EventHandler(this.txtProductSellingPrice_Leave);
            // 
            // txtProductPrice
            // 
            this.txtProductPrice.Location = new System.Drawing.Point(176, 71);
            this.txtProductPrice.Name = "txtProductPrice";
            this.txtProductPrice.Size = new System.Drawing.Size(310, 27);
            this.txtProductPrice.TabIndex = 11;
            this.txtProductPrice.TextChanged += new System.EventHandler(this.txtProductPrice_TextChanged);
            this.txtProductPrice.Leave += new System.EventHandler(this.txtProductPrice_Leave);
            // 
            // txtProductName
            // 
            this.txtProductName.Location = new System.Drawing.Point(176, 31);
            this.txtProductName.Name = "txtProductName";
            this.txtProductName.Size = new System.Drawing.Size(310, 27);
            this.txtProductName.TabIndex = 10;
            this.txtProductName.TextChanged += new System.EventHandler(this.txtProductName_TextChanged);
            this.txtProductName.Leave += new System.EventHandler(this.txtProductName_Leave);
            // 
            // labelProductProcateID
            // 
            this.labelProductProcateID.AutoSize = true;
            this.labelProductProcateID.Font = new System.Drawing.Font("Montserrat", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelProductProcateID.Location = new System.Drawing.Point(8, 217);
            this.labelProductProcateID.Name = "labelProductProcateID";
            this.labelProductProcateID.Size = new System.Drawing.Size(131, 18);
            this.labelProductProcateID.TabIndex = 7;
            this.labelProductProcateID.Text = "Product Category  :";
            // 
            // labelProductStockID
            // 
            this.labelProductStockID.AutoSize = true;
            this.labelProductStockID.Font = new System.Drawing.Font("Montserrat", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelProductStockID.Location = new System.Drawing.Point(7, 181);
            this.labelProductStockID.Name = "labelProductStockID";
            this.labelProductStockID.Size = new System.Drawing.Size(50, 18);
            this.labelProductStockID.TabIndex = 6;
            this.labelProductStockID.Text = "Stock :";
            // 
            // labelProductType
            // 
            this.labelProductType.AutoSize = true;
            this.labelProductType.Font = new System.Drawing.Font("Montserrat", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelProductType.Location = new System.Drawing.Point(7, 146);
            this.labelProductType.Name = "labelProductType";
            this.labelProductType.Size = new System.Drawing.Size(45, 18);
            this.labelProductType.TabIndex = 5;
            this.labelProductType.Text = "Type :";
            // 
            // labelProductSellingPrice
            // 
            this.labelProductSellingPrice.AutoSize = true;
            this.labelProductSellingPrice.Font = new System.Drawing.Font("Montserrat", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelProductSellingPrice.Location = new System.Drawing.Point(7, 113);
            this.labelProductSellingPrice.Name = "labelProductSellingPrice";
            this.labelProductSellingPrice.Size = new System.Drawing.Size(96, 18);
            this.labelProductSellingPrice.TabIndex = 3;
            this.labelProductSellingPrice.Text = "Selling Price :";
            this.labelProductSellingPrice.Click += new System.EventHandler(this.labelProductSellingPrice_Click);
            // 
            // labelProductPrice
            // 
            this.labelProductPrice.AutoSize = true;
            this.labelProductPrice.Font = new System.Drawing.Font("Montserrat", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelProductPrice.Location = new System.Drawing.Point(7, 74);
            this.labelProductPrice.Name = "labelProductPrice";
            this.labelProductPrice.Size = new System.Drawing.Size(47, 18);
            this.labelProductPrice.TabIndex = 2;
            this.labelProductPrice.Text = "Price :";
            // 
            // labelProductName
            // 
            this.labelProductName.AutoSize = true;
            this.labelProductName.Font = new System.Drawing.Font("Montserrat", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelProductName.Location = new System.Drawing.Point(7, 34);
            this.labelProductName.Name = "labelProductName";
            this.labelProductName.Size = new System.Drawing.Size(55, 18);
            this.labelProductName.TabIndex = 1;
            this.labelProductName.Text = "Name :";
            // 
            // btnProductAdd
            // 
            this.btnProductAdd.Font = new System.Drawing.Font("Montserrat", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnProductAdd.Location = new System.Drawing.Point(12, 454);
            this.btnProductAdd.Name = "btnProductAdd";
            this.btnProductAdd.Size = new System.Drawing.Size(100, 35);
            this.btnProductAdd.TabIndex = 1;
            this.btnProductAdd.Text = "Add";
            this.btnProductAdd.UseVisualStyleBackColor = true;
            this.btnProductAdd.Click += new System.EventHandler(this.btnProductAdd_Click);
            // 
            // btnProductEdit
            // 
            this.btnProductEdit.Font = new System.Drawing.Font("Montserrat", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnProductEdit.Location = new System.Drawing.Point(118, 454);
            this.btnProductEdit.Name = "btnProductEdit";
            this.btnProductEdit.Size = new System.Drawing.Size(100, 35);
            this.btnProductEdit.TabIndex = 2;
            this.btnProductEdit.Text = "Edit";
            this.btnProductEdit.UseVisualStyleBackColor = true;
            this.btnProductEdit.Click += new System.EventHandler(this.btnProductEdit_Click);
            // 
            // btnProductDelete
            // 
            this.btnProductDelete.Font = new System.Drawing.Font("Montserrat", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnProductDelete.Location = new System.Drawing.Point(224, 454);
            this.btnProductDelete.Name = "btnProductDelete";
            this.btnProductDelete.Size = new System.Drawing.Size(100, 35);
            this.btnProductDelete.TabIndex = 3;
            this.btnProductDelete.Text = "Delete";
            this.btnProductDelete.UseVisualStyleBackColor = true;
            this.btnProductDelete.Click += new System.EventHandler(this.btnProductDelete_Click);
            // 
            // btnProductExit
            // 
            this.btnProductExit.Font = new System.Drawing.Font("Montserrat", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnProductExit.Location = new System.Drawing.Point(330, 454);
            this.btnProductExit.Name = "btnProductExit";
            this.btnProductExit.Size = new System.Drawing.Size(100, 35);
            this.btnProductExit.TabIndex = 4;
            this.btnProductExit.Text = "Exit";
            this.btnProductExit.UseVisualStyleBackColor = true;
            this.btnProductExit.Click += new System.EventHandler(this.btnProductExit_Click);
            // 
            // labelAllProduct
            // 
            this.labelAllProduct.AutoSize = true;
            this.labelAllProduct.Font = new System.Drawing.Font("Montserrat", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAllProduct.Location = new System.Drawing.Point(520, 6);
            this.labelAllProduct.Name = "labelAllProduct";
            this.labelAllProduct.Size = new System.Drawing.Size(168, 33);
            this.labelAllProduct.TabIndex = 5;
            this.labelAllProduct.Text = "All Products";
            // 
            // txtProductSearch
            // 
            this.txtProductSearch.Font = new System.Drawing.Font("Montserrat", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtProductSearch.Location = new System.Drawing.Point(777, 12);
            this.txtProductSearch.Name = "txtProductSearch";
            this.txtProductSearch.Size = new System.Drawing.Size(382, 27);
            this.txtProductSearch.TabIndex = 6;
            // 
            // listViewAllProducts
            // 
            this.listViewAllProducts.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderName,
            this.columnHeaderPrice,
            this.columnHeaderSellingPrice,
            this.columnHeaderSale,
            this.columnHeaderType,
            this.columnHeaderProcateID,
            this.columnHeader1});
            this.listViewAllProducts.Font = new System.Drawing.Font("Montserrat", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listViewAllProducts.FullRowSelect = true;
            this.listViewAllProducts.GridLines = true;
            this.listViewAllProducts.Location = new System.Drawing.Point(526, 56);
            this.listViewAllProducts.Name = "listViewAllProducts";
            this.listViewAllProducts.Size = new System.Drawing.Size(633, 433);
            this.listViewAllProducts.TabIndex = 7;
            this.listViewAllProducts.UseCompatibleStateImageBehavior = false;
            this.listViewAllProducts.View = System.Windows.Forms.View.Details;
            this.listViewAllProducts.SelectedIndexChanged += new System.EventHandler(this.listViewAllProducts_SelectedIndexChanged);
            // 
            // columnHeaderName
            // 
            this.columnHeaderName.Text = "Name";
            this.columnHeaderName.Width = 88;
            // 
            // columnHeaderPrice
            // 
            this.columnHeaderPrice.Text = "Price";
            this.columnHeaderPrice.Width = 92;
            // 
            // columnHeaderSellingPrice
            // 
            this.columnHeaderSellingPrice.Text = "SellingPrice";
            this.columnHeaderSellingPrice.Width = 119;
            // 
            // columnHeaderSale
            // 
            this.columnHeaderSale.Text = "Sale";
            this.columnHeaderSale.Width = 82;
            // 
            // columnHeaderType
            // 
            this.columnHeaderType.Text = "Type";
            this.columnHeaderType.Width = 88;
            // 
            // columnHeaderProcateID
            // 
            this.columnHeaderProcateID.Text = "Product Category";
            this.columnHeaderProcateID.Width = 142;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Stock";
            // 
            // products_form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1171, 504);
            this.Controls.Add(this.listViewAllProducts);
            this.Controls.Add(this.txtProductSearch);
            this.Controls.Add(this.labelAllProduct);
            this.Controls.Add(this.btnProductExit);
            this.Controls.Add(this.btnProductDelete);
            this.Controls.Add(this.btnProductEdit);
            this.Controls.Add(this.btnProductAdd);
            this.Controls.Add(this.groupBoxProductForm);
            this.Font = new System.Drawing.Font("Montserrat", 8.249999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "products_form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Products Form";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.products_form_FormClosing);
            this.Load += new System.EventHandler(this.products_form_Load);
            this.groupBoxProductForm.ResumeLayout(false);
            this.groupBoxProductForm.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxProductImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxProductForm;
        private System.Windows.Forms.Label labelProductProcateID;
        private System.Windows.Forms.Label labelProductStockID;
        private System.Windows.Forms.Label labelProductType;
        private System.Windows.Forms.Label labelProductSellingPrice;
        private System.Windows.Forms.Label labelProductPrice;
        private System.Windows.Forms.Label labelProductName;
        private System.Windows.Forms.TextBox txtProductSellingPrice;
        private System.Windows.Forms.TextBox txtProductPrice;
        private System.Windows.Forms.TextBox txtProductName;
        private System.Windows.Forms.ComboBox comboBoxProductProcateID;
        private System.Windows.Forms.ComboBox comboBoxProductStockID;
        private System.Windows.Forms.TextBox txtProductType;
        private System.Windows.Forms.PictureBox pictureBoxProductImage;
        private System.Windows.Forms.Button btnProductImage;
        private System.Windows.Forms.Button btnProductAdd;
        private System.Windows.Forms.Button btnProductEdit;
        private System.Windows.Forms.Button btnProductDelete;
        private System.Windows.Forms.Button btnProductExit;
        private System.Windows.Forms.Label labelAllProduct;
        private System.Windows.Forms.TextBox txtProductSearch;
        private System.Windows.Forms.ListView listViewAllProducts;
        private System.Windows.Forms.ColumnHeader columnHeaderName;
        private System.Windows.Forms.ColumnHeader columnHeaderPrice;
        private System.Windows.Forms.ColumnHeader columnHeaderSellingPrice;
        private System.Windows.Forms.ColumnHeader columnHeaderSale;
        private System.Windows.Forms.ColumnHeader columnHeaderType;
        private System.Windows.Forms.ColumnHeader columnHeaderProcateID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnAddProcate;
        private System.Windows.Forms.ColumnHeader columnHeader1;
    }
}