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
    public partial class StockTakingForm : KryptonForm, IChildForm
    {
        StockTaking m_stocktaking = new StockTaking();
        IMainForm m_mainForm;
        Repository r_warehouse = RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.WAREHOUSE_REPOSITORY);
        Repository r_employee = RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.EMPLOYEE_REPOSITORY);
        PartRepository r_part = (PartRepository)RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.PART_REPOSITORY);
        Repository r_ccy = RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.CURRENCY_REPOSITORY);
        Repository r_unit = RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.UNIT_REPOSITORY);
        StockTakingRepository r_stocktaking = (StockTakingRepository)RepositoryFactory.GetInstance().GetTransactionRepository(RepositoryFactory.STOCKTAKING_REPOSITORY);
        IList m_units;

        public StockTakingForm(IMainForm mainForm, string formName)
        {
            InitializeComponent();
            InitializeButtonClick();
            InitializeDataSource();
            InitializeDataGridValidation();
            this.MdiParent = (Form)mainForm;
            this.Name = formName;
            m_mainForm = mainForm;
        }

        private void InitializeDataGridValidation()
        {
            dataItemskryptonDataGridView.CellValidating += new DataGridViewCellValidatingEventHandler(dataItemskryptonDataGridView_CellValidating);
            dataItemskryptonDataGridView.CellValidated += new DataGridViewCellEventHandler(dataItemskryptonDataGridView_CellValidated);
        }

        void dataItemskryptonDataGridView_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
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
            dataItemskryptonDataGridView.Rows[e.RowIndex].ErrorText = "";
            if (e.FormattedValue == dataItemskryptonDataGridView[scanColumn.Index, e.RowIndex].Tag) return;
            dataItemskryptonDataGridView[scanColumn.Index, e.RowIndex].Tag = e.FormattedValue;  
            if (e.ColumnIndex == scanColumn.Index)
            {
                if (e.FormattedValue.ToString() == "")
                    return;
                IList result = r_part.Search(e.FormattedValue.ToString());
                if (result.Count == 1)
                {
                    Part p = (Part)result[0];
                    dataItemskryptonDataGridView[codeColumn.Index, e.RowIndex].Tag = p;
                    dataItemskryptonDataGridView[codeColumn.Index, e.RowIndex].Value = p.CODE;
                    dataItemskryptonDataGridView[nameColumn.Index, e.RowIndex].Value = p.NAME;
                    dataItemskryptonDataGridView[QtyColumn.Index, e.RowIndex].Value = 0;
                    unitColumn.Items.Clear();
                    IList units = r_part.GetAllUnit(p.ID, p.UNIT.ID);
                    Utils.GetListCode(unitColumn.Items, units);
                    dataItemskryptonDataGridView[unitColumn.Index, e.RowIndex].Value = units[0].ToString(); ;
                    dataItemskryptonDataGridView[priceColumn.Index, e.RowIndex].Value = 0;
                    dataItemskryptonDataGridView[totalAmountColumn.Index, e.RowIndex].Value = 0;
                }
                if ((result.Count == 0) || (result.Count > 1))
                {
                    //Point point = dataItemskryptonDataGridView.Parent.PointToScreen(dataItemskryptonDataGridView.Location);
                    //Point xy = new Point(point.X, point.Y + (e.RowIndex == 0 ? 21 : (e.RowIndex * 21)));
                    using (SearchPartForm fr = new SearchPartForm(e.FormattedValue.ToString(), result))
                    {
                        fr.ShowDialog();
                        Part p = fr.PART;
                        if (p == null)
                        {
                            p = (Part)dataItemskryptonDataGridView[codeColumn.Index, e.RowIndex].Tag;
                            if (p == null)
                            {
                                dataItemskryptonDataGridView.Rows[e.RowIndex].ErrorText = "Pilih Part";
                                e.Cancel = true;
                            }
                        }
                        else
                        {
                            dataItemskryptonDataGridView[codeColumn.Index, e.RowIndex].Tag = p;
                            dataItemskryptonDataGridView[codeColumn.Index, e.RowIndex].Value = p.CODE;
                            dataItemskryptonDataGridView[nameColumn.Index, e.RowIndex].Value = p.NAME;
                            dataItemskryptonDataGridView[QtyColumn.Index, e.RowIndex].Value = 0;
                            unitColumn.Items.Clear();
                            IList units = r_part.GetAllUnit(p.ID, p.UNIT.ID);
                            Utils.GetListCode(unitColumn.Items, units);
                            dataItemskryptonDataGridView[unitColumn.Index, e.RowIndex].Value = units[0].ToString(); ;
                            dataItemskryptonDataGridView[priceColumn.Index, e.RowIndex].Value = 0;
                            dataItemskryptonDataGridView[totalAmountColumn.Index, e.RowIndex].Value = 0;
                        }
                    }

                }
            }
        }
        private void InitializeDataSource()
        {
            employeeKryptonComboBox.DataSource = r_employee.GetAll();
            warehousekryptonComboBox.DataSource = r_warehouse.GetAll();
            stocktakingTypekryptonComboBox.DataSource = Enum.GetValues(typeof(StockTakingType));
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
        }
        public void Save(object sender, EventArgs e)
        {
            try
            {
                if (Valid())
                {
                    this.Cursor = Cursors.WaitCursor;
                    UpdateEntity();
                    if (m_stocktaking.ID == 0)
                    {
                        r_stocktaking.Save(m_stocktaking);
                    }
                    else
                    {
                        r_stocktaking.Update(m_stocktaking);
                    }
                    KryptonMessageBox.Show("Record has been saved","Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        public bool Valid()
        {
            bool a = textBoxCode.Text == "";
            bool b = employeeKryptonComboBox.SelectedItem == null;
            bool c = warehousekryptonComboBox.SelectedItem == null;
            bool d = currencyKryptonComboBox.SelectedItem == null;
            bool e = false;
            
            if (a) errorProvider1.SetError(textBoxCode, "Code Can not Empty");
            if (b) errorProvider1.SetError(employeeKryptonComboBox, "Employee Can not Empty");
            if (c) errorProvider1.SetError(warehousekryptonComboBox, "Warehouse Can not Empty");
            if (d) errorProvider1.SetError(currencyKryptonComboBox, "Currency Can not Empty");

            for (int i = 0; i < dataItemskryptonDataGridView.Rows.Count; i++)
            {
                Part p = (Part)dataItemskryptonDataGridView[codeColumn.Index, i].Tag;
                if (dataItemskryptonDataGridView[unitColumn.Index, i].Value == null)
                    continue;
                Unit u = (Unit)Utils.FindEntityInList(dataItemskryptonDataGridView[unitColumn.Index, i].Value.ToString(), m_units);
                if ((p == null) || (u == null))
                    continue;
                double qty = Convert.ToDouble(dataItemskryptonDataGridView[QtyColumn.Index, i].Value);
                if (qty == 0)
                {
                    dataItemskryptonDataGridView.Rows[i].ErrorText = "Quantity must not 0(zero)";
                    e = true;
                }
            }
            return !a && !b && !c && !d && !e;
        }
        private void UpdateEntity()
        {
            m_stocktaking.CODE = textBoxCode.Text.Trim();
            m_stocktaking.TRANSACTION_DATE = dateKryptonDateTimePicker.Value;
            m_stocktaking.EMPLOYEE = (Employee)employeeKryptonComboBox.SelectedItem;
            m_stocktaking.WAREHOUSE = (Warehouse)warehousekryptonComboBox.SelectedItem;
            m_stocktaking.CURRENCY = (Currency)currencyKryptonComboBox.SelectedItem;
            m_stocktaking.AMOUNT = Convert.ToDouble(totalAmountkryptonNumericUpDown.Value);
            m_stocktaking.STOCK_TAKING_TYPE = (StockTakingType)Enum.Parse(typeof(StockTakingType), stocktakingTypekryptonComboBox.SelectedItem.ToString());
            m_stocktaking.NOTES = notesKryptonTextBox.Text;
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

                StockTakingItems st=(StockTakingItems)dataItemskryptonDataGridView.Rows[i].Tag;
                if(st==null)
                    st = new StockTakingItems();
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
                dateKryptonDateTimePicker.Value = DateTime.Today;
                employeeKryptonComboBox.SelectedIndex = 0;
                warehousekryptonComboBox.SelectedIndex = 0;
                currencyKryptonComboBox.SelectedIndex = 0;
                totalAmountkryptonNumericUpDown.Value = 0;
                stocktakingTypekryptonComboBox.SelectedIndex = 0;
                notesKryptonTextBox.Text = "";
                dataItemskryptonDataGridView.Rows.Clear();
                m_stocktaking = new StockTaking();
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
            dateKryptonDateTimePicker.Enabled = enable;
            employeeKryptonComboBox.Enabled = enable;
            warehousekryptonComboBox.Enabled = enable;
            currencyKryptonComboBox.Enabled = enable;
            totalAmountkryptonNumericUpDown.Enabled = enable;
            stocktakingTypekryptonComboBox.Enabled = enable;
            notesKryptonTextBox.Enabled = enable;
            dataItemskryptonDataGridView.Enabled = enable;
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
                if (m_stocktaking.ID > 0)
                {
                    this.Cursor = Cursors.WaitCursor;
                    if (KryptonMessageBox.Show("Are you sure to delete this record?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No) { this.Cursor = Cursors.Default; return; }
                    RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.BANK_REPOSITORY).Delete(m_stocktaking);
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
            stocktakingTypekryptonComboBox.Text = m_stocktaking.STOCK_TAKING_TYPE.ToString();
            foreach (StockTakingItems item in m_stocktaking.EVENT_ITEMS)
            {
                item.UNIT = (Unit)r_unit.GetById(item.UNIT);
                int i = dataItemskryptonDataGridView.Rows.Add();
                dataItemskryptonDataGridView.Rows[i].Tag = item;
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
            IList result = r_stocktaking.Search(searchToolStripTextBox.Text);
            if (result.Count == 1)
            {
                m_stocktaking = (StockTaking)result[0];
                m_stocktaking = (StockTaking)r_stocktaking.Get(m_stocktaking.ID);
                m_stocktaking.EMPLOYEE = (Employee)r_employee.GetById(m_stocktaking.EMPLOYEE);
                m_stocktaking.WAREHOUSE = (Warehouse)r_warehouse.GetById(m_stocktaking.WAREHOUSE);
                m_stocktaking.CURRENCY = (Currency)r_ccy.GetById(m_stocktaking.CURRENCY);
                loadData();
                setEnableForm(false);
            }
            else
            {
                using (SearchStockTakingForm frm = new SearchStockTakingForm(searchToolStripTextBox.Text, result))
                {
                    frm.ShowDialog();
                    m_stocktaking = frm.STOCK_TAKING;
                    if (m_stocktaking == null)
                    {
                        return;
                    }
                    else
                    {
                        m_stocktaking = (StockTaking)r_stocktaking.Get(m_stocktaking.ID);
                        m_stocktaking.EMPLOYEE = (Employee)r_employee.GetById(m_stocktaking.EMPLOYEE);
                        m_stocktaking.WAREHOUSE = (Warehouse)r_warehouse.GetById(m_stocktaking.WAREHOUSE);
                        m_stocktaking.CURRENCY = (Currency)r_ccy.GetById(m_stocktaking.CURRENCY);
                        loadData();
                        setEnableForm(false);
                    }
                }
            }
        }

        private void kryptonTextBox1_TextChanged(object sender, EventArgs e)
        {

        }      
    }
}
