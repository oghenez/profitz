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
    public partial class ExchangeRateForm : KryptonForm, IChildForm
    {
        ExchangeRate m_excRate = new ExchangeRate();
        IMainForm m_mainForm;

        public ExchangeRateForm(IMainForm mainForm, string formName)
        {
            InitializeComponent();
            InitializeButtonClick();
            InitializeComboDataSource();
            this.MdiParent = (Form)mainForm;
            this.Name = formName;
            m_mainForm = mainForm;
            loadRecords();
        }

        private void InitializeComboDataSource()
        {
            kryptonComboBoxCurrency.DataSource = RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.CURRENCY_REPOSITORY).GetAll();
        }
        private void InitializeButtonClick()
        {
            toolStripButtonSave.Click += new EventHandler(Save);
            toolStripButtonEdit.Click += new EventHandler(Edit);
            toolStripButtonDelete.Click += new EventHandler(Delete);
            toolStripButtonClear.Click += new EventHandler(Clear);
        }
        private void loadRecords()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                gridData.Rows.Clear();
                IList records = RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.EXCHANGE_RATE_REPOSITORY).GetAll();
                foreach (ExchangeRate d in records)
                {
                    d.CURRENCY = (Currency)RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.CURRENCY_REPOSITORY).GetById(d.CURRENCY);
                    int row = gridData.Rows.Add(d.CODE, d.CURRENCY.ToString(), d.START_DATE.ToString("dd-MM-yyyy"), d.END_DATE.ToString("dd-MM-yyyy"),d.RATE_TO_BASE);
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
                    if (m_excRate.ID == 0)
                    {
                        RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.EXCHANGE_RATE_REPOSITORY).Save(m_excRate);
                        ExchangeRate d = (ExchangeRate)RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.EXCHANGE_RATE_REPOSITORY).GetByCode(m_excRate);
                        int r = gridData.Rows.Add(d.CODE, d.CURRENCY.ToString(), d.START_DATE.ToString("dd-MM-yyyy"), d.END_DATE.ToString("dd-MM-yyyy"), d.RATE_TO_BASE);
                        gridData.Rows[r].Tag = d;
                    }
                    else
                    {
                        RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.EXCHANGE_RATE_REPOSITORY).Update(m_excRate);
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
                ExchangeRate dep = (ExchangeRate)item.Tag;
                if (dep.ID == m_excRate.ID)
                {
                    gridData[0, item.Index].Value = m_excRate.CODE;
                    gridData[1, item.Index].Value = m_excRate.CURRENCY.ToString();
                    gridData[2, item.Index].Value = m_excRate.START_DATE.ToString("dd-MM-yyyy");
                    gridData[3, item.Index].Value = m_excRate.END_DATE.ToString("dd-MM-yyyy");
                    gridData[4, item.Index].Value = m_excRate.RATE_TO_BASE;
                    break;
                }
            }
        }
        public bool Valid()
        {
            bool a = textBoxCode.Text == "";
            bool b = kryptonComboBoxCurrency.SelectedItem == null;
            if (a) errorProvider1.SetError(textBoxCode, "Code Can not Empty");
            if (b) errorProvider1.SetError(kryptonComboBoxCurrency, "Currency Can not Empty");
            return !a && !b;
        }
        private void UpdateEntity()
        {
            m_excRate.CODE = textBoxCode.Text.Trim();
            m_excRate.START_DATE = kryptonDateTimePickerStartDate.Value;
            m_excRate.END_DATE = kryptonDateTimePickerEndDate.Value;
            m_excRate.RATE_TO_BASE = Convert.ToDouble(kryptonNumericUpDownRAte.Value);
            m_excRate.CURRENCY = (Currency)kryptonComboBoxCurrency.SelectedItem;
        }
        public void ClearForm()
        {
            try
            {
                textBoxCode.Text = "";
                kryptonComboBoxCurrency.SelectedIndex = 0;
                kryptonDateTimePickerEndDate.Value = DateTime.Today;
                kryptonDateTimePickerStartDate.Value = DateTime.Today;
                kryptonNumericUpDownRAte.Value = 0;
                m_excRate = new ExchangeRate();
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
            textBoxCode.Focus();
        }
        public void setEnableForm(bool enable)
        {
            textBoxCode.ReadOnly = !enable;
            kryptonComboBoxCurrency.Enabled = enable;
            kryptonDateTimePickerEndDate.Enabled = enable;
            kryptonDateTimePickerStartDate.Enabled = enable;
            kryptonNumericUpDownRAte.Enabled = enable;
        }
        private void setEditMode(EditMode editmode)
        {
            toolStripButtonSave.Enabled = (editmode == EditMode.New || editmode == EditMode.Update);// && m_mainForm.CurrentUser.FORM_CCY_SAVE;
            toolStripButtonEdit.Enabled = (editmode == EditMode.View);//&& m_mainForm.CurrentUser.FORM_CCY_SAVE;
            toolStripButtonDelete.Enabled = (editmode == EditMode.View);//&& m_mainForm.CurrentUser.FORM_CCY_DELETE;
            toolStripButtonClear.Enabled = true;//m_mainForm.CurrentUser.FORM_CCY_SAVE;
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
                if (m_excRate.ID > 0)
                {
                    this.Cursor = Cursors.WaitCursor;
                    if (KryptonMessageBox.Show("Are you sure to delete this record?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No) { this.Cursor = Cursors.Default; return; }
                    RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.EXCHANGE_RATE_REPOSITORY).Delete(m_excRate);
                    removeRecord(m_excRate.ID);
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
                ExchangeRate dep = (ExchangeRate)item.Tag;
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
            m_excRate = (ExchangeRate)gridData.SelectedRows[0].Tag;
            if (m_excRate == null) return;
            loadData();
            setEnableForm(false);
            setEditMode(EditMode.View);
        }
        private void loadData()
        {
            textBoxCode.Text = m_excRate.CODE;
            kryptonDateTimePickerStartDate.Value = m_excRate.START_DATE ;
            kryptonDateTimePickerEndDate.Value = m_excRate.END_DATE;
            kryptonNumericUpDownRAte.Value = Convert.ToDecimal(m_excRate.RATE_TO_BASE);
            kryptonComboBoxCurrency.Text = m_excRate.CURRENCY.ToString();
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

        private void kryptonDateTimePicker2_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
