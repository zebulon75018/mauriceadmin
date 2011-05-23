namespace Manina.Windows.Forms.wizard
{
    partial class FWizardImport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FWizardImport));
            this.wizardControl1 = new WizardBase.WizardControl();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.startStep3 = new WizardBase.StartStep();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.intermediateStep2 = new WizardBase.IntermediateStep();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.intermediateStep1 = new WizardBase.IntermediateStep();
            this.buttonSelectAll = new System.Windows.Forms.Button();
            this.labelPath = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.imageListView1 = new Manina.Windows.Forms.ImageListView();
            this.finishStep1 = new WizardBase.FinishStep();
            this.checkBoxImages = new System.Windows.Forms.CheckBox();
            this.checkBoxCategory = new System.Windows.Forms.CheckBox();
            this.checkBoxPhotographe = new System.Windows.Forms.CheckBox();
            this.startStep2 = new WizardBase.StartStep();
            this.startStep3.SuspendLayout();
            this.intermediateStep2.SuspendLayout();
            this.intermediateStep1.SuspendLayout();
            this.finishStep1.SuspendLayout();
            this.SuspendLayout();
            // 
            // wizardControl1
            // 
            this.wizardControl1.BackButtonEnabled = true;
            this.wizardControl1.BackButtonVisible = true;
            this.wizardControl1.CancelButtonEnabled = true;
            this.wizardControl1.CancelButtonVisible = true;
            this.wizardControl1.EulaButtonEnabled = false;
            this.wizardControl1.EulaButtonText = "eula";
            this.wizardControl1.EulaButtonVisible = false;
            this.wizardControl1.HelpButtonEnabled = true;
            this.wizardControl1.HelpButtonVisible = false;
            this.wizardControl1.Location = new System.Drawing.Point(1, 2);
            this.wizardControl1.Name = "wizardControl1";
            this.wizardControl1.NextButtonEnabled = true;
            this.wizardControl1.NextButtonVisible = true;
            this.wizardControl1.Size = new System.Drawing.Size(791, 460);
            this.wizardControl1.WizardSteps.AddRange(new WizardBase.WizardStep[] {
            this.startStep3,
            this.intermediateStep2,
            this.intermediateStep1,
            this.finishStep1});
            this.wizardControl1.CancelButtonClick += new System.EventHandler(this.wizardControl1_CancelButtonClick);
            this.wizardControl1.FinishButtonClick += new System.EventHandler(this.wizardControl1_FinishButtonClick);
            this.wizardControl1.NextButtonClick += new WizardBase.GenericCancelEventHandler<WizardBase.WizardControl>(this.wizardControl1_NextButtonClick);
            // 
            // startStep3
            // 
            this.startStep3.BindingImage = ((System.Drawing.Image)(resources.GetObject("startStep3.BindingImage")));
            this.startStep3.Controls.Add(this.listBox1);
            this.startStep3.Icon = ((System.Drawing.Image)(resources.GetObject("startStep3.Icon")));
            this.startStep3.Name = "startStep3";
            this.startStep3.Subtitle = "Please Choose a photographe";
            this.startStep3.Title = "Import Pictures to category";
            // 
            // listBox1
            // 
            this.listBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 24;
            this.listBox1.Location = new System.Drawing.Point(231, 102);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(459, 196);
            this.listBox1.TabIndex = 0;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // intermediateStep2
            // 
            this.intermediateStep2.BindingImage = ((System.Drawing.Image)(resources.GetObject("intermediateStep2.BindingImage")));
            this.intermediateStep2.Controls.Add(this.treeView1);
            this.intermediateStep2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.intermediateStep2.Name = "intermediateStep2";
            this.intermediateStep2.Subtitle = "";
            this.intermediateStep2.Title = "Please Choose a destination category";
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(104, 118);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(590, 261);
            this.treeView1.TabIndex = 0;
            // 
            // intermediateStep1
            // 
            this.intermediateStep1.BindingImage = ((System.Drawing.Image)(resources.GetObject("intermediateStep1.BindingImage")));
            this.intermediateStep1.Controls.Add(this.buttonSelectAll);
            this.intermediateStep1.Controls.Add(this.labelPath);
            this.intermediateStep1.Controls.Add(this.button1);
            this.intermediateStep1.Controls.Add(this.imageListView1);
            this.intermediateStep1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.intermediateStep1.Name = "intermediateStep1";
            this.intermediateStep1.Subtitle = "Choose the directory and after Pictures";
            this.intermediateStep1.Title = "Choose Photo to Import";
            // 
            // buttonSelectAll
            // 
            this.buttonSelectAll.Location = new System.Drawing.Point(702, 65);
            this.buttonSelectAll.Name = "buttonSelectAll";
            this.buttonSelectAll.Size = new System.Drawing.Size(61, 24);
            this.buttonSelectAll.TabIndex = 3;
            this.buttonSelectAll.Text = "Select All";
            this.buttonSelectAll.UseVisualStyleBackColor = true;
            this.buttonSelectAll.Click += new System.EventHandler(this.buttonSelectAll_Click);
            // 
            // labelPath
            // 
            this.labelPath.AutoSize = true;
            this.labelPath.Location = new System.Drawing.Point(272, 72);
            this.labelPath.Name = "labelPath";
            this.labelPath.Size = new System.Drawing.Size(0, 13);
            this.labelPath.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(32, 67);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(215, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Choose source directory for pictures";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // imageListView1
            // 
            this.imageListView1.Location = new System.Drawing.Point(32, 96);
            this.imageListView1.Name = "imageListView1";
            this.imageListView1.Size = new System.Drawing.Size(732, 306);
            this.imageListView1.TabIndex = 0;
            // 
            // finishStep1
            // 
            this.finishStep1.BindingImage = ((System.Drawing.Image)(resources.GetObject("finishStep1.BindingImage")));
            this.finishStep1.Controls.Add(this.checkBoxImages);
            this.finishStep1.Controls.Add(this.checkBoxCategory);
            this.finishStep1.Controls.Add(this.checkBoxPhotographe);
            this.finishStep1.Name = "finishStep1";
            // 
            // checkBoxImages
            // 
            this.checkBoxImages.AutoSize = true;
            this.checkBoxImages.Enabled = false;
            this.checkBoxImages.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxImages.Location = new System.Drawing.Point(97, 260);
            this.checkBoxImages.Name = "checkBoxImages";
            this.checkBoxImages.Size = new System.Drawing.Size(15, 14);
            this.checkBoxImages.TabIndex = 2;
            this.checkBoxImages.UseVisualStyleBackColor = true;
            // 
            // checkBoxCategory
            // 
            this.checkBoxCategory.AutoSize = true;
            this.checkBoxCategory.Enabled = false;
            this.checkBoxCategory.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxCategory.Location = new System.Drawing.Point(97, 182);
            this.checkBoxCategory.Name = "checkBoxCategory";
            this.checkBoxCategory.Size = new System.Drawing.Size(15, 14);
            this.checkBoxCategory.TabIndex = 1;
            this.checkBoxCategory.UseVisualStyleBackColor = true;
            // 
            // checkBoxPhotographe
            // 
            this.checkBoxPhotographe.AutoSize = true;
            this.checkBoxPhotographe.Enabled = false;
            this.checkBoxPhotographe.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxPhotographe.Location = new System.Drawing.Point(97, 116);
            this.checkBoxPhotographe.Name = "checkBoxPhotographe";
            this.checkBoxPhotographe.Size = new System.Drawing.Size(15, 14);
            this.checkBoxPhotographe.TabIndex = 0;
            this.checkBoxPhotographe.UseVisualStyleBackColor = true;
            // 
            // startStep2
            // 
            this.startStep2.BindingImage = ((System.Drawing.Image)(resources.GetObject("startStep2.BindingImage")));
            this.startStep2.Icon = ((System.Drawing.Image)(resources.GetObject("startStep2.Icon")));
            this.startStep2.Name = "startStep2";
            // 
            // FWizardImport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(791, 458);
            this.Controls.Add(this.wizardControl1);
            this.Name = "FWizardImport";
            this.Text = "FWizardImport";
            this.startStep3.ResumeLayout(false);
            this.intermediateStep2.ResumeLayout(false);
            this.intermediateStep1.ResumeLayout(false);
            this.intermediateStep1.PerformLayout();
            this.finishStep1.ResumeLayout(false);
            this.finishStep1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private WizardBase.WizardControl wizardControl1;
        private WizardBase.IntermediateStep intermediateStep1;
        private WizardBase.FinishStep finishStep1;
        private WizardBase.StartStep startStep3;
        private WizardBase.StartStep startStep2;
        private WizardBase.IntermediateStep intermediateStep2;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Button button1;
        private ImageListView imageListView1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.CheckBox checkBoxPhotographe;
        private System.Windows.Forms.CheckBox checkBoxImages;
        private System.Windows.Forms.CheckBox checkBoxCategory;
        private System.Windows.Forms.Label labelPath;
        private System.Windows.Forms.Button buttonSelectAll;
    }
}