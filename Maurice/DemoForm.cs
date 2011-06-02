using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.IO;
using System.Net;
using Manina.Windows.Forms.NodeView;
using ShellLib;
using WizardBase;
using Manina.Windows.Forms.wizard;
using ThumbnailMakers;
using Manina.Windows.Forms.NetWork;
using System.Diagnostics;
using System.Threading;
using Manina.Windows.Forms.KeyboardSimulator;
using Manina.Windows.Forms.ExportExcel;
// TRansaction 42015399 Net ReactorPayPal [Instant] Verified account, Transaction id:6J083905NV8068234
namespace Manina.Windows.Forms
{
    public partial class DemoForm : Form
    {
        #region Member variables
        private BackgroundWorker bw = new BackgroundWorker();

        private static NodesCategory nodeCategory = null;
        private static NodesUser nodeCustomer = null;
        private static NodesPhotographer nodephotographer = null;
        private static NodesInfo nodeInfo = null;
        private static NodeConfig nodeConfig = null;
        private static NodePrice nodePrice = null;

        protected ImageListView currentImageListView;
        #endregion

        #region Renderer and color combobox items
        /// <summary>
        /// Represents an item in the renderer combobox.
        /// </summary>
        private struct RendererComboBoxItem
        {
            public string Name;
            public string FullName;

            public override string ToString()
            {
                return Name;
            }

            public RendererComboBoxItem(Type type)
            {
                Name = type.Name;
                FullName = type.FullName;
            }
        }

        /// <summary>
        /// Represents an item in the custom color combobox.
        /// </summary>
        private struct ColorComboBoxItem
        {
            public string Name;
            public PropertyInfo Field;

            public override string ToString()
            {
                return Name;
            }

            public ColorComboBoxItem(PropertyInfo field)
            {
                Name = field.Name;
                Field = field;
            }
        }
        #endregion

        #region Constructor
        public DemoForm()
        {            
            try
            {
                // LicenceController n'était pas présent dans le projet (tu as peut être oublié de l'inclure dans l'import initial du projet ?)
                // bref, du coup j'ai retiré la référence à la classe dans le projet
                // et forcément, ben faut que je mette en commentaire cette partie...
                /*LicenceControler lc = new LicenceControler();

                if (lc.licenceOk == false)
                {
                    Environment.Exit(1); 
                }*/

                VersionChecker vc = new VersionChecker();
                string netresult = vc.GetNetworkVersion();

                int versionMajor = 0;
                int versionMineur = 5;
                char[] separator = new char[1];
                separator[0] = '.';
                string[] splitString = netresult.Split(separator);

                if (splitString.Length == 2)
                {
                    if (Int32.Parse(splitString[0].ToString()) >= versionMajor && Int32.Parse(splitString[1].ToString()) > versionMineur)
                    {
                        if (MessageBox.Show("New version is available , do you want to download it ? ", "Update", System.Windows.Forms.MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                        {
                            vc.DownloadFile();
                        }
                    }
                }
            }
            catch (Exception e)
            {
            }
            InitializeComponent();

            currentImageListView = imageListView1;            

            // Setup the background worker
            Application.Idle += new EventHandler(Application_Idle);
            //bw.DoWork += new DoWorkEventHandler(bw_DoWork);
            //bw.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);

            // Find and add built-in renderers
            Assembly assembly = Assembly.GetAssembly(typeof(ImageListView));
            int i = 0;
            foreach (Type type in assembly.GetTypes())
            {
                if (type.BaseType ==typeof(ImageListView.ImageListViewRenderer))
                {
                    renderertoolStripComboBox.Items.Add(new RendererComboBoxItem(type));
                    if (type.Name == "DefaultRenderer")
                        renderertoolStripComboBox.SelectedIndex = i;
                    i++;
                }
            }
            // Find and add custom colors
            Type colorType = typeof(ImageListViewColor);
            i = 0;
            foreach (PropertyInfo field in colorType.GetProperties(BindingFlags.Public | BindingFlags.Static))
            {
                colorToolStripComboBox.Items.Add(new ColorComboBoxItem(field));
                if (field.Name == "Default")
                    colorToolStripComboBox.SelectedIndex = i;
                i++;
            }
            // Dynamically add aligment values
            foreach (object o in Enum.GetValues(typeof(ContentAlignment)))
            {
                ToolStripMenuItem item1 = new ToolStripMenuItem(o.ToString());
                item1.Tag = o;
                item1.Click += new EventHandler(checkboxAlignmentToolStripButton_Click);
                checkboxAlignmentToolStripMenuItem.DropDownItems.Add(item1);
                ToolStripMenuItem item2 = new ToolStripMenuItem(o.ToString());
                item2.Tag = o;
                item2.Click += new EventHandler(iconAlignmentToolStripButton_Click);
                iconAlignmentToolStripMenuItem.DropDownItems.Add(item2);
            }

            imageListView1.AllowDuplicateFileNames = true;
            imageListView1.SetRenderer(new ImageListViewRenderers.DefaultRenderer());

            /*
            TreeNode node = new TreeNode("Loading...", 3, 3);
            node.Tag = null;
            treeView1.Nodes.Clear();
            treeView1.Nodes.Add(node);
            while (bw.IsBusy) ;
            bw.RunWorkerAsync(node);

             */
            fillTree();
            showCustomerLeavingTomorrow();
        }

        #endregion

        #region Update UI while idle
        void Application_Idle(object sender, EventArgs e)
        {
            detailsToolStripButton.Checked = (imageListView1.View == View.Details);
            thumbnailsToolStripButton.Checked = (imageListView1.View == View.Thumbnails);
            galleryToolStripButton.Checked = (imageListView1.View == View.Gallery);
            paneToolStripButton.Checked = (imageListView1.View == View.Pane);

            integralScrollToolStripMenuItem.Checked = imageListView1.IntegralScroll;

            showCheckboxesToolStripMenuItem.Checked = imageListView1.ShowCheckBoxes;
            showFileIconsToolStripMenuItem.Checked = imageListView1.ShowFileIcons;

            x96ToolStripMenuItem.Checked = imageListView1.ThumbnailSize == new Size(96, 96);
            x120ToolStripMenuItem.Checked = imageListView1.ThumbnailSize == new Size(120, 120);
            x200ToolStripMenuItem.Checked = imageListView1.ThumbnailSize == new Size(200, 200);

            allowColumnClickToolStripMenuItem.Checked = imageListView1.AllowColumnClick;
            allowColumnResizeToolStripMenuItem.Checked = imageListView1.AllowColumnResize;
            allowPaneResizeToolStripMenuItem.Checked = imageListView1.AllowPaneResize;
            multiSelectToolStripMenuItem.Checked = imageListView1.MultiSelect;
            allowDragToolStripMenuItem.Checked = imageListView1.AllowDrag;
            allowDropToolStripMenuItem.Checked = imageListView1.AllowDrop;
            allowDuplicateFilenamesToolStripMenuItem.Checked = imageListView1.AllowDuplicateFileNames;
            continuousCacheModeToolStripMenuItem.Checked = (imageListView1.CacheMode == CacheMode.Continuous);

            ContentAlignment ca = imageListView1.CheckBoxAlignment;
            foreach (ToolStripMenuItem item in checkboxAlignmentToolStripMenuItem.DropDownItems)
                item.Checked = (ContentAlignment)item.Tag == ca;
            ContentAlignment ia = imageListView1.IconAlignment;
            foreach (ToolStripMenuItem item in iconAlignmentToolStripMenuItem.DropDownItems)
                item.Checked = (ContentAlignment)item.Tag == ia;

            toolStripStatusLabel1.Text = string.Format("{0} Items: {1} Selected, {2} Checked",
                imageListView1.Items.Count, imageListView1.SelectedItems.Count, imageListView1.CheckedItems.Count);
        }
        #endregion

        #region Set ImageListView options
        private void checkboxAlignmentToolStripButton_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            ContentAlignment aligment = (ContentAlignment)item.Tag;
            currentImageListView.CheckBoxAlignment = aligment;
        }

        private void iconAlignmentToolStripButton_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            ContentAlignment aligment = (ContentAlignment)item.Tag;
            currentImageListView.IconAlignment = aligment;
        }

