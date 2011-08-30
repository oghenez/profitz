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
    public partial class PurchaseReturnForm : KryptonForm, IChildForm
    {
        PurchaseReturn m_prn = new PurchaseReturn(); 
        IMainForm m_mainForm;
        Repository r_top = RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.TOP_REPOSITORY);
        Repository r_division = RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.DIVISION_REPOSITORY);
        Repository r_tax = RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.TAX_REPOSITORY);
        EmployeeRepository r_employee = (EmployeeRepository)RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.EMPLOYEE_REPOSITORY);
        PartRepository r_part = (PartRepository)RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.PART_REPOSITORY);
        Repository r_ccy = RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.CURRENCY_REPOSITORY);
        Repository r_unit = RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.UNIT_REPOSITORY);
        Repository r_warehouse = RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.WAREHOUSE_REPOSITORY);
        Repository r_sup = RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.SUPPLIER_REPOSITORY);
        UserSettingsRepository r_setting = RepositoryFactory.GetInstance().UserSetting();
        PurchaseOrderRepository r_po = (PurchaseOrderRepository)RepositoryFactory.GetInstance().GetTransactionRepository(RepositoryFactory.PURCHASEORDER_REPOSITORY);
        GoodReceiveNoteRepository r_grn = (GoodReceiveNoteRepository)RepositoryFactory.GetInstance().GetTransactionRepository(RepositoryFactory.GOODRECEIVENOTE_REPOSITORY);
        PurchaseReturnRepository r_prn = (PurchaseReturnRepository)RepositoryFactory.GetInstance().GetTransactionRepository(RepositoryFactory.PURCHASE_RETURN_REPOSITORY);
        IList m_units;
        IList m_warehouses;
        IList m_poItems = new ArrayList();

        EditMode m_editMode = EditMode.New;
        bool m_enable = false;

        public PurchaseReturnForm(IMainForm mainForm, string formName)
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
            if (e.ColumnIndex == scanColumn.Index)
            {
                int count = 0;
                foreach (GoodReceiveNoteItem itm in m_poItems)
                {
                    if (count == 0)
                    {
                        itemsDataGrid[scanColumn.Index, e.RowIndex].Value = itm.EVENT.CODE;
                        itemsDataGrid[codeColumn.Index, e.RowIndex].Value = itm.PART.CODE;
                        itemsDataGrid[nameColumn.Index, e.RowIndex].Value = itm.PART.NAME;
                        itemsDataGrid[OutstandingPOColumn.Index, e.RowIndex].Value = itm.OUTSTANDING_AMOUNT_TO_PR;
                        itemsDataGrid[OutstandingunitColumn.Index, e.RowIndex].Value = itm.PART.UNIT.CODE;
                        itemsDataGrid[QtyColumn.Index, e.RowIndex].Value = 0;
                        itemsDataGrid[unitColumn.Index, e.RowIndex].Value = itm.UNIT.CODE;
                        itemsDataGrid[warehouseColumn.Index, e.RowIndex].Value = itm.WAREHOUSE.CODE;
                        itemsDataGrid[notesColumn.Index, e.RowIndex].Value = itm.NOTES;
                        itemsDataGrid[scanColumn.Index, e.RowIndex].Tag = itm;
                        itemsDataGrid[codeColumn.Index, e.RowIndex].Tag = itm.PART;
                    }
                    else
                    {
                        int row = itemsDataGrid.Rows.Add();
                        itemsDataGrid[scanColumn.Index, row].Value = itm.EVENT.CODE;
                        itemsDataGrid[codeColumn.Index, row].Value = itm.PART.CODE;
                        itemsDataGrid[nameColumn.Index, row].Value = itm.PART.NAME;
                        itemsDataGrid[OutstandingPOColumn.Index, row].Value = itm.OUTSTANDING_AMOUNT_TO_PR;
                        itemsDataGrid[OutstandingunitColumn.Index, row].Value = itm.PART.UNIT.CODE;
                        itemsDataGrid[QtyColumn.Index, row].Value = 0;
                        itemsDataGrid[unitColumn.Index, row].Value = itm.UNIT.CODE;
                        itemsDataGrid[warehouseColumn.Index, row].Value = itm.WAREHOUSE.CODE;
                        itemsDataGrid[notesColumn.Index, row].Value = itm.NOTES;
                        itemsDataGrid[scanColumn.Index, row].Tag = itm;
                        itemsDataGrid[codeColumn.Index, row].Tag = itm.PART;
                    }
                    count++;
                }
            }
        }

        void itemsDataGrid_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex == unitColumn.Index)
            {
                unitColumn.Items.Clear();
                Part p = (Part)itemsDataGrid[codeColumn.Index, e.RowIndex].Tag;
                if (p == null) return;
                IList units = r_part.GetAllUnit(p.ID, p.UNIT.ID);
                Utils.GetListCode(unitColumn.Items, units);
            }
        }

        void dataItemskryptonDataGridView_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            //if (m_editMode == EditMode.View) return;
            //if (!itemsDataGrid[e.ColumnIndex, e.RowIndex].IsInEditMode) return;
        }

        void dataItemskryptonDataGridView_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (m_editMode == EditMode.View) return;
            itemsDataGrid.Rows[e.RowIndex].ErrorText = "";
            if (!itemsDataGrid[e.ColumnIndex, e.RowIndex].IsInEditMode) return;

            if (e.ColumnIndex == scanColumn.Index)
            {
                if (e.FormattedValue.ToString() == "")return;

                IList addedPI = new ArrayList();
                for (int i = 0; i < itemsDataGrid.Rows.Count; i++)
                {
                    if (i == e.RowIndex) continue;
                    GoodReceiveNoteItem pi = (GoodReceiveNoteItem)itemsDataGrid[scanColumn.Index, i].Tag;
                    if (pi == null) continue;
                    addedPI.Add(pi.ID);
                }
                IList res = r_grn.FindPObyPartAndGRNNo(e.FormattedValue.ToString(), addedPI, ((Supplier)supplierkryptonComboBox.SelectedItem).ID, dateKryptonDateTimePicker.Value);
                if (res.Count == 0)
                {
                    using (SearchGRNForPRForm fr = new SearchGRNForPRForm(e.FormattedValue.ToString(), ((Supplier)supplierkryptonComboBox.SelectedItem).ID, addedPI, m_mainForm.CurrentUser, dateKryptonDateTimePicker.Value))
                    {
                        fr.ShowDialog();
                        IList result = fr.RESULT;
                        m_poItems = result;
                    }
                }
                else
                {
                    m_poItems = res;
                }
            }
            if (QtyColumn.Index == e.ColumnIndex)
            {
                GoodReceiveNoteItem pi = (GoodReceiveNoteItem)itemsDataGrid[scanColumn.Index, e.RowIndex].Tag;
                if (pi == null) return;
                Part p = (Part)itemsDataGrid[codeColumn.Index, e.RowIndex].Tag;
                if (p == null) return;
                Unit u = (Unit)Utils.FindEntityInList(itemsDataGrid[unitColumn.Index, e.RowIndex].Value.ToString(), m_units);
                if (u == null) return;
                p.UNIT_CONVERSION_LIST = r_part.GetUnitConversions(p.ID);
                PurchaseReturnItem sample = new PurchaseReturnItem();
                sample.PART = p;
                sample.UNIT = u;
                sample.QYTAMOUNT = Convert.ToDouble(e.FormattedValue);
                double qty = sample.GetAmountInSmallestUnit();
                double rest = pi.OUTSTANDING_AMOUNT_TO_PR - qty;
                if (rest < 0)
                {
                    e.Cancel = true;
                    itemsDataGrid.Rows[e.RowIndex].ErrorText = "Quantity exceed outstanding quantity";
                }
                itemsDataGrid[OutstandingPOColumn.Index, e.RowIndex].Value = rest;
            }
            if (unitColumn.Index == e.ColumnIndex)
            {
                GoodReceiveNoteItem pi = (GoodReceiveNoteItem)itemsDataGrid[scanColumn.Index, e.RowIndex].Tag;
                if (pi == null) return;
                Part p = (Part)itemsDataGrid[codeColumn.Index, e.RowIndex].Tag;
                if (p == null) return;
                Unit u = (Unit)Utils.FindEntityInList(e.FormattedValue.ToString(), m_units);
                if (u == null) return;
                p.UNIT_CONVERSION_LIST = r_part.GetUnitConversions(p.ID);
                GoodReceiveNoteItem sample = new GoodReceiveNoteItem();
                sample.PART = p;
                sample.UNIT = u;
                sample.QYTAMOUNT = Convert.ToDouble(itemsDataGrid[QtyColumn.Index, e.RowIndex].Value);
                double qty = sample.GetAmountInSmallestUnit();
                double rest = pi.OUTSTANDING_AMOUNT_TO_PR - qty;
                if (rest < 0)
                {
                    e.Cancel = true;
                    itemsDataGrid.Rows[e.RowIndex].ErrorText = "Quantity exceed outstanding quantity";
                }
                itemsDataGrid[OutstandingPOColumn.Index, e.RowIndex].Value = rest;
            }
        }
        private void InitializeDataSource()
        {
            employeeKryptonComboBox.DataSource = r_employee.GetAllStoreman();
            supplierkryptonComboBox.DataSource = r_sup.GetAll();
            m_units = r_unit.GetAll();
            m_warehouses = r_warehouse.GetAll();
            Utils.GetListCode(warehouseColumn.Items, m_warehouses);
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
                    r_prn.Revise(m_prn.ID);
                    m_prn.POSTED = false;
                    KryptonMessageBox.Show("Transaction has been UNPOSTED", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    r_prn.Confirm(m_prn.ID);
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
                        r_prn.Save(m_prn);
                    }
                    else
                    {
                        r_prn.Update(m_prn);
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
            bool a = textBoxCode.Text == "" && !r_prn.IsAutoNumber();
            bool b = employeeKryptonComboBox.SelectedItem == null;
            bool k = supplierkryptonComboBox.SelectedItem == null;
            bool e = false;
            bool f = m_prn.ID > 0 ? false : r_prn.IsCodeExist(textBoxCode.Text);
            
            if (a) errorProvider1.SetError(textBoxCode, "Code Can not Empty");
            if (b) errorProvider1.SetError(employeeKryptonComboBox, "Employee Can not Empty");
            if (k) errorProvider1.SetError(supplierkryptonComboBox, "Supplier Can not Empty");
            if (f) errorProvider1.SetError(textBoxCode, a ? "Code Can not Empty & Code already used" : "Code already used");

            int j = 0;
            for (int i = 0; i < itemsDataGrid.Rows.Count; i++)
            {
                GoodReceiveNoteItem pi = (GoodReceiveNoteItem)itemsDataGrid[scanColumn.Index, i].Tag;
                if (pi == null) continue;
                Part p = (Part)itemsDataGrid[codeColumn.Index, i].Tag;
                if (p == null) continue;
                if (itemsDataGrid[unitColumn.Index, i].Value == null)
                {
                    itemsDataGrid.Rows[i].ErrorText = "Please choose unit.";
                    e = true;
                }
                //if (itemsDataGrid[warehouseColumn.Index, i].Value == null)
                //{
                //    itemsDataGrid.Rows[i].ErrorText = "Please choose warehouse.";
                //    e = true;
                //}
                double qty = Convert.ToDouble(itemsDataGrid[QtyColumn.Index, i].Value);
                if (qty == 0)
                {
                    itemsDataGrid.Rows[i].ErrorText = itemsDataGrid.Rows[i].ErrorText+" Quantity must not 0(zero)";
                    e = true;
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
            m_prn.SUPPLIER = (Supplier)supplierkryptonComboBox.SelectedItem;
            m_prn.DOCUMENT_DATE = docdatekryptonDateTimePicker.Value;
            m_prn.DOCUMENT_NO = docnokryptonTextBox.Text;
            m_prn.EVENT_ITEMS = getItems();
        }

        private IList getItems()
        {
            IList items = new ArrayList();
            for (int i = 0; i < itemsDataGrid.Rows.Count; i++)
            {
                GoodReceiveNoteItem poi = (GoodReceiveNoteItem)itemsDataGrid[scanColumn.Index, i].Tag;
                if (poi == null) continue;
                Part p = (Part)itemsDataGrid[codeColumn.Index, i].Tag;
                if (itemsDataGrid[unitColumn.Index, i].Value == null) continue;
                Unit u = (Unit)Utils.FindEntityInList(itemsDataGrid[unitColumn.Index, i].Value.ToString(), m_units);
                if ((p == null) || (u == null)) continue;
                PurchaseReturnItem st=(PurchaseReturnItem)itemsDataGrid.Rows[i].Tag;
                if(st==null) st = new PurchaseReturnItem();
                itemsDataGrid.Rows[i].Tag = st;
                st.EVENT = m_prn;
                st.PART = p;
                st.WAREHOUSE = (Warehouse)Utils.FindEntityInList(itemsDataGrid[warehouseColumn.Index, i].Value.ToString(), m_warehouses);
                st.QYTAMOUNT = Convert.ToDouble(itemsDataGrid[QtyColumn.Index, i].Value);
                st.UNIT = u;
                st.GRN_ITEM = poi;
                st.NOTES = itemsDataGrid[notesColumn.Index, i].Value == null ? "" : itemsDataGrid[notesColumn.Index, i].Value.ToString();
                if (st.QYTAMOUNT == 0) continue;
                items.Add(st);
            }
            return items;
        }
        public void ClearForm()
        {
            try
            {
                m_prn = new PurchaseReturn();
                textBoxCode.Text = "";
                dateKryptonDateTimePicker.Value = DateTime.Today;
                employeeKryptonComboBox.SelectedIndex = 0;
                notesKryptonTextBox.Text = "";
                supplierkryptonComboBox.SelectedIndex = 0;
                docdatekryptonDateTimePicker.Value = DateTime.Today;
                docnokryptonTextBox.Text = "";
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
            textBoxCode.ReadOnly = r_prn.IsAutoNumber()?true:!enable;
            dateKryptonDateTimePicker.Enabled = enable;
            employeeKryptonComboBox.Enabled = enable;
            notesKryptonTextBox.ReadOnly = !enable;
            supplierkryptonComboBox.Enabled = enable;
            itemsDataGrid.AllowUserToDeleteRows = enable;
            itemsDataGrid.AllowUserToAddRows = enable;
            scanColumn.ReadOnly = !enable;
            QtyColumn.ReadOnly = !enable;
            unitColumn.ReadOnly = !enable;
            //warehouseColumn.ReadOnly = !enable;
            notesColumn.ReadOnly = !enable;
            docnokryptonTextBox.ReadOnly = !enable;
            docdatekryptonDateTimePicker.Enabled = enable;
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
                    r_prn.Delete(m_prn);
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
            employeeKryptonComboBox.Text = m_prn.EMPLOYEE.ToString();
            notesKryptonTextBox.Text = m_prn.NOTES;
            supplierkryptonComboBox.Text = m_prn.SUPPLIER.ToString();
            docnokryptonTextBox.Text = m_prn.DOCUMENT_NO;
            docdatekryptonDateTimePicker.Value = m_prn.DOCUMENT_DATE;
            itemsDataGrid.Rows.Clear();
            foreach (PurchaseReturnItem item in m_prn.EVENT_ITEMS)
            {
                item.UNIT = (Unit)r_unit.GetById(item.UNIT);
                item.GRN_ITEM.UNIT = (Unit)r_unit.GetById(item.GRN_ITEM.UNIT);
                item.GRN_ITEM.PART.UNIT = (Unit)r_unit.GetById(item.GRN_ITEM.PART.UNIT);
                int i = itemsDataGrid.Rows.Add();
                itemsDataGrid.Rows[i].Tag = item;
                itemsDataGrid[codeColumn.Index, i].Tag = item.PART;
                itemsDataGrid[scanColumn.Index, i].Tag = item.GRN_ITEM;
                itemsDataGrid[scanColumn.Index, i].Value = item.GRN_ITEM.EVENT.CODE;
                itemsDataGrid[codeColumn.Index, i].Value = item.PART.CODE;
                itemsDataGrid[nameColumn.Index, i].Value = item.PART.NAME;
                itemsDataGrid[QtyColumn.Index, i].Value = item.QYTAMOUNT;
                itemsDataGrid[warehouseColumn.Index, i].Value = r_warehouse.GetById(item.WAREHOUSE).ToString();
                itemsDataGrid[notesColumn.Index, i].Value = item.NOTES;
                itemsDataGrid[unitColumn.Index, i].Value = item.UNIT.ToString(); ;
                itemsDataGrid[OutstandingPOColumn.Index, i].Value = item.GRN_ITEM.OUTSTANDING_AMOUNT_TO_PR;
                itemsDataGrid[OutstandingunitColumn.Index, i].Value = item.GRN_ITEM.PART.UNIT.CODE;

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
            IList result = searchToolStripTextBox.Text == string.Empty ? new ArrayList() : r_prn.Search(searchToolStripTextBox.Text);
            if (result.Count == 1)
            {
                m_prn = (PurchaseReturn)result[0];
                m_prn = (PurchaseReturn)r_prn.Get(m_prn.ID);
                m_prn.EMPLOYEE = (Employee)r_employee.GetById(m_prn.EMPLOYEE);
                m_prn.SUPPLIER = (Supplier)r_sup.GetById(m_prn.SUPPLIER);
                setEditMode(EditMode.View);
                loadData();
                setEnableForm(false);
            }
            else
            {
                using (SearchPurchaseReturnForm frm = new SearchPurchaseReturnForm(searchToolStripTextBox.Text, result))
                {
                    frm.ShowDialog();
                    if (frm.PURCHASE_RETURN == null)
                    {
                        return;
                    }
                    else
                    {
                        m_prn = frm.PURCHASE_RETURN;
                        m_prn = (PurchaseReturn)r_prn.Get(m_prn.ID);
                        m_prn.EMPLOYEE = (Employee)r_employee.GetById(m_prn.EMPLOYEE);
                        m_prn.SUPPLIER = (Supplier)r_sup.GetById(m_prn.SUPPLIER);
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
    }
}
