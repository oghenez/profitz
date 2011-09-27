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
    public partial class ReceiptForm : KryptonForm, IChildForm
    {
        Receipt m_prn = new Receipt(); 
        IMainForm m_mainForm;
        Repository r_top = RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.TOP_REPOSITORY);
        Repository r_bank = RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.BANK_REPOSITORY);
        EmployeeRepository r_employee = (EmployeeRepository)RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.EMPLOYEE_REPOSITORY);
        Repository r_ccy = RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.CURRENCY_REPOSITORY);
        Repository r_unit = RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.UNIT_REPOSITORY);
        Repository r_warehouse = RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.WAREHOUSE_REPOSITORY);
        Repository r_sup = RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.CUSTOMER_REPOSITORY);
        UserSettingsRepository r_setting = RepositoryFactory.GetInstance().UserSetting();
        SalesOrderRepository r_po = (SalesOrderRepository)RepositoryFactory.GetInstance().GetTransactionRepository(RepositoryFactory.SALES_ORDER_REPOSITORY);
        DeliveryOrderRepository r_grn = (DeliveryOrderRepository)RepositoryFactory.GetInstance().GetTransactionRepository(RepositoryFactory.DELIVERY_ORDER_REPOSITORY);
        SalesReturnRepository r_prn = (SalesReturnRepository)RepositoryFactory.GetInstance().GetTransactionRepository(RepositoryFactory.SALES_RETURN_REPOSITORY);
        ReceiptRepository r_soinv = (ReceiptRepository)RepositoryFactory.GetInstance().GetJournalRepository(RepositoryFactory.RECEIPT_REPOSITORY);
        ARCreditNoteRepository r_apdn = (ARCreditNoteRepository)RepositoryFactory.GetInstance().GetJournalRepository(RepositoryFactory.ARCREDITNOTE_REPOSITORY);
        CustomerInvoiceJournalRepository r_sij = (CustomerInvoiceJournalRepository)RepositoryFactory.GetInstance().GetJournalRepository(RepositoryFactory.CUSTOMERINVOICE_JOURNAL_REPOSITORY);
        CustomerOutStandingInvoiceRepository r_soij = (CustomerOutStandingInvoiceRepository)RepositoryFactory.GetInstance().GetJournalRepository(RepositoryFactory.CUSTOMER_OUTSTANDING_INVOICE_REPOSITORY);
        
        IList m_units;
        IList m_warehouses;
        IList m_tops;
        IList m_employee;
        IList m_bank;
        IList m_poItems = new ArrayList();

        EditMode m_editMode = EditMode.New;
        bool m_enable = false;

        public ReceiptForm(IMainForm mainForm, string formName)
        {
            InitializeComponent();
            InitializeButtonClick();
            InitializeDataSource();
            InitializeDataGridValidation();
            this.MdiParent = (Form)mainForm;
            this.Name = formName;
            m_mainForm = mainForm;
            Clear(null, null);
        }

        private void InitializeDataGridValidation()
        {
            itemsDataGrid.CellValidating += new DataGridViewCellValidatingEventHandler(dataItemskryptonDataGridView_CellValidating);
            itemsDataGrid.CellValidated += new DataGridViewCellEventHandler(dataItemskryptonDataGridView_CellValidated);
            itemsDataGrid.CellBeginEdit += new DataGridViewCellCancelEventHandler(itemsDataGrid_CellBeginEdit);
            itemsDataGrid.CellEndEdit += new DataGridViewCellEventHandler(itemsDataGrid_CellEndEdit);
        }

        void itemsDataGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //if (e.ColumnIndex == scanColumn.Index)
            //{
            //    int count = 0;
            //    foreach (DeliveryOrderItem itm in m_poItems)
            //    {
            //        if (count == 0)
            //        {
            //            itemsDataGrid[scanColumn.Index, e.RowIndex].Value = itm.EVENT.CODE;
            //            itemsDataGrid[codeColumn.Index, e.RowIndex].Value = itm.PART.CODE;
            //            itemsDataGrid[nameColumn.Index, e.RowIndex].Value = itm.PART.NAME;
            //            itemsDataGrid[OutstandingPOColumn.Index, e.RowIndex].Value = itm.OUTSTANDING_AMOUNT_TO_PR;
            //            itemsDataGrid[OutstandingunitColumn.Index, e.RowIndex].Value = itm.PART.UNIT.CODE;
            //            itemsDataGrid[QtyColumn.Index, e.RowIndex].Value = 0;
            //            itemsDataGrid[unitColumn.Index, e.RowIndex].Value = itm.UNIT.CODE;
            //            itemsDataGrid[warehouseColumn.Index, e.RowIndex].Value = itm.WAREHOUSE.CODE;
            //            itemsDataGrid[notesColumn.Index, e.RowIndex].Value = itm.NOTES;
            //            itemsDataGrid[scanColumn.Index, e.RowIndex].Tag = itm;
            //            itemsDataGrid[codeColumn.Index, e.RowIndex].Tag = itm.PART;
            //        }
            //        else
            //        {
            //            int row = itemsDataGrid.Rows.Add();
            //            itemsDataGrid[scanColumn.Index, row].Value = itm.EVENT.CODE;
            //            itemsDataGrid[codeColumn.Index, row].Value = itm.PART.CODE;
            //            itemsDataGrid[nameColumn.Index, row].Value = itm.PART.NAME;
            //            itemsDataGrid[OutstandingPOColumn.Index, row].Value = itm.OUTSTANDING_AMOUNT_TO_PR;
            //            itemsDataGrid[OutstandingunitColumn.Index, row].Value = itm.PART.UNIT.CODE;
            //            itemsDataGrid[QtyColumn.Index, row].Value = 0;
            //            itemsDataGrid[unitColumn.Index, row].Value = itm.UNIT.CODE;
            //            itemsDataGrid[warehouseColumn.Index, row].Value = itm.WAREHOUSE.CODE;
            //            itemsDataGrid[notesColumn.Index, row].Value = itm.NOTES;
            //            itemsDataGrid[scanColumn.Index, row].Tag = itm;
            //            itemsDataGrid[codeColumn.Index, row].Tag = itm.PART;
            //        }
            //        count++;
            //    }
            //}
           
        }

        void itemsDataGrid_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            //if (e.ColumnIndex == unitColumn.Index)
            //{
            //    unitColumn.Items.Clear();
            //    Part p = (Part)itemsDataGrid[codeColumn.Index, e.RowIndex].Tag;
            //    if (p == null) return;
            //    IList units = r_part.GetAllUnit(p.ID, p.UNIT.ID);
            //    Utils.GetListCode(unitColumn.Items, units);
            //}
        }

        void dataItemskryptonDataGridView_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            if (m_editMode == EditMode.View) return;
            if (!itemsDataGrid[e.ColumnIndex, e.RowIndex].IsInEditMode) return;
            if (docnoColumn.Index == e.ColumnIndex)
            {
                if (itemsDataGrid[docnoColumn.Index, e.RowIndex].Tag != null)
                {
                    ARCreditNote ap = (ARCreditNote)itemsDataGrid[docnoColumn.Index, e.RowIndex].Tag;
                    itemsDataGrid[docnoColumn.Index, e.RowIndex].Value = ap.CODE;
                }
            }
            if (e.ColumnIndex == paymentAmountColumn.Index || e.ColumnIndex == docnoColumn.Index)
            { 
                ReCalculateNetTotal(); 
            }
        }

        void dataItemskryptonDataGridView_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (m_editMode == EditMode.View) return;
            itemsDataGrid.Rows[e.RowIndex].ErrorText = "";
            if (!itemsDataGrid[e.ColumnIndex, e.RowIndex].IsInEditMode) return;
            if (e.ColumnIndex == paymentAmountColumn.Index)
            {
                double py = Convert.ToDouble(e.FormattedValue);
                if (itemsDataGrid[invoiceNoColumn.Index, e.RowIndex].Tag == null) return;
                if (itemsDataGrid[invoiceNoColumn.Index, e.RowIndex].Tag is CustomerInvoiceJournalItem)
                {
                    CustomerInvoiceJournalItem si = (CustomerInvoiceJournalItem)itemsDataGrid[invoiceNoColumn.Index, e.RowIndex].Tag;
                    si.OUTSTANDING_AMOUNT = r_sij.GetOutstanding(si.ID);
                    if (si == null)
                    {
                        e.Cancel = true;
                        itemsDataGrid.Rows[e.RowIndex].ErrorText = "Please Fill Invoice";
                        return;
                    }
                    if (py > si.OUTSTANDING_AMOUNT)
                    {
                        e.Cancel = true;
                        itemsDataGrid.Rows[e.RowIndex].ErrorText = "Receipt exceed outstanding amount";
                        return;
                    }
                }
                if (itemsDataGrid[invoiceNoColumn.Index, e.RowIndex].Tag is CustomerOutStandingInvoiceItem)
                {
                    CustomerOutStandingInvoiceItem si = (CustomerOutStandingInvoiceItem)itemsDataGrid[invoiceNoColumn.Index, e.RowIndex].Tag;
                    si.OUTSTANDING_AMOUNT = r_soij.GetOutstanding(si.ID);
                    if (si == null)
                    {
                        e.Cancel = true;
                        itemsDataGrid.Rows[e.RowIndex].ErrorText = "Please Fill Invoice";
                        return;
                    }
                    if (py > si.OUTSTANDING_AMOUNT)
                    {
                        e.Cancel = true;
                        itemsDataGrid.Rows[e.RowIndex].ErrorText = "Receipt exceed outstanding amount";
                        return;
                    }
                }
            }
            if (docnoColumn.Index == e.ColumnIndex)
            {
                ReceiptType type = (ReceiptType)Enum.Parse(typeof(ReceiptType),itemsDataGrid[paymentTypeColumn.Index, e.RowIndex].Value.ToString());
                if (type == ReceiptType.ARCreditNote)
                {
                    itemsDataGrid[docnoColumn.Index, e.RowIndex].Tag = null;
                    IList notin = new ArrayList();
                    for (int i = 0; i < itemsDataGrid.Rows.Count; i++)
                    {
                        if (i == e.RowIndex) continue;
                        if (itemsDataGrid[docnoColumn.Index, i].Tag == null) continue;
                        ARCreditNote ap = (ARCreditNote)itemsDataGrid[docnoColumn.Index, i].Tag;
                        if (ap == null) continue;
                        notin.Add(ap.ID);
                    }
                    IList result = r_apdn.FindARCRForReceipt(((Customer)supplierkryptonComboBox.SelectedItem).ID, dateKryptonDateTimePicker.Value, e.FormattedValue.ToString(), notin);
                    if (result.Count == 1)
                    {
                        itemsDataGrid[docnoColumn.Index, e.RowIndex].Tag = result[0];
                        itemsDataGrid[docnoColumn.Index, e.RowIndex].Value = ((ARCreditNote)result[0]).CODE;
                        itemsDataGrid[paymentAmountColumn.Index, e.RowIndex].Value = ((ARCreditNote)result[0]).NET_AMOUNT;
                        itemsDataGrid[docdateColumn.Index, e.RowIndex].Value = ((ARCreditNote)result[0]).TRANSACTION_DATE;
                        itemsDataGrid[noteColumn.Index, e.RowIndex].Value = ((ARCreditNote)result[0]).NOTES;
                    }
                    else
                    {
                        using (SearchARCRForReceiptForm ap = new SearchARCRForReceiptForm(e.FormattedValue.ToString(), (Customer)supplierkryptonComboBox.SelectedItem, result, m_mainForm.CurrentUser, dateKryptonDateTimePicker.Value, notin))
                        {
                            ap.ShowDialog();
                            ARCreditNote res = ap.ARCREDIT_NOTE;
                            if (res == null) return;
                            itemsDataGrid[docnoColumn.Index, e.RowIndex].Tag = res;
                            itemsDataGrid[docnoColumn.Index, e.RowIndex].Value = res.CODE;
                            itemsDataGrid[paymentAmountColumn.Index, e.RowIndex].Value = res.NET_AMOUNT;
                            itemsDataGrid[docdateColumn.Index, e.RowIndex].Value = res.TRANSACTION_DATE;
                            itemsDataGrid[noteColumn.Index, e.RowIndex].Value = res.NOTES;
                        }
                    }
                }
            }
            if (paymentTypeColumn.Index == e.ColumnIndex)
            {
                itemsDataGrid[paymentAmountColumn.Index, e.RowIndex].Value = 0;
                itemsDataGrid[paymentAmountColumn.Index, e.RowIndex].ReadOnly = false;
                itemsDataGrid[docdateColumn.Index, e.RowIndex].ReadOnly = false;
                 ReceiptType type = (ReceiptType)Enum.Parse(typeof(ReceiptType),e.FormattedValue.ToString());
                 if (type == ReceiptType.ARCreditNote)
                 {
                     itemsDataGrid[paymentAmountColumn.Index, e.RowIndex].ReadOnly = true;
                     itemsDataGrid[docdateColumn.Index, e.RowIndex].ReadOnly = true;
                 }
            }
        }
        private void ReCalculateNetTotal()
        {
            IList totalAmount = new ArrayList();
            for (int i = 0; i < itemsDataGrid.Rows.Count; i++)
            {
                double amt = Convert.ToDouble(itemsDataGrid[paymentAmountColumn.Index, i].Value);
                if (amt == 0) continue;
                totalAmount.Add(amt);
            }
            netAmountkryptonNumericUpDown.Value = Convert.ToDecimal(Utils.CalculateSumList(totalAmount, 2));
        }
        private void InitializeDataSource()
        {
            employeeKryptonComboBox.DataSource = r_employee.GetAllPurchaser();
            supplierkryptonComboBox.DataSource = r_sup.GetAll();
            currencyKryptonComboBox.DataSource = r_ccy.GetAll();
            m_tops = r_top.GetAll();
            m_employee = r_employee.GetAllPurchaser();
            m_bank = r_bank.GetAll();

            Utils.GetListCode(topColumn.Items, m_tops);
            Utils.GetListCode(invoicerColumn.Items, m_employee);
            Utils.GetListCode(bankColumn.Items, m_bank);
            paymentTypeColumn.Items.AddRange(Enum.GetNames(typeof(ReceiptType)));
        }
        private void InitializeButtonClick()
        {
            toolStripButtonSave.Click += new EventHandler(Save);
            toolStripButtonEdit.Click += new EventHandler(Edit);
            toolStripButtonDelete.Click += new EventHandler(Delete);
            toolStripButtonClear.Click += new EventHandler(Clear);
            toolStripButtonRefresh.Click+=new EventHandler(Refresh);
            postToolStripButton.Click += new EventHandler(Post);
            toolStripButtonPrint.Click += new EventHandler(Print);
        }
        void Post(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (m_prn.POSTED)
                {
                    r_soinv.Revise(m_prn.ID);
                    m_prn.POSTED = false;
                    KryptonMessageBox.Show("Transaction has been UNPOSTED", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    r_soinv.Confirm(m_prn.ID);
                    m_prn.POSTED = true;
                    KryptonMessageBox.Show("Transaction has been POSTED", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                setEditMode(EditMode.View);
                loadData();
                setEnableForm(false);
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
                    if (m_prn.ID == 0)
                    {
                        r_soinv.Save(m_prn);
                    }
                    else
                    {
                        r_soinv.Update(m_prn);
                    }
                    KryptonMessageBox.Show("Transaction '" + m_prn.CODE + "' Record has been saved", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    textBoxCode.Text = m_prn.CODE;
                    setEnableForm(false);
                    setEditMode(EditMode.View);
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
        public bool Valid()
        {
            errorProvider1.Clear();
            bool a = textBoxCode.Text == "" && !r_soinv.IsAutoNumber();
            bool b = employeeKryptonComboBox.SelectedItem == null;
            bool k = supplierkryptonComboBox.SelectedItem == null;
            bool e = false;
            bool f = m_prn.ID > 0 ? false : r_soinv.IsCodeExist(textBoxCode.Text);
            
            if (a) errorProvider1.SetError(textBoxCode, "Code Can not Empty");
            if (b) errorProvider1.SetError(employeeKryptonComboBox, "Employee Can not Empty");
            if (k) errorProvider1.SetError(supplierkryptonComboBox, "Customer Can not Empty");
            if (f) errorProvider1.SetError(textBoxCode, a ? "Code Can not Empty & Code already used" : "Code already used");

            int j = 0;
            for (int i = 0; i < itemsDataGrid.Rows.Count; i++)
            {
                itemsDataGrid.Rows[i].ErrorText = "";
                if (itemsDataGrid[invoiceNoColumn.Index, i].Tag == null) continue;
                double qty = Convert.ToDouble(itemsDataGrid[paymentAmountColumn.Index, i].Value);
                if (qty == 0)
                {
                    itemsDataGrid.Rows[i].ErrorText = " Receipt amount must not 0(zero)";
                    e = true;
                }
                if (itemsDataGrid[paymentTypeColumn.Index, i].Value == null)
                {
                    itemsDataGrid.Rows[i].ErrorText = " Receipt type can not blank";
                    e = true;
                }
                else
                {
                    ReceiptType pytype = (ReceiptType)Enum.Parse(typeof(ReceiptType), itemsDataGrid[paymentTypeColumn.Index, i].Value.ToString());
                    if (pytype == ReceiptType.Bank)
                    {
                        if (itemsDataGrid[bankColumn.Index, i].Value == null)
                        {
                            itemsDataGrid.Rows[i].ErrorText = itemsDataGrid.Rows[i].ErrorText + " Please fill Bank";
                            e = true;
                        }

                    }
                    if (pytype == ReceiptType.ARCreditNote)
                        if (itemsDataGrid[docnoColumn.Index, i].Tag  == null)
                        {
                            itemsDataGrid.Rows[i].ErrorText = itemsDataGrid.Rows[i].ErrorText + " Please fill AP debitnote";
                            e = true;
                        }
                }
                j++;
            }

            bool g = j == 0;
            if (g) errorProvider1.SetError(itemsDataGrid,"Items must at least 1(one)");
            return !a && !b && !e && !f && !g;
        }
        private void UpdateEntity()
        {
            itemsDataGrid.RefreshEdit();
            m_prn.CODE = textBoxCode.Text.Trim();
            m_prn.TRANSACTION_DATE = dateKryptonDateTimePicker.Value;
            m_prn.EMPLOYEE = (Employee)employeeKryptonComboBox.SelectedItem;
            m_prn.NOTES = notesKryptonTextBox.Text;
            m_prn.VENDOR = (Customer)supplierkryptonComboBox.SelectedItem;
            m_prn.NET_AMOUNT = Convert.ToDouble(netAmountkryptonNumericUpDown.Value);
            m_prn.CURRENCY = (Currency)currencyKryptonComboBox.SelectedItem;
            m_prn.MODIFIED_BY = m_mainForm.CurrentUser.NAME;
            m_prn.MODIFIED_COMPUTER_NAME = Environment.MachineName;
            m_prn.EVENT_JOURNAL_ITEMS = getItems();
        }

        private IList getItems()
        {
            IList items = new ArrayList();
            for (int i = 0; i < itemsDataGrid.Rows.Count; i++)
            {
                if (itemsDataGrid[invoiceNoColumn.Index, i].Tag == null) continue;
                ReceiptItem st = (ReceiptItem)itemsDataGrid.Rows[i].Tag;
                if (st == null) st = new ReceiptItem();
                itemsDataGrid.Rows[i].Tag = st;
                //CustomerInvoiceJournalItem pi = (CustomerInvoiceJournalItem)itemsDataGrid[invoiceNoColumn.Index, i].Tag;
                st.CUSTOMER_INVOICE_JOURNAL_ITEM = (ICustomerInvoiceJournalItem)itemsDataGrid[invoiceNoColumn.Index, i].Tag;
                st.EVENT_JOURNAL = m_prn;

                st.AMOUNT = Convert.ToDouble(itemsDataGrid[paymentAmountColumn.Index, i].Value);
                st.PAYMENT_TYPE = (ReceiptType)Enum.Parse(typeof(ReceiptType), itemsDataGrid[paymentTypeColumn.Index, i].Value.ToString());
                st.INVOICE_NO = itemsDataGrid[docnoColumn.Index, i].Value == null ? "" : itemsDataGrid[docnoColumn.Index, i].Value.ToString();
                st.INVOICE_DATE = Convert.ToDateTime(itemsDataGrid[docdateColumn.Index, i].Value);
                st.NOTES = itemsDataGrid[noteColumn.Index, i].Value == null ? "" : itemsDataGrid[noteColumn.Index, i].Value.ToString();
                if(st.PAYMENT_TYPE== ReceiptType.Bank)
                    st.BANK = (Bank)Utils.FindEntityInList(itemsDataGrid[bankColumn.Index, i].Value.ToString(), m_bank);
                if (st.PAYMENT_TYPE == ReceiptType.ARCreditNote)
                    st.AR_CREDIT_NOTE = (ARCreditNote)itemsDataGrid[docnoColumn.Index, i].Tag;
                st.VENDOR = m_prn.VENDOR;
                st.CURRENCY = m_prn.CURRENCY;
                st.EMPLOYEE = m_prn.EMPLOYEE;

                //st.INVOICE_NO = itemsDataGrid[invoiceNoColumn.Index, i].Value.ToString();
                //st.INVOICE_DATE = Convert.ToDateTime(itemsDataGrid[invoiceDateColumn.Index, i].Value);
                //st.TOP = (TermOfReceipt)Utils.FindEntityInList(itemsDataGrid[topColumn.Index, i].Value.ToString(), m_tops);
                //st.DUE_DATE = Convert.ToDateTime(itemsDataGrid[dueDateColumn.Index, i].Value);
                //st.EMPLOYEE = (Employee)Utils.FindEntityInList(itemsDataGrid[invoicerColumn.Index, i].Value.ToString(), m_employee);
                //st.VENDOR = m_prn.VENDOR;
                //st.CURRENCY = m_prn.CURRENCY;
                items.Add(st);
            }
            return items;
        }
        public void ClearForm()
        {
            try
            {
                m_prn = new Receipt();
                textBoxCode.Text = "";
                dateKryptonDateTimePicker.Value = DateTime.Today;
                employeeKryptonComboBox.SelectedIndex = 0;
                notesKryptonTextBox.Text = "";
                supplierkryptonComboBox.SelectedIndex = 0;
                netAmountkryptonNumericUpDown.Value = 0;
                currencyKryptonComboBox.SelectedIndex = 0;

                itemsDataGrid.Rows.Clear();
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
            textBoxCode.ReadOnly = r_soinv.IsAutoNumber()?true:!enable;
            dateKryptonDateTimePicker.Enabled = enable;
            employeeKryptonComboBox.Enabled = enable;
            notesKryptonTextBox.ReadOnly = !enable;
            supplierkryptonComboBox.Enabled = enable;
            itemsDataGrid.AllowUserToDeleteRows = enable;
            itemsDataGrid.AllowUserToAddRows = enable;
            currencyKryptonComboBox.Enabled = enable;
            paymentTypeColumn.ReadOnly = !enable;
            paymentAmountColumn.ReadOnly = !enable;
            docdateColumn.ReadOnly = !enable;
            docnoColumn.ReadOnly = !enable;
            noteColumn.ReadOnly = !enable;
            bankColumn.ReadOnly = !enable;

            m_enable = enable;
        }
        private void setEditMode(EditMode editmode)
        {
            toolStripButtonSave.Enabled = (editmode == EditMode.New || editmode == EditMode.Update) && !m_prn.POSTED && m_mainForm.CurrentUser.FORM_ACCESS_LIST[Name].SAVE;
            toolStripButtonEdit.Enabled = (editmode == EditMode.View) && !m_prn.POSTED && m_mainForm.CurrentUser.FORM_ACCESS_LIST[Name].SAVE;
            toolStripButtonDelete.Enabled = (editmode == EditMode.View) && !m_prn.POSTED && m_mainForm.CurrentUser.FORM_ACCESS_LIST[Name].DELETE;
            toolStripButtonClear.Enabled = m_mainForm.CurrentUser.FORM_ACCESS_LIST[Name].SAVE;
            toolStripButtonPrint.Enabled = m_prn.POSTED && m_mainForm.CurrentUser.FORM_ACCESS_LIST[Name].PRINT;
            postToolStripButton.Enabled = (m_prn.ID > 0) && (editmode == EditMode.View) && m_mainForm.CurrentUser.FORM_ACCESS_LIST[Name].POST;
            postToolStripButton.Text = m_prn.POSTED ? "Unpost" : "Post";
            statusKryptonLabel.Text = m_prn.POSTED ? "POSTED" : "ENTRY";
            toolStripButtonSearchSI.Enabled = toolStripButtonSave.Enabled;
            toolStripButtonOutstandingInvoice.Enabled = toolStripButtonSave.Enabled;
            m_editMode = editmode;
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
                if (m_prn.ID > 0)
                {
                    this.Cursor = Cursors.WaitCursor;
                    if (KryptonMessageBox.Show("Are you sure to delete this record?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No) { this.Cursor = Cursors.Default; return; }
                    r_soinv.Delete(m_prn);
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
        public void Clear(object sender, EventArgs e)
        {
            ClearForm();
            setEnableForm(true);
            setEditMode(EditMode.New);
            textBoxCode.Focus();
        }
        private void loadData()
        {
            textBoxCode.Text = m_prn.CODE;
            dateKryptonDateTimePicker.Value = m_prn.TRANSACTION_DATE;
            employeeKryptonComboBox.Text = r_employee.GetById(m_prn.EMPLOYEE).ToString();
            notesKryptonTextBox.Text = m_prn.NOTES;
            supplierkryptonComboBox.Text = r_sup.GetById((Customer)m_prn.VENDOR).ToString();
            currencyKryptonComboBox.Text = r_ccy.GetById(m_prn.CURRENCY).ToString();
            netAmountkryptonNumericUpDown.Value = Convert.ToDecimal(m_prn.NET_AMOUNT);
            itemsDataGrid.Rows.Clear();
            foreach (ReceiptItem item in m_prn.EVENT_JOURNAL_ITEMS)
            {
                int i = itemsDataGrid.Rows.Add();

                if (item.CUSTOMER_INVOICE_JOURNAL_ITEM is CustomerInvoiceJournalItem)
                {
                    CustomerInvoiceJournalItem siji = (CustomerInvoiceJournalItem)item.CUSTOMER_INVOICE_JOURNAL_ITEM;
                    itemsDataGrid[invoiceNoColumn.Index, i].Tag = siji;
                    itemsDataGrid[invoiceNoColumn.Index, i].Value = siji.EVENT_JOURNAL.CODE;
                    itemsDataGrid[invoiceDateColumn.Index, i].Value = siji.EVENT_JOURNAL.TRANSACTION_DATE;
                    itemsDataGrid[topColumn.Index, i].Value = siji.TOP.ToString();
                    itemsDataGrid[dueDateColumn.Index, i].Value = siji.DUE_DATE;
                    itemsDataGrid[invoicerColumn.Index, i].Value = siji.EMPLOYEE.ToString();
                    itemsDataGrid[amountColumn.Index, i].Value = siji.AMOUNT;
                    itemsDataGrid[OutstandingAmountColumn.Index, i].Value = r_sij.GetOutstanding(siji.ID);
                    itemsDataGrid[paidAmountColumn.Index, i].Value = r_sij.GetPaid(siji.ID);
                }
                if (item.CUSTOMER_INVOICE_JOURNAL_ITEM is CustomerOutStandingInvoiceItem)
                {
                    CustomerOutStandingInvoiceItem siji = (CustomerOutStandingInvoiceItem)item.CUSTOMER_INVOICE_JOURNAL_ITEM;
                    itemsDataGrid[invoiceNoColumn.Index, i].Tag = siji;
                    itemsDataGrid[invoiceNoColumn.Index, i].Value = siji.EVENT_JOURNAL.CODE;
                    itemsDataGrid[invoiceDateColumn.Index, i].Value = siji.EVENT_JOURNAL.TRANSACTION_DATE;
                    itemsDataGrid[topColumn.Index, i].Value = siji.TOP.ToString();
                    itemsDataGrid[dueDateColumn.Index, i].Value = siji.DUE_DATE;
                    itemsDataGrid[invoicerColumn.Index, i].Value = siji.EMPLOYEE.ToString();
                    itemsDataGrid[amountColumn.Index, i].Value = siji.AMOUNT;
                    itemsDataGrid[OutstandingAmountColumn.Index, i].Value = r_soij.GetOutstanding(siji.ID);
                    itemsDataGrid[paidAmountColumn.Index, i].Value = r_soij.GetPaid(siji.ID);
                }
                itemsDataGrid.Rows[i].Tag = item;
                itemsDataGrid[paymentAmountColumn.Index, i].Value = item.AMOUNT;
                itemsDataGrid[paymentTypeColumn.Index, i].Value = item.PAYMENT_TYPE.ToString();
                itemsDataGrid[docnoColumn.Index, i].Value = item.INVOICE_NO;
                itemsDataGrid[docdateColumn.Index, i].Value = item.INVOICE_DATE;
                itemsDataGrid[noteColumn.Index, i].Value = item.NOTES;
                itemsDataGrid[bankColumn.Index, i].Value = item.PAYMENT_TYPE == ReceiptType.Bank ? item.BANK.ToString() : "";
                if (item.PAYMENT_TYPE == ReceiptType.ARCreditNote)
                {
                    itemsDataGrid[docnoColumn.Index, i].Tag = item.AR_CREDIT_NOTE;
                    itemsDataGrid[docnoColumn.Index, i].Value = item.AR_CREDIT_NOTE.CODE;
                    itemsDataGrid[paymentAmountColumn.Index, i].Value = item.AR_CREDIT_NOTE.NET_AMOUNT;
                    itemsDataGrid[docdateColumn.Index, i].Value = item.AR_CREDIT_NOTE.TRANSACTION_DATE;
                }
            }
        }
        public void Refresh(object sender, EventArgs e)
        {
            //loadRecords(); 
            //gridData.ClearSelection(); 
            if ((m_editMode == EditMode.New) || (m_editMode == EditMode.Update))
            {
                if (m_enable)
                {
                    InitializeDataSource();
                }
            }
        }
        public void Print(object sender, EventArgs e)
        {
            
        }
        private void BankForm_Activated(object sender, EventArgs e)
        {
            ReloadMainFormButton();
        }
        private void employeeKryptonComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Employee em = (Employee)employeeKryptonComboBox.SelectedItem;
            EmployeekryptonTextBox.Text = em.NAME;
        }

        private void searchToolStripButton_Click(object sender, EventArgs e)
        {
            IList result = searchToolStripTextBox.Text == string.Empty ? new ArrayList() : r_soinv.Search(searchToolStripTextBox.Text);
            if (result.Count == 1)
            {
                m_prn = (Receipt)result[0];
                m_prn = (Receipt)r_soinv.Get(m_prn.ID);
                m_prn.EMPLOYEE = (Employee)r_employee.GetById(m_prn.EMPLOYEE);
                m_prn.VENDOR = (Customer)r_sup.GetById((Customer)m_prn.VENDOR);
                setEditMode(EditMode.View);
                loadData();
                setEnableForm(false);
            }
            else
            {
                using (SearchReceiptForm frm = new SearchReceiptForm(searchToolStripTextBox.Text, result, m_mainForm.CurrentUser))
                {
                    frm.ShowDialog();
                    if (frm.RECEIPT == null)
                    {
                        return;
                    }
                    else
                    {
                        m_prn = frm.RECEIPT;
                        m_prn = (Receipt)r_soinv.Get(m_prn.ID);
                        m_prn.EMPLOYEE = (Employee)r_employee.GetById(m_prn.EMPLOYEE);
                        // m_prn.VENDOR = (Customer)r_sup.GetById(m_prn.VENDOR);
                        setEditMode(EditMode.View);
                        loadData();
                        setEnableForm(false);
                    }
                }
            }
        }

        private void fieldChooserTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FieldChooserForm cm = new FieldChooserForm(m_mainForm.CurrentUser.ID, this.Name,itemsDataGrid);
            cm.ShowDialog();
        }

        private void SalesReturnForm_Load(object sender, EventArgs e)
        {
            UserSetting.LoadSetting(itemsDataGrid, m_mainForm.CurrentUser.ID, this.Name);
        }

        private void SalesReturnrForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            UserSetting.SaveSetting(itemsDataGrid, m_mainForm.CurrentUser.ID, this.Name);
        }

        private void supplierkryptonComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Customer em = (Customer)supplierkryptonComboBox.SelectedItem;
            supplierKryptonTextBox.Text = em == null ? "" : em.NAME;
            contactPersonKryptonTextBox.Text = em == null ? "" : em.CONTACT;
            addressKryptonTextBox.Text = em == null ? "" : em.ADDRESS;
            itemsDataGrid.Rows.Clear();
        }

        private void itemsDataGrid_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            for (int count = 0; (count <= (itemsDataGrid.Rows.Count - 2)); count++)
            {
                itemsDataGrid.Rows[count].HeaderCell.Value = string.Format((count + 1).ToString(), "0");
                itemsDataGrid.Rows[count].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
        }

        private void toolStripButtonSearchSI_Click(object sender, EventArgs e)
        {
            Customer sp = (Customer)supplierkryptonComboBox.SelectedItem;
            Currency ccy = (Currency)currencyKryptonComboBox.SelectedItem;
            if (sp == null) return;
            if (ccy == null) return;
            IList addedPI = new ArrayList();
            for (int i = 0; i < itemsDataGrid.Rows.Count; i++)
            {
                if (itemsDataGrid[invoiceNoColumn.Index, i].Tag is CustomerInvoiceJournalItem)
                {
                    CustomerInvoiceJournalItem pi = (CustomerInvoiceJournalItem)itemsDataGrid[invoiceNoColumn.Index, i].Tag;
                    if (pi == null) continue;
                    addedPI.Add(pi.ID);
                }
            }
            using (SearchCussInvJForReceiptForm frm = new SearchCussInvJForReceiptForm(ccy, sp, addedPI, m_mainForm.CurrentUser,
                dateKryptonDateTimePicker.Value))
            {
                frm.ShowDialog();
                IList result = frm.RESULT;
                foreach (CustomerInvoiceJournalItem item in result)
                {
                    int i = itemsDataGrid.Rows.Add();
                    itemsDataGrid[invoiceNoColumn.Index, i].Tag = item;
                    itemsDataGrid[invoiceNoColumn.Index, i].Value = item.EVENT_JOURNAL.CODE;
                    itemsDataGrid[invoiceDateColumn.Index, i].Value = item.EVENT_JOURNAL.TRANSACTION_DATE;
                    itemsDataGrid[topColumn.Index, i].Value = item.TOP.ToString();
                    itemsDataGrid[amountColumn.Index, i].Value = item.AMOUNT;
                    itemsDataGrid[dueDateColumn.Index, i].Value = item.DUE_DATE;
                    itemsDataGrid[invoicerColumn.Index, i].Value = item.EMPLOYEE.ToString();
                    itemsDataGrid[OutstandingAmountColumn.Index, i].Value = item.OUTSTANDING_AMOUNT;
                    itemsDataGrid[paidAmountColumn.Index, i].Value = item.RECEIPT_AMOUNT;

                    //Init---
                    itemsDataGrid[paymentTypeColumn.Index, i].Value = ReceiptType.Bank;
                    itemsDataGrid[docdateColumn.Index, i].Value = DateTime.Today;
                    //---
                }
            }
        }

        private void toolStripButtonOutstandingInvoice_Click(object sender, EventArgs e)
        {
            Customer sp = (Customer)supplierkryptonComboBox.SelectedItem;
            Currency ccy = (Currency)currencyKryptonComboBox.SelectedItem;
            if (sp == null) return;
            if (ccy == null) return;
            IList addedPI = new ArrayList();
            for (int i = 0; i < itemsDataGrid.Rows.Count; i++)
            {
                if (itemsDataGrid[invoiceNoColumn.Index, i].Tag is CustomerOutStandingInvoiceItem)
                {
                    CustomerOutStandingInvoiceItem pi = (CustomerOutStandingInvoiceItem)itemsDataGrid[invoiceNoColumn.Index, i].Tag;
                    if (pi == null) continue;
                    addedPI.Add(pi.ID);
                }
            }
            using (SearchOstCussInvForReceiptForm frm = new SearchOstCussInvForReceiptForm(ccy, sp, addedPI, m_mainForm.CurrentUser,
                dateKryptonDateTimePicker.Value))
            {
                frm.ShowDialog();
                IList result = frm.RESULT;
                foreach (CustomerOutStandingInvoiceItem item in result)
                {
                    int i = itemsDataGrid.Rows.Add();
                    itemsDataGrid[invoiceNoColumn.Index, i].Tag = item;
                    itemsDataGrid[invoiceNoColumn.Index, i].Value = item.EVENT_JOURNAL.CODE;
                    itemsDataGrid[invoiceDateColumn.Index, i].Value = item.EVENT_JOURNAL.TRANSACTION_DATE;
                    itemsDataGrid[topColumn.Index, i].Value = item.TOP.ToString();
                    itemsDataGrid[dueDateColumn.Index, i].Value = item.DUE_DATE;
                    itemsDataGrid[invoicerColumn.Index, i].Value = item.EMPLOYEE.ToString();
                    itemsDataGrid[amountColumn.Index, i].Value = item.AMOUNT;
                    itemsDataGrid[OutstandingAmountColumn.Index, i].Value = item.OUTSTANDING_AMOUNT;
                    itemsDataGrid[paidAmountColumn.Index, i].Value = item.RECEIPT_AMOUNT;

                    //Init---
                    itemsDataGrid[paymentTypeColumn.Index, i].Value = ReceiptType.Bank;
                    itemsDataGrid[docdateColumn.Index, i].Value = DateTime.Today;
                    //---
                }
            }
        }

        private void currencyKryptonComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_editMode == EditMode.View) return;
            itemsDataGrid.Rows.Clear();
        }

        private void searchToolStripTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                searchToolStripButton_Click(null, null);
            }
        }
    }
}
