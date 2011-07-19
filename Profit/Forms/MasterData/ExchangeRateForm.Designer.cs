namespace Profit
{
    partial class ExchangeRateForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.kryptonHeader1 = new ComponentFactory.Krypton.Toolkit.KryptonHeader();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonEdit = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonClear = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonRefresh = new System.Windows.Forms.ToolStripButton();
            this.kryptonSplitContainer1 = new ComponentFactory.Krypton.Toolkit.KryptonSplitContainer();
            this.kryptonPanel1 = new ComponentFactory.Krypton.Toolkit.KryptonPanel();
            this.textBoxCode = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.kryptonLabel1 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel2 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.gridData = new ComponentFactory.Krypton.Toolkit.KryptonDataGridView();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.kryptonDateTimePickerStartDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();
            this.kryptonDateTimePickerEndDate = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();
            this.kryptonNumericUpDownRAte = new ComponentFactory.Krypton.Toolkit.KryptonNumericUpDown();
            this.kryptonComboBoxCurrency = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();
            this.kryptonLabel3 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel4 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel5 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.dgName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CCY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StartDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EndDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RAte = new ComponentFactory.Krypton.Toolkit.KryptonDataGridViewNumericUpDownColumn();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1.Panel1)).BeginInit();
            this.kryptonSplitContainer1.Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1.Panel2)).BeginInit();
            this.kryptonSplitContainer1.Panel2.SuspendLayout();
            this.kryptonSplitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonComboBoxCurrency)).BeginInit();
            this.SuspendLayout();
            // 
            // kryptonHeader1
            // 
            this.kryptonHeader1.Dock = System.Windows.Forms.DockStyle.Top;
            this.kryptonHeader1.Location = new System.Drawing.Point(0, 0);
            this.kryptonHeader1.Name = "kryptonHeader1";
            this.kryptonHeader1.Size = new System.Drawing.Size(609, 29);
            this.kryptonHeader1.TabIndex = 2;
            this.kryptonHeader1.Values.Description = "";
            this.kryptonHeader1.Values.Heading = "MSTF003 - Exchange Rate";
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
            this.toolStrip1.Size = new System.Drawing.Size(609, 25);
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
            // kryptonSplitContainer1
            // 
            this.kryptonSplitContainer1.Cursor = System.Windows.Forms.Cursors.Default;
            this.kryptonSplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonSplitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.kryptonSplitContainer1.Location = new System.Drawing.Point(0, 54);
            this.kryptonSplitContainer1.Name = "kryptonSplitContainer1";
            this.kryptonSplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // kryptonSplitContainer1.Panel1
            // 
            this.kryptonSplitContainer1.Panel1.Controls.Add(this.kryptonPanel1);
            // 
            // kryptonSplitContainer1.Panel2
            // 
            this.kryptonSplitContainer1.Panel2.Controls.Add(this.gridData);
            this.kryptonSplitContainer1.Size = new System.Drawing.Size(609, 357);
            this.kryptonSplitContainer1.SplitterDistance = 120;
            this.kryptonSplitContainer1.TabIndex = 4;
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.AutoScroll = true;
            this.kryptonPanel1.Controls.Add(this.kryptonLabel5);
            this.kryptonPanel1.Controls.Add(this.kryptonLabel4);
            this.kryptonPanel1.Controls.Add(this.kryptonLabel3);
            this.kryptonPanel1.Controls.Add(this.kryptonComboBoxCurrency);
            this.kryptonPanel1.Controls.Add(this.kryptonNumericUpDownRAte);
            this.kryptonPanel1.Controls.Add(this.kryptonDateTimePickerEndDate);
            this.kryptonPanel1.Controls.Add(this.kryptonDateTimePickerStartDate);
            this.kryptonPanel1.Controls.Add(this.textBoxCode);
            this.kryptonPanel1.Controls.Add(this.kryptonLabel1);
            this.kryptonPanel1.Controls.Add(this.kryptonLabel2);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(609, 120);
            this.kryptonPanel1.TabIndex = 4;
            // 
            // textBoxCode
            // 
            this.textBoxCode.Location = new System.Drawing.Point(90, 6);
            this.textBoxCode.Name = "textBoxCode";
            this.textBoxCode.Size = new System.Drawing.Size(71, 22);
            this.textBoxCode.TabIndex = 2;
            // 
            // kryptonLabel1
            // 
            this.kryptonLabel1.Location = new System.Drawing.Point(45, 6);
            this.kryptonLabel1.Name = "kryptonLabel1";
            this.kryptonLabel1.Size = new System.Drawing.Size(42, 19);
            this.kryptonLabel1.TabIndex = 0;
            this.kryptonLabel1.Values.Text = "Code :";
            // 
            // kryptonLabel2
            // 
            this.kryptonLabel2.Location = new System.Drawing.Point(21, 37);
            this.kryptonLabel2.Name = "kryptonLabel2";
            this.kryptonLabel2.Size = new System.Drawing.Size(66, 19);
            this.kryptonLabel2.TabIndex = 1;
            this.kryptonLabel2.Values.Text = "Start Date :";
            // 
            // gridData
            // 
            this.gridData.AllowUserToAddRows = false;
            this.gridData.AllowUserToDeleteRows = false;
            this.gridData.AllowUserToResizeRows = false;
            this.gridData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgName,
            this.CCY,
            this.StartDate,
            this.EndDate,
            this.RAte});
            this.gridData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridData.GridStyles.Style = ComponentFactory.Krypton.Toolkit.DataGridViewStyle.Mixed;
            this.gridData.GridStyles.StyleBackground = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.ControlClient;
            this.gridData.HideOuterBorders = true;
            this.gridData.Location = new System.Drawing.Point(0, 0);
            this.gridData.MultiSelect = false;
            this.gridData.Name = "gridData";
            this.gridData.ReadOnly = true;
            this.gridData.RowHeadersVisible = false;
            this.gridData.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gridData.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridData.Size = new System.Drawing.Size(609, 232);
            this.gridData.StateCommon.BackStyle = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.ControlClient;
            this.gridData.StateCommon.DataCell.Border.DrawBorders = ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom;
            this.gridData.TabIndex = 1;
            this.gridData.SelectionChanged += new System.EventHandler(this.gridData_SelectionChanged);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // kryptonDateTimePickerStartDate
            // 
            this.kryptonDateTimePickerStartDate.CustomFormat = "dd-MM-yyyy";
            this.kryptonDateTimePickerStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.kryptonDateTimePickerStartDate.Location = new System.Drawing.Point(90, 36);
            this.kryptonDateTimePickerStartDate.Name = "kryptonDateTimePickerStartDate";
            this.kryptonDateTimePickerStartDate.Size = new System.Drawing.Size(83, 20);
            this.kryptonDateTimePickerStartDate.TabIndex = 4;
            // 
            // kryptonDateTimePickerEndDate
            // 
            this.kryptonDateTimePickerEndDate.CustomFormat = "dd-MM-yyyy";
            this.kryptonDateTimePickerEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.kryptonDateTimePickerEndDate.Location = new System.Drawing.Point(90, 61);
            this.kryptonDateTimePickerEndDate.Name = "kryptonDateTimePickerEndDate";
            this.kryptonDateTimePickerEndDate.Size = new System.Drawing.Size(83, 20);
            this.kryptonDateTimePickerEndDate.TabIndex = 5;
            this.kryptonDateTimePickerEndDate.ValueChanged += new System.EventHandler(this.kryptonDateTimePicker2_ValueChanged);
            // 
            // kryptonNumericUpDownRAte
            // 
            this.kryptonNumericUpDownRAte.DecimalPlaces = 2;
            this.kryptonNumericUpDownRAte.Increment = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.kryptonNumericUpDownRAte.Location = new System.Drawing.Point(90, 87);
            this.kryptonNumericUpDownRAte.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.kryptonNumericUpDownRAte.Name = "kryptonNumericUpDownRAte";
            this.kryptonNumericUpDownRAte.Size = new System.Drawing.Size(120, 21);
            this.kryptonNumericUpDownRAte.TabIndex = 6;
            this.kryptonNumericUpDownRAte.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.kryptonNumericUpDownRAte.ThousandsSeparator = true;
            // 
            // kryptonComboBoxCurrency
            // 
            this.kryptonComboBoxCurrency.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.kryptonComboBoxCurrency.DropDownWidth = 121;
            this.kryptonComboBoxCurrency.Location = new System.Drawing.Point(272, 6);
            this.kryptonComboBoxCurrency.Name = "kryptonComboBoxCurrency";
            this.kryptonComboBoxCurrency.Size = new System.Drawing.Size(121, 22);
            this.kryptonComboBoxCurrency.TabIndex = 7;
            // 
            // kryptonLabel3
            // 
            this.kryptonLabel3.Location = new System.Drawing.Point(26, 62);
            this.kryptonLabel3.Name = "kryptonLabel3";
            this.kryptonLabel3.Size = new System.Drawing.Size(61, 19);
            this.kryptonLabel3.TabIndex = 8;
            this.kryptonLabel3.Values.Text = "End Date :";
            // 
            // kryptonLabel4
            // 
            this.kryptonLabel4.Location = new System.Drawing.Point(7, 87);
            this.kryptonLabel4.Name = "kryptonLabel4";
            this.kryptonLabel4.Size = new System.Drawing.Size(80, 19);
            this.kryptonLabel4.TabIndex = 9;
            this.kryptonLabel4.Values.Text = "Rate To Base :";
            // 
            // kryptonLabel5
            // 
            this.kryptonLabel5.Location = new System.Drawing.Point(206, 9);
            this.kryptonLabel5.Name = "kryptonLabel5";
            this.kryptonLabel5.Size = new System.Drawing.Size(60, 19);
            this.kryptonLabel5.TabIndex = 10;
            this.kryptonLabel5.Values.Text = "Currency :";
            // 
            // dgName
            // 
            this.dgName.DataPropertyName = "Code";
            this.dgName.HeaderText = "Code";
            this.dgName.MinimumWidth = 100;
            this.dgName.Name = "dgName";
            this.dgName.ReadOnly = true;
            // 
            // CCY
            // 
            this.CCY.HeaderText = "Ccy";
            this.CCY.Name = "CCY";
            this.CCY.ReadOnly = true;
            this.CCY.Width = 50;
            // 
            // StartDate
            // 
            this.StartDate.HeaderText = "Start Date";
            this.StartDate.Name = "StartDate";
            this.StartDate.ReadOnly = true;
            this.StartDate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.StartDate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // EndDate
            // 
            this.EndDate.HeaderText = "End Date";
            this.EndDate.Name = "EndDate";
            this.EndDate.ReadOnly = true;
            this.EndDate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.EndDate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // RAte
            // 
            this.RAte.DecimalPlaces = 2;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.RAte.DefaultCellStyle = dataGridViewCellStyle1;
            this.RAte.HeaderText = "Rate To Base";
            this.RAte.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.RAte.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.RAte.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.RAte.Name = "RAte";
            this.RAte.ReadOnly = true;
            this.RAte.ThousandsSeparator = true;
            this.RAte.Width = 100;
            // 
            // ExchangeRateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(609, 411);
            this.Controls.Add(this.kryptonSplitContainer1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.kryptonHeader1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "ExchangeRateForm";
            this.Text = "Exchange Rate";
            this.Activated += new System.EventHandler(this.BankForm_Activated);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1.Panel1)).EndInit();
            this.kryptonSplitContainer1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1.Panel2)).EndInit();
            this.kryptonSplitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1)).EndInit();
            this.kryptonSplitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            this.kryptonPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonComboBoxCurrency)).EndInit();
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
        private ComponentFactory.Krypton.Toolkit.KryptonSplitContainer kryptonSplitContainer1;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel2;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel1;
        private ComponentFactory.Krypton.Toolkit.KryptonDataGridView gridData;
        private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private ComponentFactory.Krypton.Toolkit.KryptonTextBox textBoxCode;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private ComponentFactory.Krypton.Toolkit.KryptonNumericUpDown kryptonNumericUpDownRAte;
        private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker kryptonDateTimePickerEndDate;
        private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker kryptonDateTimePickerStartDate;
        private ComponentFactory.Krypton.Toolkit.KryptonComboBox kryptonComboBoxCurrency;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel3;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel4;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgName;
        private System.Windows.Forms.DataGridViewTextBoxColumn CCY;
        private System.Windows.Forms.DataGridViewTextBoxColumn StartDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn EndDate;
        private ComponentFactory.Krypton.Toolkit.KryptonDataGridViewNumericUpDownColumn RAte;
    }
}