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
    public partial class SupplierTransactionSummaryForm : KryptonForm, IChildForm
    {
        IMainForm m_mainForm;
        SupplierRepository r_sup = (SupplierRepository)RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.SUPPLIER_REPOSITORY);
        Repository r_emp = RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.EMPLOYEE_REPOSITORY);
        Repository r_top = RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.TOP_REPOSITORY);
        Repository r_ccy = RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.CURRENCY_REPOSITORY);

        ArrayList l_supplier = new ArrayList();

        public SupplierTransactionSummaryForm(IMainForm mainForm, string formName)
        {
            InitializeComponent();
            InitialDataSource();
            this.MdiParent = (Form)mainForm;
            this.Name = formName;
            m_mainForm = mainForm;
        }

        private void InitialDataSource()
        {
            l_supplier.Add(new Supplier(0, "ALL"));
            l_supplier.AddRange(r_sup.GetAllActive());

            IList status = new ArrayList();
            status.Add("ALL");
            status.Add("TRUE");
            status.Add("FALSE");
            

            supplierkryptonComboBox.DataSource = l_supplier;
            trtypekryptonComboBox1.DataSource = Enum.GetValues(typeof(StockCardEntryTypeSupplier));
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
            Supplier s = (Supplier)supplierkryptonComboBox.SelectedItem;
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
                        Supplier sup = (Supplier)r_sup.GetById((Supplier)t.VENDOR);
                        Employee emp = (Employee)r_emp.GetById(t.EMPLOYEE);
                        transactionkryptonDataGridView[supplierColumn.Index, r].Value = sup.NAME;
                        transactionkryptonDataGridView[supCodeColumn.Index, r].Value = sup.CODE;
                        transactionkryptonDataGridView[supAddressColumn.Index, r].Value = sup.ADDRESS;
                        transactionkryptonDataGridView[employeeColumn.Index, r].Value = emp.CODE;
                        if (t is PurchaseOrder)
                        {
                            PurchaseOrder p = (PurchaseOrder)t;
                            p.TOP = (TermOfPayment)r_top.GetById(p.TOP);
                            p.CURRENCY = (Currency)r_ccy.GetById(p.CURRENCY);
                            transactionkryptonDataGridView[topColumn.Index, r].Value = p.TOP.CODE;
                            transactionkryptonDataGridView[amountColumn.Index, r].Value = p.NET_TOTAL;
                            transactionkryptonDataGridView[ccyColumn.Index, r].Value = p.CURRENCY.CODE;
                        }
                        if (t is SupplierInvoice)
                        {
                            SupplierInvoice p = (SupplierInvoice)t;
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
                        Supplier sup = (Supplier)r_sup.GetById((Supplier)t.VENDOR);
                        Employee emp = (Employee)r_emp.GetById(t.EMPLOYEE);
                        transactionkryptonDataGridView[supplierColumn.Index, r].Value = sup.NAME;
                        transactionkryptonDataGridView[supCodeColumn.Index, r].Value = sup.CODE;
                        transactionkryptonDataGridView[supAddressColumn.Index, r].Value = sup.ADDRESS;
                        transactionkryptonDataGridView[employeeColumn.Index, r].Value = emp.CODE;
                        if (t is Payment)
                        {
                            Payment p = (Payment)t;
                            p.CURRENCY = (Currency)r_ccy.GetById(p.CURRENCY);
                            transactionkryptonDataGridView[amountColumn.Index, r].Value = p.NET_AMOUNT;
                            transactionkryptonDataGridView[ccyColumn.Index, r].Value = p.CURRENCY.CODE;
                        }
                        if (t is SupplierOutStandingInvoice)
                        {
                            SupplierOutStandingInvoice p = (SupplierOutStandingInvoice)t;
                            p.CURRENCY = (Currency)r_ccy.GetById(p.CURRENCY);
                            transactionkryptonDataGridView[amountColumn.Index, r].Value = p.NET_AMOUNT;
                            transactionkryptonDataGridView[ccyColumn.Index, r].Value = p.CURRENCY.CODE;
                        }
                        if (t is APDebitNote)
                        {
                            APDebitNote p = (APDebitNote)t;
                            p.CURRENCY = (Currency)r_ccy.GetById(p.CURRENCY);
                            transactionkryptonDataGridView[amountColumn.Index, r].Value = p.NET_AMOUNT;
                            transactionkryptonDataGridView[ccyColumn.Index, r].Value = p.CURRENCY.CODE;
                        }
                    }
                }
            }
        }

        private void SupplierTransactionSummaryForm_Load(object sender, EventArgs e)
        {
            UserSetting.LoadSetting(transactionkryptonDataGridView, m_mainForm.CurrentUser.ID, this.Name);
        }

        private void SupplierTransactionSummaryForm_FormClosing(object sender, FormClosingEventArgs e)
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

        private void SupplierTransactionSummaryForm_Activated(object sender, EventArgs e)
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
            Supplier em = (Supplier)supplierkryptonComboBox.SelectedItem;
            supplierKryptonTextBox.Text = em == null ? "" : em.NAME;
        }
    }
}
