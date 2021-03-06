﻿using System;
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
    public partial class OpeningStockForm : KryptonForm, IChildForm
    {
        OpeningStock m_stocktaking = new OpeningStock();
        IMainForm m_mainForm;
        Repository r_warehouse = RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.WAREHOUSE_REPOSITORY);
        Repository r_employee = RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.EMPLOYEE_REPOSITORY);
        PartRepository r_part = (PartRepository)RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.PART_REPOSITORY);
        Repository r_ccy = RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.CURRENCY_REPOSITORY);
        Repository r_unit = RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.UNIT_REPOSITORY);
        OpeningStockRepository r_openingstock = (OpeningStockRepository)RepositoryFactory.GetInstance().GetTransactionRepository(RepositoryFactory.OPENING_STOCK_REPOSITORY);
        IList m_units;
        EditMode m_editMode = EditMode.New;

        public OpeningStockForm(IMainForm mainForm, string formName)
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
            dataItemskryptonDataGridView.CellValidating += new DataGridViewCellValidatingEventHandler(dataItemskryptonDataGridView_CellValidating);
            dataItemskryptonDataGridView.CellValidated += new DataGridViewCellEventHandler(dataItemskryptonDataGridView_CellValidated);
            dataItemskryptonDataGridView.CellBeginEdit+=new DataGridViewCellCancelEventHandler(itemsDataGrid_CellBeginEdit);
        }
        void itemsDataGrid_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.ColumnIndex == unitColumn.Index)
            {
                Part p = (Part)dataItemskryptonDataGridView[codeColumn.Index, e.RowIndex].Tag;
                if (p == null) return;
                unitColumn.Items.Clear();
                IList units = r_part.GetAllUnit(p.ID, p.UNIT.ID);
                Utils.GetListCode(unitColumn.Items, units);
            }
        }
        void dataItemskryptonDataGridView_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            if (m_editMode == EditMode.View) return;
            if (!dataItemskryptonDataGridView[e.ColumnIndex, e.RowIndex].IsInEditMode) return;
            if ((e.ColumnIndex == priceColumn.Index) || (e.ColumnIndex == QtyColumn.Index))
            {
                decimal qty = Convert.ToDecimal(dataItemskryptonDataGridView[QtyColumn.Index, e.RowIndex].Value);
                decimal price = Convert.ToDecimal(dataItemskryptonDataGridView[priceColumn.Index, e.RowIndex].Value);
                dataItemskryptonDataGridView[totalAmountColumn.Index, e.RowIndex].Value = qty * price;
                updateTotalAmount();
            }
        }

        private void updateTotalAmount()
        {
            decimal amttotal = 0;
            for (int i = 0; i < dataItemskryptonDataGridView.Rows.Count; i++)
            {
                decimal price = Convert.ToDecimal(dataItemskryptonDataGridView[totalAmountColumn.Index, i].Value);
                amttotal += price;
            }
            totalAmountkryptonNumericUpDown.Value = amttotal;
        }

        void dataItemskryptonDataGridView_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (m_editMode == EditMode.View) return;
            dataItemskryptonDataGridView.Rows[e.RowIndex].ErrorText = "";
            if (!dataItemskryptonDataGridView[e.ColumnIndex, e.RowIndex].IsInEditMode) return;
            if (e.ColumnIndex == scanColumn.Index)
            {
                if (!((DataGridViewTextBoxCell)dataItemskryptonDataGridView[scanColumn.Index, e.RowIndex]).IsInEditMode)return;
                if (e.FormattedValue.ToString() == "")return;
                IList result = r_part.SearchActivePart(e.FormattedValue.ToString(), true);
                if (result.Count == 1)
                {
                    Part p = (Part)result[0];
                    for (int i = 0; i < dataItemskryptonDataGridView.Rows.Count; i++)
                    {
                        if (i == e.RowIndex) continue;
                        Part pi = (Part)dataItemskryptonDataGridView[codeColumn.Index, i].Tag;
                        if (pi == null) continue;
                        if (pi.ID == p.ID)
                        {
                            dataItemskryptonDataGridView.Rows[e.RowIndex].ErrorText = "Part : "+p.NAME+" already add.";
                            e.Cancel = true;
                            return;
                        }
                    }
                    dataItemskryptonDataGridView[codeColumn.Index, e.RowIndex].Tag = p;
                    dataItemskryptonDataGridView[codeColumn.Index, e.RowIndex].Value = p.CODE;
                    dataItemskryptonDataGridView[nameColumn.Index, e.RowIndex].Value = p.NAME;
                    //dataItemskryptonDataGridView[QtyColumn.Index, e.RowIndex].Value = 0;
                    unitColumn.Items.Clear();
                    IList units = r_part.GetAllUnit(p.ID, p.UNIT.ID);
                    Utils.GetListCode(unitColumn.Items, units);
                    dataItemskryptonDataGridView[unitColumn.Index, e.RowIndex].Value = units[0].ToString(); ;
                    //dataItemskryptonDataGridView[priceColumn.Index, e.RowIndex].Value = 0;
                    //dataItemskryptonDataGridView[totalAmountColumn.Index, e.RowIndex].Value = 0;
                }
                if ((result.Count == 0) || (result.Count > 1))
                {
                    using (SearchPartForm fr = new SearchPartForm(e.FormattedValue.ToString(), result))
                    {
                        fr.ShowDialog();
                        Part p = fr.PART;
                        if (p == null)
                        {
                            p = (Part)dataItemskryptonDataGridView[codeColumn.Index, e.RowIndex].Tag;
                            if (p == null)
                            {
                                e.Cancel = true;
                                return;
                            }
                        }
                        else
                        {
                            for (int i = 0; i < dataItemskryptonDataGridView.Rows.Count; i++)
                            {
                                if (i == e.RowIndex) continue;
                                Part pi = (Part)dataItemskryptonDataGridView[codeColumn.Index, i].Tag;
                                if (pi == null) continue;
                                if (pi.ID == p.ID)
                                {
                                    dataItemskryptonDataGridView.Rows[e.RowIndex].ErrorText = "Part : " + p.NAME + " already add.";
                                    e.Cancel = true;
                                    return;
                                }
                            }
                            dataItemskryptonDataGridView[codeColumn.Index, e.RowIndex].Tag = p;
                            dataItemskryptonDataGridView[codeColumn.Index, e.RowIndex].Value = p.CODE;
                            dataItemskryptonDataGridView[nameColumn.Index, e.RowIndex].Value = p.NAME;
                            //dataItemskryptonDataGridView[QtyColumn.Index, e.RowIndex].Value = 0;
                            unitColumn.Items.Clear();
                            IList units = r_part.GetAllUnit(p.ID, p.UNIT.ID);
                            Utils.GetListCode(unitColumn.Items, units);
                            dataItemskryptonDataGridView[unitColumn.Index, e.RowIndex].Value = units[0].ToString(); ;
                            //dataItemskryptonDataGridView[priceColumn.Index, e.RowIndex].Value = 0;
                           // dataItemskryptonDataGridView[totalAmountColumn.Index, e.RowIndex].Value = 0;
                        }
                    }

                }
            }
        }
        private void InitializeDataSource()
        {
            employeeKryptonComboBox.DataSource = r_employee.GetAll();
            warehousekryptonComboBox.DataSource = r_warehouse.GetAll();
            currencyKryptonComboBox.DataSource = r_ccy.GetAll();
            m_units = r_unit.GetAll();
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
                if (m_stocktaking.POSTED)
                {
                    r_openingstock.Revise(m_stocktaking.ID);
                    m_stocktaking.POSTED = false;
                    KryptonMessageBox.Show("Transaction has been UNPOSTED", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    r_openingstock.Confirm(m_stocktaking.ID);
                    m_stocktaking.POSTED = true;
                    KryptonMessageBox.Show("Transaction has been POSTED", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                //m_stocktaking = (OpeningStock)r_openingstock.Get(m_stocktaking.ID);
                //m_stocktaking.EMPLOYEE = (Employee)r_employee.GetById(m_stocktaking.EMPLOYEE);
                //m_stocktaking.WAREHOUSE = (Warehouse)r_warehouse.GetById(m_stocktaking.WAREHOUSE);
                //m_stocktaking.CURRENCY = (Currency)r_ccy.GetById(m_stocktaking.CURRENCY);
                //loadData();
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
                dataItemskryptonDataGridView.RefreshEdit();
                if (Valid())
                {
                    this.Cursor = Cursors.WaitCursor;
                    UpdateEntity();
                    if (m_stocktaking.ID == 0)
                    {
                        r_openingstock.Save(m_stocktaking);
                    }
                    else
                    {
                        r_openingstock.Update(m_stocktaking);
                    }
                    KryptonMessageBox.Show("Transaction '" + m_stocktaking.CODE + "' Record has been saved", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //ClearForm();
                    textBoxCode.Text = m_stocktaking.CODE;
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
            bool a = textBoxCode.Text == "" && !r_openingstock.IsAutoNumber();
            bool b = employeeKryptonComboBox.SelectedItem == null;
            bool c = warehousekryptonComboBox.SelectedItem == null;
            bool d = currencyKryptonComboBox.SelectedItem == null;
            bool e = false;
            bool f = m_stocktaking.ID > 0 ? false : r_openingstock.IsCodeExist(textBoxCode.Text);
            
            if (a) errorProvider1.SetError(textBoxCode, "Code Can not Empty");
            if (b) errorProvider1.SetError(employeeKryptonComboBox, "Employee Can not Empty");
            if (c) errorProvider1.SetError(warehousekryptonComboBox, "Warehouse Can not Empty");
            if (d) errorProvider1.SetError(currencyKryptonComboBox, "Currency Can not Empty");
            if (f) errorProvider1.SetError(textBoxCode, a ? "Code Can not Empty & Code already used" : "Code already used");

            int j = 0;
            for (int i = 0; i < dataItemskryptonDataGridView.Rows.Count; i++)
            {
                Part p = (Part)dataItemskryptonDataGridView[codeColumn.Index, i].Tag;
              //  if (dataItemskryptonDataGridView[unitColumn.Index, i].Value == null)
              //      continue;
               // Unit u = (Unit)Utils.FindEntityInList(dataItemskryptonDataGridView[unitColumn.Index, i].Value.ToString(), m_units);
                if (p == null)
                    continue;
                if (dataItemskryptonDataGridView[unitColumn.Index, i].Value == null)
                {
                    dataItemskryptonDataGridView.Rows[i].ErrorText = "Please choose unit.";
                    e = true;
                }
                double qty = Convert.ToDouble(dataItemskryptonDataGridView[QtyColumn.Index, i].Value);
                if (qty == 0)
                {
                    dataItemskryptonDataGridView.Rows[i].ErrorText = dataItemskryptonDataGridView.Rows[i].ErrorText+" Quantity must not 0(zero)";
                    e = true;
                }
                j++;
            }

            bool g = j == 0;
            if (g) errorProvider1.SetError(dataItemskryptonDataGridView,"Items must at least 1(one)");
            return !a && !b && !c && !d && !e && !f && !g;
        }
        private void UpdateEntity()
        {
            m_stocktaking.CODE = textBoxCode.Text.Trim();
            m_stocktaking.TRANSACTION_DATE = dateKryptonDateTimePicker.Value;
            m_stocktaking.EMPLOYEE = (Employee)employeeKryptonComboBox.SelectedItem;
            m_stocktaking.WAREHOUSE = (Warehouse)warehousekryptonComboBox.SelectedItem;
            m_stocktaking.CURRENCY = (Currency)currencyKryptonComboBox.SelectedItem;
            m_stocktaking.AMOUNT = Convert.ToDouble(totalAmountkryptonNumericUpDown.Value);
            m_stocktaking.NOTES = notesKryptonTextBox.Text;
            m_stocktaking.MODIFIED_BY = m_mainForm.CurrentUser.NAME;
            m_stocktaking.MODIFIED_COMPUTER_NAME = Environment.MachineName;
            m_stocktaking.EVENT_ITEMS = getItems();
        }

        private IList getItems()
        {
            IList items = new ArrayList();
            for (int i = 0; i < dataItemskryptonDataGridView.Rows.Count; i++)
            {
                Part p = (Part)dataItemskryptonDataGridView[codeColumn.Index, i].Tag;
                if (dataItemskryptonDataGridView[unitColumn.Index, i].Value == null)
                    continue;
                Unit u = (Unit)Utils.FindEntityInList(dataItemskryptonDataGridView[unitColumn.Index, i].Value.ToString(), m_units);
                if ((p == null) || (u == null))
                    continue;

                OpeningStockItem st=(OpeningStockItem)dataItemskryptonDataGridView.Rows[i].Tag;
                if(st==null)
                    st = new OpeningStockItem();
                dataItemskryptonDataGridView.Rows[i].Tag = st;
                st.EVENT = m_stocktaking;
                st.PART = p;
                st.WAREHOUSE = m_stocktaking.WAREHOUSE;
                st.QYTAMOUNT = Convert.ToDouble(dataItemskryptonDataGridView[QtyColumn.Index, i].Value);
                st.UNIT = u;
                st.PRICE = Convert.ToDouble(dataItemskryptonDataGridView[priceColumn.Index,i].Value);
                st.TOTAL_AMOUNT = Convert.ToDouble(dataItemskryptonDataGridView[totalAmountColumn.Index, i].Value);
                
                if (st.QYTAMOUNT == 0) continue;
                items.Add(st);
            }
            return items;
        }
        public void ClearForm()
        {
            try
            {
                textBoxCode.Text = "";
                dateKryptonDateTimePicker.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                employeeKryptonComboBox.SelectedIndex = 0;
                warehousekryptonComboBox.SelectedIndex = 0;
                currencyKryptonComboBox.SelectedIndex = 0;
                totalAmountkryptonNumericUpDown.Value = 0;
                notesKryptonTextBox.Text = "";
                dataItemskryptonDataGridView.Rows.Clear();
                m_stocktaking = new OpeningStock();
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
            textBoxCode.ReadOnly = r_openingstock.IsAutoNumber()?true:!enable;
            //dateKryptonDateTimePicker.Enabled = enable;
            employeeKryptonComboBox.Enabled = enable;
            warehousekryptonComboBox.Enabled = enable;
            currencyKryptonComboBox.Enabled = enable;
            //totalAmountkryptonNumericUpDown.Enabled = enable;
            notesKryptonTextBox.ReadOnly = !enable;
            //dataItemskryptonDataGridView.Enabled = enable;
            dataItemskryptonDataGridView.AllowUserToDeleteRows = enable;
            dataItemskryptonDataGridView.AllowUserToAddRows = enable;
            scanColumn.ReadOnly = !enable;
            QtyColumn.ReadOnly = !enable;
            unitColumn.ReadOnly = !enable;
            priceColumn.ReadOnly = !enable;
        }
        private void setEditMode(EditMode editmode)
        {
            toolStripButtonSave.Enabled = (editmode == EditMode.New || editmode == EditMode.Update) && !m_stocktaking.POSTED && m_mainForm.CurrentUser.FORM_ACCESS_LIST[Name].SAVE;
            toolStripButtonEdit.Enabled = (editmode == EditMode.View) && !m_stocktaking.POSTED && m_mainForm.CurrentUser.FORM_ACCESS_LIST[Name].SAVE;
            toolStripButtonDelete.Enabled = (editmode == EditMode.View) && !m_stocktaking.POSTED && m_mainForm.CurrentUser.FORM_ACCESS_LIST[Name].DELETE;
            toolStripButtonClear.Enabled = m_mainForm.CurrentUser.FORM_ACCESS_LIST[Name].SAVE;
            toolStripButtonPrint.Enabled = m_stocktaking.POSTED && m_mainForm.CurrentUser.FORM_ACCESS_LIST[Name].PRINT;
            postToolStripButton.Enabled = (m_stocktaking.ID > 0) && (editmode == EditMode.View) && m_mainForm.CurrentUser.FORM_ACCESS_LIST[Name].POST;
            postToolStripButton.Text = m_stocktaking.POSTED ? "Unpost" : "Post";
            statusKryptonLabel.Text = m_stocktaking.POSTED ? "POSTED" : "ENTRY";
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
                if (m_stocktaking.ID > 0)
                {
                    this.Cursor = Cursors.WaitCursor;
                    if (KryptonMessageBox.Show("Are you sure to delete this record?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No) { this.Cursor = Cursors.Default; return; }
                    r_openingstock.Delete(m_stocktaking);
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
            //gridData.ClearSelection();
            ClearForm();
            setEnableForm(true);
            setEditMode(EditMode.New);
            textBoxCode.Focus();
        }
        private void loadData()
        {
            textBoxCode.Text = m_stocktaking.CODE;
            dateKryptonDateTimePicker.Value = m_stocktaking.TRANSACTION_DATE;
            employeeKryptonComboBox.Text = m_stocktaking.EMPLOYEE.ToString();
            warehousekryptonComboBox.Text = m_stocktaking.WAREHOUSE.ToString();
            currencyKryptonComboBox.Text = m_stocktaking.CURRENCY.ToString();
            totalAmountkryptonNumericUpDown.Value = Convert.ToDecimal(m_stocktaking.AMOUNT);
            dataItemskryptonDataGridView.Rows.Clear();
            foreach (OpeningStockItem item in m_stocktaking.EVENT_ITEMS)
            {
                item.UNIT = (Unit)r_unit.GetById(item.UNIT);
                int i = dataItemskryptonDataGridView.Rows.Add();
                dataItemskryptonDataGridView.Rows[i].Tag = item;
                dataItemskryptonDataGridView[scanColumn.Index, i].Value = item.PART.BARCODE;
                dataItemskryptonDataGridView[codeColumn.Index, i].Tag = item.PART;
                dataItemskryptonDataGridView[codeColumn.Index, i].Value = item.PART.CODE;
                dataItemskryptonDataGridView[nameColumn.Index, i].Value = item.PART.NAME;
                dataItemskryptonDataGridView[QtyColumn.Index, i].Value = item.QYTAMOUNT;
                unitColumn.Items.Clear();
                IList units = r_part.GetAllUnit(item.PART.ID, item.PART.UNIT.ID);
                Utils.GetListCode(unitColumn.Items, units);
                dataItemskryptonDataGridView[unitColumn.Index, i].Value = item.UNIT.ToString(); ;
                dataItemskryptonDataGridView[priceColumn.Index, i].Value = item.PRICE;
                dataItemskryptonDataGridView[totalAmountColumn.Index, i].Value = item.TOTAL_AMOUNT;
            }
        }
        public void Refresh(object sender, EventArgs e)
        {
            //loadRecords(); 
            //gridData.ClearSelection(); 
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
        private void warehousekryptonComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Warehouse em = (Warehouse)warehousekryptonComboBox.SelectedItem;
            warehousekryptonTextBox.Text = em.NAME;
        }

        private void searchToolStripButton_Click(object sender, EventArgs e)
        {
            IList result = searchToolStripTextBox.Text == string.Empty?new ArrayList() : r_openingstock.Search(searchToolStripTextBox.Text);
            if (result.Count == 1)
            {
                m_stocktaking = (OpeningStock)result[0];
                m_stocktaking = (OpeningStock)r_openingstock.Get(m_stocktaking.ID);
                m_stocktaking.EMPLOYEE = (Employee)r_employee.GetById(m_stocktaking.EMPLOYEE);
                m_stocktaking.WAREHOUSE = (Warehouse)r_warehouse.GetById(m_stocktaking.WAREHOUSE);
                m_stocktaking.CURRENCY = (Currency)r_ccy.GetById(m_stocktaking.CURRENCY);
                loadData();
                setEnableForm(false);
                setEditMode(EditMode.View);
            }
            else
            {
                using (SearchOpeningStockForm frm = new SearchOpeningStockForm(searchToolStripTextBox.Text, result))
                {
                    frm.ShowDialog();
                    if (frm.OPENING_STOCK == null)
                    {
                        return;
                    }
                    else
                    {
                        m_stocktaking = frm.OPENING_STOCK;
                        m_stocktaking = (OpeningStock)r_openingstock.Get(m_stocktaking.ID);
                        m_stocktaking.EMPLOYEE = (Employee)r_employee.GetById(m_stocktaking.EMPLOYEE);
                        m_stocktaking.WAREHOUSE = (Warehouse)r_warehouse.GetById(m_stocktaking.WAREHOUSE);
                        m_stocktaking.CURRENCY = (Currency)r_ccy.GetById(m_stocktaking.CURRENCY);
                        loadData();
                        setEnableForm(false);
                        setEditMode(EditMode.View);
                    }
                }
            }
        }

        private void OpeningStockForm_Load(object sender, EventArgs e)
        {
            UserSetting.LoadSetting(dataItemskryptonDataGridView, m_mainForm.CurrentUser.ID, this.Name);
        }

        private void OpeningStockForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            UserSetting.SaveSetting(dataItemskryptonDataGridView, m_mainForm.CurrentUser.ID, this.Name);
        }

        private void dataItemskryptonDataGridView_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            for (int count = 0; (count <= (dataItemskryptonDataGridView.Rows.Count - 2)); count++)
            {
                dataItemskryptonDataGridView.Rows[count].HeaderCell.Value = string.Format((count + 1).ToString(), "0");
                dataItemskryptonDataGridView.Rows[count].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
        }
        public void WizardFromPart(Part p)
        {
            Clear(null, null);
            int row = dataItemskryptonDataGridView.Rows.Add();
            dataItemskryptonDataGridView[codeColumn.Index, row].Tag = p;
            dataItemskryptonDataGridView[codeColumn.Index, row].Value = p.CODE;
            dataItemskryptonDataGridView[nameColumn.Index, row].Value = p.NAME;
            //dataItemskryptonDataGridView[QtyColumn.Index, e.RowIndex].Value = 0;
            unitColumn.Items.Clear();
            IList units = r_part.GetAllUnit(p.ID, p.UNIT.ID);
            Utils.GetListCode(unitColumn.Items, units);
            dataItemskryptonDataGridView[unitColumn.Index, row].Value = units[0].ToString(); ;
            //dataItemskryptonDataGridView[priceColumn.Index, e.RowIndex].Value = 0;
            //dataItemskryptonDataGridView[totalAmountColumn.Index, e.RowIndex].Value = 0;
        }

        private void exittoolStripButton1_Click(object sender, EventArgs e)
        {
            if (KryptonMessageBox.Show("Are you sure to Exit this Form?", "Exit Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                this.Close();
        }
    }
}
