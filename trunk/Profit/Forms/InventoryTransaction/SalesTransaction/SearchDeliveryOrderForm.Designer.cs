namespace Profit
{
    partial class SearchDeliveryOrderForm
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
            this.kryptonLabel1 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.searchText = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.startSearch = new ComponentFactory.Krypton.Toolkit.ButtonSpecAny();
            this.CANCELkryptonButton = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.OKkryptonButton = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.gridData = new ComponentFactory.Krypton.Toolkit.KryptonDataGridView();
            this.kryptonPanel1 = new ComponentFactory.Krypton.Toolkit.KryptonPanel();
            this.dgName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgSubject = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.supplierColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EmpCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PostedCol = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gridData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            this.SuspendLayout();
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
            this.searchText.Location = new System.Drawing.Point(67, 12);
            this.searchText.Name = "searchText";
            this.searchText.Size = new System.Drawing.Size(346, 22);
            this.searchText.TabIndex = 1;
            this.searchText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.searchText_KeyDown);
            // 
            // startSearch
            // 
            this.startSearch.Type = ComponentFactory.Krypton.Toolkit.PaletteButtonSpecStyle.Next;
            this.startSearch.UniqueName = "C21DE323E2AD4CD47CA5A3C9304450B8";
            this.startSearch.Click += new System.EventHandler(this.buttonSpecAny1_Click);
            // 
            // CANCELkryptonButton
            // 
            this.CANCELkryptonButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CANCELkryptonButton.Location = new System.Drawing.Point(358, 310);
            this.CANCELkryptonButton.Name = "CANCELkryptonButton";
            this.CANCELkryptonButton.Size = new System.Drawing.Size(63, 25);
            this.CANCELkryptonButton.TabIndex = 3;
            this.CANCELkryptonButton.Values.Text = "Cancel";
            this.CANCELkryptonButton.Click += new System.EventHandler(this.CANCELkryptonButton_Click);
            // 
            // OKkryptonButton
            // 
            this.OKkryptonButton.Location = new System.Drawing.Point(289, 310);
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
            this.dgName,
            this.dgSubject,
            this.supplierColumn,
            this.EmpCol,
            this.PostedCol});
            this.gridData.GridStyles.Style = ComponentFactory.Krypton.Toolkit.DataGridViewStyle.Mixed;
            this.gridData.GridStyles.StyleBackground = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.GridDataCellCustom1;
            this.gridData.Location = new System.Drawing.Point(12, 37);
            this.gridData.MultiSelect = false;
            this.gridData.Name = "gridData";
            this.gridData.ReadOnly = true;
            this.gridData.RowHeadersVisible = false;
            this.gridData.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gridData.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridData.Size = new System.Drawing.Size(409, 264);
            this.gridData.StateCommon.BackStyle = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.GridDataCellCustom1;
            this.gridData.StateCommon.DataCell.Border.DrawBorders = ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom;
            this.gridData.TabIndex = 5;
            this.gridData.TabStop = false;
            this.gridData.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.gridData_CellMouseDoubleClick);
            this.gridData.KeyDown += new System.Windows.Forms.KeyEventHandler(this.gridData_KeyDown);
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.kryptonLabel1);
            this.kryptonPanel1.Controls.Add(this.gridData);
            this.kryptonPanel1.Controls.Add(this.searchText);
            this.kryptonPanel1.Controls.Add(this.OKkryptonButton);
            this.kryptonPanel1.Controls.Add(this.CANCELkryptonButton);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(430, 341);
            this.kryptonPanel1.TabIndex = 6;
            // 
            // dgName
            // 
            this.dgName.DataPropertyName = "Code";
            this.dgName.HeaderText = "Code";
            this.dgName.MinimumWidth = 100;
            this.dgName.Name = "dgName";
            this.dgName.ReadOnly = true;
            // 
            // dgSubject
            // 
            this.dgSubject.DataPropertyName = "Date";
            this.dgSubject.HeaderText = "Date";
            this.dgSubject.MinimumWidth = 50;
            this.dgSubject.Name = "dgSubject";
            this.dgSubject.ReadOnly = true;
            this.dgSubject.Width = 80;
            // 
            // supplierColumn
            // 
            this.supplierColumn.HeaderText = "Supplier";
            this.supplierColumn.Name = "supplierColumn";
            this.supplierColumn.ReadOnly = true;
            this.supplierColumn.Width = 50;
            // 
            // EmpCol
            // 
            this.EmpCol.HeaderText = "Employee";
            this.EmpCol.Name = "EmpCol";
            this.EmpCol.ReadOnly = true;
            // 
            // PostedCol
            // 
            this.PostedCol.HeaderText = "Posted";
            this.PostedCol.Name = "PostedCol";
            this.PostedCol.ReadOnly = true;
            this.PostedCol.Width = 50;
            // 
            // SearchGoodReceiveNoteForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CANCELkryptonButton;
            this.ClientSize = new System.Drawing.Size(430, 341);
            this.Controls.Add(this.kryptonPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.KeyPreview = true;
            this.Name = "SearchGoodReceiveNoteForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Find";
            this.Load += new System.EventHandler(this.SearchDeliveryOrderForm_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SearchDeliveryOrderForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.gridData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel1;
        private ComponentFactory.Krypton.Toolkit.KryptonTextBox searchText;
        private ComponentFactory.Krypton.Toolkit.ButtonSpecAny startSearch;
        private ComponentFactory.Krypton.Toolkit.KryptonButton CANCELkryptonButton;
        private ComponentFactory.Krypton.Toolkit.KryptonButton OKkryptonButton;
        private ComponentFactory.Krypton.Toolkit.KryptonDataGridView gridData;
        private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgSubject;
        private System.Windows.Forms.DataGridViewTextBoxColumn supplierColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn EmpCol;
        private System.Windows.Forms.DataGridViewCheckBoxColumn PostedCol;
    }
}