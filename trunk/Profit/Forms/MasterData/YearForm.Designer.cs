namespace Profit
{
    partial class YearForm
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
            this.kryptonHeader1 = new ComponentFactory.Krypton.Toolkit.KryptonHeader();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonEdit = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonClear = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonRefresh = new System.Windows.Forms.ToolStripButton();
            this.kryptonSplitContainer1 = new ComponentFactory.Krypton.Toolkit.KryptonSplitContainer();
            this.kryptonPanel1 = new ComponentFactory.Krypton.Toolkit.KryptonPanel();
            this.kryptonGroup1 = new ComponentFactory.Krypton.Toolkit.KryptonGroup();
            this.periodskryptonDataGridView1 = new ComponentFactory.Krypton.Toolkit.KryptonDataGridView();
            this.PeriodCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Start = new ComponentFactory.Krypton.Toolkit.KryptonDataGridViewDateTimePickerColumn();
            this.EndPeriod = new ComponentFactory.Krypton.Toolkit.KryptonDataGridViewDateTimePickerColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.endDatekryptonDateTimePicker2 = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();
            this.startDatekryptonDateTimePicker1 = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();
            this.kryptonLabel4 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel3 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.textBoxCode = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.textBoxName = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.kryptonLabel1 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel2 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.gridData = new ComponentFactory.Krypton.Toolkit.KryptonDataGridView();
            this.dgName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgSubject = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exittoolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1.Panel1)).BeginInit();
            this.kryptonSplitContainer1.Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonSplitContainer1.Panel2)).BeginInit();
            this.kryptonSplitContainer1.Panel2.SuspendLayout();
            this.kryptonSplitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroup1.Panel)).BeginInit();
            this.kryptonGroup1.Panel.SuspendLayout();
            this.kryptonGroup1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.periodskryptonDataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
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
            this.kryptonHeader1.Values.Heading = "MSTF004 - Year";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonSave,
            this.toolStripButtonEdit,
            this.toolStripButtonDelete,
            this.toolStripButtonClear,
            this.toolStripButtonRefresh,
            this.toolStripSeparator1,
            this.exittoolStripButton1});
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
            this.kryptonSplitContainer1.SplitterDistance = 297;
            this.kryptonSplitContainer1.TabIndex = 4;
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.AutoScroll = true;
            this.kryptonPanel1.Controls.Add(this.kryptonGroup1);
            this.kryptonPanel1.Controls.Add(this.endDatekryptonDateTimePicker2);
            this.kryptonPanel1.Controls.Add(this.startDatekryptonDateTimePicker1);
            this.kryptonPanel1.Controls.Add(this.kryptonLabel4);
            this.kryptonPanel1.Controls.Add(this.kryptonLabel3);
            this.kryptonPanel1.Controls.Add(this.textBoxCode);
            this.kryptonPanel1.Controls.Add(this.textBoxName);
            this.kryptonPanel1.Controls.Add(this.kryptonLabel1);
            this.kryptonPanel1.Controls.Add(this.kryptonLabel2);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(609, 297);
            this.kryptonPanel1.TabIndex = 4;
            // 
            // kryptonGroup1
            // 
            this.kryptonGroup1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.kryptonGroup1.Location = new System.Drawing.Point(3, 59);
            this.kryptonGroup1.Name = "kryptonGroup1";
            // 
            // kryptonGroup1.Panel
            // 
            this.kryptonGroup1.Panel.Controls.Add(this.periodskryptonDataGridView1);
            this.kryptonGroup1.Size = new System.Drawing.Size(536, 232);
            this.kryptonGroup1.TabIndex = 9;
            // 
            // periodskryptonDataGridView1
            // 
            this.periodskryptonDataGridView1.AllowUserToAddRows = false;
            this.periodskryptonDataGridView1.AllowUserToDeleteRows = false;
            this.periodskryptonDataGridView1.AllowUserToResizeRows = false;
            this.periodskryptonDataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.PeriodCode,
            this.Start,
            this.EndPeriod,
            this.Status});
            this.periodskryptonDataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.periodskryptonDataGridView1.HideOuterBorders = true;
            this.periodskryptonDataGridView1.Location = new System.Drawing.Point(0, 0);
            this.periodskryptonDataGridView1.Name = "periodskryptonDataGridView1";
            this.periodskryptonDataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.periodskryptonDataGridView1.Size = new System.Drawing.Size(534, 230);
            this.periodskryptonDataGridView1.StateCommon.Background.Color1 = System.Drawing.Color.White;
            this.periodskryptonDataGridView1.StateCommon.Background.Color2 = System.Drawing.Color.White;
            this.periodskryptonDataGridView1.StateCommon.BackStyle = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.GridBackgroundList;
            this.periodskryptonDataGridView1.TabIndex = 8;
            // 
            // PeriodCode
            // 
            this.PeriodCode.HeaderText = "Period Code";
            this.PeriodCode.Name = "PeriodCode";
            this.PeriodCode.ReadOnly = true;
            // 
            // Start
            // 
            this.Start.CalendarTodayDate = new System.DateTime(2011, 7, 21, 0, 0, 0, 0);
            this.Start.Checked = false;
            this.Start.CustomFormat = "dd-MM-yyyy";
            this.Start.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.Start.HeaderText = "Start Period";
            this.Start.Name = "Start";
            this.Start.ReadOnly = true;
            this.Start.Width = 100;
            // 
            // EndPeriod
            // 
            this.EndPeriod.CalendarTodayDate = new System.DateTime(2011, 7, 21, 0, 0, 0, 0);
            this.EndPeriod.Checked = false;
            this.EndPeriod.CustomFormat = "dd-MM-yyyy";
            this.EndPeriod.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.EndPeriod.HeaderText = "End Period";
            this.EndPeriod.Name = "EndPeriod";
            this.EndPeriod.ReadOnly = true;
            this.EndPeriod.Width = 100;
            // 
            // Status
            // 
            this.Status.HeaderText = "Period Status";
            this.Status.Name = "Status";
            this.Status.ReadOnly = true;
            this.Status.Width = 150;
            // 
            // endDatekryptonDateTimePicker2
            // 
            this.endDatekryptonDateTimePicker2.CustomFormat = "dd-MM-yyyy";
            this.endDatekryptonDateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.endDatekryptonDateTimePicker2.Location = new System.Drawing.Point(360, 33);
            this.endDatekryptonDateTimePicker2.Name = "endDatekryptonDateTimePicker2";
            this.endDatekryptonDateTimePicker2.Size = new System.Drawing.Size(87, 20);
            this.endDatekryptonDateTimePicker2.TabIndex = 7;
            // 
            // startDatekryptonDateTimePicker1
            // 
            this.startDatekryptonDateTimePicker1.CustomFormat = "dd-MM-yyyy";
            this.startDatekryptonDateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.startDatekryptonDateTimePicker1.Location = new System.Drawing.Point(360, 9);
            this.startDatekryptonDateTimePicker1.Name = "startDatekryptonDateTimePicker1";
            this.startDatekryptonDateTimePicker1.Size = new System.Drawing.Size(87, 20);
            this.startDatekryptonDateTimePicker1.TabIndex = 6;
            // 
            // kryptonLabel4
            // 
            this.kryptonLabel4.Location = new System.Drawing.Point(293, 34);
            this.kryptonLabel4.Name = "kryptonLabel4";
            this.kryptonLabel4.Size = new System.Drawing.Size(61, 19);
            this.kryptonLabel4.TabIndex = 5;
            this.kryptonLabel4.Values.Text = "End Date :";
            // 
            // kryptonLabel3
            // 
            this.kryptonLabel3.Location = new System.Drawing.Point(288, 9);
            this.kryptonLabel3.Name = "kryptonLabel3";
            this.kryptonLabel3.Size = new System.Drawing.Size(66, 19);
            this.kryptonLabel3.TabIndex = 4;
            this.kryptonLabel3.Values.Text = "Start Date :";
            // 
            // textBoxCode
            // 
            this.textBoxCode.Location = new System.Drawing.Point(59, 6);
            this.textBoxCode.Name = "textBoxCode";
            this.textBoxCode.Size = new System.Drawing.Size(71, 22);
            this.textBoxCode.TabIndex = 2;
            // 
            // textBoxName
            // 
            this.textBoxName.Location = new System.Drawing.Point(59, 34);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(192, 22);
            this.textBoxName.TabIndex = 3;
            // 
            // kryptonLabel1
            // 
            this.kryptonLabel1.Location = new System.Drawing.Point(11, 6);
            this.kryptonLabel1.Name = "kryptonLabel1";
            this.kryptonLabel1.Size = new System.Drawing.Size(42, 19);
            this.kryptonLabel1.TabIndex = 0;
            this.kryptonLabel1.Values.Text = "Code :";
            // 
            // kryptonLabel2
            // 
            this.kryptonLabel2.Location = new System.Drawing.Point(7, 34);
            this.kryptonLabel2.Name = "kryptonLabel2";
            this.kryptonLabel2.Size = new System.Drawing.Size(46, 19);
            this.kryptonLabel2.TabIndex = 1;
            this.kryptonLabel2.Values.Text = "Name :";
            // 
            // gridData
            // 
            this.gridData.AllowUserToAddRows = false;
            this.gridData.AllowUserToDeleteRows = false;
            this.gridData.AllowUserToResizeRows = false;
            this.gridData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgName,
            this.dgSubject});
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
            this.gridData.Size = new System.Drawing.Size(609, 55);
            this.gridData.StateCommon.BackStyle = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.ControlClient;
            this.gridData.StateCommon.DataCell.Border.DrawBorders = ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom;
            this.gridData.TabIndex = 1;
            this.gridData.SelectionChanged += new System.EventHandler(this.gridData_SelectionChanged);
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
            this.dgSubject.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgSubject.DataPropertyName = "Name";
            this.dgSubject.HeaderText = "Name";
            this.dgSubject.MinimumWidth = 100;
            this.dgSubject.Name = "dgSubject";
            this.dgSubject.ReadOnly = true;
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // exittoolStripButton1
            // 
            this.exittoolStripButton1.Image = global::Profit.Properties.Resources.Exit;
            this.exittoolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.exittoolStripButton1.Name = "exittoolStripButton1";
            this.exittoolStripButton1.Size = new System.Drawing.Size(45, 22);
            this.exittoolStripButton1.Text = "Exit";
            this.exittoolStripButton1.Click += new System.EventHandler(this.exittoolStripButton1_Click);
            // 
            // YearForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(609, 411);
            this.Controls.Add(this.kryptonSplitContainer1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.kryptonHeader1);
            this.Name = "YearForm";
            this.Text = "Year";
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
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroup1.Panel)).EndInit();
            this.kryptonGroup1.Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroup1)).EndInit();
            this.kryptonGroup1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.periodskryptonDataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridData)).EndInit();
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
        private ComponentFactory.Krypton.Toolkit.KryptonSplitContainer kryptonSplitContainer1;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel2;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel1;
        private ComponentFactory.Krypton.Toolkit.KryptonDataGridView gridData;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgSubject;
        private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private ComponentFactory.Krypton.Toolkit.KryptonTextBox textBoxCode;
        private ComponentFactory.Krypton.Toolkit.KryptonTextBox textBoxName;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker endDatekryptonDateTimePicker2;
        private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker startDatekryptonDateTimePicker1;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel4;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel3;
        private ComponentFactory.Krypton.Toolkit.KryptonDataGridView periodskryptonDataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn PeriodCode;
        private ComponentFactory.Krypton.Toolkit.KryptonDataGridViewDateTimePickerColumn Start;
        private ComponentFactory.Krypton.Toolkit.KryptonDataGridViewDateTimePickerColumn EndPeriod;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
        private ComponentFactory.Krypton.Toolkit.KryptonGroup kryptonGroup1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton exittoolStripButton1;
    }
}