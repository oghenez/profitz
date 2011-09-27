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
using System.Collections;

namespace Profit
{
    public partial class GeneralSetupForm : KryptonForm, IChildForm
    {
        GeneralSetup m_user = new GeneralSetup();
        GeneralSetupRepository r_gensetupRep = (GeneralSetupRepository)RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.GENERAL_SETUP_REPOSITORY);
        PeriodRepository r_period = (PeriodRepository)RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.PERIOD_REPOSITORY);
        ProcessTransactionRepository r_prtr = (ProcessTransactionRepository)RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.PROCESS_TRANSACTION_REPOSITORY);
        IMainForm m_mainForm;

        public GeneralSetupForm(IMainForm mainForm, string formName)
        {
            InitializeComponent();
            InitializeButtonClick();
            InitDataSource();
            this.MdiParent = (Form)mainForm;
            this.Name = formName;
            m_mainForm = mainForm;
            InitializeDataSource();
            loadData();
            setEditMode(EditMode.View);
            setEnableForm(false);
        }

        private void InitDataSource()
        {
            startEntryPeriodkryptonComboBox1.DataSource = r_period.GetAll();
        }

        private void InitializeDataSource()
        {
            InitColumn.Items.AddRange(Enum.GetNames(typeof(InitialAutoNumberSetup)));
            AutonumberColumn.Items.AddRange(Enum.GetNames(typeof(AutoNumberSetupType)));
        }
        private void InitializeButtonClick()
        {
            toolStripButtonSave.Click += new EventHandler(Save);
            toolStripButtonEdit.Click += new EventHandler(Edit);
            toolStripButtonDelete.Click += new EventHandler(Delete);
            toolStripButtonClear.Click += new EventHandler(Clear);
            toolStripButtonRefresh.Click+=new EventHandler(Refresh);
        }
        
        public void Save(object sender, EventArgs e)
        {
            try
            {
                if (Valid())
                {
                    this.Cursor = Cursors.WaitCursor;
                    UpdateEntity();
                    if (m_user.ID == 0)
                    {
                        r_gensetupRep.Save(m_user);
                    }
                    else
                    {
                        r_gensetupRep.Update(m_user);
                    }
                    KryptonMessageBox.Show("Record has been saved","Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //ClearForm();
                    //companyNameTextBox.Focus();
                    setEditMode(EditMode.View);
                    setEnableForm(false);
                    this.Cursor = Cursors.Default;
                }
            }
            catch (Exception x)
            {
                KryptonMessageBox.Show(x.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }


        public bool Valid()
        {
            bool a = companyNameTextBox.Text == "";
            bool b = addresstextBox.Text == "";
            

            if (a) errorProvider1.SetError(companyNameTextBox, "Code Can not Empty");
            if (b) errorProvider1.SetError(addresstextBox, "Name Can not Empty");

            return !a && !b;
        }
        private void UpdateEntity()
        {
            m_user.COMPANY_NAME = companyNameTextBox.Text.Trim();
            m_user.ADDRESS = addresstextBox.Text.Trim();
            m_user.EMAIL = emailKryptonTextBox.Text.Trim();
            m_user.FAX = faxKryptonTextBox.Text.Trim();
            m_user.PHONE = phoneKryptonTextBox.Text.Trim();
            m_user.REG_DATE = regDatekryptonDateTimePicker.Value;
            m_user.TAX_NO = taxNokryptonTextBox.Text.Trim();
            m_user.WEBSITE = websiteKryptonTextBox.Text.Trim();
            m_user.START_ENTRY_PERIOD = (Period)startEntryPeriodkryptonComboBox1.SelectedItem;
            m_user.MODIFIED_BY = m_mainForm.CurrentUser.NAME;
            m_user.MODIFIED_COMPUTER_NAME = Environment.MachineName;
            m_user.AUTONUMBER_LIST.Clear();
            for (int i = 0; i < gridAutonumber.Rows.Count; i++)
            {
                if(gridAutonumber[FormNameColumn.Index, i].Value==null)
                    continue;
                AutoNumberSetup c = (AutoNumberSetup)gridAutonumber.Rows[i].Tag;
                if (c == null)
                    c = new AutoNumberSetup();
                c.AUTONUMBER_SETUP_TYPE = (AutoNumberSetupType)Enum.Parse(typeof(AutoNumberSetupType), gridAutonumber[AutonumberColumn.Index, i].Value.ToString());
                c.DIGIT = Convert.ToInt32(gridAutonumber[DigitColumn.Index, i].Value);
                c.INITIAL_AUTO_NUMBER = (InitialAutoNumberSetup)Enum.Parse(typeof(InitialAutoNumberSetup), gridAutonumber[InitColumn.Index, i].Value.ToString());
                c.PREFIX = gridAutonumber[PrefixColumn.Index,i].Value.ToString();
                c.START = Convert.ToInt32(gridAutonumber[StartColumn.Index, i].Value);
                m_user.AUTONUMBER_LIST.Add(c.FORM_CODE, c);
            }
        }
        public void ClearForm()
        {
            try
            {
                companyNameTextBox.Text = "";
                addresstextBox.Text = "";
                gridAutonumber.Rows.Clear();
                m_user = new GeneralSetup();
                errorProvider1.Clear();
            }
            catch (Exception x)
            {
                KryptonMessageBox.Show(x.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        public void Edit(object sender, EventArgs e)
        {
            setEnableForm(true);
            setEditMode(EditMode.Update);
            companyNameTextBox.Focus();
        }
        public void setEnableForm(bool enable)
        {
            companyNameTextBox.ReadOnly = !enable;
            addresstextBox.ReadOnly = !enable;
            emailKryptonTextBox.ReadOnly = !enable;
            faxKryptonTextBox.ReadOnly = !enable;
            phoneKryptonTextBox.ReadOnly = !enable;
            regDatekryptonDateTimePicker.Enabled = enable;
            taxNokryptonTextBox.ReadOnly = !enable;
            websiteKryptonTextBox.ReadOnly = !enable;
            startEntryPeriodkryptonComboBox1.Enabled = enable;

            //FormNameColumn.ReadOnly = !enable;//186
            PrefixColumn.ReadOnly = !enable;//100
            StartColumn.ReadOnly = !enable;//35
            DigitColumn.ReadOnly = !enable;//38
            InitColumn.ReadOnly = !enable;//51
            AutonumberColumn.ReadOnly = !enable;//76
        }
        private void setEditMode(EditMode editmode)
        {
            toolStripButtonSave.Enabled = (editmode == EditMode.New || editmode == EditMode.Update) && m_mainForm.CurrentUser.FORM_ACCESS_LIST[Name].SAVE;
            toolStripButtonEdit.Enabled = (editmode == EditMode.View) && m_mainForm.CurrentUser.FORM_ACCESS_LIST[Name].SAVE;
            toolStripButtonDelete.Enabled = false; //(editmode == EditMode.View) && m_mainForm.CurrentUser.FORM_ACCESS_LIST[Name].DELETE;
            toolStripButtonClear.Enabled = false; // m_mainForm.CurrentUser.FORM_ACCESS_LIST[Name].SAVE;
            ReloadMainFormButton();
        }
        private void ReloadMainFormButton()
        {
            m_mainForm.EnableButtonSave(toolStripButtonSave.Enabled);
            m_mainForm.EnableButtonEdit(toolStripButtonEdit.Enabled);
            m_mainForm.EnableButtonDelete(toolStripButtonDelete.Enabled);
            m_mainForm.EnableButtonClear(toolStripButtonClear.Enabled);
        }
        public void Delete(object obj, EventArgs e)
        {
            try
            {
                if (m_user.ID > 0)
                {
                    //this.Cursor = Cursors.WaitCursor;
                    //if (KryptonMessageBox.Show("Are you sure to delete this record?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No) { this.Cursor = Cursors.Default; return; }
                    //r_gensetupRep.Delete(m_user);
                    //ClearForm();
                    //setEnableForm(true);
                    //setEditMode(EditMode.New);
                    //companyNameTextBox.Focus();
                    //this.Cursor = Cursors.Default;
                }

            }
            catch (Exception x)
            {
                KryptonMessageBox.Show(x.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
        public void Clear(object sender, EventArgs e)
        {
            //ClearForm();
            //setEnableForm(true);
            //setEditMode(EditMode.New);
            //companyNameTextBox.Focus();
        }
        private void loadData()
        {
            try
            {
                m_user = (GeneralSetup)r_gensetupRep.GetById(new GeneralSetup(1));
                companyNameTextBox.Text = m_user.COMPANY_NAME;
                addresstextBox.Text = m_user.ADDRESS;
                emailKryptonTextBox.Text = m_user.EMAIL;
                faxKryptonTextBox.Text = m_user.FAX;
                phoneKryptonTextBox.Text = m_user.PHONE;
                regDatekryptonDateTimePicker.Value = m_user.REG_DATE;
                taxNokryptonTextBox.Text = m_user.TAX_NO;
                websiteKryptonTextBox.Text = m_user.WEBSITE;
                startEntryPeriodkryptonComboBox1.Text = m_user.START_ENTRY_PERIOD.ToString();
                foreach (string kys in m_user.AUTONUMBER_LIST.Keys)
                {
                    AutoNumberSetup f = m_user.AUTONUMBER_LIST[kys];
                    int t = gridAutonumber.Rows.Add(
                        f.FORM_CODE, f.PREFIX, f.START, f.DIGIT, f.INITIAL_AUTO_NUMBER, f.AUTONUMBER_SETUP_TYPE
                        );
                    gridAutonumber.Rows[t].Tag = f;
                }
            }
            catch (Exception x)
            { 
            }
        }

        #region IChildForm Members


        public void Refresh(object sender, EventArgs e)
        {
            loadData();
        }

        public void Print(object sender, EventArgs e)
        {
            
        }

        #endregion

        private void BankForm_Activated(object sender, EventArgs e)
        {
            ReloadMainFormButton();
        }

        private void faxKryptonTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void startEntryPeriodkryptonComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void startEntryPeriodkryptonComboBox1_Validating(object sender, CancelEventArgs e)
        {
            Period per = (Period)startEntryPeriodkryptonComboBox1.SelectedItem;
            if (r_prtr.GetTotalTransactionCount() > 0)
            {
                MessageBox.Show("Can not change start entry month, some transaction has been process");
                startEntryPeriodkryptonComboBox1.Text = per.ToString();
            }

        }
    }
}
