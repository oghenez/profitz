namespace Profit
{
    partial class FieldChooserForm
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
            this.columnKryptonCheckedListBox = new ComponentFactory.Krypton.Toolkit.KryptonCheckedListBox();
            this.exitButton = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.SuspendLayout();
            // 
            // columnKryptonCheckedListBox
            // 
            this.columnKryptonCheckedListBox.Location = new System.Drawing.Point(3, 3);
            this.columnKryptonCheckedListBox.Name = "columnKryptonCheckedListBox";
            this.columnKryptonCheckedListBox.Size = new System.Drawing.Size(179, 248);
            this.columnKryptonCheckedListBox.TabIndex = 0;
            this.columnKryptonCheckedListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.columnKryptonCheckedListBox_ItemCheck);
            // 
            // exitButton
            // 
            this.exitButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.exitButton.Location = new System.Drawing.Point(138, 257);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(44, 25);
            this.exitButton.TabIndex = 1;
            this.exitButton.Values.Text = "Exit";
            this.exitButton.Click += new System.EventHandler(this.exitButton_Click);
            // 
            // FieldChooserForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.exitButton;
            this.ClientSize = new System.Drawing.Size(184, 287);
            this.Controls.Add(this.exitButton);
            this.Controls.Add(this.columnKryptonCheckedListBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FieldChooserForm";
            this.Text = "Field Chooser";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FieldChooserForm_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonCheckedListBox columnKryptonCheckedListBox;
        private ComponentFactory.Krypton.Toolkit.KryptonButton exitButton;
    }
}