        private void renderertoolStripComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Assembly assembly = Assembly.GetAssembly(typeof(ImageListView));
            RendererComboBoxItem item = (RendererComboBoxItem)renderertoolStripComboBox.SelectedItem;
            ImageListView.ImageListViewRenderer renderer = (ImageListView.ImageListViewRenderer)assembly.CreateInstance(item.FullName);
            if (renderer == null)
            {
                assembly = Assembly.GetExecutingAssembly();
                renderer = (ImageListView.ImageListViewRenderer)assembly.CreateInstance(item.FullName);
            }
            colorToolStripComboBox.Enabled = renderer.CanApplyColors;
            currentImageListView.SetRenderer(renderer);
            currentImageListView.Focus();
        }

        private void colorToolStripComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Type t = typeof(ImageListViewColor);
            PropertyInfo field = ((ColorComboBoxItem)colorToolStripComboBox.SelectedItem).Field;
            ImageListViewColor color = (ImageListViewColor)field.GetValue(null, null);
            currentImageListView.Colors = color;
        }

        private void detailsToolStripButton_Click(object sender, EventArgs e)
        {
            currentImageListView.View = View.Details;
        }

        private void thumbnailsToolStripButton_Click(object sender, EventArgs e)
        {
            currentImageListView.View = View.Thumbnails;
        }

        private void galleryToolStripButton_Click(object sender, EventArgs e)
        {
            currentImageListView.View = View.Gallery;
        }

        private void paneToolStripButton_Click(object sender, EventArgs e)
        {
            currentImageListView.View = View.Pane;
        }

        private void clearThumbsToolStripButton_Click(object sender, EventArgs e)
        {
            currentImageListView.ClearThumbnailCache();
        }

        private void x96ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currentImageListView.ThumbnailSize = new Size(96, 96);
        }

        private void x120ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currentImageListView.ThumbnailSize = new Size(120, 120);
        }

        private void x200ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currentImageListView.ThumbnailSize = new Size(200, 200);
        }

        private void showCheckboxesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currentImageListView.ShowCheckBoxes = !currentImageListView.ShowCheckBoxes;
        }

        private void showFileIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currentImageListView.ShowFileIcons = !currentImageListView.ShowFileIcons;
        }

        private void allowColumnClickToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currentImageListView.AllowColumnClick = !currentImageListView.AllowColumnClick;
        }

        private void allowColumnResizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currentImageListView.AllowColumnResize = !currentImageListView.AllowColumnResize;
        }

        private void allowPaneResizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currentImageListView.AllowPaneResize = !currentImageListView.AllowPaneResize;
        }

        private void multiSelectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currentImageListView.MultiSelect = !currentImageListView.MultiSelect;
        }

        private void allowDragToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currentImageListView.AllowDrag = !currentImageListView.AllowDrag;
        }

        private void allowDropToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currentImageListView.AllowDrop = !currentImageListView.AllowDrop;
        }

        private void allowDuplicateFilenamesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currentImageListView.AllowDuplicateFileNames = !currentImageListView.AllowDuplicateFileNames;
        }

        private void continuousCacheModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentImageListView.CacheMode == CacheMode.Continuous)
                currentImageListView.CacheMode = CacheMode.OnDemand;
            else
                currentImageListView.CacheMode = CacheMode.Continuous;
        }

        private void integralScrollToolStripMenuItem_Click(object sender, EventArgs e)
        {
            currentImageListView.IntegralScroll = !currentImageListView.IntegralScroll;
        }
        #endregion

        #region Set selected image to PropertyGrid
        private void imageListView1_SelectionChanged(object sender, EventArgs e)
        {
            ImageListViewItem sel = null;
            if (((ImageListView)sender).SelectedItems.Count > 0)
                sel = ((ImageListView)sender).SelectedItems[0];
            propertyGrid1.SelectedObject = sel;
        }

        private void imageListView2_SelectionChanged(object sender, EventArgs e)
        {
            ImageListViewItem sel = null;
            if (((ImageListView)sender).SelectedItems.Count > 0)
                sel = ((ImageListView)sender).SelectedItems[0];

            try 
            {
            NodeUser nu = (NodeUser) treeView1.SelectedNode;
            if (nu != null)
            {
                this.labelFormatImage.Text =  nu.getFormatImage(sel.FileName);
            }
            
            } catch(Exception exp)
            {
                this.labelFormatImage.Text = "";
            }

            propertyGrid1.SelectedObject = sel;
        }
        #endregion

        #region Change Selection/Checkboxes
        private void imageListView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                if (e.KeyCode == Keys.A)
                    imageListView1.SelectAll();
                else if (e.KeyCode == Keys.U)
                    imageListView1.ClearSelection();
                else if (e.KeyCode == Keys.I)
                    imageListView1.InvertSelection();
            }
            else if (e.Alt)
            {
                if (e.KeyCode == Keys.A)
                    imageListView1.CheckAll();
                else if (e.KeyCode == Keys.U)
                    imageListView1.UncheckAll();
                else if (e.KeyCode == Keys.I)
                    imageListView1.InvertCheckState();
            }
        }
        #endregion

        #region Update folder list asynchronously
        private void PopulateListView(DirectoryInfo path)
        {
            imageListView1.Items.Clear();
            imageListView1.SuspendLayout();
            foreach (FileInfo p in path.GetFiles("*.*"))
            {
                if (p.Name.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                    p.Name.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                    p.Name.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase) ||
                    p.Name.EndsWith(".ico", StringComparison.OrdinalIgnoreCase) ||
                    p.Name.EndsWith(".cur", StringComparison.OrdinalIgnoreCase) ||
                    p.Name.EndsWith(".emf", StringComparison.OrdinalIgnoreCase) ||
                    p.Name.EndsWith(".wmf", StringComparison.OrdinalIgnoreCase) ||
                    p.Name.EndsWith(".tif", StringComparison.OrdinalIgnoreCase) ||
                    p.Name.EndsWith(".tiff", StringComparison.OrdinalIgnoreCase) ||
                    p.Name.EndsWith(".gif", StringComparison.OrdinalIgnoreCase))
                    imageListView1.Items.Add(p.FullName);
            }
            imageListView1.ResumeLayout();
        }

        private void PopulateListViewCustomer()
        {
            imageListView2.Items.Clear();
            imageListView2.SuspendLayout();
            NodeUser nu = (NodeUser) treeView1.SelectedNode;
            for (int n = 0; n < nu.NbPhoto(); n++)
            {
                imageListView2.Items.Add(nu.FullFilenamePhoto(n));
            }
            /*
            foreach (FileInfo p in path.GetFiles("*.*"))
            {
                if (p.Name.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                    p.Name.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                    p.Name.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase) ||
                    p.Name.EndsWith(".ico", StringComparison.OrdinalIgnoreCase) ||
                    p.Name.EndsWith(".cur", StringComparison.OrdinalIgnoreCase) ||
                    p.Name.EndsWith(".emf", StringComparison.OrdinalIgnoreCase) ||
                    p.Name.EndsWith(".wmf", StringComparison.OrdinalIgnoreCase) ||
                    p.Name.EndsWith(".tif", StringComparison.OrdinalIgnoreCase) ||
                    p.Name.EndsWith(".tiff", StringComparison.OrdinalIgnoreCase) ||
                    p.Name.EndsWith(".gif", StringComparison.OrdinalIgnoreCase))
                    imageListView2.Items.Add(p.FullName);
            }
             * */
            imageListView2.ResumeLayout();
        }

        private void treeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            /*
            TreeNode node = e.Node;
            KeyValuePair<DirectoryInfo, bool> ktag = (KeyValuePair<DirectoryInfo, bool>)node.Tag;
            if (ktag.Value == true)
                return;
            node.Nodes.Clear();
            node.Nodes.Add("", "Loading...", 3, 3);
            while (bw.IsBusy) ;
            bw.RunWorkerAsync(node);
             */
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            toolStrip1.Visible = false;                       
            
            if (e.Node.GetType().Name == "NodesCategory")
            {
                tabControl1.SelectedIndex = 1;
            }

            if (e.Node.GetType().Name == "NodeCategory")
            {
                tabControl1.SelectedIndex = 0;
                NodeCategory node = (NodeCategory)e.Node;
                try
                {
                    PopulateListView(new DirectoryInfo(node.getDirectory()));
                }
                catch (Exception exp)
                {
                    MessageBox.Show("Error in category " + exp.Message);
                }
                FillGuiCategory(node);
                currentImageListView = imageListView1;
                toolStrip1.Visible = true;                
            }

            if (e.Node.GetType().Name == "NodeUser")
            {              
                buttonPrint.Visible = false;
                buttonCash.Visible = true;
                tabControl1.SelectedIndex = 2;
                NodeUser node = (NodeUser)e.Node;
                PopulateListViewCustomer();
                FillGuiCustomer(node);
                currentImageListView = imageListView2;
                toolStrip1.Visible = true;
            }
            if (e.Node.GetType().Name == "NodesUser")
            {
                tabControl1.SelectedIndex = 3; // Blanck
            }
            if (e.Node.GetType().Name == "NodesPhotographer")
            {
                tabControl1.SelectedIndex = 4; 
            }
            if (e.Node.GetType().Name == "NodePhotographer")
            {
                tabControl1.SelectedIndex = 5; 
                NodePhotographer node = (NodePhotographer) e.Node;
                FillInfoPhotoGraphe(node);
            }
            if (e.Node.GetType().Name == "NodesInfo")
            {
                tabControl1.SelectedIndex = 6; 
            }
            if (e.Node.GetType().Name == "NodeInfo")
            {
                 NodeInfo node = (NodeInfo)e.Node;
                 FillGuiInfo(node);
                tabControl1.SelectedIndex = 7; // Blanck
            }

            if (e.Node.GetType().Name == "NodeConfig")
            {
                NodeConfig node = (NodeConfig)e.Node;
                FillGuiConfig(node);
                tabControl1.SelectedIndex = 8; 
            }

            if (e.Node.GetType().Name == "NodePrice")
            {
                NodePrice node = (NodePrice)e.Node;
                FillGuiPrice(node);
                tabControl1.SelectedIndex = 9;
            }


         

            //@"C:\Users\c\Pictures"));
            //if (e.Node.Tag == null) return;
            //KeyValuePair<DirectoryInfo, bool> ktag = (KeyValuePair<DirectoryInfo, bool>)e.Node.Tag;
            //PopulateListView(ktag.Key);
        }

        private void  FillInfoPhotoGraphe(NodePhotographer n)
        {                        
        //    labelNomPhotographe.
         //   n.Text 
        }

        private void FillGuiCustomer(NodeUser n)
        {
            this.labelChamberNumber.Text = n.Text;
            this.labelPassword.Text = n.Password;
            this.labelLeavingDate.Text = n.LeavingDate;
            this.labelNameUser.Text = n.name +"/" +n.firstname;
            if (n.FactureEdit == true)
            {
                buttonPrint.Visible = true;
            }
            else
            {
                buttonPrint.Visible = false;
            }
            if (n.orderCD)
            {
              this.checkBoxCustomerOrderCD.CheckState = CheckState.Checked;
            }
            else
            {
                this.checkBoxCustomerOrderCD.CheckState = CheckState.Unchecked;
            }
        }

        private void FillGuiCategory(NodeCategory n)
        {
            textBoxEnglish.Text = n.EnglishText;
            textBoxFrench.Text = n.FenchText;
            textBoxPictureCategory.Text = n.Bitmap;
            if (textBoxPictureCategory.Text == "")
            {
                pictureBoxCategoryPicture.Visible = false;
            }
            else
            {
                pictureBoxCategoryPicture.Visible = true;
            }
            textBoxDeutch.Text = n.DeutchText;
            textBoxItalian.Text = n.ItalianText;
            textBoxSpanish.Text = n.SpanishText;
             labelPathCategory.Text = n.Directory;
        }

        private void FillGuiInfo(NodeInfo n)
        {
            textBoxInfoEnglish.Text = n.EnglishText;
            textBoxInfoFrench.Text = n.FenchText;
            textBoxPictureInfo.Text = n.Bitmap;
            textBoxInfoDeutch.Text = n.DeutchText;
            textBoxInfoItalian.Text = n.ItalianText;
            textBoxInfoSpanish.Text = n.SpanishText;
            textBoxInfoUrl.Text = n.Directory;
        
        }

        private void FillGuiConfig(NodeConfig n)
        {
            textBoxguiconfigLanguage.Text = n.ImageLangue;
            textBoxguiconfigInfo.Text = n.ImageInfo;
            textBoxguiconfigImagePhoto.Text = n.ImagePhoto;
            panelColorEnd.BackColor = n.colorEnd;
            panelColorStart.BackColor = n.colorStart;
            angleSelector1.Angle = n.Angle;
            if (n.RotateImage == "true")
            {
                checkBoxRotateImage.Checked = true;
            }
            else
            {
                checkBoxRotateImage.Checked = false;
            }
        }

        private void FillGuiPrice(NodePrice n)
        {
         
        }


        private void showCustomerLeavingTomorrow()
        {
            FFindCustomerByDate ffc = new FFindCustomerByDate(nodeCustomer, FFindCustomerByDate.typeSearchCustomer.TOMOROW);
            if (ffc.users.Count > 0)
            {
                ffc.ShowDialog();
            }
        }

        private static void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            TreeNode rootNode = e.Argument as TreeNode;

          //  List<TreeNode> nodes = GetNodes(rootNode);
            //List<TreeNode> nodes = new List<TreeNode>();
            
            /*DemoForm.nodeCategory = new TreeNode("Category");
            DemoForm.nodeCustomer = new TreeNode("Customer");
            DemoForm.nodeCategory = new TreeNode("Photographer");
            DemoForm.nodeCustomer = new TreeNode("Info");
            nodes.Add(DemoForm.nodeCategory);
            nodes.Add(DemoForm.nodeCustomer);
            nodes.Add(DemoForm.nodephotographer);
            nodes.Add(DemoForm.nodeInfo);
            
            e.Result = new KeyValuePair<TreeNode, List<TreeNode>>(rootNode, nodes);
             * */
        }

        private  void fillTree()
        {
            nodeCategory = new NodesCategory();
            nodeCustomer = new NodesUser();
            nodephotographer = new NodesPhotographer();
            nodeInfo = new NodesInfo();
            nodeConfig = new NodeConfig();
            nodePrice = new NodePrice();
            treeView1.Nodes.Add(nodeConfig);
            treeView1.Nodes.Add(nodeCategory);
            treeView1.Nodes.Add(nodeCustomer);
            treeView1.Nodes.Add(nodephotographer);
            treeView1.Nodes.Add(nodeInfo);
            treeView1.Nodes.Add(nodePrice);
            
            for (int n = 0; n < nodePrice.NbProduct(); n++)
            {
                flowLayoutPanelProduct.Controls.Add((Control)new ProductControl(nodePrice.getProduct(n)));
            }
            photoPromotionEntry1.setXmlNode(nodePrice.getPromotion(0));
            photoPromotionEntry2.setXmlNode(nodePrice.getPromotion(1));
            photoPromotionEntry3.setXmlNode(nodePrice.getPromotion(2));
            photoPromotionEntry4.setXmlNode(nodePrice.getPromotion(3));
            photoPromotionEntry5.setXmlNode(nodePrice.getPromotion(4));
            photoPromotionEntry6.setXmlNode(nodePrice.getPromotion(5));
        }

        #endregion

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FCategoryDialog fc = new FCategoryDialog();
            if (fc.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {                
            }
        }

        private void buttonAddNewCategory_Click(object sender, EventArgs e)
        {
            FAddChildCategory facc = new FAddChildCategory();
            if (facc.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string dirName =  facc.NameDirectory;
                if (facc.IsCreateDirectoryFromParent)
                {
                    if (nodeCategory.Directory == "") { MessageBox.Show("Error Creation Directory "); return; };
                    
                        try {
                            Directory.CreateDirectory(DirUtil.JoinDirAndFile(nodeCategory.Directory, facc.NameCategory));
                            dirName = DirUtil.JoinDirAndFile(nodeCategory.Directory, facc.NameCategory);
                        } catch(Exception exp) { MessageBox.Show("Error Creation Directory "+exp.Message); return; }
                }
                nodeCategory.AddCategory(facc.NameCategory,dirName);
                nodeCategory.Save();
            }            
        }

        private void buttonDeleteCategory_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null) return;
            if (MessageBox.Show("Are you sure you want to delete category " +treeView1.SelectedNode.Text ,"Delete category",MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
            {
                if (treeView1.SelectedNode.GetType().Name == "NodeCategory")
                {

                    nodeCategory.DeleteElem((NodesBase)treeView1.SelectedNode);
                    treeView1.Nodes.Remove(treeView1.SelectedNode);
                }
            }
        }

        private void buttonAddChildCategory_Click(object sender, EventArgs e)
        {
            FAddChildCategory facc = new FAddChildCategory();
            if (facc.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (treeView1.SelectedNode.GetType().Name == "NodeCategory")
                {
                    string dirName = facc.NameDirectory;                    
                    NodeCategory nodeParent = (NodeCategory) treeView1.SelectedNode;
                    if (nodeParent.Directory == "") { MessageBox.Show("Error Creation Directory "); return; };
                    
                        try {
                           Directory.CreateDirectory(DirUtil.JoinDirAndFile(nodeParent.Directory , facc.NameCategory));
                           dirName = DirUtil.JoinDirAndFile(nodeParent.Directory, facc.NameCategory);
                        } catch(Exception exp) { MessageBox.Show("Error Creation Directory "+exp.Message); return; }
                    tabControl1.SelectedIndex = 0;                    
                    NodeCategory node = (NodeCategory) treeView1.SelectedNode;
                    node.AddCategory(facc.NameCategory,dirName);            
                }                
            }            
        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }
       

        private void buttonAddInfo_Click(object sender, EventArgs e)
        {
            if (textBoxInfo.Text.Trim() == "")
            {
                MessageBox.Show("Name is Empty, please Fill ");
                return;
            }
            nodeInfo.AddInfo(textBoxInfo.Text);
        }

        private void buttonDeleteInfo_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null) return;
            if (MessageBox.Show("Are you sure you want to delete information " + treeView1.SelectedNode.Text, "Delete information", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
            {
                if (treeView1.SelectedNode.GetType().Name == "NodeInfo")
                {
                    nodeInfo.DeleteElem((NodeInfo)treeView1.SelectedNode);
                    nodeInfo.Save();
                    treeView1.Nodes.Remove(treeView1.SelectedNode);
                }
            }            
        }

        private void buttonDeleteCustomer_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null) return;
            if (MessageBox.Show("Are you sure you want to delete customer " + treeView1.SelectedNode.Text, "Delete customer", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
            {
                if (treeView1.SelectedNode.GetType().Name == "NodeUser")
                {
                    try
                    {
                        NodeUser nu = (NodeUser)treeView1.SelectedNode;
                        String destDir = nu.getBackupDirectory();
                        Directory.CreateDirectory(destDir);
                        File.Copy(
                                  DirUtil.JoinDirAndFile
                                  (NodesUser.path, nu.Text + ".xml")
                                         ,
                                  DirUtil.JoinDirAndFile(destDir, nu.Text + ".xml")
                                );

                        Directory.Move(nu.getDirectory(), DirUtil.JoinDirAndFile( destDir, nu.chamberNumber));                 
                        treeView1.Nodes.Remove(treeView1.SelectedNode);                        
                        File.Delete(DirUtil.JoinDirAndFile(NodesUser.path, nu.Text + ".xml"));
                    }
                    catch (Exception exp)
                    {
                        MessageBox.Show("Error Delete " + exp.Message);
                    }
                }
            }           

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode == null) return;
            if (MessageBox.Show("Are you sure you want to delete photographe " + treeView1.SelectedNode.Text, "Delete Photographe", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
            {
                if (treeView1.SelectedNode.GetType().Name == "NodePhotographer")
                {
                    nodephotographer.DeleteElem((NodePhotographer)treeView1.SelectedNode);
                    nodephotographer.Save();
                    treeView1.Nodes.Remove(treeView1.SelectedNode);
                }
            }           
        }

        private void buttonAddPhotographer_Click(object sender, EventArgs e)
        {
            if (textBoxNamePhotographer.Text.Trim() == "")
            {
                MessageBox.Show("Name is Empty, please Fill ");
                return;
            }
            nodephotographer.AddPhotographe(textBoxNamePhotographer.Text);
        }

        private void buttonChoosePicture_Click(object sender, EventArgs e)
        {
            if (openFileDialogPicture.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBoxPictureCategory.Text = openFileDialogPicture.FileName;
            }
        }

        private void buttonChoosePictureInfo_Click(object sender, EventArgs e)
        {
            if (openFileDialogPicture.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.textBoxPictureInfo.Text = openFileDialogPicture.FileName;
            }
        }

        private void treeView1_DragEnter(object sender, DragEventArgs e)
        {
            if (treeView1.SelectedNode ==null) 
            {
                e.Effect = DragDropEffects.None;
            }
            if (treeView1.SelectedNode.GetType().Name == "NodeCategory")
            {
                e.Effect = DragDropEffects.Move;
            }
        }

        private void treeView1_DragDrop(object sender, DragEventArgs e)
        {
            
            //Control dragNode = treeView1.Nodes.(new Point(e.X, e.Y));
            //MessageBox.Show(dragNode.Text);

            string[] filenames = (string[])e.Data.GetData(DataFormats.FileDrop);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you really want to quit ?", "Quit ", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
            {
                Application.Exit();
            }
        }

        private bool DeleteFiles(ImageListView imgListView,bool iscategory=false)
        {
            ImageListView.ImageListViewSelectedItemCollection selected = imgListView.SelectedItems;
            // On multiplie par deux pour les images et miniatures.
            String[] files;
            List<String> imgFile = new List<string>();

            try
           {  
                                              
              foreach (ImageListViewItem img in selected)
               {
                                  
                    imgFile.Add(img.FileName);
                    FileInfo fi = new FileInfo(img.FileName);
                    FileInfo fiMiniature = new FileInfo(DirUtil.JoinDirAndFile(fi.DirectoryName, "miniatures/") + fi.Name);
                    if (fiMiniature.Exists && iscategory)
                    {
                        imgFile.Add(DirUtil.JoinDirAndFile(fi.DirectoryName, "miniatures/") + fi.Name);                           
                    }                    
              }

              files = new string[imgFile.Count];
              for (int n = 0; n < imgFile.Count; n++)
              {
                files[n] = imgFile[n];
              }
            ShellFileOperation sfo = new ShellFileOperation();                       
            sfo.SourceFiles = files;
            sfo.Operation = ShellFileOperation.FileOperations.FO_DELETE;
            return sfo.DoOperation();
        }
            catch (Exception e)
                {
                    MessageBox.Show(" Error deleting \n " + e.Message);
                }
            return false;
        }

        private void DeleteDirectory(string dir)
        {
            string[] dirs = new string[1];
            dirs[0] = dir;
            ShellFileOperation sfo = new ShellFileOperation();
            sfo.SourceFiles = dirs;
            sfo.Operation = ShellFileOperation.FileOperations.FO_DELETE;
            sfo.DoOperation();
        }
        /*
        private void CopyFiles(ImageListView imgListView)
        {
            List<String> lf = new List<String>();
            ImageListView.ImageListViewSelectedItemCollection selected = imgListView.SelectedItems;

            foreach (ImageListViewItem img in selected)
            {
                lf.Add(img.FileName);
            }
            CopyPasteManager.Copy(lf);
        }
        private void CutFiles(ImageListView imgListView)
        {
            List<String> lf = new List<String>();
            ImageListView.ImageListViewSelectedItemCollection selected = imgListView.SelectedItems;

            foreach (ImageListViewItem img in selected)
            {
                lf.Add(img.FileName);
            }
            CopyPasteManager.Cut(lf);
        }

        private void deleteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode.GetType().Name == "NodeCategory")
            {
                DeleteFiles(imageListView1);
            }

            if (treeView1.SelectedNode.GetType().Name == "NodeUser")
            {
                DeleteFiles(imageListView2);
            }
         
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode.GetType().Name == "NodeCategory")
            {
               CopyFiles(imageListView1);                
            }
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode.GetType().Name == "NodeCategory")
            {
                CutFiles(imageListView1);
            }
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode.GetType().Name == "NodeCategory")
            {
                NodeCategory nc = (NodeCategory)treeView1.SelectedNode;
                CopyPasteManager.Paste(nc.getDirectory());
                PopulateListView(new DirectoryInfo(nc.getDirectory()));

            }
        }
         * */

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            nodeCategory.Save();
            nodeInfo.Save();
            nodephotographer.Save();
            nodeConfig.Save();
            //savePrice();
            nodePrice.Save();
        }

        private void textBoxEnglish_TextChanged(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode.GetType().Name == "NodeCategory")
            {
                NodeCategory n = (NodeCategory)treeView1.SelectedNode;
                n.EnglishText = textBoxEnglish.Text;
            }
        }

        private void textBoxFrench_TextChanged(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode.GetType().Name == "NodeCategory")
            {
                NodeCategory n = (NodeCategory)treeView1.SelectedNode;
                n.FenchText = textBoxFrench.Text;
            }
        }

        private void textBoxPictureCategory_TextChanged(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode.GetType().Name == "NodeCategory")
            {
                try
                {
                    NodeCategory n = (NodeCategory)treeView1.SelectedNode;
                    n.Bitmap = textBoxPictureCategory.Text;
                    FileInfo fi = new FileInfo(textBoxPictureCategory.Text);
                    if (fi.Exists)
                    {
                        pictureBoxCategoryPicture.Visible = true;
                        pictureBoxCategoryPicture.SizeMode = PictureBoxSizeMode.StretchImage;
                        pictureBoxCategoryPicture.Load(textBoxPictureCategory.Text);
                    }
                    else
                    {
                        pictureBoxCategoryPicture.Visible = false;
                    }
                }
                catch (Exception exp)
                {
                    MessageBox.Show("Error Path " + textBoxPictureCategory.Text + " \n " + exp.Message);
                }
            }
        }

        private void textBoxInfoEnglish_TextChanged(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode.GetType().Name == "NodeInfo")
            {
                NodeInfo n = (NodeInfo)treeView1.SelectedNode;
                n.EnglishText = textBoxInfoEnglish.Text;
            }
        }

        private void textBoxInfoFrench_TextChanged(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode.GetType().Name == "NodeInfo")
            {
                NodeInfo n = (NodeInfo)treeView1.SelectedNode;
                n.FenchText =  textBoxInfoFrench.Text;
            }
        }

        private void textBoxDeutch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                NodeCategory n = (NodeCategory)treeView1.SelectedNode;
                n.DeutchText = textBoxDeutch.Text;
            }
            catch (Exception exp) { }
        }

        private void textBoxItalian_TextChanged(object sender, EventArgs e)
        {
            try
            {
                NodeCategory n = (NodeCategory)treeView1.SelectedNode;
                n.ItalianText = textBoxItalian.Text;
            }
            catch (Exception exp) { }
        }

        private void textBoxSpanish_TextChanged(object sender, EventArgs e)
        {
            try
            {
                NodeCategory n = (NodeCategory)treeView1.SelectedNode;
                n.SpanishText=  textBoxSpanish.Text;
            }
            catch (Exception exp) { }
        }

        private void textBoxInfoDeutch_TextChanged(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode.GetType().Name == "NodeInfo")
            {
                NodeInfo n = (NodeInfo)treeView1.SelectedNode;
                n.DeutchText = textBoxInfoDeutch.Text;
            }
        }

        private void textBoxInfoItalian_TextChanged(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode.GetType().Name == "NodeInfo")
            {
                NodeInfo n = (NodeInfo)treeView1.SelectedNode;
                n.ItalianText= textBoxInfoItalian.Text;                   
            }
        }

        private void textBoxInfoSpanish_TextChanged(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode.GetType().Name == "NodeInfo")
            {
                NodeInfo n = (NodeInfo)treeView1.SelectedNode;
                n.SpanishText = textBoxInfoSpanish.Text;
            }
        }

        private void textBoxPictureInfo_TextChanged(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode.GetType().Name == "NodeInfo")
            {
                NodeInfo n = (NodeInfo)treeView1.SelectedNode;
                n.Bitmap = textBoxPictureInfo.Text;

                FileInfo fi = new FileInfo(textBoxPictureInfo.Text);
                if (fi.Exists)
                {
                    pictureBoxInformation.Visible = true;
                    pictureBoxInformation.SizeMode = PictureBoxSizeMode.StretchImage;
                    pictureBoxInformation.Load(textBoxPictureInfo.Text);
                }
                else
                {
                    pictureBoxInformation.Visible = false;
                }                
            }
        }

        private void tableLayoutPanel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBoxInfoUrl_TextChanged(object sender, EventArgs e)
        {
             if (treeView1.SelectedNode.GetType().Name == "NodeInfo")
            {
                NodeInfo n = (NodeInfo)treeView1.SelectedNode;
                n.Directory= textBoxInfoUrl.Text;
            }            
        }

        private void buttonImportPhoto_Click(object sender, EventArgs e)
        {          
        }

        private void toolStripButtonImportPhoto_Click(object sender, EventArgs e)
        {
            FWizardImport wizard = new FWizardImport(nodephotographer,nodeCategory);
            if (wizard.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (wizard.hasError == false)
                {
                    this.Cursor = Cursors.WaitCursor;
                    ShellFileOperation sfo = new ShellFileOperation();
                    sfo.Operation = ShellFileOperation.FileOperations.FO_COPY;
                    //sfo.OperationFlags = ShellFileOperation.ShellFileOperationFlags.FOF_SIMPLEPROGRESS;
                    sfo.OwnerWindow = this.Handle;
                    sfo.SourceFiles = wizard.ListImage;
                    sfo.DestFiles = wizard.DstImage;
                    sfo.ProgressTitle = "Import...";
                    sfo.DoOperation();                    

                    toolStripProgressBarThumb.Visible = true;
                    toolStripProgressBarThumb.Minimum = 0;
                    toolStripProgressBarThumb.Maximum = sfo.DestFiles.Length;
                    ThumbMaker tm = new ThumbMaker();
                    tm.processFiles(sfo.DestFiles, wizard.DirectoryDst + "\\miniatures\\", delegate(int f, string m)
                    {
                        if (f == -1)
                        {
                           toolStripStatusLabel1.Text = m;                                                       
                          //treeView1.SelectedNode = ncat;  
                        }
                        else
                        {
                            toolStripStatusLabel1.Text = " Processing thumbnail File " + f.ToString();
                            toolStripProgressBarThumb.Value = f;
                            if (toolStripProgressBarThumb.Maximum-1 == f)
                            {
                                
                                NodeCategory ncat = wizard.getSelectedCategory();
                                tabControl1.SelectedIndex = 0;
                                try
                                {
                                    PopulateListView(new DirectoryInfo(ncat.getDirectory()));
                                }
                                catch (Exception exp)
                                {
                                    MessageBox.Show("Error in category " + exp.Message);
                                }
                                FillGuiCategory(ncat);
                                //TreeNode node = (TreeNode)nodeCategory;
                                //ExpandNode(nodeCategory, node);
                                //nodeCategory.Expand();                                         
                            }
                        }
                    });
                    this.Cursor = Cursors.Arrow;
                    toolStripProgressBarThumb.Visible = false;
                }
                else
                {
                    MessageBox.Show("Error Happen in the wizard configuration please try again ");
                }
            }
        }

        private bool ExpandNode(TreeNode node,TreeNode Final)
        {
            if (node == Final) return true;
            foreach (TreeNode n in node.Nodes)
            {
                if (ExpandNode(node, Final))
                {
                    node.Expand();
                    return true;
                }
                else
                {
                    return false;
                }
            }
                return false;
        }

        private void buttonPrint_Click_1(object sender, EventArgs e)
        {
            string appPath = @"C:\Program Files (x86)\IrfanView\";
            Process myProcess = new Process();
            try
            {
                ImageListView.ImageListViewSelectedItemCollection selected = imageListView2.SelectedItems;
                if (selected.Count != 1)
                {
                    MessageBox.Show("Error please select only one photo ");
                    return;
                }

                myProcess.StartInfo.FileName = "\"" + appPath + "\\i_view32\"";
                myProcess.StartInfo.Arguments = (" \""+selected[0].FileName+"\" ");
                myProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                myProcess.StartInfo.UseShellExecute = false;
                myProcess.StartInfo.CreateNoWindow = true;                
                myProcess.Start();
                Thread.Sleep(500);

                KeyboardSimulator.KeyboardSimulator.SimulateStandardShortcut(KeyboardSimulator.StandardShortcut.Print);

                //TODO FIND THE FORMAT OF THE IMAGE.....
                string format = ((NodeUser)treeView1.SelectedNode).getFormatImage(selected[0].FileName);
                printandCommandControler.PrintAndCommandControler.PrintLogger(selected[0].FileName, format);
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Printing "+ex.Message);
            }
        }

        private void buttonChoosePictureInfo_Click_1(object sender, EventArgs e)
        {
            if (openFileDialogPicture.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.textBoxPictureInfo.Text = openFileDialogPicture.FileName;
            }
        }

        private void buttonCash_Click(object sender, EventArgs e)
        {            
            buttonPrint.Visible = true;            
            ExcelExporter exe = new ExcelExporter();
            if (treeView1.SelectedNode.GetType().Name == "NodeUser")
            {
                NodeUser nu = (NodeUser)treeView1.SelectedNode;
                nu.FactureEdit = true;
                nu.Save();
                exe.ExcelCustomer((NodeUser)treeView1.SelectedNode,nodePrice);
                printandCommandControler.PrintAndCommandControler.CommandLogger((NodeUser)treeView1.SelectedNode,nodePrice);
            }

        }
        
        private void buttonCustomerSearch(object sender, EventArgs e)
        {
            FFindCustomerByDate ffc = new FFindCustomerByDate(nodeCustomer, FFindCustomerByDate.typeSearchCustomer.PAST);

            if (sender == leaveTomorowToolStripMenuItem)
            {
                ffc = new FFindCustomerByDate(nodeCustomer, FFindCustomerByDate.typeSearchCustomer.TOMOROW);
            }
            if (sender == leaveTodayToolStripMenuItem)
            {
                ffc = new FFindCustomerByDate(nodeCustomer, FFindCustomerByDate.typeSearchCustomer.TODAY);
            }
            if (sender == lEftToolStripMenuItem)
            {
                ffc = new FFindCustomerByDate(nodeCustomer, FFindCustomerByDate.typeSearchCustomer.PAST);
            }
            if (ffc.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (ffc.Selected != "")
                {
                    try
                    {
                        treeView1.SelectedNode = nodeCustomer.Select(ffc.Selected);
                    }
                    catch (Exception exp)
                    {
                        MessageBox.Show("Error selecting Client " + exp.Message);
                    }
                }
            }
        }

        private void buttonDialogImgInfo_Click(object sender, EventArgs e)
        {
            if (openFileDialogguiImage.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBoxguiconfigInfo.Text = openFileDialogguiImage.FileName;
            }
        }

        private void textBoxguiconfigInfo_TextChanged(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode.GetType().Name == "NodeConfig")
            {
                NodeConfig n = (NodeConfig)treeView1.SelectedNode;
                n.ImageInfo = textBoxguiconfigInfo.Text;

                FileInfo fi = new FileInfo(textBoxguiconfigInfo.Text);
                if (fi.Exists)
                {
                    pictureBoxGuiInfo.SizeMode = PictureBoxSizeMode.StretchImage;
                    pictureBoxGuiInfo.Load(textBoxguiconfigInfo.Text);                    
                }
            }
        }

        private void buttonFindImagePhoto_Click(object sender, EventArgs e)
        {
            if (openFileDialogguiImage.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.textBoxguiconfigImagePhoto.Text = openFileDialogguiImage.FileName;
            }
        }

        private void buttonFindImageLanguage_Click(object sender, EventArgs e)
        {
            if (openFileDialogguiImage.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.textBoxguiconfigLanguage.Text = openFileDialogguiImage.FileName;
            }
        }

        private void textBoxguiconfigImagePhoto_TextChanged(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode.GetType().Name == "NodeConfig")
            {
                NodeConfig n = (NodeConfig)treeView1.SelectedNode;
                n.ImagePhoto = textBoxguiconfigImagePhoto.Text;

                FileInfo fi = new FileInfo(textBoxguiconfigImagePhoto.Text);
                if (fi.Exists)
                {
                    pictureBoxguiconfigPhoto.SizeMode = PictureBoxSizeMode.StretchImage;
                    pictureBoxguiconfigPhoto.Load(textBoxguiconfigImagePhoto.Text);
                }
            }
        }

        private void textBoxguiconfigLanguage_TextChanged(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode.GetType().Name == "NodeConfig")
            {
                NodeConfig n = (NodeConfig)treeView1.SelectedNode;
                n.ImageLangue = textBoxguiconfigLanguage.Text;

                FileInfo fi = new FileInfo(textBoxguiconfigLanguage.Text);
                if (fi.Exists)
                {
                    pictureBoxguiconfigLanguage.SizeMode = PictureBoxSizeMode.StretchImage;
                    pictureBoxguiconfigLanguage.Load(textBoxguiconfigLanguage.Text);
                }
            }
        }

        private void buttonChangeStartColor_Click(object sender, EventArgs e)
        {
            if (colorDialogStart.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (treeView1.SelectedNode.GetType().Name == "NodeConfig")
                {
                NodeConfig n = (NodeConfig)treeView1.SelectedNode;
                n.colorStart = colorDialogStart.Color;
                }
                panelColorStart.BackColor = colorDialogStart.Color;
                panelGradient.Invalidate();
            }
        }

        private void buttonChangeColorEnd_Click(object sender, EventArgs e)
        {
            if (colorDialogEnd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (treeView1.SelectedNode.GetType().Name == "NodeConfig")
                {
                    NodeConfig n = (NodeConfig)treeView1.SelectedNode;
                    n.colorEnd = colorDialogEnd.Color;
                }
                panelColorEnd.BackColor = colorDialogEnd.Color;
                panelGradient.Invalidate();
            }
        }

        private void angleSelector1_AngleChanged()
        {
            if (treeView1.SelectedNode.GetType().Name == "NodeConfig")
            {
                NodeConfig n = (NodeConfig)treeView1.SelectedNode;
                n.Angle = angleSelector1.Angle;
                panelGradient.Invalidate();
            }
        }
        private void UpdateGradient()
        {            
        }

        private void panelGradient_Paint(object sender, PaintEventArgs e)
        {
            LinearGradientBrush lgb = new LinearGradientBrush(panelGradient.ClientRectangle, panelColorStart.BackColor, panelColorEnd.BackColor, (float)angleSelector1.Angle, true);
            e.Graphics.FillRectangle(lgb, panelGradient.ClientRectangle);
            lgb.Dispose();
        }

        private void deleteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode.GetType().Name == "NodeCategory")
            {
                DeleteFiles(imageListView1,true);
                PopulateListView(new DirectoryInfo(((NodeCategory)treeView1.SelectedNode).getDirectory()));
            }

            if (treeView1.SelectedNode.GetType().Name == "NodeUser")
            {
                NodeUser nu = (NodeUser)treeView1.SelectedNode;
                ImageListView.ImageListViewSelectedItemCollection selected = this.imageListView2.SelectedItems;
                foreach (ImageListViewItem img in selected)
                {
                    nu.DeleteImage(img.FileName);                    
                }
                nu.Save();
                DeleteFiles(imageListView2,false);
                PopulateListViewCustomer();
            }
        }

        private void checkBoxCustomerOrderCD_CheckedChanged(object sender, EventArgs e)
        {
            NodeUser nu = (NodeUser)treeView1.SelectedNode;
            nu.orderCD = checkBoxCustomerOrderCD.Checked;
            nu.Save();
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void label31_Click(object sender, EventArgs e)
        {

        }

        private void imageListView1_DropFiles(object sender, DropFileEventArgs e)
        {
          if (treeView1.SelectedNode.GetType().Name == "NodeCategory")
          {
              NodeCategory nCat = (NodeCategory)treeView1.SelectedNode;
              if (nCat.IsLeaf() == false)
              {
                  MessageBox.Show("You can't drop on a category with child ");
                  e.Cancel = true;
                  return;
              }
            FChoosePhotographer choosePhotographer = new FChoosePhotographer(nodephotographer);
            if (choosePhotographer.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                
                FilenamePhotoProvider fpp = new FilenamePhotoProvider(choosePhotographer.Photographe);

                ImageListView.ImageListViewSelectedItemCollection selected = imageListView1.SelectedItems;
                String DirectoryDst = nCat.getDirectory();
                List<string> Lfile = fpp.getPossibleFilename(DirectoryDst, e.FileNames.Length);

                string[] fileDst = new String[Lfile.Count];

                int n = 0;
                foreach (String s in Lfile)
                {
                    fileDst[n] = s;
                    n++;
                }


                this.Cursor = Cursors.WaitCursor;
                ShellFileOperation sfo = new ShellFileOperation();
                sfo.Operation = ShellFileOperation.FileOperations.FO_COPY;
                sfo.OwnerWindow = this.Handle;
                sfo.SourceFiles = e.FileNames;
                sfo.DestFiles =fileDst;
                sfo.ProgressTitle = "Import...";
                sfo.DoOperation();

                toolStripProgressBarThumb.Visible = true;
                toolStripProgressBarThumb.Minimum = 0;
                toolStripProgressBarThumb.Maximum = sfo.DestFiles.Length;
                ThumbMaker tm = new ThumbMaker();
                
                  tm.processFiles(sfo.DestFiles, DirectoryDst + "\\miniatures\\", delegate(int f, string m)
                  {
                        if (f == -1)
                        {
                           toolStripStatusLabel1.Text = m;                                                                                
                        }
                        else
                        {
                            toolStripStatusLabel1.Text = " Processing thumbnail File " + f.ToString();
                            toolStripProgressBarThumb.Value = f;
                            if (toolStripProgressBarThumb.Maximum-1 == f)
                            {                                                                
                                tabControl1.SelectedIndex = 0;
                                try
                                {
                                    PopulateListView(new DirectoryInfo(nCat.getDirectory()));
                                }
                                catch (Exception exp)
                                {
                                    MessageBox.Show("Error in category " + exp.Message);
                                }
                                FillGuiCategory(nCat);
                                //TreeNode node = (TreeNode)nodeCategory;
                                //ExpandNode(nodeCategory, node);
                                //nodeCategory.Expand();                                         
                            }
                        }
                    });
                    this.Cursor = Cursors.Arrow;
                    toolStripProgressBarThumb.Visible = false;
                }
                else
                {
                    MessageBox.Show("Error Happen in the wizard configuration please try again ");
                }
                 
                 }
             e.Cancel = true;      
            }

        private void buttonOpenDir_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode.GetType().Name == "NodeCategory")
            {
                NodeCategory nodeCat = (NodeCategory)treeView1.SelectedNode;
                ShellExecute se = new ShellExecute();
                se.Path = nodeCat.getDirectory();
                se.Execute();
            }
        }

        private void checkBoxRotateImage_CheckedChanged(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode.GetType().Name == "NodeConfig")
            {
                NodeConfig n = (NodeConfig)treeView1.SelectedNode;
                if (checkBoxRotateImage.Checked)
                {
                    n.RotateImage = "true";
                }
                else
                {
                    n.RotateImage = "false";
                }
            }

        }
       
        }           
}
