namespace coffee_shop
{
    partial class add_product_category_form
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
            this.gpAddproductCategory = new System.Windows.Forms.GroupBox();
            this.txtDescriptions = new System.Windows.Forms.TextBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.labelDescriptions = new System.Windows.Forms.Label();
            this.labelName = new System.Windows.Forms.Label();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.gpAddproductCategory.SuspendLayout();
            this.SuspendLayout();
            // 
            // gpAddproductCategory
            // 
            this.gpAddproductCategory.Controls.Add(this.txtDescriptions);
            this.gpAddproductCategory.Controls.Add(this.txtName);
            this.gpAddproductCategory.Controls.Add(this.labelDescriptions);
            this.gpAddproductCategory.Controls.Add(this.labelName);
            this.gpAddproductCategory.Font = new System.Drawing.Font("Montserrat", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gpAddproductCategory.Location = new System.Drawing.Point(12, 12);
            this.gpAddproductCategory.Name = "gpAddproductCategory";
            this.gpAddproductCategory.Size = new System.Drawing.Size(435, 168);
            this.gpAddproductCategory.TabIndex = 0;
            this.gpAddproductCategory.TabStop = false;
            this.gpAddproductCategory.Text = "Add Product Category";
            // 
            // txtDescriptions
            // 
            this.txtDescriptions.Location = new System.Drawing.Point(138, 82);
            this.txtDescriptions.Multiline = true;
            this.txtDescriptions.Name = "txtDescriptions";
            this.txtDescriptions.Size = new System.Drawing.Size(286, 69);
            this.txtDescriptions.TabIndex = 3;
            this.txtDescriptions.TextChanged += new System.EventHandler(this.txtDescriptions_TextChanged);
            this.txtDescriptions.Leave += new System.EventHandler(this.txtDescriptions_Leave);
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(138, 49);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(286, 27);
            this.txtName.TabIndex = 2;
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            this.txtName.Leave += new System.EventHandler(this.txtName_Leave);
            // 
            // labelDescriptions
            // 
            this.labelDescriptions.AutoSize = true;
            this.labelDescriptions.Font = new System.Drawing.Font("Montserrat", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDescriptions.Location = new System.Drawing.Point(6, 89);
            this.labelDescriptions.Name = "labelDescriptions";
            this.labelDescriptions.Size = new System.Drawing.Size(96, 18);
            this.labelDescriptions.TabIndex = 1;
            this.labelDescriptions.Text = "Descriptions :";
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Font = new System.Drawing.Font("Montserrat", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelName.Location = new System.Drawing.Point(6, 56);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(55, 18);
            this.labelName.TabIndex = 0;
            this.labelName.Text = "Name :";
            // 
            // btnAdd
            // 
            this.btnAdd.Font = new System.Drawing.Font("Montserrat", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdd.Location = new System.Drawing.Point(150, 186);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(100, 35);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("Montserrat", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.Location = new System.Drawing.Point(347, 186);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(100, 35);
            this.btnExit.TabIndex = 2;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            // 
            // add_product_category_form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(460, 231);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.gpAddproductCategory);
            this.Font = new System.Drawing.Font("Montserrat", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "add_product_category_form";
            this.Text = "Add Product Category";
            this.Load += new System.EventHandler(this.add_product_category_form_Load);
            this.gpAddproductCategory.ResumeLayout(false);
            this.gpAddproductCategory.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gpAddproductCategory;
        private System.Windows.Forms.Label labelDescriptions;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.TextBox txtDescriptions;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnExit;
    }
}