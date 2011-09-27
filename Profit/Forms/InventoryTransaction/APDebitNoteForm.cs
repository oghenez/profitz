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
    public partial class APDebitNoteForm : KryptonForm, IChildForm
    {
        APDebitNote m_prn = new APDebitNote(); 
        IMainForm m_mainForm;
        Repository r_top = RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.TOP_REPOSITORY);
        EmployeeRepository r_employee = (EmployeeRepository)RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.EMPLOYEE_REPOSITORY);
        Repository r_ccy = RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.CURRENCY_REPOSITORY);
        Repository r_unit = RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.UNIT_REPOSITORY);
        Repository r_warehouse = RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.WAREHOUSE_REPOSITORY);
        SupplierRepository r_sup = (SupplierRepository)RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.SUPPLIER_REPOSITORY);
        UserSettingsRepository r_setting = RepositoryFactory.GetInstance().UserSetting();
        PurchaseOrderRepository r_po = (PurchaseOrderRepository)RepositoryFactory.GetInstance().GetTransactionRepository(RepositoryFactory.PURCHASEORDER_REPOSITORY);
        GoodReceiveNoteRepository r_grn = (GoodReceiveNoteRepository)RepositoryFactory.GetInstance().GetTransactionRepository(RepositoryFactory.GOODRECEIVENOTE_REPOSITORY);
        PurchaseReturnRepository r_prn = (PurchaseReturnRepository)RepositoryFactory.GetInstance().GetTransactionRepository(RepositoryFactory.PURCHASE_RETURN_REPOSITORY);
        APDebitNoteRepository r_apdn = (APDebitNoteRepository)RepositoryFactory.GetInstance().GetJournalRepository(RepositoryFactory.APDEBITNOTE_REPOSITORY);
        
        IList m_units;
        IList m_warehouses;
        IList m_tops;
        IList m_employee;
        IList m_poItems = new ArrayList();

        EditMode m_editMode = EditMode.New;
        bool m_enable = false;

        public APDebitNoteForm(IMainForm mainForm, string formName)
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
            //    foreach (GoodReceiveNoteItem itm in m_poItems)
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
            if (e.ColumnIndex == invoiceNoColumn.Index)
            {
                if (itemsDataGrid[invoiceDateColumn.Index, e.RowIndex].Value == null)
                {
                    itemsDataGrid[invoiceDateColumn.Index, e.RowIndex].Value = DateTime.Today;
                }
            }
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
            if (e.ColumnIndex == amountColumn.Index)
            { 
                ReCalculateNetTotal(); 
            }
        }

        void dataItemskryptonDataGridView_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (m_editMode == EditMode.View) return;
            itemsDataGrid.Rows[e.RowIndex].ErrorText = "";
            if (!itemsDataGrid[e.ColumnIndex, e.RowIndex].IsInEditMode) return;

            //if (e.ColumnIndex == scanColumn.Index)
            //{
            //    if (e.FormattedValue.ToString() == "")return;

            //    IList addedPI = new ArrayList();
            //    for (int i = 0; i < itemsDataGrid.Rows.Count; i++)
            //    {
            //        if (i == e.RowIndex) continue;
            //        GoodReceiveNoteItem pi = (GoodReceiveNoteItem)itemsDataGrid[scanColumn.Index, i].Tag;
            //        if (pi == null) continue;
            //        addedPI.Add(pi.ID);
            //    }
            //    IList res = r_grn.FindPObyPartAndGRNNo(e.FormattedValue.ToString(), addedPI, ((Supplier)supplierkryptonComboBox.SelectedItem).ID, dateKryptonDateTimePicker.Value);
            //    if (res.Count == 0)
            //    {
            //        using (SearchGRNForPRForm fr = new SearchGRNForPRForm(e.FormattedValue.ToString(), ((Supplier)supplierkryptonComboBox.SelectedItem).ID, addedPI, m_mainForm.CurrentUser, dateKryptonDateTimePicker.Value))
            //        {
            //            fr.ShowDialog();
            //            IList result = fr.RESULT;
            //            m_poItems = result;
            //        }
            //    }
            //    else
            //    {
            //        m_poItems = res;
            //    }
            //}
            //if (QtyColumn.Index == e.ColumnIndex)
            //{
            //    GoodReceiveNoteItem pi = (GoodReceiveNoteItem)itemsDataGrid[scanColumn.Index, e.RowIndex].Tag;
            //    if (pi == null) return;
            //    Part p = (Part)itemsDataGrid[codeColumn.Index, e.RowIndex].Tag;
            //    if (p == null) return;
            //    Unit u = (Unit)Utils.FindEntityInList(itemsDataGrid[unitColumn.Index, e.RowIndex].Value.ToString(), m_units);
            //    if (u == null) return;
            //    p.UNIT_CONVERSION_LIST = r_part.GetAllUnit(p.ID, p.UNIT.ID);
            //    APDebitNoteItem sample = new APDebitNoteItem();
            //    sample.PART = p;
            //    sample.UNIT = u;
            //    sample.QYTAMOUNT = Convert.ToDouble(e.FormattedValue);
            //    double qty = sample.GetAmountInSmallestUnit();
            //    double rest = pi.OUTSTANDING_AMOUNT_TO_PR - qty;
            //    if (rest < 0)
            //    {
            //        e.Cancel = true;
            //        itemsDataGrid.Rows[e.RowIndex].ErrorText = "Quantity exceed outstanding quantity";
            //    }
            //    itemsDataGrid[OutstandingPOColumn.Index, e.RowIndex].Value = rest;
            //}
            // if (e.ColumnIndex == invoiceNoColumn.Index)
            //{
            //    if (itemsDataGrid[invoiceDateColumn.Index, e.RowIndex].Value == null)
            //    {
            //        itemsDataGrid[invoiceDateColumn.Index, e.RowIndex].Value = DateTime.Today;
            //        itemsDataGrid[dueDateColumn.Index, e.RowIndex].Value = DateTime.Today;
            //    }
            //}
            //if (e.ColumnIndex == topColumn.Index)
            //{
            //    TermOfPayment top = (TermOfPayment)Utils.FindEntityInList(e.FormattedValue.ToString(), m_tops);
            //    if (top == null) return;
            //    DateTime trdate = Convert.ToDateTime( itemsDataGrid[invoiceDateColumn.Index, e.RowIndex].Value);
            //    itemsDataGrid[dueDateColumn.Index, e.RowIndex].Value = trdate.AddDays(top.DAYS);
            //}
        }
        private void ReCalculateNetTotal()
        {
            IList totalAmount = new ArrayList();
            for (int i = 0; i < itemsDataGrid.Rows.Count; i++)
            {
                double amt = Convert.ToDouble(itemsDataGrid[amountColumn.Index, i].Value);
                if (amt == 0) continue;
                totalAmount.Add(amt);
            }
            netAmountkryptonNumericUpDown.Value = Convert.ToDecimal(Utils.CalculateSumList(totalAmount, 2));
        }
        private void InitializeDataSource()
        {
            employeeKryptonComboBox.DataSource = r_employee.GetAllPurchaser();
            supplierkryptonComboBox.DataSource = r_sup.GetAllActive();
            currencyKryptonComboBox.DataSource = r_ccy.GetAll();
            //m_tops = r_top.GetAll();
            //m_employee = r_employee.GetAllPurchaser();
            //Utils.GetListCode(topColumn.Items, m_tops);
            //Utils.GetListCode(invoicerColumn.Items, m_employee);
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
                    r_apdn.Revise(m_prn.ID);
                    m_prn.POSTED = false;
                    KryptonMessageBox.Show("Transaction has been UNPOSTED", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    r_apdn.Confirm(m_prn.ID);
                    m_prn.POSTED = true;
                    KryptonMessageBox.Show("Transaction has been POSTED", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                setEnableForm(false);
                setEditMode(EditMode.View);
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
                        r_apdn.Save(m_prn);
                    }
                    else
                    {
                        r_apdn.Update(m_prn);
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
            bool a = textBoxCode.Text == "" && !r_apdn.IsAutoNumber();
            bool b = employeeKryptonComboBox.SelectedItem == null;
            bool k = supplierkryptonComboBox.SelectedItem == null;
            bool c = currencyKryptonComboBox.SelectedItem == null;
            bool e = false;
            bool f = m_prn.ID > 0 ? false : r_apdn.IsCodeExist(textBoxCode.Text);
            
            if (a) errorProvider1.SetError(textBoxCode, "Code Can not Empty");
            if (b) errorProvider1.SetError(employeeKryptonComboBox, "Employee Can not Empty");
            if (k) errorProvider1.SetError(supplierkryptonComboBox, "Supplier Can not Empty");
            if (c) errorProvider1.SetError(currencyKryptonComboBox, "Currency Can not Empty");
            if (f) errorProvider1.SetError(textBoxCode, a ? "Code Can not Empty & Code already used" : "Code already used");

            int j = 0;
            for (int i = 0; i < itemsDataGrid.Rows.Count; i++)
            {
                if (itemsDataGrid[invoiceNoColumn.Index, i].Value == null) continue;
                if (itemsDataGrid[invoiceNoColumn.Index, i].Value.ToString() == "") continue;
                double qty = Convert.ToDouble(itemsDataGrid[amountColumn.Index, i].Value);
                if (qty == 0)
                {
                    itemsDataGrid.Rows[i].ErrorText = itemsDataGrid.Rows[i].ErrorText + " Amount must not 0(zero)";
                    e = true;
                }
                //if (itemsDataGrid[invoicerColumn.Index, i].Value == null)
                //{
                //    itemsDataGrid.Rows[i].ErrorText = itemsDataGrid.Rows[i].ErrorText + " Please choose Invoicer unit.";
                //    e = true;
                //}
                //if (itemsDataGrid[topColumn.Index, i].Value == null)
                //{
                //    itemsDataGrid.Rows[i].ErrorText = itemsDataGrid.Rows[i].ErrorText + " Please choose TOP unit.";
                //    e = true;
                //}
                //if(itemsDataGrid[invoiceDateColumn.Index, i].Value==null)
                //{
                //    itemsDataGrid.Rows[i].ErrorText = itemsDataGrid.Rows[i].ErrorText + " Please choose Invoice Date.";
                //    e = true;
                //}
                //if(itemsDataGrid[dueDateColumn.Index, i].Value==null)
                //{
                //    itemsDataGrid.Rows[i].ErrorText = itemsDataGrid.Rows[i].ErrorText + " Please choose Due Date.";
                //    e = true;
                //}
                //if (itemsDataGrid[invoiceDateColumn.Index, i].Value != null && itemsDataGrid[dueDateColumn.Index, i].Value != null)
                //{
                //    DateTime invdate = Convert.ToDateTime(itemsDataGrid[invoiceDateColumn.Index, i].Value);
                //    DateTime duedate = Convert.ToDateTime(itemsDataGrid[dueDateColumn.Index, i].Value);
                //    if (duedate< invdate)
                //    {
                //        itemsDataGrid.Rows[i].ErrorText = itemsDataGrid.Rows[i].ErrorText + "Due Date exceed Invoice date.";
                //        e = true;
                //    }
                //}
                j++;
            }

            bool g = j == 0;
            if (g) errorProvider1.SetError(itemsDataGrid,"Items must at least 1(one)");
            return !a && !b && !k && !e && !f && !g;
        }
        private void UpdateEntity()
        {
            itemsDataGrid.RefreshEdit();
            m_prn.CODE = textBoxCode.Text.Trim();
            m_prn.TRANSACTION_DATE = dateKryptonDateTimePicker.Value;
            m_prn.EMPLOYEE = (Employee)employeeKryptonComboBox.SelectedItem;
            m_prn.NOTES = notesKryptonTextBox.Text;
            m_prn.VENDOR = (Supplier)supplierkryptonComboBox.SelectedItem;
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
                if (itemsDataGrid[invoiceNoColumn.Index, i].Value == null) continue;
                if (itemsDataGrid[invoiceNoColumn.Index, i].Value.ToString() == "") continue;
                APDebitNoteItem st = (APDebitNoteItem)itemsDataGrid.Rows[i].Tag;
                if (st == null) st = new APDebitNoteItem();
                itemsDataGrid.Rows[i].Tag = st;
                st.EVENT_JOURNAL = m_prn;
                st.AMOUNT = Convert.ToDouble(itemsDataGrid[amountColumn.Index, i].Value);
                st.INVOICE_NO = itemsDataGrid[invoiceNoColumn.Index, i].Value.ToString();
                st.INVOICE_DATE = Convert.ToDateTime(itemsDataGrid[invoiceDateColumn.Index, i].Value);
               // st.TOP = (TermOfPayment)Utils.FindEntityInList(itemsDataGrid[topColumn.Index, i].Value.ToString(), m_tops);
              //  st.DUE_DATE = Convert.ToDateTime(itemsDataGrid[dueDateColumn.Index, i].Value);
               // st.EMPLOYEE = (Employee)Utils.FindEntityInList(itemsDataGrid[invoicerColumn.Index, i].Value.ToString(), m_employee);
                st.VENDOR = m_prn.VENDOR;
                st.CURRENCY = m_prn.CURRENCY;
                st.PURCHASE_RETURN = (PurchaseReturn)itemsDataGrid[invoiceNoColumn.Index, i].Tag;
                st.NOTES = itemsDataGrid[notesColumn.Index, i].Value == null ? "" : itemsDataGrid[notesColumn.Index, i].Value.ToString();
                st.EMPLOYEE = m_prn.EMPLOYEE;
                items.Add(st);
            }
            return items;
        }
        public void ClearForm()
        {
            try
            {
                m_prn = new APDebitNote();
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
            textBoxCode.ReadOnly = r_apdn.IsAutoNumber()?true:!enable;
            dateKryptonDateTimePicker.Enabled = enable;
            employeeKryptonComboBox.Enabled = enable;
            notesKryptonTextBox.ReadOnly = !enable;
            supplierkryptonComboBox.Enabled = enable;
            currencyKryptonComboBox.Enabled = enable;
            itemsDataGrid.AllowUserToDeleteRows = enable;
            itemsDataGrid.AllowUserToAddRows = enable;
            invoiceNoColumn.ReadOnly = !enable;
            notesColumn.ReadOnly = !enable;
            invoiceDateColumn.ReadOnly = !enable;
            //topColumn.ReadOnly = !enable;
            //dueDateColumn.ReadOnly = !enable;
            //invoicerColumn.ReadOnly = !enable;
            amountColumn.ReadOnly = !enable;
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
            toolStripButtonFromPR.Enabled = toolStripButtonSave.Enabled;
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
                    r_apdn.Delete(m_prn);
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
            supplierkryptonComboBox.Text = r_sup.GetById((Supplier)m_prn.VENDOR).ToString();
            currencyKryptonComboBox.Text = r_ccy.GetById(m_prn.CURRENCY).ToString();
            netAmountkryptonNumericUpDown.Value = Convert.ToDecimal(m_prn.NET_AMOUNT);
            itemsDataGrid.Rows.Clear();
            foreach (APDebitNoteItem item in m_prn.EVENT_JOURNAL_ITEMS)
            {
                int i = itemsDataGrid.Rows.Add();
                itemsDataGrid.Rows[i].Tag = item;
                itemsDataGrid[invoiceNoColumn.Index, i].Tag = item.PURCHASE_RETURN;
                itemsDataGrid[invoiceNoColumn.Index, i].Value = item.INVOICE_NO;
                itemsDataGrid[invoiceDateColumn.Index, i].Value = item.INVOICE_DATE;
                //itemsDataGrid[topColumn.Index, i].Value = (TermOfPayment)r_top.GetById(item.TOP);
                //itemsDataGrid[dueDateColumn.Index, i].Value = item.DUE_DATE;
                //itemsDataGrid[invoicerColumn.Index, i].Value = (Employee)r_employee.GetById(item.EMPLOYEE);  
                itemsDataGrid[amountColumn.Index, i].Value = item.AMOUNT;
                itemsDataGrid[notesColumn.Index, i].Value = item.NOTES;

                //item.UNIT = (Unit)r_unit.GetById(item.UNIT);
                //item.GRN_ITEM.UNIT = (Unit)r_unit.GetById(item.GRN_ITEM.UNIT);
                //item.GRN_ITEM.PART.UNIT = (Unit)r_unit.GetById(item.GRN_ITEM.PART.UNIT);
                //int i = itemsDataGrid.Rows.Add();
                //itemsDataGrid.Rows[i].Tag = item;
                //itemsDataGrid[codeColumn.Index, i].Tag = item.PART;
                //itemsDataGrid[scanColumn.Index, i].Tag = item.GRN_ITEM;
                //itemsDataGrid[scanColumn.Index, i].Value = item.GRN_ITEM.EVENT.CODE;
                //itemsDataGrid[codeColumn.Index, i].Value = item.PART.CODE;
                //itemsDataGrid[nameColumn.Index, i].Value = item.PART.NAME;
                //itemsDataGrid[QtyColumn.Index, i].Value = item.QYTAMOUNT;
                //itemsDataGrid[warehouseColumn.Index, i].Value = r_warehouse.GetById(item.WAREHOUSE).ToString();
                //itemsDataGrid[notesColumn.Index, i].Value = item.NOTES;
                //itemsDataGrid[unitColumn.Index, i].Value = item.UNIT.ToString(); ;
                //itemsDataGrid[OutstandingPOColumn.Index, i].Value = item.GRN_ITEM.OUTSTANDING_AMOUNT_TO_PR;
                //itemsDataGrid[OutstandingunitColumn.Index, i].Value = item.GRN_ITEM.PART.UNIT.CODE;

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
            IList result = searchToolStripTextBox.Text == string.Empty ? new ArrayList() : r_apdn.Search(searchToolStripTextBox.Text);
            if (result.Count == 1)
            {
                m_prn = (APDebitNote)result[0];
                m_prn = (APDebitNote)r_apdn.Get(m_prn.ID);
                //m_prn.EMPLOYEE = (Employee)r_employee.GetById(m_prn.EMPLOYEE);
                // m_prn.VENDOR = (Supplier)r_sup.GetById(m_prn.VENDOR);
                setEditMode(EditMode.View);
                loadData();
                setEnableForm(false);
            }
            else
            {
                using (SearchAPDebitNoteForm frm = new SearchAPDebitNoteForm(searchToolStripTextBox.Text, result, m_mainForm.CurrentUser))
                {
                    frm.ShowDialog();
                    if (frm.APDEBIT_NOTE == null)
                    {
                        return;
                    }
                    else
                    {
                        m_prn = frm.APDEBIT_NOTE  ;
                        m_prn = (APDebitNote)r_apdn.Get(m_prn.ID);
                        m_prn.EMPLOYEE = (Employee)r_employee.GetById(m_prn.EMPLOYEE);
                       // m_prn.VENDOR = (Supplier)r_sup.GetById(m_prn.VENDOR);
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

        private void PurchaseReturnForm_Load(object sender, EventArgs e)
        {
            UserSetting.LoadSetting(itemsDataGrid, m_mainForm.CurrentUser.ID, this.Name);
        }

        private void PurchaseReturnrForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            UserSetting.SaveSetting(itemsDataGrid, m_mainForm.CurrentUser.ID, this.Name);
        }

        private void supplierkryptonComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Supplier em = (Supplier)supplierkryptonComboBox.SelectedItem;
            supplierKryptonTextBox.Text = em == null ? "" : em.NAME;
            addressKryptonTextBox.Text = em == null ? "" : em.ADDRESS;
            contactPersonKryptonTextBox.Text = em == null ? "" : em.CONTACT;
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

        private void toolStripButtonFromPR_Click(object sender, EventArgs e)
        {
            IList addedPI = new ArrayList();
            for (int i = 0; i < itemsDataGrid.Rows.Count; i++)
            {
                PurchaseReturn pi = (PurchaseReturn)itemsDataGrid[invoiceNoColumn.Index, i].Tag;
                if (pi == null) continue;
                addedPI.Add(pi.ID);
            }
            using (SearchPRForAPDebitNoteForm frm = new SearchPRForAPDebitNoteForm(
                 (Supplier)supplierkryptonComboBox.SelectedItem, addedPI, m_mainForm.CurrentUser, dateKryptonDateTimePicker.Value))
            {
                frm.ShowDialog();
                IList result = frm.RESULT;
                foreach (PurchaseReturn item in result)
                {
                    int row = itemsDataGrid.Rows.Add();
                    itemsDataGrid[invoiceNoColumn.Index, row].Tag = item;
                    itemsDataGrid[invoiceNoColumn.Index, row].Value = item.CODE;
                    itemsDataGrid[invoiceDateColumn.Index, row].Value = item.TRANSACTION_DATE;
                    itemsDataGrid[amountColumn.Index, row].Value = item.TOTAL_AMOUNT_FROM_PO;
                    itemsDataGrid[notesColumn.Index, row].Value = "Wizard from PR# " + item.CODE;
                    ReCalculateNetTotal();
                }
            }
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
