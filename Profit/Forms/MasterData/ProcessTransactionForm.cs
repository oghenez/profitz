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
    public partial class ProcessTransactionForm : KryptonForm, IChildForm
    {
        Period m_period;
        IMainForm m_mainForm;
        ProcessTransactionRepository r_prtr = (ProcessTransactionRepository)RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.PROCESS_TRANSACTION_REPOSITORY);
        PeriodRepository r_period = (PeriodRepository)RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.PERIOD_REPOSITORY);

        public ProcessTransactionForm(IMainForm mainForm, string formName)
        {
            InitializeComponent();
            InitializeButtonClick();
            this.MdiParent = (Form)mainForm;
            this.Name = formName;
            m_mainForm = mainForm;
            setEditMode(EditMode.New);
            kryptonDateTimePicker1.Value = m_mainForm.CurrentPeriod.START_DATE;
        }
        private void InitializeButtonClick()
        {
            //toolStripButtonSave.Click += new EventHandler(Save);
            //toolStripButtonEdit.Click += new EventHandler(Edit);
           // toolStripButtonDelete.Click += new EventHandler(Delete);
            //toolStripButtonClear.Click += new EventHandler(Clear);
            //toolStripButtonRefresh.Click+=new EventHandler(Refresh);
        }
    
        private void setEditMode(EditMode editmode)
        {
            toolStripButtonSave.Enabled = false;//(editmode == EditMode.New || editmode == EditMode.Update) && m_mainForm.CurrentUser.FORM_ACCESS_LIST[Name].SAVE;
            toolStripButtonEdit.Enabled = false;//(editmode == EditMode.View) && m_mainForm.CurrentUser.FORM_ACCESS_LIST[Name].SAVE;
            toolStripButtonDelete.Enabled = false; //(editmode == EditMode.View) && m_mainForm.CurrentUser.FORM_ACCESS_LIST[Name].DELETE;
            toolStripButtonClear.Enabled = false;// m_mainForm.CurrentUser.FORM_ACCESS_LIST[Name].SAVE;
            ReloadMainFormButton();
        }
        private void ReloadMainFormButton()
        {
            m_mainForm.EnableButtonSave(false);
            m_mainForm.EnableButtonEdit(false);
            m_mainForm.EnableButtonDelete(false);
            m_mainForm.EnableButtonClear(true);
        }

     


     
        #region IChildForm Members


        public void Refresh(object sender, EventArgs e)
        {

        }

        public void Print(object sender, EventArgs e)
        {
            
        }

        #endregion

        private void BankForm_Activated(object sender, EventArgs e)
        {
            ReloadMainFormButton();
        }

        #region IChildForm Members

        public void Save(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public void Edit(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public void Delete(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public void Clear(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion

        private void kryptonButtonRollBack_Click(object sender, EventArgs e)
        {
            try
            {
                kryptonTextBox1.Text = "";
                if (KryptonMessageBox.Show("Are Sure Want To Rollback The Current Month?", "Transaction Process", MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.Yes))
                {
                    r_prtr.RollBackTransaction(m_mainForm.CurrentPeriod.ID);
                    m_mainForm.CurrentPeriod = r_period.FindCurrentPeriod();
                    kryptonDateTimePicker1.Value = m_mainForm.CurrentPeriod.START_DATE;
                    kryptonTextBox1.Text = "Rollback Succeeded";
                }

            }
            catch (Exception x)
            {
                kryptonTextBox1.Text = x.Message;
            }
        }

        private void processNexkryptonButton_Click(object sender, EventArgs e)
        {
            try
            {
                kryptonTextBox1.Text = "";
                if (KryptonMessageBox.Show("Are Sure Want To Process The Current Month?", "Transaction Process", MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.Yes))
                {
                    r_prtr.ProcessTransaction(m_mainForm.CurrentPeriod.ID);
                    m_mainForm.CurrentPeriod = r_period.FindCurrentPeriod();
                    kryptonDateTimePicker1.Value = m_mainForm.CurrentPeriod.START_DATE;
                    kryptonTextBox1.Text = "Process Succeeded";

                }

            }
            catch (Exception x)
            {
                kryptonTextBox1.Text = x.Message;
            }
        }
    }
}
