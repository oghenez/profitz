namespace Profit
{
    partial class ProcessTransactionForm
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
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.kryptonPanel1 = new ComponentFactory.Krypton.Toolkit.KryptonPanel();
            this.kryptonGroupBox1 = new ComponentFactory.Krypton.Toolkit.KryptonGroupBox();
            this.kryptonTextBox1 = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.processNexkryptonButton = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.kryptonButtonRollBack = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.kryptonDateTimePicker1 = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();
            this.kryptonLabel1 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exittoolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1.Panel)).BeginInit();
            this.kryptonGroupBox1.Panel.SuspendLayout();
            this.kryptonGroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // kryptonHeader1
            // 
            this.kryptonHeader1.Dock = System.Windows.Forms.DockStyle.Top;
            this.kryptonHeader1.Location = new System.Drawing.Point(0, 0);
            this.kryptonHeader1.Name = "kryptonHeader1";
            this.kryptonHeader1.Size = new System.Drawing.Size(541, 29);
            this.kryptonHeader1.TabIndex = 2;
            this.kryptonHeader1.Values.Description = "";
            this.kryptonHeader1.Values.Heading = "GSTP003 - Process Transaction";
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
            this.toolStrip1.Size = new System.Drawing.Size(541, 25);
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
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.kryptonGroupBox1);
            this.kryptonPanel1.Controls.Add(this.kryptonDateTimePicker1);
            this.kryptonPanel1.Controls.Add(this.kryptonLabel1);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 54);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(541, 278);
            this.kryptonPanel1.TabIndex = 4;
            // 
            // kryptonGroupBox1
            // 
            this.kryptonGroupBox1.Location = new System.Drawing.Point(12, 47);
            this.kryptonGroupBox1.Name = "kryptonGroupBox1";
            // 
            // kryptonGroupBox1.Panel
            // 
            this.kryptonGroupBox1.Panel.Controls.Add(this.kryptonTextBox1);
            this.kryptonGroupBox1.Panel.Controls.Add(this.processNexkryptonButton);
            this.kryptonGroupBox1.Panel.Controls.Add(this.kryptonButtonRollBack);
            this.kryptonGroupBox1.Size = new System.Drawing.Size(516, 221);
            this.kryptonGroupBox1.TabIndex = 2;
            this.kryptonGroupBox1.Text = "Process";
            this.kryptonGroupBox1.Values.Heading = "Process";
            // 
            // kryptonTextBox1
            // 
            this.kryptonTextBox1.Location = new System.Drawing.Point(3, 34);
            this.kryptonTextBox1.Multiline = true;
            this.kryptonTextBox1.Name = "kryptonTextBox1";
            this.kryptonTextBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.kryptonTextBox1.Size = new System.Drawing.Size(506, 161);
            this.kryptonTextBox1.TabIndex = 2;
            // 
            // processNexkryptonButton
            // 
            this.processNexkryptonButton.Location = new System.Drawing.Point(351, 3);
            this.processNexkryptonButton.Name = "processNexkryptonButton";
            this.processNexkryptonButton.Size = new System.Drawing.Size(158, 25);
            this.processNexkryptonButton.TabIndex = 1;
            this.processNexkryptonButton.Values.Text = "Process To Next Period >>";
            this.processNexkryptonButton.Click += new System.EventHandler(this.processNexkryptonButton_Click);
            // 
            // kryptonButtonRollBack
            // 
            this.kryptonButtonRollBack.Location = new System.Drawing.Point(3, 3);
            this.kryptonButtonRollBack.Name = "kryptonButtonRollBack";
            this.kryptonButtonRollBack.Size = new System.Drawing.Size(158, 25);
            this.kryptonButtonRollBack.TabIndex = 0;
            this.kryptonButtonRollBack.Values.Text = "<< Roll Back Period";
            this.kryptonButtonRollBack.Click += new System.EventHandler(this.kryptonButtonRollBack_Click);
            // 
            // kryptonDateTimePicker1
            // 
            this.kryptonDateTimePicker1.CustomFormat = "MMM-yyyy";
            this.kryptonDateTimePicker1.Enabled = false;
            this.kryptonDateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.kryptonDateTimePicker1.Location = new System.Drawing.Point(106, 15);
            this.kryptonDateTimePicker1.Name = "kryptonDateTimePicker1";
            this.kryptonDateTimePicker1.Size = new System.Drawing.Size(115, 31);
            this.kryptonDateTimePicker1.StateCommon.Content.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.kryptonDateTimePicker1.StateDisabled.Content.Color1 = System.Drawing.Color.Black;
            this.kryptonDateTimePicker1.StateDisabled.Content.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.kryptonDateTimePicker1.TabIndex = 1;
            // 
            // kryptonLabel1
            // 
            this.kryptonLabel1.LabelStyle = ComponentFactory.Krypton.Toolkit.LabelStyle.BoldPanel;
            this.kryptonLabel1.Location = new System.Drawing.Point(12, 22);
            this.kryptonLabel1.Name = "kryptonLabel1";
            this.kryptonLabel1.Size = new System.Drawing.Size(88, 19);
            this.kryptonLabel1.TabIndex = 0;
            this.kryptonLabel1.Values.Text = "Active Period :";
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
            // ProcessTransactionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(541, 332);
            this.Controls.Add(this.kryptonPanel1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.kryptonHeader1);
            this.Name = "ProcessTransactionForm";
            this.Text = "Process Transaction";
            this.Activated += new System.EventHandler(this.BankForm_Activated);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            this.kryptonPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1.Panel)).EndInit();
            this.kryptonGroupBox1.Panel.ResumeLayout(false);
            this.kryptonGroupBox1.Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1)).EndInit();
            this.kryptonGroupBox1.ResumeLayout(false);
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
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker kryptonDateTimePicker1;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel1;
        private ComponentFactory.Krypton.Toolkit.KryptonGroupBox kryptonGroupBox1;
        private ComponentFactory.Krypton.Toolkit.KryptonButton processNexkryptonButton;
        private ComponentFactory.Krypton.Toolkit.KryptonButton kryptonButtonRollBack;
        private ComponentFactory.Krypton.Toolkit.KryptonTextBox kryptonTextBox1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton exittoolStripButton1;
    }
}