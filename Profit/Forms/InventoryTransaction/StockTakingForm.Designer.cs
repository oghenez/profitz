namespace Profit
{
    partial class StockTakingForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.kryptonHeader1 = new ComponentFactory.Krypton.Toolkit.KryptonHeader();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonEdit = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonClear = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonRefresh = new System.Windows.Forms.ToolStripButton();
            this.kryptonPanel1 = new ComponentFactory.Krypton.Toolkit.KryptonPanel();
            this.kryptonBorderEdge1 = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.totalAmountkryptonNumericUpDown = new ComponentFactory.Krypton.Toolkit.KryptonNumericUpDown();
            this.kryptonLabel6 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.currencyKryptonComboBox = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();
            this.kryptonLabel5 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.stocktakingTypekryptonComboBox = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();
            this.warehousekryptonComboBox = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();
            this.buttonSpecAny7 = new ComponentFactory.Krypton.Toolkit.ButtonSpecAny();
            this.kryptonLabel4 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel3 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.warehousekryptonTextBox = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.kryptonLabel2 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.EmployeekryptonTextBox = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.employeeKryptonComboBox = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();
            this.EmployeeSearchbuttonSpecAny = new ComponentFactory.Krypton.Toolkit.ButtonSpecAny();
            this.dateKryptonDateTimePicker = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();
            this.dataItemskryptonDataGridView = new ComponentFactory.Krypton.Toolkit.KryptonDataGridView();
            this.scanColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.codeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.QtyColumn = new ComponentFactory.Krypton.Toolkit.KryptonDataGridViewNumericUpDownColumn();
            this.unitColumn = new ComponentFactory.Krypton.Toolkit.KryptonDataGridViewComboBoxColumn();
            this.priceColumn = new ComponentFactory.Krypton.Toolkit.KryptonDataGridViewNumericUpDownColumn();
            this.totalAmountColumn = new ComponentFactory.Krypton.Toolkit.KryptonDataGridViewNumericUpDownColumn();
            this.textBoxCode = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.kryptonLabel1 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.buttonSpecAny1 = new ComponentFactory.Krypton.Toolkit.ButtonSpecAny();
            this.buttonSpecAny2 = new ComponentFactory.Krypton.Toolkit.ButtonSpecAny();
            this.buttonSpecAny3 = new ComponentFactory.Krypton.Toolkit.ButtonSpecAny();
            this.buttonSpecAny4 = new ComponentFactory.Krypton.Toolkit.ButtonSpecAny();
            this.buttonSpecAny5 = new ComponentFactory.Krypton.Toolkit.ButtonSpecAny();
            this.buttonSpecAny6 = new ComponentFactory.Krypton.Toolkit.ButtonSpecAny();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.currencyKryptonComboBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.stocktakingTypekryptonComboBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.warehousekryptonComboBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.employeeKryptonComboBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataItemskryptonDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // kryptonHeader1
            // 
            this.kryptonHeader1.Dock = System.Windows.Forms.DockStyle.Top;
            this.kryptonHeader1.Location = new System.Drawing.Point(0, 0);
            this.kryptonHeader1.Name = "kryptonHeader1";
            this.kryptonHeader1.Size = new System.Drawing.Size(856, 29);
            this.kryptonHeader1.TabIndex = 2;
            this.kryptonHeader1.Values.Description = "";
            this.kryptonHeader1.Values.Heading = "TRCI001 - Stock Taking";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonSave,
            this.toolStripButtonEdit,
            this.toolStripButtonDelete,
            this.toolStripButtonClear,
            this.toolStripButtonRefresh});
            this.toolStrip1.Location = new System.Drawing.Point(0, 29);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(856, 25);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButtonSave
            // 
            this.toolStripButtonSave.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripButtonSave.Image = global::Profit.Properties.Resources.save;
            this.toolStripButtonSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSave.Name = "toolStripButtonSave";
            this.toolStripButtonSave.Size = new System.Drawing.Size(49, 22);
            this.toolStripButtonSave.Text = "Save";
            // 
            // toolStripButtonEdit
            // 
            this.toolStripButtonEdit.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripButtonEdit.Image = global::Profit.Properties.Resources.edit;
            this.toolStripButtonEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonEdit.Name = "toolStripButtonEdit";
            this.toolStripButtonEdit.Size = new System.Drawing.Size(45, 22);
            this.toolStripButtonEdit.Text = "Edit";
            // 
            // toolStripButtonDelete
            // 
            this.toolStripButtonDelete.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripButtonDelete.Image = global::Profit.Properties.Resources.delete;
            this.toolStripButtonDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDelete.Name = "toolStripButtonDelete";
            this.toolStripButtonDelete.Size = new System.Drawing.Size(59, 22);
            this.toolStripButtonDelete.Text = "Delete";
            // 
            // toolStripButtonClear
            // 
            this.toolStripButtonClear.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripButtonClear.Image = global::Profit.Properties.Resources.Gnome_Edit_Redo_32;
            this.toolStripButtonClear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonClear.Name = "toolStripButtonClear";
            this.toolStripButtonClear.Size = new System.Drawing.Size(52, 22);
            this.toolStripButtonClear.Text = "Clear";
            // 
            // toolStripButtonRefresh
            // 
            this.toolStripButtonRefresh.Image = global::Profit.Properties.Resources.refresh;
            this.toolStripButtonRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonRefresh.Name = "toolStripButtonRefresh";
            this.toolStripButtonRefresh.Size = new System.Drawing.Size(66, 22);
            this.toolStripButtonRefresh.Text = "Refresh";
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.AutoScroll = true;
            this.kryptonPanel1.Controls.Add(this.kryptonBorderEdge1);
            this.kryptonPanel1.Controls.Add(this.totalAmountkryptonNumericUpDown);
            this.kryptonPanel1.Controls.Add(this.kryptonLabel6);
            this.kryptonPanel1.Controls.Add(this.currencyKryptonComboBox);
            this.kryptonPanel1.Controls.Add(this.kryptonLabel5);
            this.kryptonPanel1.Controls.Add(this.stocktakingTypekryptonComboBox);
            this.kryptonPanel1.Controls.Add(this.warehousekryptonComboBox);
            this.kryptonPanel1.Controls.Add(this.kryptonLabel4);
            this.kryptonPanel1.Controls.Add(this.kryptonLabel3);
            this.kryptonPanel1.Controls.Add(this.warehousekryptonTextBox);
            this.kryptonPanel1.Controls.Add(this.kryptonLabel2);
            this.kryptonPanel1.Controls.Add(this.EmployeekryptonTextBox);
            this.kryptonPanel1.Controls.Add(this.employeeKryptonComboBox);
            this.kryptonPanel1.Controls.Add(this.dateKryptonDateTimePicker);
            this.kryptonPanel1.Controls.Add(this.dataItemskryptonDataGridView);
            this.kryptonPanel1.Controls.Add(this.textBoxCode);
            this.kryptonPanel1.Controls.Add(this.kryptonLabel1);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 54);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(856, 357);
            this.kryptonPanel1.TabIndex = 4;
            // 
            // kryptonBorderEdge1
            // 
            this.kryptonBorderEdge1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.kryptonBorderEdge1.AutoSize = false;
            this.kryptonBorderEdge1.Location = new System.Drawing.Point(4, 306);
            this.kryptonBorderEdge1.Name = "kryptonBorderEdge1";
            this.kryptonBorderEdge1.Size = new System.Drawing.Size(831, 1);
            this.kryptonBorderEdge1.Text = "kryptonBorderEdge1";
            // 
            // totalAmountkryptonNumericUpDown
            // 
            this.totalAmountkryptonNumericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.totalAmountkryptonNumericUpDown.DecimalPlaces = 2;
            this.totalAmountkryptonNumericUpDown.Enabled = false;
            this.totalAmountkryptonNumericUpDown.Increment = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.totalAmountkryptonNumericUpDown.Location = new System.Drawing.Point(631, 313);
            this.totalAmountkryptonNumericUpDown.Maximum = new decimal(new int[] {
            -727379968,
            232,
            0,
            0});
            this.totalAmountkryptonNumericUpDown.Name = "totalAmountkryptonNumericUpDown";
            this.totalAmountkryptonNumericUpDown.Size = new System.Drawing.Size(202, 32);
            this.totalAmountkryptonNumericUpDown.StateCommon.Content.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.totalAmountkryptonNumericUpDown.TabIndex = 19;
            this.totalAmountkryptonNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.totalAmountkryptonNumericUpDown.ThousandsSeparator = true;
            // 
            // kryptonLabel6
            // 
            this.kryptonLabel6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.kryptonLabel6.Location = new System.Drawing.Point(479, 326);
            this.kryptonLabel6.Name = "kryptonLabel6";
            this.kryptonLabel6.Size = new System.Drawing.Size(84, 19);
            this.kryptonLabel6.TabIndex = 17;
            this.kryptonLabel6.Values.Text = "Total Amount :";
            // 
            // currencyKryptonComboBox
            // 
            this.currencyKryptonComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.currencyKryptonComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.currencyKryptonComboBox.DropDownWidth = 90;
            this.currencyKryptonComboBox.Location = new System.Drawing.Point(569, 323);
            this.currencyKryptonComboBox.Name = "currencyKryptonComboBox";
            this.currencyKryptonComboBox.Size = new System.Drawing.Size(56, 22);
            this.currencyKryptonComboBox.TabIndex = 16;
            // 
            // kryptonLabel5
            // 
            this.kryptonLabel5.Location = new System.Drawing.Point(647, 85);
            this.kryptonLabel5.Name = "kryptonLabel5";
            this.kryptonLabel5.Size = new System.Drawing.Size(40, 19);
            this.kryptonLabel5.TabIndex = 15;
            this.kryptonLabel5.Values.Text = "Type :";
            // 
            // stocktakingTypekryptonComboBox
            // 
            this.stocktakingTypekryptonComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.stocktakingTypekryptonComboBox.DropDownWidth = 90;
            this.stocktakingTypekryptonComboBox.Location = new System.Drawing.Point(693, 85);
            this.stocktakingTypekryptonComboBox.Name = "stocktakingTypekryptonComboBox";
            this.stocktakingTypekryptonComboBox.Size = new System.Drawing.Size(140, 22);
            this.stocktakingTypekryptonComboBox.TabIndex = 14;
            // 
            // warehousekryptonComboBox
            // 
            this.warehousekryptonComboBox.ButtonSpecs.AddRange(new ComponentFactory.Krypton.Toolkit.ButtonSpecAny[] {
            this.buttonSpecAny7});
            this.warehousekryptonComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.warehousekryptonComboBox.DropDownWidth = 90;
            this.warehousekryptonComboBox.Location = new System.Drawing.Point(90, 85);
            this.warehousekryptonComboBox.Name = "warehousekryptonComboBox";
            this.warehousekryptonComboBox.Size = new System.Drawing.Size(108, 22);
            this.warehousekryptonComboBox.TabIndex = 13;
            this.warehousekryptonComboBox.SelectedIndexChanged += new System.EventHandler(this.warehousekryptonComboBox_SelectedIndexChanged);
            // 
            // buttonSpecAny7
            // 
            this.buttonSpecAny7.Type = ComponentFactory.Krypton.Toolkit.PaletteButtonSpecStyle.Context;
            this.buttonSpecAny7.UniqueName = "6422A42769A8412C499D5727061121AA";
            // 
            // kryptonLabel4
            // 
            this.kryptonLabel4.Location = new System.Drawing.Point(45, 34);
            this.kryptonLabel4.Name = "kryptonLabel4";
            this.kryptonLabel4.Size = new System.Drawing.Size(39, 19);
            this.kryptonLabel4.TabIndex = 12;
            this.kryptonLabel4.Values.Text = "Date :";
            // 
            // kryptonLabel3
            // 
            this.kryptonLabel3.Location = new System.Drawing.Point(19, 60);
            this.kryptonLabel3.Name = "kryptonLabel3";
            this.kryptonLabel3.Size = new System.Drawing.Size(65, 19);
            this.kryptonLabel3.TabIndex = 11;
            this.kryptonLabel3.Values.Text = "Employee :";
            // 
            // warehousekryptonTextBox
            // 
            this.warehousekryptonTextBox.Enabled = false;
            this.warehousekryptonTextBox.Location = new System.Drawing.Point(197, 85);
            this.warehousekryptonTextBox.Name = "warehousekryptonTextBox";
            this.warehousekryptonTextBox.Size = new System.Drawing.Size(144, 22);
            this.warehousekryptonTextBox.TabIndex = 10;
            // 
            // kryptonLabel2
            // 
            this.kryptonLabel2.Location = new System.Drawing.Point(12, 85);
            this.kryptonLabel2.Name = "kryptonLabel2";
            this.kryptonLabel2.Size = new System.Drawing.Size(72, 19);
            this.kryptonLabel2.TabIndex = 9;
            this.kryptonLabel2.Values.Text = "Warehouse :";
            // 
            // EmployeekryptonTextBox
            // 
            this.EmployeekryptonTextBox.Enabled = false;
            this.EmployeekryptonTextBox.Location = new System.Drawing.Point(197, 60);
            this.EmployeekryptonTextBox.Name = "EmployeekryptonTextBox";
            this.EmployeekryptonTextBox.ReadOnly = true;
            this.EmployeekryptonTextBox.Size = new System.Drawing.Size(144, 22);
            this.EmployeekryptonTextBox.TabIndex = 7;
            // 
            // employeeKryptonComboBox
            // 
            this.employeeKryptonComboBox.ButtonSpecs.AddRange(new ComponentFactory.Krypton.Toolkit.ButtonSpecAny[] {
            this.EmployeeSearchbuttonSpecAny});
            this.employeeKryptonComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.employeeKryptonComboBox.DropDownWidth = 90;
            this.employeeKryptonComboBox.Location = new System.Drawing.Point(90, 60);
            this.employeeKryptonComboBox.Name = "employeeKryptonComboBox";
            this.employeeKryptonComboBox.Size = new System.Drawing.Size(108, 22);
            this.employeeKryptonComboBox.TabIndex = 6;
            this.employeeKryptonComboBox.SelectedIndexChanged += new System.EventHandler(this.employeeKryptonComboBox_SelectedIndexChanged);
            // 
            // EmployeeSearchbuttonSpecAny
            // 
            this.EmployeeSearchbuttonSpecAny.Type = ComponentFactory.Krypton.Toolkit.PaletteButtonSpecStyle.Context;
            this.EmployeeSearchbuttonSpecAny.UniqueName = "6422A42769A8412C499D5727061121AA";
            // 
            // dateKryptonDateTimePicker
            // 
            this.dateKryptonDateTimePicker.Location = new System.Drawing.Point(90, 34);
            this.dateKryptonDateTimePicker.Name = "dateKryptonDateTimePicker";
            this.dateKryptonDateTimePicker.Size = new System.Drawing.Size(154, 20);
            this.dateKryptonDateTimePicker.TabIndex = 5;
            // 
            // dataItemskryptonDataGridView
            // 
            this.dataItemskryptonDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.dataItemskryptonDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.scanColumn,
            this.codeColumn,
            this.nameColumn,
            this.QtyColumn,
            this.unitColumn,
            this.priceColumn,
            this.totalAmountColumn});
            this.dataItemskryptonDataGridView.Location = new System.Drawing.Point(3, 115);
            this.dataItemskryptonDataGridView.Name = "dataItemskryptonDataGridView";
            this.dataItemskryptonDataGridView.Size = new System.Drawing.Size(841, 176);
            this.dataItemskryptonDataGridView.TabIndex = 4;
            // 
            // scanColumn
            // 
            this.scanColumn.HeaderText = "Cari";
            this.scanColumn.Name = "scanColumn";
            // 
            // codeColumn
            // 
            this.codeColumn.HeaderText = "Code";
            this.codeColumn.Name = "codeColumn";
            this.codeColumn.Width = 80;
            // 
            // nameColumn
            // 
            this.nameColumn.HeaderText = "Name";
            this.nameColumn.Name = "nameColumn";
            this.nameColumn.ReadOnly = true;
            this.nameColumn.Width = 300;
            // 
            // QtyColumn
            // 
            this.QtyColumn.HeaderText = "Qty";
            this.QtyColumn.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.QtyColumn.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.QtyColumn.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.QtyColumn.Name = "QtyColumn";
            this.QtyColumn.Width = 50;
            // 
            // unitColumn
            // 
            this.unitColumn.DropDownWidth = 121;
            this.unitColumn.HeaderText = "Unit";
            this.unitColumn.Name = "unitColumn";
            this.unitColumn.Width = 60;
            // 
            // priceColumn
            // 
            this.priceColumn.DecimalPlaces = 2;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.priceColumn.DefaultCellStyle = dataGridViewCellStyle3;
            this.priceColumn.HeaderText = "Price";
            this.priceColumn.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.priceColumn.Maximum = new decimal(new int[] {
            -727379968,
            232,
            0,
            0});
            this.priceColumn.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.priceColumn.Name = "priceColumn";
            this.priceColumn.ThousandsSeparator = true;
            this.priceColumn.Width = 100;
            // 
            // totalAmountColumn
            // 
            this.totalAmountColumn.DecimalPlaces = 2;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.totalAmountColumn.DefaultCellStyle = dataGridViewCellStyle4;
            this.totalAmountColumn.HeaderText = "Amount";
            this.totalAmountColumn.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.totalAmountColumn.Maximum = new decimal(new int[] {
            -727379968,
            232,
            0,
            0});
            this.totalAmountColumn.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.totalAmountColumn.Name = "totalAmountColumn";
            this.totalAmountColumn.ReadOnly = true;
            this.totalAmountColumn.ThousandsSeparator = true;
            this.totalAmountColumn.Width = 100;
            // 
            // textBoxCode
            // 
            this.textBoxCode.Location = new System.Drawing.Point(90, 6);
            this.textBoxCode.Name = "textBoxCode";
            this.textBoxCode.Size = new System.Drawing.Size(154, 22);
            this.textBoxCode.TabIndex = 2;
            // 
            // kryptonLabel1
            // 
            this.kryptonLabel1.Location = new System.Drawing.Point(42, 6);
            this.kryptonLabel1.Name = "kryptonLabel1";
            this.kryptonLabel1.Size = new System.Drawing.Size(42, 19);
            this.kryptonLabel1.TabIndex = 0;
            this.kryptonLabel1.Values.Text = "Code :";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // buttonSpecAny1
            // 
            this.buttonSpecAny1.Type = ComponentFactory.Krypton.Toolkit.PaletteButtonSpecStyle.Context;
            this.buttonSpecAny1.UniqueName = "3ED14974C39D424AF483B15489413085";
            // 
            // buttonSpecAny2
            // 
            this.buttonSpecAny2.Type = ComponentFactory.Krypton.Toolkit.PaletteButtonSpecStyle.Context;
            this.buttonSpecAny2.UniqueName = "F6D098FF97AB48FB2A99BBB2D3AE16BB";
            // 
            // buttonSpecAny3
            // 
            this.buttonSpecAny3.Type = ComponentFactory.Krypton.Toolkit.PaletteButtonSpecStyle.Previous;
            this.buttonSpecAny3.UniqueName = "5FF9C97ECDF840FFA4AD11EAEA81925E";
            // 
            // buttonSpecAny4
            // 
            this.buttonSpecAny4.Type = ComponentFactory.Krypton.Toolkit.PaletteButtonSpecStyle.Next;
            this.buttonSpecAny4.UniqueName = "B56143FA8A234E7613B74670410D55F5";
            // 
            // buttonSpecAny5
            // 
            this.buttonSpecAny5.Type = ComponentFactory.Krypton.Toolkit.PaletteButtonSpecStyle.Context;
            this.buttonSpecAny5.UniqueName = "A78BD21009064A446EB0B46846D279AF";
            // 
            // buttonSpecAny6
            // 
            this.buttonSpecAny6.Type = ComponentFactory.Krypton.Toolkit.PaletteButtonSpecStyle.Close;
            this.buttonSpecAny6.UniqueName = "95BED338EBB14361B2927C3CB003454A";
            // 
            // StockTakingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(856, 411);
            this.Controls.Add(this.kryptonPanel1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.kryptonHeader1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "StockTakingForm";
            this.Text = "Stock Taking";
            this.Activated += new System.EventHandler(this.BankForm_Activated);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            this.kryptonPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.currencyKryptonComboBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.stocktakingTypekryptonComboBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.warehousekryptonComboBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.employeeKryptonComboBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataItemskryptonDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonHeader kryptonHeader1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonSave;
        private System.Windows.Forms.ToolStripButton toolStripButtonEdit;
        private System.Windows.Forms.ToolStripButton toolStripButtonDelete;
        private System.Windows.Forms.ToolStripButton toolStripButtonClear;
        private System.Windows.Forms.ToolStripButton toolStripButtonRefresh;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel1;
        private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private ComponentFactory.Krypton.Toolkit.KryptonTextBox textBoxCode;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private ComponentFactory.Krypton.Toolkit.KryptonDataGridView dataItemskryptonDataGridView;
        private ComponentFactory.Krypton.Toolkit.ButtonSpecAny buttonSpecAny1;
        private ComponentFactory.Krypton.Toolkit.ButtonSpecAny buttonSpecAny2;
        private ComponentFactory.Krypton.Toolkit.ButtonSpecAny buttonSpecAny3;
        private ComponentFactory.Krypton.Toolkit.ButtonSpecAny buttonSpecAny4;
        private ComponentFactory.Krypton.Toolkit.ButtonSpecAny buttonSpecAny5;
        private ComponentFactory.Krypton.Toolkit.ButtonSpecAny buttonSpecAny6;
        private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker dateKryptonDateTimePicker;
        private ComponentFactory.Krypton.Toolkit.KryptonTextBox EmployeekryptonTextBox;
        private ComponentFactory.Krypton.Toolkit.KryptonComboBox employeeKryptonComboBox;
        private ComponentFactory.Krypton.Toolkit.ButtonSpecAny EmployeeSearchbuttonSpecAny;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel2;
        private ComponentFactory.Krypton.Toolkit.KryptonTextBox warehousekryptonTextBox;
        private ComponentFactory.Krypton.Toolkit.KryptonComboBox warehousekryptonComboBox;
        private ComponentFactory.Krypton.Toolkit.ButtonSpecAny buttonSpecAny7;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel4;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel3;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel5;
        private ComponentFactory.Krypton.Toolkit.KryptonComboBox stocktakingTypekryptonComboBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn scanColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn codeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameColumn;
        private ComponentFactory.Krypton.Toolkit.KryptonDataGridViewNumericUpDownColumn QtyColumn;
        private ComponentFactory.Krypton.Toolkit.KryptonDataGridViewComboBoxColumn unitColumn;
        private ComponentFactory.Krypton.Toolkit.KryptonDataGridViewNumericUpDownColumn priceColumn;
        private ComponentFactory.Krypton.Toolkit.KryptonDataGridViewNumericUpDownColumn totalAmountColumn;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel6;
        private ComponentFactory.Krypton.Toolkit.KryptonComboBox currencyKryptonComboBox;
        private ComponentFactory.Krypton.Toolkit.KryptonNumericUpDown totalAmountkryptonNumericUpDown;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdge1;
    }
}