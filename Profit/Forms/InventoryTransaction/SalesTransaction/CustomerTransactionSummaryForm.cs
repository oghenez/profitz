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
    public partial class CustomerTransactionSummaryForm : KryptonForm, IChildForm
    {
        IMainForm m_mainForm;
        CustomerRepository r_sup = (CustomerRepository)RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.CUSTOMER_REPOSITORY);
        Repository r_emp = RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.EMPLOYEE_REPOSITORY);
        Repository r_top = RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.TOP_REPOSITORY);
        Repository r_ccy = RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.CURRENCY_REPOSITORY);

        ArrayList l_supplier = new ArrayList();

        public CustomerTransactionSummaryForm(IMainForm mainForm, string formName)
        {
            InitializeComponent();
            InitialDataSource();
            this.MdiParent = (Form)mainForm;
            this.Name = formName;
            m_mainForm = mainForm;
        }

        private void InitialDataSource()
        {
            l_supplier.Add(new Customer(0, "ALL"));
            l_supplier.AddRange(r_sup.GetAllActive());

            IList status = new ArrayList();
            status.Add("ALL");
            status.Add("TRUE");
            status.Add("FALSE");
            

            supplierkryptonComboBox.DataSource = l_supplier;
            trtypekryptonComboBox1.DataSource = Enum.GetValues(typeof(StockCardEntryTypeCustomer));
            statuskryptonComboBox2.DataSource = status;
        }

        private void kryptonComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void toolStripButtonSave_Click(object sender, EventArgs e)
        {

        }

        private void runReporttoolStripButton_Click(object sender, EventArgs e)
        {
            Customer s = (Customer)supplierkryptonComboBox.SelectedItem;
            bool allStatus = statuskryptonComboBox2.Text == "ALL";
            bool status = true;
            if(!allStatus)
            {
                status = Boolean.Parse(statuskryptonComboBox2.Text);
            }
            string type = trtypekryptonComboBox1.Text;

            IList trs = r_sup.GetAllTransactions(s.ID, startdateKryptonDateTimePicker.Value, enDatekryptonDateTimePicker1.Value,
                allStatus, status);
            transactionkryptonDataGridView.Rows.Clear();
            if (trs.Count > 0)
            {
                foreach (object ev in trs)
                {
                    if (ev is Event)
                    {
                        Event t = (Event)ev;
                        if (type != "ALL")
                        {
                            if (t.STOCK_CARD_ENTRY_TYPE.ToString() != type)
                                continue;
                        }
                        int r = transactionkryptonDataGridView.Rows.Add();
                        transactionkryptonDataGridView[datetrColumn.Index, r].Value = t.TRANSACTION_DATE;
                        transactionkryptonDataGridView[typeTrColumn.Index, r].Value = t.STOCK_CARD_ENTRY_TYPE.ToString();
                        transactionkryptonDataGridView[codeTrColumn.Index, r].Value = t.CODE;
                        transactionkryptonDataGridView[postedColumn.Index, r].Value = t.POSTED.ToString();
                        Customer sup = (Customer)r_sup.GetById((Customer)t.VENDOR);
                        Employee emp = (Employee)r_emp.GetById(t.EMPLOYEE);
                        transactionkryptonDataGridView[supplierColumn.Index, r].Value = sup.NAME;
                        transactionkryptonDataGridView[supCodeColumn.Index, r].Value = sup.CODE;
                        transactionkryptonDataGridView[supAddressColumn.Index, r].Value = sup.ADDRESS;
                        transactionkryptonDataGridView[employeeColumn.Index, r].Value = emp.CODE;
                        if (t is SalesOrder)
                        {
                            SalesOrder p = (SalesOrder)t;
                            p.TOP = (TermOfPayment)r_top.GetById(p.TOP);
                            p.CURRENCY = (Currency)r_ccy.GetById(p.CURRENCY);
                            transactionkryptonDataGridView[topColumn.Index, r].Value = p.TOP.CODE;
                            transactionkryptonDataGridView[amountColumn.Index, r].Value = p.NET_TOTAL;
                            transactionkryptonDataGridView[ccyColumn.Index, r].Value = p.CURRENCY.CODE;
                        }
                        if (t is CustomerInvoice)
                        {
                            CustomerInvoice p = (CustomerInvoice)t;
                            p.TOP = (TermOfPayment)r_top.GetById(p.TOP);
                            p.CURRENCY = (Currency)r_ccy.GetById(p.CURRENCY);
                            transactionkryptonDataGridView[topColumn.Index, r].Value = p.TOP.CODE;
                            transactionkryptonDataGridView[amountColumn.Index, r].Value = p.NET_TOTAL;
                            transactionkryptonDataGridView[ccyColumn.Index, r].Value = p.CURRENCY.CODE;
                        }
                        if (t is POS)
                        {
                            POS p = (POS)t;
                            p.TOP = (TermOfPayment)r_top.GetById(p.TOP);
                            p.CURRENCY = (Currency)r_ccy.GetById(p.CURRENCY);
                            transactionkryptonDataGridView[topColumn.Index, r].Value = p.TOP.CODE;
                            transactionkryptonDataGridView[amountColumn.Index, r].Value = p.NET_TOTAL;
                            transactionkryptonDataGridView[ccyColumn.Index, r].Value = p.CURRENCY.CODE;
                        }
                    }
                    if (ev is EventJournal)
                    {
                        EventJournal t = (EventJournal)ev;
                        if (type != "ALL")
                        {
                            if (t.VENDOR_BALANCE_ENTRY_TYPE.ToString() != type)
                                continue;
                        }
                        int r = transactionkryptonDataGridView.Rows.Add();
                        transactionkryptonDataGridView[datetrColumn.Index, r].Value = t.TRANSACTION_DATE;
                        transactionkryptonDataGridView[typeTrColumn.Index, r].Value = t.VENDOR_BALANCE_ENTRY_TYPE.ToString();
                        transactionkryptonDataGridView[codeTrColumn.Index, r].Value = t.CODE;
                        transactionkryptonDataGridView[postedColumn.Index, r].Value = t.POSTED.ToString();
                        Customer sup = (Customer)r_sup.GetById((Customer)t.VENDOR);
                        Employee emp = (Employee)r_emp.GetById(t.EMPLOYEE);
                        transactionkryptonDataGridView[supplierColumn.Index, r].Value = sup.NAME;
                        transactionkryptonDataGridView[supCodeColumn.Index, r].Value = sup.CODE;
                        transactionkryptonDataGridView[supAddressColumn.Index, r].Value = sup.ADDRESS;
                        transactionkryptonDataGridView[employeeColumn.Index, r].Value = emp.CODE;
                        if (t is Receipt)
                        {
                            Receipt p = (Receipt)t;
                            p.CURRENCY = (Currency)r_ccy.GetById(p.CURRENCY);
                            transactionkryptonDataGridView[amountColumn.Index, r].Value = p.NET_AMOUNT;
                            transactionkryptonDataGridView[ccyColumn.Index, r].Value = p.CURRENCY.CODE;
                        }
                        if (t is CustomerOutStandingInvoice)
                        {
                            CustomerOutStandingInvoice p = (CustomerOutStandingInvoice)t;
                            p.CURRENCY = (Currency)r_ccy.GetById(p.CURRENCY);
                            transactionkryptonDataGridView[amountColumn.Index, r].Value = p.NET_AMOUNT;
                            transactionkryptonDataGridView[ccyColumn.Index, r].Value = p.CURRENCY.CODE;
                        }
                        if (t is ARCreditNote)
                        {
                            ARCreditNote p = (ARCreditNote)t;
                            p.CURRENCY = (Currency)r_ccy.GetById(p.CURRENCY);
                            transactionkryptonDataGridView[amountColumn.Index, r].Value = p.NET_AMOUNT;
                            transactionkryptonDataGridView[ccyColumn.Index, r].Value = p.CURRENCY.CODE;
                        }
                    }
                }
            }
        }

        private void CustomerTransactionSummaryForm_Load(object sender, EventArgs e)
        {
            UserSetting.LoadSetting(transactionkryptonDataGridView, m_mainForm.CurrentUser.ID, this.Name);
        }

        private void CustomerTransactionSummaryForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            UserSetting.SaveSetting(transactionkryptonDataGridView, m_mainForm.CurrentUser.ID, this.Name);
        }

        private void toolStripButtonClear_Click(object sender, EventArgs e)
        {
            //InitialDataSource();
            transactionkryptonDataGridView.Rows.Clear();
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
            toolStripButtonClear_Click(null, null);
        }

        public void Refresh(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public void Print(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion

        private void CustomerTransactionSummaryForm_Activated(object sender, EventArgs e)
        {
            m_mainForm.EnableButtonSave(toolStripButtonSave.Enabled);
            m_mainForm.EnableButtonEdit(toolStripButtonEdit.Enabled);
            m_mainForm.EnableButtonDelete(toolStripButtonDelete.Enabled);
            m_mainForm.EnableButtonRefresh(toolStripButtonRefresh.Enabled);
            m_mainForm.EnableButtonClear(true);
        }

        private void transactionkryptonDataGridView_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            UserSetting.AddNumberToGrid(transactionkryptonDataGridView);
        }

        private void transactionkryptonDataGridView_Sorted(object sender, EventArgs e)
        {
            UserSetting.AddNumberToGrid(transactionkryptonDataGridView);
        }

        private void supplierkryptonComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Customer em = (Customer)supplierkryptonComboBox.SelectedItem;
            supplierKryptonTextBox.Text = em == null ? "" : em.NAME;
        }
    }
}
