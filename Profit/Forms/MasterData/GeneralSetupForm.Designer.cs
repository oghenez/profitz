namespace Profit
{
    partial class GeneralSetupForm
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
            this.kryptonPanel1 = new ComponentFactory.Krypton.Toolkit.KryptonPanel();
            this.kryptonLabel8 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel7 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel6 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel5 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.faxKryptonTextBox = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.websiteKryptonTextBox = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.emailKryptonTextBox = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.regDatekryptonDateTimePicker = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();
            this.taxNokryptonTextBox = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.phoneKryptonTextBox = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.kryptonLabel3 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel4 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.gridAutonumber = new ComponentFactory.Krypton.Toolkit.KryptonDataGridView();
            this.companyNameTextBox = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.addresstextBox = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.kryptonLabel1 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel2 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.FormNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PrefixColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StartColumn = new ComponentFactory.Krypton.Toolkit.KryptonDataGridViewNumericUpDownColumn();
            this.DigitColumn = new ComponentFactory.Krypton.Toolkit.KryptonDataGridViewNumericUpDownColumn();
            this.InitColumn = new ComponentFactory.Krypton.Toolkit.KryptonDataGridViewComboBoxColumn();
            this.AutonumberColumn = new ComponentFactory.Krypton.Toolkit.KryptonDataGridViewComboBoxColumn();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridAutonumber)).BeginInit();
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
            this.kryptonHeader1.Values.Heading = "GSTP002 - General Setup";
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
            // kryptonPanel1
            // 
            this.kryptonPanel1.AutoScroll = true;
            this.kryptonPanel1.Controls.Add(this.kryptonLabel8);
            this.kryptonPanel1.Controls.Add(this.kryptonLabel7);
            this.kryptonPanel1.Controls.Add(this.kryptonLabel6);
            this.kryptonPanel1.Controls.Add(this.kryptonLabel5);
            this.kryptonPanel1.Controls.Add(this.faxKryptonTextBox);
            this.kryptonPanel1.Controls.Add(this.websiteKryptonTextBox);
            this.kryptonPanel1.Controls.Add(this.emailKryptonTextBox);
            this.kryptonPanel1.Controls.Add(this.regDatekryptonDateTimePicker);
            this.kryptonPanel1.Controls.Add(this.taxNokryptonTextBox);
            this.kryptonPanel1.Controls.Add(this.phoneKryptonTextBox);
            this.kryptonPanel1.Controls.Add(this.kryptonLabel3);
            this.kryptonPanel1.Controls.Add(this.kryptonLabel4);
            this.kryptonPanel1.Controls.Add(this.gridAutonumber);
            this.kryptonPanel1.Controls.Add(this.companyNameTextBox);
            this.kryptonPanel1.Controls.Add(this.addresstextBox);
            this.kryptonPanel1.Controls.Add(this.kryptonLabel1);
            this.kryptonPanel1.Controls.Add(this.kryptonLabel2);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 54);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(609, 381);
            this.kryptonPanel1.TabIndex = 4;
            // 
            // kryptonLabel8
            // 
            this.kryptonLabel8.Location = new System.Drawing.Point(361, 149);
            this.kryptonLabel8.Name = "kryptonLabel8";
            this.kryptonLabel8.Size = new System.Drawing.Size(62, 19);
            this.kryptonLabel8.TabIndex = 21;
            this.kryptonLabel8.Values.Text = "Reg Date :";
            // 
            // kryptonLabel7
            // 
            this.kryptonLabel7.Location = new System.Drawing.Point(370, 125);
            this.kryptonLabel7.Name = "kryptonLabel7";
            this.kryptonLabel7.Size = new System.Drawing.Size(53, 19);
            this.kryptonLabel7.TabIndex = 20;
            this.kryptonLabel7.Values.Text = "Fax No. :";
            // 
            // kryptonLabel6
            // 
            this.kryptonLabel6.Location = new System.Drawing.Point(78, 203);
            this.kryptonLabel6.Name = "kryptonLabel6";
            this.kryptonLabel6.Size = new System.Drawing.Size(56, 19);
            this.kryptonLabel6.TabIndex = 19;
            this.kryptonLabel6.Values.Text = "Website :";
            // 
            // kryptonLabel5
            // 
            this.kryptonLabel5.Location = new System.Drawing.Point(92, 178);
            this.kryptonLabel5.Name = "kryptonLabel5";
            this.kryptonLabel5.Size = new System.Drawing.Size(42, 19);
            this.kryptonLabel5.TabIndex = 18;
            this.kryptonLabel5.Values.Text = "Email :";
            // 
            // faxKryptonTextBox
            // 
            this.faxKryptonTextBox.Location = new System.Drawing.Point(436, 122);
            this.faxKryptonTextBox.Name = "faxKryptonTextBox";
            this.faxKryptonTextBox.Size = new System.Drawing.Size(153, 22);
            this.faxKryptonTextBox.TabIndex = 17;
            this.faxKryptonTextBox.TextChanged += new System.EventHandler(this.faxKryptonTextBox_TextChanged);
            // 
            // websiteKryptonTextBox
            // 
            this.websiteKryptonTextBox.Location = new System.Drawing.Point(140, 203);
            this.websiteKryptonTextBox.Name = "websiteKryptonTextBox";
            this.websiteKryptonTextBox.Size = new System.Drawing.Size(192, 22);
            this.websiteKryptonTextBox.TabIndex = 16;
            // 
            // emailKryptonTextBox
            // 
            this.emailKryptonTextBox.Location = new System.Drawing.Point(140, 175);
            this.emailKryptonTextBox.Name = "emailKryptonTextBox";
            this.emailKryptonTextBox.Size = new System.Drawing.Size(192, 22);
            this.emailKryptonTextBox.TabIndex = 15;
            // 
            // regDatekryptonDateTimePicker
            // 
            this.regDatekryptonDateTimePicker.Location = new System.Drawing.Point(436, 149);
            this.regDatekryptonDateTimePicker.Name = "regDatekryptonDateTimePicker";
            this.regDatekryptonDateTimePicker.Size = new System.Drawing.Size(139, 20);
            this.regDatekryptonDateTimePicker.TabIndex = 14;
            // 
            // taxNokryptonTextBox
            // 
            this.taxNokryptonTextBox.Location = new System.Drawing.Point(140, 150);
            this.taxNokryptonTextBox.Name = "taxNokryptonTextBox";
            this.taxNokryptonTextBox.Size = new System.Drawing.Size(192, 22);
            this.taxNokryptonTextBox.TabIndex = 13;
            // 
            // phoneKryptonTextBox
            // 
            this.phoneKryptonTextBox.Location = new System.Drawing.Point(140, 122);
            this.phoneKryptonTextBox.Name = "phoneKryptonTextBox";
            this.phoneKryptonTextBox.Size = new System.Drawing.Size(192, 22);
            this.phoneKryptonTextBox.TabIndex = 12;
            // 
            // kryptonLabel3
            // 
            this.kryptonLabel3.Location = new System.Drawing.Point(87, 125);
            this.kryptonLabel3.Name = "kryptonLabel3";
            this.kryptonLabel3.Size = new System.Drawing.Size(47, 19);
            this.kryptonLabel3.TabIndex = 11;
            this.kryptonLabel3.Values.Text = "Phone :";
            // 
            // kryptonLabel4
            // 
            this.kryptonLabel4.Location = new System.Drawing.Point(83, 150);
            this.kryptonLabel4.Name = "kryptonLabel4";
            this.kryptonLabel4.Size = new System.Drawing.Size(51, 19);
            this.kryptonLabel4.TabIndex = 10;
            this.kryptonLabel4.Values.Text = "Tax No :";
            // 
            // gridAutonumber
            // 
            this.gridAutonumber.AllowUserToAddRows = false;
            this.gridAutonumber.AllowUserToDeleteRows = false;
            this.gridAutonumber.AllowUserToResizeRows = false;
            this.gridAutonumber.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.gridAutonumber.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.FormNameColumn,
            this.PrefixColumn,
            this.StartColumn,
            this.DigitColumn,
            this.InitColumn,
            this.AutonumberColumn});
            this.gridAutonumber.Location = new System.Drawing.Point(11, 244);
            this.gridAutonumber.Name = "gridAutonumber";
            this.gridAutonumber.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridAutonumber.Size = new System.Drawing.Size(534, 112);
            this.gridAutonumber.StateCommon.Background.Color1 = System.Drawing.Color.White;
            this.gridAutonumber.StateCommon.Background.Color2 = System.Drawing.Color.White;
            this.gridAutonumber.StateCommon.BackStyle = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.GridBackgroundList;
            this.gridAutonumber.TabIndex = 9;
            // 
            // companyNameTextBox
            // 
            this.companyNameTextBox.Location = new System.Drawing.Point(140, 6);
            this.companyNameTextBox.Name = "companyNameTextBox";
            this.companyNameTextBox.Size = new System.Drawing.Size(192, 22);
            this.companyNameTextBox.TabIndex = 2;
            // 
            // addresstextBox
            // 
            this.addresstextBox.Location = new System.Drawing.Point(140, 34);
            this.addresstextBox.Multiline = true;
            this.addresstextBox.Name = "addresstextBox";
            this.addresstextBox.Size = new System.Drawing.Size(286, 82);
            this.addresstextBox.TabIndex = 3;
            // 
            // kryptonLabel1
            // 
            this.kryptonLabel1.Location = new System.Drawing.Point(38, 6);
            this.kryptonLabel1.Name = "kryptonLabel1";
            this.kryptonLabel1.Size = new System.Drawing.Size(96, 19);
            this.kryptonLabel1.TabIndex = 0;
            this.kryptonLabel1.Values.Text = "Company Name :";
            // 
            // kryptonLabel2
            // 
            this.kryptonLabel2.Location = new System.Drawing.Point(78, 34);
            this.kryptonLabel2.Name = "kryptonLabel2";
            this.kryptonLabel2.Size = new System.Drawing.Size(56, 19);
            this.kryptonLabel2.TabIndex = 1;
            this.kryptonLabel2.Values.Text = "Address :";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // FormNameColumn
            // 
            this.FormNameColumn.HeaderText = "Form";
            this.FormNameColumn.Name = "FormNameColumn";
            this.FormNameColumn.Width = 186;
            // 
            // PrefixColumn
            // 
            this.PrefixColumn.HeaderText = "Prefix";
            this.PrefixColumn.Name = "PrefixColumn";
            // 
            // StartColumn
            // 
            this.StartColumn.HeaderText = "Start";
            this.StartColumn.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.StartColumn.Maximum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.StartColumn.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.StartColumn.Name = "StartColumn";
            this.StartColumn.Width = 35;
            // 
            // DigitColumn
            // 
            this.DigitColumn.HeaderText = "Digit";
            this.DigitColumn.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.DigitColumn.Maximum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.DigitColumn.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.DigitColumn.Name = "DigitColumn";
            this.DigitColumn.Width = 38;
            // 
            // InitColumn
            // 
            this.InitColumn.DropDownWidth = 121;
            this.InitColumn.HeaderText = "Initial";
            this.InitColumn.Name = "InitColumn";
            this.InitColumn.Width = 51;
            // 
            // AutonumberColumn
            // 
            this.AutonumberColumn.DropDownWidth = 121;
            this.AutonumberColumn.HeaderText = "Autonumber";
            this.AutonumberColumn.Name = "AutonumberColumn";
            this.AutonumberColumn.Width = 76;
            // 
            // GeneralSetupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(609, 435);
            this.Controls.Add(this.kryptonPanel1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.kryptonHeader1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "GeneralSetupForm";
            this.Text = "General Setup";
            this.Activated += new System.EventHandler(this.BankForm_Activated);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            this.kryptonPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridAutonumber)).EndInit();
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
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel2;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel1;
        private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private ComponentFactory.Krypton.Toolkit.KryptonTextBox companyNameTextBox;
        private ComponentFactory.Krypton.Toolkit.KryptonTextBox addresstextBox;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private ComponentFactory.Krypton.Toolkit.KryptonDataGridView gridAutonumber;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel4;
        private ComponentFactory.Krypton.Toolkit.KryptonTextBox phoneKryptonTextBox;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel3;
        private ComponentFactory.Krypton.Toolkit.KryptonTextBox taxNokryptonTextBox;
        private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker regDatekryptonDateTimePicker;
        private ComponentFactory.Krypton.Toolkit.KryptonTextBox websiteKryptonTextBox;
        private ComponentFactory.Krypton.Toolkit.KryptonTextBox emailKryptonTextBox;
        private ComponentFactory.Krypton.Toolkit.KryptonTextBox faxKryptonTextBox;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel8;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel7;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel6;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel5;
        private System.Windows.Forms.DataGridViewTextBoxColumn FormNameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn PrefixColumn;
        private ComponentFactory.Krypton.Toolkit.KryptonDataGridViewNumericUpDownColumn StartColumn;
        private ComponentFactory.Krypton.Toolkit.KryptonDataGridViewNumericUpDownColumn DigitColumn;
        private ComponentFactory.Krypton.Toolkit.KryptonDataGridViewComboBoxColumn InitColumn;
        private ComponentFactory.Krypton.Toolkit.KryptonDataGridViewComboBoxColumn AutonumberColumn;
    }
}