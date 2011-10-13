namespace Profit
{
    partial class LoginForm
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
            this.kryptonPanel1 = new ComponentFactory.Krypton.Toolkit.KryptonPanel();
            this.messageLabelkryptonLabel = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.userCodekryptonTextBox1 = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.loginkryptonButton1 = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.kryptonLabel2 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.passwordkryptonTextBox2 = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.kryptonLabel3 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.fadeTimer = new System.Windows.Forms.Timer(this.components);
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.kryptonManager1 = new ComponentFactory.Krypton.Toolkit.KryptonManager(this.components);
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.messageLabelkryptonLabel);
            this.kryptonPanel1.Controls.Add(this.pictureBox2);
            this.kryptonPanel1.Controls.Add(this.loginkryptonButton1);
            this.kryptonPanel1.Controls.Add(this.userCodekryptonTextBox1);
            this.kryptonPanel1.Controls.Add(this.passwordkryptonTextBox2);
            this.kryptonPanel1.Controls.Add(this.kryptonLabel2);
            this.kryptonPanel1.Controls.Add(this.kryptonLabel3);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(564, 372);
            this.kryptonPanel1.StateNormal.Image = global::Profit.Properties.Resources.FlashScreen;
            this.kryptonPanel1.TabIndex = 1;
            // 
            // messageLabelkryptonLabel
            // 
            this.messageLabelkryptonLabel.LabelStyle = ComponentFactory.Krypton.Toolkit.LabelStyle.ItalicPanel;
            this.messageLabelkryptonLabel.Location = new System.Drawing.Point(224, 124);
            this.messageLabelkryptonLabel.Name = "messageLabelkryptonLabel";
            this.messageLabelkryptonLabel.Size = new System.Drawing.Size(6, 2);
            this.messageLabelkryptonLabel.StateCommon.ShortText.Color1 = System.Drawing.Color.Red;
            this.messageLabelkryptonLabel.TabIndex = 9;
            this.messageLabelkryptonLabel.Values.Text = "";
            // 
            // userCodekryptonTextBox1
            // 
            this.userCodekryptonTextBox1.Location = new System.Drawing.Point(77, 62);
            this.userCodekryptonTextBox1.Name = "userCodekryptonTextBox1";
            this.userCodekryptonTextBox1.Size = new System.Drawing.Size(141, 22);
            this.userCodekryptonTextBox1.TabIndex = 4;
            this.userCodekryptonTextBox1.Text = "ADMIN";
            // 
            // loginkryptonButton1
            // 
            this.loginkryptonButton1.Location = new System.Drawing.Point(150, 118);
            this.loginkryptonButton1.Name = "loginkryptonButton1";
            this.loginkryptonButton1.Size = new System.Drawing.Size(68, 25);
            this.loginkryptonButton1.TabIndex = 6;
            this.loginkryptonButton1.Values.Text = "LOGIN";
            this.loginkryptonButton1.Click += new System.EventHandler(this.loginkryptonButton1_Click);
            // 
            // kryptonLabel2
            // 
            this.kryptonLabel2.LabelStyle = ComponentFactory.Krypton.Toolkit.LabelStyle.BoldControl;
            this.kryptonLabel2.Location = new System.Drawing.Point(3, 63);
            this.kryptonLabel2.Name = "kryptonLabel2";
            this.kryptonLabel2.Size = new System.Drawing.Size(70, 19);
            this.kryptonLabel2.StateCommon.ShortText.Color1 = System.Drawing.Color.White;
            this.kryptonLabel2.TabIndex = 2;
            this.kryptonLabel2.Values.Text = "User Code :";
            // 
            // passwordkryptonTextBox2
            // 
            this.passwordkryptonTextBox2.Location = new System.Drawing.Point(77, 90);
            this.passwordkryptonTextBox2.Name = "passwordkryptonTextBox2";
            this.passwordkryptonTextBox2.PasswordChar = '#';
            this.passwordkryptonTextBox2.Size = new System.Drawing.Size(141, 22);
            this.passwordkryptonTextBox2.TabIndex = 5;
            this.passwordkryptonTextBox2.Text = "admin1234";
            // 
            // kryptonLabel3
            // 
            this.kryptonLabel3.LabelStyle = ComponentFactory.Krypton.Toolkit.LabelStyle.BoldControl;
            this.kryptonLabel3.Location = new System.Drawing.Point(8, 92);
            this.kryptonLabel3.Name = "kryptonLabel3";
            this.kryptonLabel3.Size = new System.Drawing.Size(67, 19);
            this.kryptonLabel3.StateCommon.ShortText.Color1 = System.Drawing.Color.White;
            this.kryptonLabel3.TabIndex = 3;
            this.kryptonLabel3.Values.Text = "Password :";
            // 
            // fadeTimer
            // 
            this.fadeTimer.Tick += new System.EventHandler(this.fadeTimer_Tick);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // kryptonManager1
            // 
            this.kryptonManager1.GlobalPaletteMode = ComponentFactory.Krypton.Toolkit.PaletteModeManager.Office2007Blue;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox2.Image = global::Profit.Properties.Resources._lock;
            this.pictureBox2.Location = new System.Drawing.Point(540, 21);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(21, 22);
            this.pictureBox2.TabIndex = 8;
            this.pictureBox2.TabStop = false;
            // 
            // LoginForm
            // 
            this.AcceptButton = this.loginkryptonButton1;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(564, 372);
            this.Controls.Add(this.kryptonPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login Form";
            this.Load += new System.EventHandler(this.LoginForm_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LoginForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            this.kryptonPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel3;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel2;
        private ComponentFactory.Krypton.Toolkit.KryptonTextBox passwordkryptonTextBox2;
        private ComponentFactory.Krypton.Toolkit.KryptonTextBox userCodekryptonTextBox1;
        private ComponentFactory.Krypton.Toolkit.KryptonButton loginkryptonButton1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Timer fadeTimer;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel messageLabelkryptonLabel;
        private ComponentFactory.Krypton.Toolkit.KryptonManager kryptonManager1;
    }
}