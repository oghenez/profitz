namespace WindowsFormsApplication1
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            System.Windows.Forms.TreeNode treeNode32 = new System.Windows.Forms.TreeNode("TRCS001 - Sales");
            System.Windows.Forms.TreeNode treeNode33 = new System.Windows.Forms.TreeNode("TRCS002 - Delivery Order");
            System.Windows.Forms.TreeNode treeNode34 = new System.Windows.Forms.TreeNode("TRCS003 - Customer Invoice");
            System.Windows.Forms.TreeNode treeNode35 = new System.Windows.Forms.TreeNode("TRCS004 - Customer Payment");
            System.Windows.Forms.TreeNode treeNode36 = new System.Windows.Forms.TreeNode("TRCS005 - Sales Return");
            System.Windows.Forms.TreeNode treeNode37 = new System.Windows.Forms.TreeNode("TRCS006 - Credit Note");
            System.Windows.Forms.TreeNode treeNode38 = new System.Windows.Forms.TreeNode("TRCP001 - Purchase");
            System.Windows.Forms.TreeNode treeNode39 = new System.Windows.Forms.TreeNode("TRCP002 - Good Receipt");
            System.Windows.Forms.TreeNode treeNode40 = new System.Windows.Forms.TreeNode("TRCP003 - Supplier Invoice");
            System.Windows.Forms.TreeNode treeNode41 = new System.Windows.Forms.TreeNode("TRCP004 - Payment");
            System.Windows.Forms.TreeNode treeNode42 = new System.Windows.Forms.TreeNode("TRCP005 - Purchase Return");
            System.Windows.Forms.TreeNode treeNode43 = new System.Windows.Forms.TreeNode("TRCP006 - Debit Note");
            System.Windows.Forms.TreeNode treeNode44 = new System.Windows.Forms.TreeNode("TRCI001 - Stock Taking");
            System.Windows.Forms.TreeNode treeNode45 = new System.Windows.Forms.TreeNode("TRCI002 - Part Master");
            System.Windows.Forms.TreeNode treeNode46 = new System.Windows.Forms.TreeNode("MSTF001 - Bank");
            System.Windows.Forms.TreeNode treeNode47 = new System.Windows.Forms.TreeNode("MSTF002 - Document Type");
            System.Windows.Forms.TreeNode treeNode48 = new System.Windows.Forms.TreeNode("MSTF003 - Exchange Rate");
            System.Windows.Forms.TreeNode treeNode49 = new System.Windows.Forms.TreeNode("MSTG001 - Currency");
            System.Windows.Forms.TreeNode treeNode50 = new System.Windows.Forms.TreeNode("MSTG002 - Employee");
            System.Windows.Forms.TreeNode treeNode51 = new System.Windows.Forms.TreeNode("MSTG003 - Division");
            System.Windows.Forms.TreeNode treeNode52 = new System.Windows.Forms.TreeNode("MSTI001 - Part Group");
            System.Windows.Forms.TreeNode treeNode53 = new System.Windows.Forms.TreeNode("MSTI002 - Part Category");
            System.Windows.Forms.TreeNode treeNode54 = new System.Windows.Forms.TreeNode("MSTI003 - Unit");
            System.Windows.Forms.TreeNode treeNode55 = new System.Windows.Forms.TreeNode("MSTI004 - Warehouse");
            System.Windows.Forms.TreeNode treeNode56 = new System.Windows.Forms.TreeNode("MSTD001 - Customer");
            System.Windows.Forms.TreeNode treeNode57 = new System.Windows.Forms.TreeNode("MSTD002 - Supplier");
            System.Windows.Forms.TreeNode treeNode58 = new System.Windows.Forms.TreeNode("MSTD003 - Customer Category");
            System.Windows.Forms.TreeNode treeNode59 = new System.Windows.Forms.TreeNode("MSTD004 - Supplier Category");
            System.Windows.Forms.TreeNode treeNode60 = new System.Windows.Forms.TreeNode("MSTD005 - Price Category");
            System.Windows.Forms.TreeNode treeNode61 = new System.Windows.Forms.TreeNode("MSTD006 - Tax");
            System.Windows.Forms.TreeNode treeNode62 = new System.Windows.Forms.TreeNode("MSTD007 - Term Of Payment");
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logInToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.lockApplicationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripComboBox1 = new System.Windows.Forms.ToolStripComboBox();
            this.kryptonManager1 = new ComponentFactory.Krypton.Toolkit.KryptonManager(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.kryptonNavigatorMain = new ComponentFactory.Krypton.Navigator.KryptonNavigator();
            this.buttonSpecExpandCollapse = new ComponentFactory.Krypton.Navigator.ButtonSpecNavigator();
            this.kryptonPageMail = new ComponentFactory.Krypton.Navigator.KryptonPage();
            this.treeView3 = new System.Windows.Forms.TreeView();
            this.kryptonHeader2 = new ComponentFactory.Krypton.Toolkit.KryptonHeader();
            this.treeView2 = new System.Windows.Forms.TreeView();
            this.kryptonHeader1 = new ComponentFactory.Krypton.Toolkit.KryptonHeader();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.kryptonHeaderFolders = new ComponentFactory.Krypton.Toolkit.KryptonHeader();
            this.kryptonPageCalendar = new ComponentFactory.Krypton.Navigator.KryptonPage();
            this.kryptonPageNotes = new ComponentFactory.Krypton.Navigator.KryptonPage();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.treeView4 = new System.Windows.Forms.TreeView();
            this.kryptonHeader3 = new ComponentFactory.Krypton.Toolkit.KryptonHeader();
            this.treeView5 = new System.Windows.Forms.TreeView();
            this.kryptonHeader4 = new ComponentFactory.Krypton.Toolkit.KryptonHeader();
            this.treeView6 = new System.Windows.Forms.TreeView();
            this.kryptonHeader5 = new ComponentFactory.Krypton.Toolkit.KryptonHeader();
            this.treeView7 = new System.Windows.Forms.TreeView();
            this.kryptonHeader6 = new ComponentFactory.Krypton.Toolkit.KryptonHeader();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonNavigatorMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageMail)).BeginInit();
            this.kryptonPageMail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageCalendar)).BeginInit();
            this.kryptonPageCalendar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageNotes)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolStripComboBox1});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(637, 25);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.logInToolStripMenuItem,
            this.toolStripSeparator1,
            this.lockApplicationToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 21);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // logInToolStripMenuItem
            // 
            this.logInToolStripMenuItem.Name = "logInToolStripMenuItem";
            this.logInToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.logInToolStripMenuItem.Text = "Log In";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(163, 6);
            // 
            // lockApplicationToolStripMenuItem
            // 
            this.lockApplicationToolStripMenuItem.Name = "lockApplicationToolStripMenuItem";
            this.lockApplicationToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.lockApplicationToolStripMenuItem.Text = "Lock Application";
            // 
            // toolStripComboBox1
            // 
            this.toolStripComboBox1.DropDownWidth = 200;
            this.toolStripComboBox1.Name = "toolStripComboBox1";
            this.toolStripComboBox1.Size = new System.Drawing.Size(121, 21);
            this.toolStripComboBox1.TextChanged += new System.EventHandler(this.toolStripComboBox1_TextChanged);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.statusStrip1.Location = new System.Drawing.Point(0, 511);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.ManagerRenderMode;
            this.statusStrip1.Size = new System.Drawing.Size(637, 22);
            this.statusStrip1.TabIndex = 7;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // kryptonNavigatorMain
            // 
            this.kryptonNavigatorMain.Button.ButtonDisplayLogic = ComponentFactory.Krypton.Navigator.ButtonDisplayLogic.None;
            this.kryptonNavigatorMain.Button.ButtonSpecs.AddRange(new ComponentFactory.Krypton.Navigator.ButtonSpecNavigator[] {
            this.buttonSpecExpandCollapse});
            this.kryptonNavigatorMain.Button.CloseButtonDisplay = ComponentFactory.Krypton.Navigator.ButtonDisplay.Hide;
            this.kryptonNavigatorMain.Dock = System.Windows.Forms.DockStyle.Left;
            this.kryptonNavigatorMain.Header.HeaderValuesPrimary.MapImage = ComponentFactory.Krypton.Navigator.MapKryptonPageImage.None;
            this.kryptonNavigatorMain.Location = new System.Drawing.Point(0, 25);
            this.kryptonNavigatorMain.Name = "kryptonNavigatorMain";
            this.kryptonNavigatorMain.NavigatorMode = ComponentFactory.Krypton.Navigator.NavigatorMode.OutlookFull;
            this.kryptonNavigatorMain.Pages.AddRange(new ComponentFactory.Krypton.Navigator.KryptonPage[] {
            this.kryptonPageMail,
            this.kryptonPageCalendar,
            this.kryptonPageNotes});
            this.kryptonNavigatorMain.SelectedIndex = 0;
            this.kryptonNavigatorMain.Size = new System.Drawing.Size(194, 486);
            this.kryptonNavigatorMain.StateCommon.CheckButton.Content.AdjacentGap = 5;
            this.kryptonNavigatorMain.TabIndex = 9;
            this.kryptonNavigatorMain.Text = "kryptonNavigator1";
            // 
            // buttonSpecExpandCollapse
            // 
            this.buttonSpecExpandCollapse.Type = ComponentFactory.Krypton.Toolkit.PaletteButtonSpecStyle.ArrowLeft;
            this.buttonSpecExpandCollapse.TypeRestricted = ComponentFactory.Krypton.Navigator.PaletteNavButtonSpecStyle.ArrowLeft;
            this.buttonSpecExpandCollapse.UniqueName = "1B343938A2284A991B343938A2284A99";
            this.buttonSpecExpandCollapse.Click += new System.EventHandler(this.buttonSpecExpandCollapse_Click);
            // 
            // kryptonPageMail
            // 
            this.kryptonPageMail.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageMail.Controls.Add(this.treeView3);
            this.kryptonPageMail.Controls.Add(this.kryptonHeader2);
            this.kryptonPageMail.Controls.Add(this.treeView2);
            this.kryptonPageMail.Controls.Add(this.kryptonHeader1);
            this.kryptonPageMail.Controls.Add(this.treeView1);
            this.kryptonPageMail.Controls.Add(this.kryptonHeaderFolders);
            this.kryptonPageMail.Flags = 65534;
            this.kryptonPageMail.ImageLarge = ((System.Drawing.Image)(resources.GetObject("kryptonPageMail.ImageLarge")));
            this.kryptonPageMail.ImageMedium = ((System.Drawing.Image)(resources.GetObject("kryptonPageMail.ImageMedium")));
            this.kryptonPageMail.ImageSmall = ((System.Drawing.Image)(resources.GetObject("kryptonPageMail.ImageSmall")));
            this.kryptonPageMail.LastVisibleSet = true;
            this.kryptonPageMail.MinimumSize = new System.Drawing.Size(180, 230);
            this.kryptonPageMail.Name = "kryptonPageMail";
            this.kryptonPageMail.Size = new System.Drawing.Size(192, 332);
            this.kryptonPageMail.Text = "Transaction";
            this.kryptonPageMail.TextTitle = "Transaction";
            this.kryptonPageMail.ToolTipTitle = "Page ToolTip";
            this.kryptonPageMail.UniqueName = "6D4A539F5AB946C76D4A539F5AB946C7";
            // 
            // treeView3
            // 
            this.treeView3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeView3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView3.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeView3.FullRowSelect = true;
            this.treeView3.Location = new System.Drawing.Point(0, 195);
            this.treeView3.Name = "treeView3";
            treeNode32.Name = "Node0";
            treeNode32.Text = "TRCS001 - Sales";
            treeNode33.Name = "Node1";
            treeNode33.Text = "TRCS002 - Delivery Order";
            treeNode34.Name = "Node2";
            treeNode34.Text = "TRCS003 - Customer Invoice";
            treeNode35.Name = "Node3";
            treeNode35.Text = "TRCS004 - Customer Payment";
            treeNode36.Name = "Node4";
            treeNode36.Text = "TRCS005 - Sales Return";
            treeNode37.Name = "Node5";
            treeNode37.Text = "TRCS006 - Credit Note";
            this.treeView3.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode32,
            treeNode33,
            treeNode34,
            treeNode35,
            treeNode36,
            treeNode37});
            this.treeView3.Size = new System.Drawing.Size(192, 137);
            this.treeView3.TabIndex = 9;
            // 
            // kryptonHeader2
            // 
            this.kryptonHeader2.Dock = System.Windows.Forms.DockStyle.Top;
            this.kryptonHeader2.HeaderStyle = ComponentFactory.Krypton.Toolkit.HeaderStyle.Secondary;
            this.kryptonHeader2.Location = new System.Drawing.Point(0, 174);
            this.kryptonHeader2.Name = "kryptonHeader2";
            this.kryptonHeader2.Size = new System.Drawing.Size(192, 21);
            this.kryptonHeader2.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)));
            this.kryptonHeader2.TabIndex = 7;
            this.kryptonHeader2.Values.Description = "";
            this.kryptonHeader2.Values.Heading = "Sales";
            this.kryptonHeader2.Values.Image = null;
            // 
            // treeView2
            // 
            this.treeView2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeView2.Dock = System.Windows.Forms.DockStyle.Top;
            this.treeView2.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeView2.FullRowSelect = true;
            this.treeView2.Location = new System.Drawing.Point(0, 76);
            this.treeView2.Name = "treeView2";
            treeNode38.Name = "Node0";
            treeNode38.Text = "TRCP001 - Purchase";
            treeNode39.Name = "Node1";
            treeNode39.Text = "TRCP002 - Good Receipt";
            treeNode40.Name = "Node2";
            treeNode40.Text = "TRCP003 - Supplier Invoice";
            treeNode41.Name = "Node3";
            treeNode41.Text = "TRCP004 - Payment";
            treeNode42.Name = "Node4";
            treeNode42.Text = "TRCP005 - Purchase Return";
            treeNode43.Name = "Node5";
            treeNode43.Text = "TRCP006 - Debit Note";
            this.treeView2.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode38,
            treeNode39,
            treeNode40,
            treeNode41,
            treeNode42,
            treeNode43});
            this.treeView2.Size = new System.Drawing.Size(192, 98);
            this.treeView2.TabIndex = 6;
            // 
            // kryptonHeader1
            // 
            this.kryptonHeader1.Dock = System.Windows.Forms.DockStyle.Top;
            this.kryptonHeader1.HeaderStyle = ComponentFactory.Krypton.Toolkit.HeaderStyle.Secondary;
            this.kryptonHeader1.Location = new System.Drawing.Point(0, 55);
            this.kryptonHeader1.Name = "kryptonHeader1";
            this.kryptonHeader1.Size = new System.Drawing.Size(192, 21);
            this.kryptonHeader1.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)));
            this.kryptonHeader1.TabIndex = 5;
            this.kryptonHeader1.Values.Description = "";
            this.kryptonHeader1.Values.Heading = "Purchase";
            this.kryptonHeader1.Values.Image = null;
            // 
            // treeView1
            // 
            this.treeView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Top;
            this.treeView1.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeView1.Location = new System.Drawing.Point(0, 21);
            this.treeView1.Name = "treeView1";
            treeNode44.Name = "Node0";
            treeNode44.Text = "TRCI001 - Stock Taking";
            treeNode45.Name = "Node1";
            treeNode45.Text = "TRCI002 - Part Master";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode44,
            treeNode45});
            this.treeView1.Size = new System.Drawing.Size(192, 34);
            this.treeView1.TabIndex = 4;
            // 
            // kryptonHeaderFolders
            // 
            this.kryptonHeaderFolders.Dock = System.Windows.Forms.DockStyle.Top;
            this.kryptonHeaderFolders.HeaderStyle = ComponentFactory.Krypton.Toolkit.HeaderStyle.Secondary;
            this.kryptonHeaderFolders.Location = new System.Drawing.Point(0, 0);
            this.kryptonHeaderFolders.Name = "kryptonHeaderFolders";
            this.kryptonHeaderFolders.Size = new System.Drawing.Size(192, 21);
            this.kryptonHeaderFolders.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)));
            this.kryptonHeaderFolders.TabIndex = 3;
            this.kryptonHeaderFolders.Values.Description = "";
            this.kryptonHeaderFolders.Values.Heading = "Internal";
            this.kryptonHeaderFolders.Values.Image = null;
            // 
            // kryptonPageCalendar
            // 
            this.kryptonPageCalendar.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageCalendar.Controls.Add(this.treeView7);
            this.kryptonPageCalendar.Controls.Add(this.kryptonHeader6);
            this.kryptonPageCalendar.Controls.Add(this.treeView6);
            this.kryptonPageCalendar.Controls.Add(this.kryptonHeader5);
            this.kryptonPageCalendar.Controls.Add(this.treeView5);
            this.kryptonPageCalendar.Controls.Add(this.kryptonHeader4);
            this.kryptonPageCalendar.Controls.Add(this.treeView4);
            this.kryptonPageCalendar.Controls.Add(this.kryptonHeader3);
            this.kryptonPageCalendar.Flags = 65534;
            this.kryptonPageCalendar.ImageLarge = ((System.Drawing.Image)(resources.GetObject("kryptonPageCalendar.ImageLarge")));
            this.kryptonPageCalendar.ImageMedium = ((System.Drawing.Image)(resources.GetObject("kryptonPageCalendar.ImageMedium")));
            this.kryptonPageCalendar.ImageSmall = ((System.Drawing.Image)(resources.GetObject("kryptonPageCalendar.ImageSmall")));
            this.kryptonPageCalendar.LastVisibleSet = true;
            this.kryptonPageCalendar.MinimumSize = new System.Drawing.Size(190, 155);
            this.kryptonPageCalendar.Name = "kryptonPageCalendar";
            this.kryptonPageCalendar.Size = new System.Drawing.Size(192, 332);
            this.kryptonPageCalendar.Text = "Master Data";
            this.kryptonPageCalendar.TextTitle = "Master Data";
            this.kryptonPageCalendar.ToolTipTitle = "Page ToolTip";
            this.kryptonPageCalendar.UniqueName = "20332D6AA91B4AF120332D6AA91B4AF1";
            // 
            // kryptonPageNotes
            // 
            this.kryptonPageNotes.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPageNotes.Flags = 65534;
            this.kryptonPageNotes.ImageLarge = ((System.Drawing.Image)(resources.GetObject("kryptonPageNotes.ImageLarge")));
            this.kryptonPageNotes.ImageMedium = ((System.Drawing.Image)(resources.GetObject("kryptonPageNotes.ImageMedium")));
            this.kryptonPageNotes.ImageSmall = ((System.Drawing.Image)(resources.GetObject("kryptonPageNotes.ImageSmall")));
            this.kryptonPageNotes.LastVisibleSet = true;
            this.kryptonPageNotes.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPageNotes.Name = "kryptonPageNotes";
            this.kryptonPageNotes.Padding = new System.Windows.Forms.Padding(20);
            this.kryptonPageNotes.Size = new System.Drawing.Size(192, 332);
            this.kryptonPageNotes.Text = "Reports";
            this.kryptonPageNotes.TextTitle = "Reports";
            this.kryptonPageNotes.ToolTipTitle = "Page ToolTip";
            this.kryptonPageNotes.UniqueName = "F896ACB8955B498FF896ACB8955B498F";
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(194, 25);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 486);
            this.splitter1.TabIndex = 10;
            this.splitter1.TabStop = false;
            // 
            // treeView4
            // 
            this.treeView4.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeView4.Dock = System.Windows.Forms.DockStyle.Top;
            this.treeView4.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeView4.Location = new System.Drawing.Point(0, 21);
            this.treeView4.Name = "treeView4";
            treeNode46.Name = "Node0";
            treeNode46.Text = "MSTF001 - Bank";
            treeNode47.Name = "Node1";
            treeNode47.Text = "MSTF002 - Document Type";
            treeNode48.Name = "Node0";
            treeNode48.Text = "MSTF003 - Exchange Rate";
            this.treeView4.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode46,
            treeNode47,
            treeNode48});
            this.treeView4.Size = new System.Drawing.Size(192, 49);
            this.treeView4.TabIndex = 6;
            // 
            // kryptonHeader3
            // 
            this.kryptonHeader3.Dock = System.Windows.Forms.DockStyle.Top;
            this.kryptonHeader3.HeaderStyle = ComponentFactory.Krypton.Toolkit.HeaderStyle.Secondary;
            this.kryptonHeader3.Location = new System.Drawing.Point(0, 0);
            this.kryptonHeader3.Name = "kryptonHeader3";
            this.kryptonHeader3.Size = new System.Drawing.Size(192, 21);
            this.kryptonHeader3.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)));
            this.kryptonHeader3.TabIndex = 5;
            this.kryptonHeader3.Values.Description = "";
            this.kryptonHeader3.Values.Heading = "Finance";
            this.kryptonHeader3.Values.Image = null;
            // 
            // treeView5
            // 
            this.treeView5.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeView5.Dock = System.Windows.Forms.DockStyle.Top;
            this.treeView5.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeView5.Location = new System.Drawing.Point(0, 91);
            this.treeView5.Name = "treeView5";
            treeNode49.Name = "Node0";
            treeNode49.Text = "MSTG001 - Currency";
            treeNode50.Name = "Node1";
            treeNode50.Text = "MSTG002 - Employee";
            treeNode51.Name = "Node0";
            treeNode51.Text = "MSTG003 - Division";
            this.treeView5.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode49,
            treeNode50,
            treeNode51});
            this.treeView5.Size = new System.Drawing.Size(192, 51);
            this.treeView5.TabIndex = 8;
            // 
            // kryptonHeader4
            // 
            this.kryptonHeader4.Dock = System.Windows.Forms.DockStyle.Top;
            this.kryptonHeader4.HeaderStyle = ComponentFactory.Krypton.Toolkit.HeaderStyle.Secondary;
            this.kryptonHeader4.Location = new System.Drawing.Point(0, 70);
            this.kryptonHeader4.Name = "kryptonHeader4";
            this.kryptonHeader4.Size = new System.Drawing.Size(192, 21);
            this.kryptonHeader4.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)));
            this.kryptonHeader4.TabIndex = 7;
            this.kryptonHeader4.Values.Description = "";
            this.kryptonHeader4.Values.Heading = "General Menu";
            this.kryptonHeader4.Values.Image = null;
            // 
            // treeView6
            // 
            this.treeView6.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeView6.Dock = System.Windows.Forms.DockStyle.Top;
            this.treeView6.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeView6.Location = new System.Drawing.Point(0, 163);
            this.treeView6.Name = "treeView6";
            treeNode52.Name = "Node0";
            treeNode52.Text = "MSTI001 - Part Group";
            treeNode53.Name = "Node1";
            treeNode53.Text = "MSTI002 - Part Category";
            treeNode54.Name = "Node0";
            treeNode54.Text = "MSTI003 - Unit";
            treeNode55.Name = "Node1";
            treeNode55.Text = "MSTI004 - Warehouse";
            this.treeView6.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode52,
            treeNode53,
            treeNode54,
            treeNode55});
            this.treeView6.Size = new System.Drawing.Size(192, 65);
            this.treeView6.TabIndex = 10;
            // 
            // kryptonHeader5
            // 
            this.kryptonHeader5.Dock = System.Windows.Forms.DockStyle.Top;
            this.kryptonHeader5.HeaderStyle = ComponentFactory.Krypton.Toolkit.HeaderStyle.Secondary;
            this.kryptonHeader5.Location = new System.Drawing.Point(0, 142);
            this.kryptonHeader5.Name = "kryptonHeader5";
            this.kryptonHeader5.Size = new System.Drawing.Size(192, 21);
            this.kryptonHeader5.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)));
            this.kryptonHeader5.TabIndex = 9;
            this.kryptonHeader5.Values.Description = "";
            this.kryptonHeader5.Values.Heading = "Inventory";
            this.kryptonHeader5.Values.Image = null;
            // 
            // treeView7
            // 
            this.treeView7.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeView7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView7.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treeView7.Location = new System.Drawing.Point(0, 249);
            this.treeView7.Name = "treeView7";
            treeNode56.Name = "Node0";
            treeNode56.Text = "MSTD001 - Customer";
            treeNode57.Name = "Node1";
            treeNode57.Text = "MSTD002 - Supplier";
            treeNode58.Name = "Node0";
            treeNode58.Text = "MSTD003 - Customer Category";
            treeNode59.Name = "Node1";
            treeNode59.Text = "MSTD004 - Supplier Category";
            treeNode60.Name = "Node2";
            treeNode60.Text = "MSTD005 - Price Category";
            treeNode61.Name = "Node3";
            treeNode61.Text = "MSTD006 - Tax";
            treeNode62.Name = "Node4";
            treeNode62.Text = "MSTD007 - Term Of Payment";
            this.treeView7.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode56,
            treeNode57,
            treeNode58,
            treeNode59,
            treeNode60,
            treeNode61,
            treeNode62});
            this.treeView7.Size = new System.Drawing.Size(192, 83);
            this.treeView7.TabIndex = 12;
            // 
            // kryptonHeader6
            // 
            this.kryptonHeader6.Dock = System.Windows.Forms.DockStyle.Top;
            this.kryptonHeader6.HeaderStyle = ComponentFactory.Krypton.Toolkit.HeaderStyle.Secondary;
            this.kryptonHeader6.Location = new System.Drawing.Point(0, 228);
            this.kryptonHeader6.Name = "kryptonHeader6";
            this.kryptonHeader6.Size = new System.Drawing.Size(192, 21);
            this.kryptonHeader6.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)));
            this.kryptonHeader6.TabIndex = 11;
            this.kryptonHeader6.Values.Description = "";
            this.kryptonHeader6.Values.Heading = "Distribution";
            this.kryptonHeader6.Values.Image = null;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(637, 533);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.kryptonNavigatorMain);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IsMdiContainer = true;
            this.Name = "Form1";
            this.Text = "Profitz v.0.0.0.1";
            this.TextExtra = "";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonNavigatorMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageMail)).EndInit();
            this.kryptonPageMail.ResumeLayout(false);
            this.kryptonPageMail.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageCalendar)).EndInit();
            this.kryptonPageCalendar.ResumeLayout(false);
            this.kryptonPageCalendar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPageNotes)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem logInToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem lockApplicationToolStripMenuItem;
        private ComponentFactory.Krypton.Toolkit.KryptonManager kryptonManager1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private ComponentFactory.Krypton.Navigator.KryptonNavigator kryptonNavigatorMain;
        private ComponentFactory.Krypton.Navigator.ButtonSpecNavigator buttonSpecExpandCollapse;
        private ComponentFactory.Krypton.Navigator.KryptonPage kryptonPageMail;
        private ComponentFactory.Krypton.Navigator.KryptonPage kryptonPageCalendar;
        private ComponentFactory.Krypton.Navigator.KryptonPage kryptonPageNotes;
        private System.Windows.Forms.Splitter splitter1;
        private ComponentFactory.Krypton.Toolkit.KryptonHeader kryptonHeader2;
        private System.Windows.Forms.TreeView treeView2;
        private ComponentFactory.Krypton.Toolkit.KryptonHeader kryptonHeader1;
        private System.Windows.Forms.TreeView treeView1;
        private ComponentFactory.Krypton.Toolkit.KryptonHeader kryptonHeaderFolders;
        private System.Windows.Forms.TreeView treeView3;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBox1;
        private System.Windows.Forms.TreeView treeView6;
        private ComponentFactory.Krypton.Toolkit.KryptonHeader kryptonHeader5;
        private System.Windows.Forms.TreeView treeView5;
        private ComponentFactory.Krypton.Toolkit.KryptonHeader kryptonHeader4;
        private System.Windows.Forms.TreeView treeView4;
        private ComponentFactory.Krypton.Toolkit.KryptonHeader kryptonHeader3;
        private System.Windows.Forms.TreeView treeView7;
        private ComponentFactory.Krypton.Toolkit.KryptonHeader kryptonHeader6;

    }
}