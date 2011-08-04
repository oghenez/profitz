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
    public partial class SupplierForm : KryptonForm, IChildForm
    {
        Supplier m_supplier = new Supplier();
        IMainForm m_mainForm;

        public SupplierForm(IMainForm mainForm, string formName)
        {
            InitializeComponent();
            InitializeButtonClick();
            InitializeDataSource();
            this.MdiParent = (Form)mainForm;
            this.Name = formName;
            m_mainForm = mainForm;
            loadRecords();
        }

        private void InitializeDataSource()
        {
            currencykryptonComboBox4.DataSource = RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.CURRENCY_REPOSITORY).GetAll();
            suppliercatkryptonComboBox5.DataSource = RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.SUPPLIER_CATEGORY_REPOSITORY).GetAll();
            purchaserkryptonComboBox2.DataSource = ((EmployeeRepository)RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.EMPLOYEE_REPOSITORY)).GetAllPurchaser();
            pricecategorykryptonComboBox6.DataSource = RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.PRICE_CATEGORY_REPOSITORY).GetAll();
            taxkryptonComboBox3.DataSource = RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.TAX_REPOSITORY).GetAll();
            topkryptonComboBox1.DataSource = RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.TOP_REPOSITORY).GetAll();
        }
        private void InitializeButtonClick()
        {
            toolStripButtonSave.Click += new EventHandler(Save);
            toolStripButtonEdit.Click += new EventHandler(Edit);
            toolStripButtonDelete.Click += new EventHandler(Delete);
            toolStripButtonRefresh.Click += new EventHandler(Refresh);
            toolStripButtonClear.Click += new EventHandler(Clear);
        }
        private void loadRecords()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                gridData.Rows.Clear();
                IList records = RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.SUPPLIER_REPOSITORY).GetAll();
                foreach (Supplier d in records)
                {

                    d.CURRENCY = (Currency)RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.CURRENCY_REPOSITORY).GetById(d.CURRENCY);
                    d.SUPPLIER_CATEGORY = (SupplierCategory)RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.SUPPLIER_CATEGORY_REPOSITORY).GetById(d.SUPPLIER_CATEGORY);
                    d.EMPLOYEE = (Employee)RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.EMPLOYEE_REPOSITORY).GetById(d.EMPLOYEE);
                    d.PRICE_CATEGORY = (PriceCategory)RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.PRICE_CATEGORY_REPOSITORY).GetById(d.PRICE_CATEGORY);
                    d.TAX = (Tax)RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.TAX_REPOSITORY).GetById(d.TAX);
                    d.TERM_OF_PAYMENT = (TermOfPayment)RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.TOP_REPOSITORY).GetById(d.TERM_OF_PAYMENT);

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
                    if (m_supplier.ID == 0)
                    {
                        RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.SUPPLIER_REPOSITORY).Save(m_supplier);
                        Supplier bank = (Supplier)RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.SUPPLIER_REPOSITORY).GetByCode(m_supplier);
                        int r = gridData.Rows.Add(bank.CODE, bank.NAME);
                        gridData.Rows[r].Tag = bank;
                    }
                    else
                    {
                        RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.SUPPLIER_REPOSITORY).Update(m_supplier);
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
                Supplier dep = (Supplier)item.Tag;
                if (dep.ID == m_supplier.ID)
                {
                    gridData[0, item.Index].Value = m_supplier.CODE;
                    gridData[1, item.Index].Value = m_supplier.NAME;
                    break;
                }
            }
        }
        public bool Valid()
        {
            bool a = textBoxCode.Text == "";
            bool b = textBoxName.Text == "";
            if (a) errorProvider1.SetError(textBoxCode, "Code Can not Empty");
            if (b) errorProvider1.SetError(textBoxName, "Name Can not Empty");
            return !a && !b;
        }
        private void UpdateEntity()
        {
            m_supplier.CODE = textBoxCode.Text.Trim();
            m_supplier.NAME = textBoxName.Text.Trim();
            m_supplier.ACTIVE = activekryptonCheckBox1.Checked;
            m_supplier.ADDRESS = addresskryptonTextBox1.Text;
            m_supplier.CONTACT = contactkryptonTextBox3.Text;
            m_supplier.CREDIT_LIMIT = Convert.ToDouble(creditlimitkryptonNumericUpDown1.Value);
            m_supplier.CURRENCY = (Currency)currencykryptonComboBox4.SelectedItem;
            m_supplier.SUPPLIER_CATEGORY = (SupplierCategory)suppliercatkryptonComboBox5.SelectedItem;
            m_supplier.EMAIL = emailkryptonTextBox6.Text;
            m_supplier.EMPLOYEE = (Employee)purchaserkryptonComboBox2.SelectedItem;
            m_supplier.FAX = faxkryptonTextBox5.Text;
            m_supplier.PHONE = phonekryptonTextBox4.Text;
            m_supplier.PRICE_CATEGORY = (PriceCategory)pricecategorykryptonComboBox6.SelectedItem;
            m_supplier.TAX = (Tax)taxkryptonComboBox3.SelectedItem;
            m_supplier.TAX_NO = taxnokryptonTextBox8.Text;
            m_supplier.TERM_OF_PAYMENT = (TermOfPayment)topkryptonComboBox1.SelectedItem;
            m_supplier.WEBSITE = websitekryptonTextBox7.Text;
            m_supplier.ZIPCODE = zipcodekryptonTextBox2.Text;
        }
        public void ClearForm()
        {
            try
            {
                textBoxCode.Text = "";
                textBoxName.Text = "";
                activekryptonCheckBox1.Checked = false;
                addresskryptonTextBox1.Text = "";
                contactkryptonTextBox3.Text = "";
                creditlimitkryptonNumericUpDown1.Value = 0;
                currencykryptonComboBox4.SelectedIndex = 0;
                suppliercatkryptonComboBox5.SelectedIndex = 0;
                emailkryptonTextBox6.Text = "";
                purchaserkryptonComboBox2.SelectedIndex = 0;
                faxkryptonTextBox5.Text = "";
                phonekryptonTextBox4.Text = "";
                pricecategorykryptonComboBox6.SelectedIndex = 0;
                taxkryptonComboBox3.SelectedIndex = 0;
                taxnokryptonTextBox8.Text = "";
                topkryptonComboBox1.SelectedIndex = 0;
                websitekryptonTextBox7.Text = "";
                zipcodekryptonTextBox2.Text = "";
                m_supplier = new Supplier();
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
            textBoxName.ReadOnly = !enable;

            activekryptonCheckBox1.Enabled = enable;
            addresskryptonTextBox1.ReadOnly = !enable;
            contactkryptonTextBox3.ReadOnly = !enable;
            creditlimitkryptonNumericUpDown1.Enabled = enable;
            currencykryptonComboBox4.Enabled = enable;
            suppliercatkryptonComboBox5.Enabled = enable;
            emailkryptonTextBox6.ReadOnly = !enable;
            purchaserkryptonComboBox2.Enabled = enable;
            faxkryptonTextBox5.ReadOnly = !enable;
            phonekryptonTextBox4.ReadOnly = !enable;
            pricecategorykryptonComboBox6.Enabled = enable;
            taxkryptonComboBox3.Enabled = enable;
            taxnokryptonTextBox8.ReadOnly = !enable;
            topkryptonComboBox1.Enabled = enable;
            websitekryptonTextBox7.ReadOnly = !enable;
            zipcodekryptonTextBox2.ReadOnly = !enable;
        }
        private void setEditMode(EditMode editmode)
        {
            toolStripButtonSave.Enabled = (editmode == EditMode.New || editmode == EditMode.Update) && m_mainForm.CurrentUser.FORM_ACCESS_LIST[Name].SAVE;
            toolStripButtonEdit.Enabled = (editmode == EditMode.View) && m_mainForm.CurrentUser.FORM_ACCESS_LIST[Name].SAVE;
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
                if (m_supplier.ID > 0)
                {
                    this.Cursor = Cursors.WaitCursor;
                    if (KryptonMessageBox.Show("Are you sure to delete this record?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No) { this.Cursor = Cursors.Default; return; }
                    RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.SUPPLIER_REPOSITORY).Delete(m_supplier);
                    removeRecord(m_supplier.ID);
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
                Supplier dep = (Supplier)item.Tag;
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
            m_supplier = (Supplier)gridData.SelectedRows[0].Tag;
            if (m_supplier == null) return;
            loadData();
            setEnableForm(false);
            setEditMode(EditMode.View);
        }
        private void loadData()
        {
            textBoxCode.Text = m_supplier.CODE;
            textBoxName.Text = m_supplier.NAME;
            activekryptonCheckBox1.Checked = m_supplier.ACTIVE;
            addresskryptonTextBox1.Text = m_supplier.ADDRESS;
            contactkryptonTextBox3.Text = m_supplier.CONTACT;
            creditlimitkryptonNumericUpDown1.Value = Convert.ToDecimal(m_supplier.CREDIT_LIMIT);
            currencykryptonComboBox4.Text = m_supplier.CURRENCY.ToString();
            suppliercatkryptonComboBox5.Text = m_supplier.SUPPLIER_CATEGORY.ToString();
            emailkryptonTextBox6.Text = m_supplier.EMAIL;
            purchaserkryptonComboBox2.Text = m_supplier.EMPLOYEE.ToString();
            faxkryptonTextBox5.Text = m_supplier.FAX;
            phonekryptonTextBox4.Text = m_supplier.PHONE;
            pricecategorykryptonComboBox6.Text = m_supplier.PRICE_CATEGORY.ToString();
            taxkryptonComboBox3.Text = m_supplier.TAX.ToString();
            taxnokryptonTextBox8.Text = m_supplier.TAX_NO;
            topkryptonComboBox1.Text = m_supplier.TERM_OF_PAYMENT.ToString();
            websitekryptonTextBox7.Text = m_supplier.WEBSITE;
            zipcodekryptonTextBox2.Text = m_supplier.ZIPCODE;
        }

        #region IChildForm Members


        public void Refresh(object sender, EventArgs e)
        {
            loadRecords();
            InitializeDataSource();
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
    }
}
