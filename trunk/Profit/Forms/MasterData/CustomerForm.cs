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
    public partial class CustomerForm : KryptonForm, IChildForm
    {
        Customer m_customer = new Customer();
        IMainForm m_mainForm;

        public CustomerForm(IMainForm mainForm, string formName)
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
            customercatkryptonComboBox5.DataSource = RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.CUSTOMER_CATEGORY_REPOSITORY).GetAll();
            purchaserkryptonComboBox2.DataSource = ((EmployeeRepository)RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.EMPLOYEE_REPOSITORY)).GetAllSalesman();
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
                IList records = RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.CUSTOMER_REPOSITORY).GetAll();
                foreach (Customer d in records)
                {

                    d.CURRENCY = (Currency)RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.CURRENCY_REPOSITORY).GetById(d.CURRENCY);
                    d.CUSTOMER_CATEGORY = (CustomerCategory)RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.CUSTOMER_CATEGORY_REPOSITORY).GetById(d.CUSTOMER_CATEGORY);
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
                    if (m_customer.ID == 0)
                    {
                        RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.CUSTOMER_REPOSITORY).Save(m_customer);
                        Customer bank = (Customer)RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.CUSTOMER_REPOSITORY).GetByCode(m_customer);
                        int r = gridData.Rows.Add(bank.CODE, bank.NAME);
                        gridData.Rows[r].Tag = bank;
                    }
                    else
                    {
                        RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.CUSTOMER_REPOSITORY).Update(m_customer);
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
                Customer dep = (Customer)item.Tag;
                if (dep.ID == m_customer.ID)
                {
                    gridData[0, item.Index].Value = m_customer.CODE;
                    gridData[1, item.Index].Value = m_customer.NAME;
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
            m_customer.CODE = textBoxCode.Text.Trim();
            m_customer.NAME = textBoxName.Text.Trim();
            m_customer.ACTIVE = activekryptonCheckBox1.Checked;
            m_customer.ADDRESS = addresskryptonTextBox1.Text;
            m_customer.CONTACT = contactkryptonTextBox3.Text;
            m_customer.CREDIT_LIMIT = Convert.ToDouble(creditlimitkryptonNumericUpDown1.Value);
            m_customer.CURRENCY = (Currency)currencykryptonComboBox4.SelectedItem;
            m_customer.CUSTOMER_CATEGORY = (CustomerCategory)customercatkryptonComboBox5.SelectedItem;
            m_customer.EMAIL = emailkryptonTextBox6.Text;
            m_customer.EMPLOYEE = (Employee)purchaserkryptonComboBox2.SelectedItem;
            m_customer.FAX = faxkryptonTextBox5.Text;
            m_customer.PHONE = phonekryptonTextBox4.Text;
            m_customer.PRICE_CATEGORY = (PriceCategory)pricecategorykryptonComboBox6.SelectedItem;
            m_customer.TAX = (Tax)taxkryptonComboBox3.SelectedItem;
            m_customer.TAX_NO = taxnokryptonTextBox8.Text;
            m_customer.TERM_OF_PAYMENT = (TermOfPayment)topkryptonComboBox1.SelectedItem;
            m_customer.WEBSITE = websitekryptonTextBox7.Text;
            m_customer.ZIPCODE = zipcodekryptonTextBox2.Text;
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
                customercatkryptonComboBox5.SelectedIndex = 0;
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
                m_customer = new Customer();
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
            customercatkryptonComboBox5.Enabled = enable;
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
                if (m_customer.ID > 0)
                {
                    this.Cursor = Cursors.WaitCursor;
                    if (KryptonMessageBox.Show("Are you sure to delete this record?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No) { this.Cursor = Cursors.Default; return; }
                    RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.CUSTOMER_REPOSITORY).Delete(m_customer);
                    removeRecord(m_customer.ID);
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
                Customer dep = (Customer)item.Tag;
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
            m_customer = (Customer)gridData.SelectedRows[0].Tag;
            if (m_customer == null) return;
            loadData();
            setEnableForm(false);
            setEditMode(EditMode.View);
        }
        private void loadData()
        {
            textBoxCode.Text = m_customer.CODE;
            textBoxName.Text = m_customer.NAME;
            activekryptonCheckBox1.Checked = m_customer.ACTIVE;
            addresskryptonTextBox1.Text = m_customer.ADDRESS;
            contactkryptonTextBox3.Text = m_customer.CONTACT;
            creditlimitkryptonNumericUpDown1.Value = Convert.ToDecimal(m_customer.CREDIT_LIMIT);
            currencykryptonComboBox4.Text = m_customer.CURRENCY.ToString();
            customercatkryptonComboBox5.Text = m_customer.CUSTOMER_CATEGORY.ToString();
            emailkryptonTextBox6.Text = m_customer.EMAIL;
            purchaserkryptonComboBox2.Text = m_customer.EMPLOYEE.ToString();
            faxkryptonTextBox5.Text = m_customer.FAX;
            phonekryptonTextBox4.Text = m_customer.PHONE;
            pricecategorykryptonComboBox6.Text = m_customer.PRICE_CATEGORY.ToString();
            taxkryptonComboBox3.Text = m_customer.TAX.ToString();
            taxnokryptonTextBox8.Text = m_customer.TAX_NO;
            topkryptonComboBox1.Text = m_customer.TERM_OF_PAYMENT.ToString();
            websitekryptonTextBox7.Text = m_customer.WEBSITE;
            zipcodekryptonTextBox2.Text = m_customer.ZIPCODE;
        }

        #region IChildForm Members


        public void Refresh(object sender, EventArgs e)
        {
            loadRecords();
            //object a = currencykryptonComboBox4.SelectedItem;
            //object b = customercatkryptonComboBox5.SelectedItem;
            //object c = purchaserkryptonComboBox2.SelectedItem;
            //object d = pricecategorykryptonComboBox6.SelectedItem;
            //object f = taxkryptonComboBox3.SelectedItem;
            //object g = topkryptonComboBox1.SelectedItem;
            InitializeDataSource();
            //currencykryptonComboBox4.Text = a.ToString();
            //customercatkryptonComboBox5.Text = b.ToString();
            //purchaserkryptonComboBox2.Text = c.ToString();
            //pricecategorykryptonComboBox6.Text = d.ToString();
            //taxkryptonComboBox3.Text = f.ToString();
            //topkryptonComboBox1.Text = g.ToString();
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
