using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using Profit.Server;

namespace Profit
{
    public partial class LoginForm : KryptonForm
    {
        bool m_showing = true;
        MainForm mainForm;
        UserRepository r_user = (UserRepository)RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.USER_REPOSITORY);
        PeriodRepository r_period = (PeriodRepository)RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.PERIOD_REPOSITORY);
        User m_currentUser = null;
        Period m_currentPeriod = null;


        public LoginForm()
        {
            InitializeComponent();
            this.Opacity = 0.0;
            fadeTimer.Start();
            mainForm = new MainForm(this);
            kryptonManager1.GlobalPaletteMode = PaletteModeManager.Office2010Blue;
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            fadeTimer.Start();
        }

        private void fadeTimer_Tick(object sender, EventArgs e)
        {
            if (m_showing)
            {
                double d = 1000.0 / fadeTimer.Interval / 100.0;
                if (Opacity + d >= 1.0)
                {
                    Opacity = 1.0;
                    fadeTimer.Stop();
                }
                else
                {
                    Opacity += d;
                }
            }
            else
            {
                double d = 1000.0 / fadeTimer.Interval / 100.0;
                if (Opacity - d <= 0.0)
                {
                    Opacity = 0.0;
                    fadeTimer.Stop();
                }
                else
                {
                    Opacity -= d;
                }
            }
        }

        private void LoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_showing = false;
            fadeTimer.Start();
        }

        private void loginkryptonButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if (isValid())
                {
                    this.Cursor = Cursors.WaitCursor;
                    errorProvider1.Clear();
                    m_currentUser = (User)r_user.GetUser(userCodekryptonTextBox1.Text.Trim(), passwordkryptonTextBox2.Text.Trim());
                    if (m_currentUser == null)
                        throw new Exception("Invelid User");
                    m_currentPeriod = r_period.FindCurrentPeriod();
                    mainForm.CurrentUser = m_currentUser;
                    mainForm.CurrentPeriod = m_currentPeriod;
                    this.Cursor = Cursors.Default;
                    this.Hide();
                    mainForm.Show();
                }

            }
            catch (Exception x)
            {
                messageLabelkryptonLabel.Text = x.Message;
                this.Cursor = Cursors.Default;

            }
        }
        private bool isValid()
        {
            bool valida = !userCodekryptonTextBox1.Text.Trim().Equals(string.Empty);
            bool validb = !passwordkryptonTextBox2.Text.Trim().Equals(string.Empty);
            if (!valida)
                errorProvider1.SetError(userCodekryptonTextBox1, "Fill User ID");
            if (!validb)
                errorProvider1.SetError(passwordkryptonTextBox2, "Fill Password");
            return valida && validb;
        }
    }
}
