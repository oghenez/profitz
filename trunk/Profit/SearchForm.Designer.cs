namespace Profit
{
    partial class SearchForm
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
            this.resultData = new ComponentFactory.Krypton.Toolkit.KryptonDataGridView();
            this.CodeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.kryptonButton1 = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.kryptonButton2 = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            ((System.ComponentModel.ISupportInitialize)(this.resultData)).BeginInit();
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
            this.searchText.Location = new System.Drawing.Point(71, 12);
            this.searchText.Name = "searchText";
            this.searchText.Size = new System.Drawing.Size(342, 22);
            this.searchText.TabIndex = 1;
            this.searchText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.searchText_KeyDown);
            // 
            // startSearch
            // 
            this.startSearch.Type = ComponentFactory.Krypton.Toolkit.PaletteButtonSpecStyle.Next;
            this.startSearch.UniqueName = "C21DE323E2AD4CD47CA5A3C9304450B8";
            this.startSearch.Click += new System.EventHandler(this.buttonSpecAny1_Click);
            // 
            // resultData
            // 
            this.resultData.AllowUserToAddRows = false;
            this.resultData.AllowUserToDeleteRows = false;
            this.resultData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CodeColumn,
            this.NameColumn});
            this.resultData.Location = new System.Drawing.Point(12, 40);
            this.resultData.MultiSelect = false;
            this.resultData.Name = "resultData";
            this.resultData.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.resultData.Size = new System.Drawing.Size(409, 264);
            this.resultData.TabIndex = 2;
            // 
            // CodeColumn
            // 
            this.CodeColumn.HeaderText = "Code";
            this.CodeColumn.Name = "CodeColumn";
            this.CodeColumn.ReadOnly = true;
            // 
            // NameColumn
            // 
            this.NameColumn.HeaderText = "Name";
            this.NameColumn.Name = "NameColumn";
            this.NameColumn.ReadOnly = true;
            this.NameColumn.Width = 250;
            // 
            // kryptonButton1
            // 
            this.kryptonButton1.Location = new System.Drawing.Point(358, 310);
            this.kryptonButton1.Name = "kryptonButton1";
            this.kryptonButton1.Size = new System.Drawing.Size(63, 25);
            this.kryptonButton1.TabIndex = 3;
            this.kryptonButton1.Values.Text = "Cancel";
            // 
            // kryptonButton2
            // 
            this.kryptonButton2.Location = new System.Drawing.Point(289, 310);
            this.kryptonButton2.Name = "kryptonButton2";
            this.kryptonButton2.Size = new System.Drawing.Size(63, 25);
            this.kryptonButton2.TabIndex = 4;
            this.kryptonButton2.Values.Text = "OK";
            // 
            // SearchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(431, 342);
            this.Controls.Add(this.kryptonButton2);
            this.Controls.Add(this.kryptonButton1);
            this.Controls.Add(this.resultData);
            this.Controls.Add(this.searchText);
            this.Controls.Add(this.kryptonLabel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "SearchForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Find";
            ((System.ComponentModel.ISupportInitialize)(this.resultData)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel1;
        private ComponentFactory.Krypton.Toolkit.KryptonTextBox searchText;
        private ComponentFactory.Krypton.Toolkit.ButtonSpecAny startSearch;
        private ComponentFactory.Krypton.Toolkit.KryptonDataGridView resultData;
        private ComponentFactory.Krypton.Toolkit.KryptonButton kryptonButton1;
        private ComponentFactory.Krypton.Toolkit.KryptonButton kryptonButton2;
        private System.Windows.Forms.DataGridViewTextBoxColumn CodeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn NameColumn;
    }
}