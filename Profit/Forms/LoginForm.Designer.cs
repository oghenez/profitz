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
            this.kryptonGroupBox1 = new ComponentFactory.Krypton.Toolkit.KryptonGroupBox();
            this.messageLabelkryptonLabel = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.userCodekryptonTextBox1 = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.loginkryptonButton1 = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.kryptonLabel2 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.passwordkryptonTextBox2 = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.kryptonLabel3 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel1 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.fadeTimer = new System.Windows.Forms.Timer(this.components);
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1.Panel)).BeginInit();
            this.kryptonGroupBox1.Panel.SuspendLayout();
            this.kryptonGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.kryptonGroupBox1);
            this.kryptonPanel1.Controls.Add(this.kryptonLabel1);
            this.kryptonPanel1.Controls.Add(this.pictureBox1);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(615, 249);
            this.kryptonPanel1.TabIndex = 1;
            // 
            // kryptonGroupBox1
            // 
            this.kryptonGroupBox1.Location = new System.Drawing.Point(327, 3);
            this.kryptonGroupBox1.Name = "kryptonGroupBox1";
            // 
            // kryptonGroupBox1.Panel
            // 
            this.kryptonGroupBox1.Panel.Controls.Add(this.messageLabelkryptonLabel);
            this.kryptonGroupBox1.Panel.Controls.Add(this.pictureBox2);
            this.kryptonGroupBox1.Panel.Controls.Add(this.userCodekryptonTextBox1);
            this.kryptonGroupBox1.Panel.Controls.Add(this.loginkryptonButton1);
            this.kryptonGroupBox1.Panel.Controls.Add(this.kryptonLabel2);
            this.kryptonGroupBox1.Panel.Controls.Add(this.passwordkryptonTextBox2);
            this.kryptonGroupBox1.Panel.Controls.Add(this.kryptonLabel3);
            this.kryptonGroupBox1.Size = new System.Drawing.Size(280, 117);
            this.kryptonGroupBox1.TabIndex = 7;
            this.kryptonGroupBox1.Text = "User Login";
            this.kryptonGroupBox1.Values.Heading = "User Login";
            // 
            // messageLabelkryptonLabel
            // 
            this.messageLabelkryptonLabel.Location = new System.Drawing.Point(3, 72);
            this.messageLabelkryptonLabel.Name = "messageLabelkryptonLabel";
            this.messageLabelkryptonLabel.Size = new System.Drawing.Size(6, 2);
            this.messageLabelkryptonLabel.TabIndex = 9;
            this.messageLabelkryptonLabel.Values.Text = "";
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox2.Image = global::Profit.Properties.Resources._lock;
            this.pictureBox2.Location = new System.Drawing.Point(3, 3);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(21, 22);
            this.pictureBox2.TabIndex = 8;
            this.pictureBox2.TabStop = false;
            // 
            // userCodekryptonTextBox1
            // 
            this.userCodekryptonTextBox1.Location = new System.Drawing.Point(108, 1);
            this.userCodekryptonTextBox1.Name = "userCodekryptonTextBox1";
            this.userCodekryptonTextBox1.Size = new System.Drawing.Size(141, 22);
            this.userCodekryptonTextBox1.TabIndex = 4;
            this.userCodekryptonTextBox1.Text = "ADMIN";
            // 
            // loginkryptonButton1
            // 
            this.loginkryptonButton1.Location = new System.Drawing.Point(181, 59);
            this.loginkryptonButton1.Name = "loginkryptonButton1";
            this.loginkryptonButton1.Size = new System.Drawing.Size(68, 25);
            this.loginkryptonButton1.TabIndex = 6;
            this.loginkryptonButton1.Values.Text = "LOGIN";
            this.loginkryptonButton1.Click += new System.EventHandler(this.loginkryptonButton1_Click);
            // 
            // kryptonLabel2
            // 
            this.kryptonLabel2.Location = new System.Drawing.Point(34, 2);
            this.kryptonLabel2.Name = "kryptonLabel2";
            this.kryptonLabel2.Size = new System.Drawing.Size(68, 19);
            this.kryptonLabel2.TabIndex = 2;
            this.kryptonLabel2.Values.Text = "User Code :";
            // 
            // passwordkryptonTextBox2
            // 
            this.passwordkryptonTextBox2.Location = new System.Drawing.Point(108, 29);
            this.passwordkryptonTextBox2.Name = "passwordkryptonTextBox2";
            this.passwordkryptonTextBox2.PasswordChar = '#';
            this.passwordkryptonTextBox2.Size = new System.Drawing.Size(141, 22);
            this.passwordkryptonTextBox2.TabIndex = 5;
            this.passwordkryptonTextBox2.Text = "admin1234";
            // 
            // kryptonLabel3
            // 
            this.kryptonLabel3.Location = new System.Drawing.Point(39, 31);
            this.kryptonLabel3.Name = "kryptonLabel3";
            this.kryptonLabel3.Size = new System.Drawing.Size(63, 19);
            this.kryptonLabel3.TabIndex = 3;
            this.kryptonLabel3.Values.Text = "Password :";
            // 
            // kryptonLabel1
            // 
            this.kryptonLabel1.Location = new System.Drawing.Point(3, 227);
            this.kryptonLabel1.Name = "kryptonLabel1";
            this.kryptonLabel1.Size = new System.Drawing.Size(199, 19);
            this.kryptonLabel1.TabIndex = 1;
            this.kryptonLabel1.Values.Text = "Copyright 2011 - dagado@gmail.com";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Profit.Properties.Resources.Logo;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(321, 222);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // fadeTimer
            // 
            this.fadeTimer.Tick += new System.EventHandler(this.fadeTimer_Tick);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(615, 249);
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
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1.Panel)).EndInit();
            this.kryptonGroupBox1.Panel.ResumeLayout(false);
            this.kryptonGroupBox1.Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1)).EndInit();
            this.kryptonGroupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel1;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel3;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel2;
        private ComponentFactory.Krypton.Toolkit.KryptonTextBox passwordkryptonTextBox2;
        private ComponentFactory.Krypton.Toolkit.KryptonTextBox userCodekryptonTextBox1;
        private ComponentFactory.Krypton.Toolkit.KryptonGroupBox kryptonGroupBox1;
        private ComponentFactory.Krypton.Toolkit.KryptonButton loginkryptonButton1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Timer fadeTimer;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel messageLabelkryptonLabel;
    }
}