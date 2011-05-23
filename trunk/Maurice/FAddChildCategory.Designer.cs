namespace Manina.Windows.Forms
{
    partial class FAddChildCategory
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.textBoxDirectory = new System.Windows.Forms.TextBox();
            this.buttonChooseDirectory = new System.Windows.Forms.Button();
            this.checkBoxCreatePath = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Directory";
            this.label2.Visible = false;
            // 
            // textBoxName
            // 
            this.textBoxName.Location = new System.Drawing.Point(128, 14);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(228, 20);
            this.textBoxName.TabIndex = 2;
            // 
            // textBoxDirectory
            // 
            this.textBoxDirectory.Location = new System.Drawing.Point(128, 41);
            this.textBoxDirectory.Name = "textBoxDirectory";
            this.textBoxDirectory.Size = new System.Drawing.Size(227, 20);
            this.textBoxDirectory.TabIndex = 3;
            this.textBoxDirectory.Visible = false;
            // 
            // buttonChooseDirectory
            // 
            this.buttonChooseDirectory.Location = new System.Drawing.Point(363, 40);
            this.buttonChooseDirectory.Name = "buttonChooseDirectory";
            this.buttonChooseDirectory.Size = new System.Drawing.Size(34, 20);
            this.buttonChooseDirectory.TabIndex = 4;
            this.buttonChooseDirectory.Text = "...";
            this.buttonChooseDirectory.UseVisualStyleBackColor = true;
            this.buttonChooseDirectory.Visible = false;
            this.buttonChooseDirectory.Click += new System.EventHandler(this.buttonChooseDirectory_Click);
            // 
            // checkBoxCreatePath
            // 
            this.checkBoxCreatePath.AutoSize = true;
            this.checkBoxCreatePath.Checked = true;
            this.checkBoxCreatePath.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxCreatePath.Location = new System.Drawing.Point(27, 87);
            this.checkBoxCreatePath.Name = "checkBoxCreatePath";
            this.checkBoxCreatePath.Size = new System.Drawing.Size(202, 17);
            this.checkBoxCreatePath.TabIndex = 5;
            this.checkBoxCreatePath.Text = "Create Directory from parent category";
            this.checkBoxCreatePath.UseVisualStyleBackColor = true;
            this.checkBoxCreatePath.CheckedChanged += new System.EventHandler(this.checkBoxCreatePath_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(25, 146);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(103, 30);
            this.button1.TabIndex = 6;
            this.button1.Text = "Ok";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(287, 150);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(110, 26);
            this.buttonCancel.TabIndex = 7;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // FAddChildCategory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(444, 202);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.checkBoxCreatePath);
            this.Controls.Add(this.buttonChooseDirectory);
            this.Controls.Add(this.textBoxDirectory);
            this.Controls.Add(this.textBoxName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "FAddChildCategory";
            this.Text = "Add Child Category";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.TextBox textBoxDirectory;
        private System.Windows.Forms.Button buttonChooseDirectory;
        private System.Windows.Forms.CheckBox checkBoxCreatePath;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
    }
}