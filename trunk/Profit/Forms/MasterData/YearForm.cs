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
    public partial class YearForm : KryptonForm, IChildForm
    {
        Year m_year = new Year();
        IMainForm m_mainForm;

        public YearForm(IMainForm mainForm, string formName)
        {
            InitializeComponent();
            InitializeButtonClick();
            this.MdiParent = (Form)mainForm;
            this.Name = formName;
            m_mainForm = mainForm;
            loadRecords();
        }
        private void InitializeButtonClick()
        {
            toolStripButtonSave.Click += new EventHandler(Save);
            toolStripButtonEdit.Click += new EventHandler(Edit);
            toolStripButtonDelete.Click += new EventHandler(Delete);
            toolStripButtonClear.Click += new EventHandler(Clear);
            toolStripButtonRefresh.Click+=new EventHandler(Refresh);
        }
        private void loadRecords()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                gridData.Rows.Clear();
                IList records = RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.YEAR_REPOSITORY).GetAll();
                foreach (Year d in records)
                {
                    int row = gridData.Rows.Add(d.CODE, d.NAME);
                    gridData.Rows[row].Tag = d;
                }
                this.Cursor = Cursors.Default;
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
        public void Save(object sender, EventArgs e)
        {
            try
            {
                if (Valid())
                {
                    this.Cursor = Cursors.WaitCursor;
                    UpdateEntity();
                    if (m_year.ID == 0)
                    {
                        RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.YEAR_REPOSITORY).Save(m_year);
                        Year bank = (Year)RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.YEAR_REPOSITORY).GetByCode(m_year);
                        int r = gridData.Rows.Add(bank.CODE, bank.NAME);
                        gridData.Rows[r].Tag = bank;
                    }
                    else
                    {
                        RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.YEAR_REPOSITORY).Update(m_year);
                        updateRecord();
                    }
                    KryptonMessageBox.Show("Record has been saved","Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    gridData.ClearSelection();
                    ClearForm();
                    textBoxCode.Focus();
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

        private void updateRecord()
        {
            foreach (DataGridViewRow item in gridData.Rows)
            {
                Year dep = (Year)item.Tag;
                if (dep.ID == m_year.ID)
                {
                    gridData[0, item.Index].Value = m_year.CODE;
                    gridData[1, item.Index].Value = m_year.NAME;
                    break;
                }
            }
        }
        public bool Valid()
        {
            bool a = textBoxCode.Text == "";
            bool b = textBoxName.Text == "";
            bool c = endDatekryptonDateTimePicker2.Value <= startDatekryptonDateTimePicker1.Value;
            if (a) errorProvider1.SetError(textBoxCode, "Code Can not Empty");
            if (b) errorProvider1.SetError(textBoxName, "Name Can not Empty");
            if (c) errorProvider1.SetError(endDatekryptonDateTimePicker2, "End Date can not be same or less than Start Date");

            return !a && !b && !c;
        }
        private void UpdateEntity()
        {
            m_year.CODE = textBoxCode.Text.Trim();
            m_year.NAME = textBoxName.Text.Trim();
            m_year.START_DATE = startDatekryptonDateTimePicker1.Value;
            m_year.END_DATE = endDatekryptonDateTimePicker2.Value;
            m_year.MODIFIED_BY = m_mainForm.CurrentUser.NAME;
            m_year.MODIFIED_COMPUTER_NAME = Environment.MachineName;
        }
        public void ClearForm()
        {
            try
            {
                textBoxCode.Text = "";
                textBoxName.Text = "";
                startDatekryptonDateTimePicker1.Value = 
                    new DateTime(DateTime.Today.Year,1,1);
                endDatekryptonDateTimePicker2.Value = startDatekryptonDateTimePicker1.Value.AddMonths(11);
                m_year = new Year();
                errorProvider1.Clear();
                periodskryptonDataGridView1.Rows.Clear();
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
            textBoxCode.Focus();
        }
        public void setEnableForm(bool enable)
        {
            textBoxCode.ReadOnly = !enable;
            textBoxName.ReadOnly = !enable;
            startDatekryptonDateTimePicker1.Enabled = enable;
            endDatekryptonDateTimePicker2.Enabled = enable;
        }
        private void setEditMode(EditMode editmode)
        {
            toolStripButtonSave.Enabled = (editmode == EditMode.New || editmode == EditMode.Update) && m_mainForm.CurrentUser.FORM_ACCESS_LIST[Name].SAVE;
            toolStripButtonEdit.Enabled = false;// (editmode == EditMode.View) && m_mainForm.CurrentUser.FORM_ACCESS_LIST[Name].SAVE;
            toolStripButtonDelete.Enabled = (editmode == EditMode.View) && m_mainForm.CurrentUser.FORM_ACCESS_LIST[Name].DELETE;
            toolStripButtonClear.Enabled = m_mainForm.CurrentUser.FORM_ACCESS_LIST[Name].SAVE;
            ReloadMainFormButton();
        }
        private void ReloadMainFormButton()
        {
            m_mainForm.EnableButtonSave(toolStripButtonSave.Enabled);
            m_mainForm.EnableButtonEdit(toolStripButtonEdit.Enabled);
            m_mainForm.EnableButtonDelete(toolStripButtonDelete.Enabled);
            m_mainForm.EnableButtonClear(true);
        }
        public void Delete(object obj, EventArgs e)
        {
            try
            {
                if (m_year.ID > 0)
                {
                    this.Cursor = Cursors.WaitCursor;
                    if (KryptonMessageBox.Show("Are you sure to delete this record?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No) { this.Cursor = Cursors.Default; return; }
                    RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.YEAR_REPOSITORY).Delete(m_year);
                    removeRecord(m_year.ID);
                    ClearForm();
                    setEnableForm(true);
                    setEditMode(EditMode.New);
                    textBoxCode.Focus();
                    this.Cursor = Cursors.Default;
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
        private void removeRecord(int id)
        {
            foreach (DataGridViewRow item in gridData.Rows)
            {
                Year dep = (Year)item.Tag;
                if (dep.ID == id)
                {
                    gridData.Rows.Remove(item);
                    break;
                }
            }
            gridData.ClearSelection();
        }
        public void Clear(object sender, EventArgs e)
        {
            gridData.ClearSelection();
            ClearForm();
            setEnableForm(true);
            setEditMode(EditMode.New);
            textBoxCode.Focus();
        }

        private void gridData_SelectionChanged(object sender, EventArgs e)
        {
            if (gridData.SelectedRows.Count == 0) return;
            ClearForm();
            m_year = (Year)gridData.SelectedRows[0].Tag;
            if (m_year == null) return;
            loadData();
            setEnableForm(false);
            setEditMode(EditMode.View);
        }
        private void loadData()
        {
            textBoxCode.Text = m_year.CODE;
            textBoxName.Text = m_year.NAME;
            startDatekryptonDateTimePicker1.Value = m_year.START_DATE;
            endDatekryptonDateTimePicker2.Value = m_year.END_DATE;
            
            periodskryptonDataGridView1.Rows.Clear();
            m_year = (Year)((YearRepository)RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.YEAR_REPOSITORY)).GetPeriodByYearID(m_year);
            foreach (Period p in m_year.PERIODS)
            {
                periodskryptonDataGridView1.Rows.Add(p.CODE, p.START_DATE, p.END_DATA, p.PERIOD_STATUS.ToString());
            }
        }

        #region IChildForm Members


        public void Refresh(object sender, EventArgs e)
        {
            loadRecords(); 
            gridData.ClearSelection(); 
        }

        public void Print(object sender, EventArgs e)
        {
            
        }

        #endregion

        private void BankForm_Activated(object sender, EventArgs e)
        {
            ReloadMainFormButton();
        }

        private void exittoolStripButton1_Click(object sender, EventArgs e)
        {
            if (KryptonMessageBox.Show("Are you sure to Exit this Form?", "Exit Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                this.Close();
        }
    }
}
