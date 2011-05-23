namespace Manina.Windows.Forms
{
    partial class PhotoPromotionEntry
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.numericUpDownPourcentage = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownNbPhoto = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPourcentage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownNbPhoto)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.numericUpDownPourcentage);
            this.groupBox1.Controls.Add(this.numericUpDownNbPhoto);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(2, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(135, 32);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            // 
            // numericUpDownPourcentage
            // 
            this.numericUpDownPourcentage.Location = new System.Drawing.Point(61, 12);
            this.numericUpDownPourcentage.Name = "numericUpDownPourcentage";
            this.numericUpDownPourcentage.Size = new System.Drawing.Size(47, 20);
            this.numericUpDownPourcentage.TabIndex = 8;
            this.numericUpDownPourcentage.ValueChanged += new System.EventHandler(this.numericUpDownPourcentage_ValueChanged);
            // 
            // numericUpDownNbPhoto
            // 
            this.numericUpDownNbPhoto.Location = new System.Drawing.Point(5, 12);
            this.numericUpDownNbPhoto.Name = "numericUpDownNbPhoto";
            this.numericUpDownNbPhoto.Size = new System.Drawing.Size(39, 20);
            this.numericUpDownNbPhoto.TabIndex = 7;
            this.numericUpDownNbPhoto.ValueChanged += new System.EventHandler(this.numericUpDownNbPhoto_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(111, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(15, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "%";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(43, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "P";
            // 
            // PhotoPromotionEntry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "PhotoPromotionEntry";
            this.Size = new System.Drawing.Size(140, 34);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPourcentage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownNbPhoto)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown numericUpDownPourcentage;
        private System.Windows.Forms.NumericUpDown numericUpDownNbPhoto;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;

    }
}
