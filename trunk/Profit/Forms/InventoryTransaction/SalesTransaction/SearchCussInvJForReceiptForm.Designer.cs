namespace Profit
{
    partial class SearchCussInvJForReceiptForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.CANCELkryptonButton = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.OKkryptonButton = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.gridData = new ComponentFactory.Krypton.Toolkit.KryptonDataGridView();
            this.kryptonPanel1 = new ComponentFactory.Krypton.Toolkit.KryptonPanel();
            this.checkAllkryptonCheckBox1 = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
            this.kryptonLabel2 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel1 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.searchText = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.startSearch = new ComponentFactory.Krypton.Toolkit.ButtonSpecAny();
            this.checkColumn = new ComponentFactory.Krypton.Toolkit.KryptonDataGridViewCheckBoxColumn();
            this.purchaseorderNoColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.poDateColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.supplierColumn = new ComponentFactory.Krypton.Toolkit.KryptonDataGridViewTextBoxColumn();
            this.ccyColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.qtyColumn = new ComponentFactory.Krypton.Toolkit.KryptonDataGridViewNumericUpDownColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gridData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // CANCELkryptonButton
            // 
            this.CANCELkryptonButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CANCELkryptonButton.Location = new System.Drawing.Point(563, 307);
            this.CANCELkryptonButton.Name = "CANCELkryptonButton";
            this.CANCELkryptonButton.Size = new System.Drawing.Size(63, 25);
            this.CANCELkryptonButton.TabIndex = 3;
            this.CANCELkryptonButton.Values.Text = "Cancel";
            this.CANCELkryptonButton.Click += new System.EventHandler(this.CANCELkryptonButton_Click);
            // 
            // OKkryptonButton
            // 
            this.OKkryptonButton.Location = new System.Drawing.Point(494, 307);
            this.OKkryptonButton.Name = "OKkryptonButton";
            this.OKkryptonButton.Size = new System.Drawing.Size(63, 25);
            this.OKkryptonButton.TabIndex = 4;
            this.OKkryptonButton.Values.Text = "OK";
            this.OKkryptonButton.Click += new System.EventHandler(this.OKkryptonButton_Click);
            // 
            // gridData
            // 
            this.gridData.AllowUserToAddRows = false;
            this.gridData.AllowUserToDeleteRows = false;
            this.gridData.AllowUserToResizeRows = false;
            this.gridData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.checkColumn,
            this.purchaseorderNoColumn,
            this.poDateColumn,
            this.supplierColumn,
            this.ccyColumn,
            this.qtyColumn});
            this.gridData.GridStyles.Style = ComponentFactory.Krypton.Toolkit.DataGridViewStyle.Mixed;
            this.gridData.GridStyles.StyleBackground = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.ControlClient;
            this.gridData.HideOuterBorders = true;
            this.gridData.Location = new System.Drawing.Point(12, 40);
            this.gridData.MultiSelect = false;
            this.gridData.Name = "gridData";
            this.gridData.RowHeadersVisible = false;
            this.gridData.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gridData.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridData.Size = new System.Drawing.Size(614, 261);
            this.gridData.StateCommon.BackStyle = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.ControlClient;
            this.gridData.StateCommon.DataCell.Border.DrawBorders = ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom;
            this.gridData.TabIndex = 5;
            this.gridData.TabStop = false;
            this.gridData.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.gridData_CellMouseDoubleClick);
            this.gridData.KeyDown += new System.Windows.Forms.KeyEventHandler(this.gridData_KeyDown);
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.checkAllkryptonCheckBox1);
            this.kryptonPanel1.Controls.Add(this.kryptonLabel2);
            this.kryptonPanel1.Controls.Add(this.kryptonLabel1);
            this.kryptonPanel1.Controls.Add(this.gridData);
            this.kryptonPanel1.Controls.Add(this.searchText);
            this.kryptonPanel1.Controls.Add(this.OKkryptonButton);
            this.kryptonPanel1.Controls.Add(this.CANCELkryptonButton);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(638, 341);
            this.kryptonPanel1.TabIndex = 6;
            // 
            // checkAllkryptonCheckBox1
            // 
            this.checkAllkryptonCheckBox1.LabelStyle = ComponentFactory.Krypton.Toolkit.LabelStyle.NormalControl;
            this.checkAllkryptonCheckBox1.Location = new System.Drawing.Point(12, 310);
            this.checkAllkryptonCheckBox1.Name = "checkAllkryptonCheckBox1";
            this.checkAllkryptonCheckBox1.Size = new System.Drawing.Size(69, 19);
            this.checkAllkryptonCheckBox1.TabIndex = 9;
            this.checkAllkryptonCheckBox1.Text = "Check All";
            this.checkAllkryptonCheckBox1.Values.Text = "Check All";
            this.checkAllkryptonCheckBox1.CheckedChanged += new System.EventHandler(this.checkAllkryptonCheckBox1_CheckedChanged);
            // 
            // kryptonLabel2
            // 
            this.kryptonLabel2.Location = new System.Drawing.Point(427, 15);
            this.kryptonLabel2.Name = "kryptonLabel2";
            this.kryptonLabel2.Size = new System.Drawing.Size(53, 19);
            this.kryptonLabel2.TabIndex = 6;
            this.kryptonLabel2.Values.Text = "* Invoice";
            // 
            // kryptonLabel1
            // 
            this.kryptonLabel1.Location = new System.Drawing.Point(12, 12);
            this.kryptonLabel1.Name = "kryptonLabel1";
            this.kryptonLabel1.Size = new System.Drawing.Size(37, 19);
            this.kryptonLabel1.TabIndex = 0;
            this.kryptonLabel1.Values.Text = "Find :";
            // 
            // searchText
            // 
            this.searchText.ButtonSpecs.AddRange(new ComponentFactory.Krypton.Toolkit.ButtonSpecAny[] {
            this.startSearch});
            this.searchText.Location = new System.Drawing.Point(70, 12);
            this.searchText.Name = "searchText";
            this.searchText.Size = new System.Drawing.Size(351, 22);
            this.searchText.TabIndex = 1;
            this.searchText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.searchText_KeyDown);
            // 
            // startSearch
            // 
            this.startSearch.Type = ComponentFactory.Krypton.Toolkit.PaletteButtonSpecStyle.Next;
            this.startSearch.UniqueName = "C21DE323E2AD4CD47CA5A3C9304450B8";
            this.startSearch.Click += new System.EventHandler(this.buttonSpecAny1_Click);
            // 
            // checkColumn
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.NullValue = false;
            this.checkColumn.DefaultCellStyle = dataGridViewCellStyle1;
            this.checkColumn.FalseValue = null;
            this.checkColumn.HeaderText = "Check";
            this.checkColumn.IndeterminateValue = null;
            this.checkColumn.Name = "checkColumn";
            this.checkColumn.TrueValue = null;
            this.checkColumn.Width = 50;
            // 
            // purchaseorderNoColumn
            // 
            this.purchaseorderNoColumn.HeaderText = "Invoice No.";
            this.purchaseorderNoColumn.Name = "purchaseorderNoColumn";
            this.purchaseorderNoColumn.ReadOnly = true;
            // 
            // poDateColumn
            // 
            this.poDateColumn.HeaderText = "Invoice Date";
            this.poDateColumn.Name = "poDateColumn";
            this.poDateColumn.ReadOnly = true;
            // 
            // supplierColumn
            // 
            this.supplierColumn.HeaderText = "Customer";
            this.supplierColumn.Name = "supplierColumn";
            this.supplierColumn.ReadOnly = true;
            this.supplierColumn.Width = 100;
            // 
            // ccyColumn
            // 
            this.ccyColumn.HeaderText = "Ccy";
            this.ccyColumn.Name = "ccyColumn";
            this.ccyColumn.ReadOnly = true;
            // 
            // qtyColumn
            // 
            this.qtyColumn.DecimalPlaces = 2;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.NullValue = "0";
            this.qtyColumn.DefaultCellStyle = dataGridViewCellStyle2;
            this.qtyColumn.HeaderText = "Amount";
            this.qtyColumn.Increment = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.qtyColumn.Maximum = new decimal(new int[] {
            1410065408,
            2,
            0,
            0});
            this.qtyColumn.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.qtyColumn.Name = "qtyColumn";
            this.qtyColumn.ReadOnly = true;
            this.qtyColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.qtyColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.qtyColumn.ThousandsSeparator = true;
            this.qtyColumn.Width = 100;
            // 
            // SearchCussInvJForReceiptForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CANCELkryptonButton;
            this.ClientSize = new System.Drawing.Size(638, 341);
            this.Controls.Add(this.kryptonPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.KeyPreview = true;
            this.Name = "SearchCussInvJForReceiptForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Find";
            this.Load += new System.EventHandler(this.SearchPOForGRNForm_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SearchPOForGRNForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.gridData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonButton CANCELkryptonButton;
        private ComponentFactory.Krypton.Toolkit.KryptonButton OKkryptonButton;
        private ComponentFactory.Krypton.Toolkit.KryptonDataGridView gridData;
        private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel2;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel1;
        private ComponentFactory.Krypton.Toolkit.KryptonTextBox searchText;
        private ComponentFactory.Krypton.Toolkit.ButtonSpecAny startSearch;
        private ComponentFactory.Krypton.Toolkit.KryptonCheckBox checkAllkryptonCheckBox1;
        private ComponentFactory.Krypton.Toolkit.KryptonDataGridViewCheckBoxColumn checkColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn purchaseorderNoColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn poDateColumn;
        private ComponentFactory.Krypton.Toolkit.KryptonDataGridViewTextBoxColumn supplierColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ccyColumn;
        private ComponentFactory.Krypton.Toolkit.KryptonDataGridViewNumericUpDownColumn qtyColumn;
    }
}