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
    public partial class SalesOrderForm : KryptonForm, IChildForm
    {
        SalesOrder m_po = new SalesOrder(); 
        IMainForm m_mainForm;
        Repository r_top = RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.TOP_REPOSITORY);
        Repository r_division = RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.DIVISION_REPOSITORY);
        Repository r_tax = RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.TAX_REPOSITORY);
        EmployeeRepository r_employee = (EmployeeRepository)RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.EMPLOYEE_REPOSITORY);
        PartRepository r_part = (PartRepository)RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.PART_REPOSITORY);
        Repository r_ccy = RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.CURRENCY_REPOSITORY);
        Repository r_unit = RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.UNIT_REPOSITORY);
        Repository r_warehouse = RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.WAREHOUSE_REPOSITORY);
        CustomerRepository r_cus = (CustomerRepository)RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.CUSTOMER_REPOSITORY);
        UserSettingsRepository r_setting = RepositoryFactory.GetInstance().UserSetting();

        SalesOrderRepository r_po = (SalesOrderRepository)RepositoryFactory.GetInstance().GetTransactionRepository(RepositoryFactory.SALES_ORDER_REPOSITORY);
        IList m_units;
        IList m_warehouses;

        EditMode m_editMode = EditMode.New;
        bool m_enable = false;

        public SalesOrderForm(IMainForm mainForm, string formName)
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
            if (m_editMode == EditMode.View) return;
            if (!itemsDataGrid[e.ColumnIndex, e.RowIndex].IsInEditMode) return;
            if ((e.ColumnIndex == priceColumn.Index) || (e.ColumnIndex == QtyColumn.Index)
                || (e.ColumnIndex == discpercentColumn.Index) || (e.ColumnIndex == discAmountColumn.Index)
                || (e.ColumnIndex == discabcColumn.Index)
                )
            {
                updateSubtotal(e.RowIndex);
            }
            if (e.ColumnIndex == unitColumn.Index)
            {
                Part p = (Part)itemsDataGrid[codeColumn.Index, e.RowIndex].Tag;
                object ou = itemsDataGrid[unitColumn.Index, e.RowIndex].Value;
                if ((p == null) || (ou == null)) return;
                Unit u = (Unit)Utils.FindEntityInList(ou.ToString(), m_units);
                itemsDataGrid[priceColumn.Index, e.RowIndex].Value = r_po.GetTheLatestPOPrice(((Customer)supplierkryptonComboBox.SelectedItem).ID, p.ID, u.ID);
                updateSubtotal(e.RowIndex);
            }
             
        }

        private void updateSubtotal(int row)
        {
            double q = Convert.ToDouble(itemsDataGrid[QtyColumn.Index, row].Value);
            double p = Convert.ToDouble(itemsDataGrid[priceColumn.Index, row].Value);
            double a = Convert.ToDouble(itemsDataGrid[discpercentColumn.Index, row].Value);
            double b = Convert.ToDouble(itemsDataGrid[discAmountColumn.Index, row].Value);
            string abc = itemsDataGrid[discabcColumn.Index, row].Value==null?"":itemsDataGrid[discabcColumn.Index, row].Value.ToString();
            double c = abc==""?0:splitDiscString(abc,0);
            double d = abc==""?0:splitDiscString(abc,0);
            double e = abc==""?0:splitDiscString(abc, 0);
            double totaldiscount = Utils.CalculateTotalDiscount(q, p, a, b, c, d, e, 2);
            double subtotal = Utils.CalculateSubTotal(q, p, totaldiscount, 2);
            itemsDataGrid[totalDiscColumn.Index,row].Value = totaldiscount;
            itemsDataGrid[totalAmountColumn.Index, row].Value = subtotal;
            ReCalculateNetTotal();
        }
        private void ReCalculateNetTotal()
        {
            IList totalsubTotalList = new ArrayList();
            for (int i = 0; i < itemsDataGrid.Rows.Count; i++)
            {
                double sbtol = Convert.ToDouble(itemsDataGrid[totalAmountColumn.Index, i].Value);
                if (sbtol == 0) continue;
                totalsubTotalList.Add(sbtol);
            }
            double totalsubTotal = Utils.CalculateSumList(totalsubTotalList, 2);
            subTotalKryptonNumericUpDown.Value = Convert.ToDecimal(totalsubTotal);
            CalculateDiscPercentTotal();
            CalculateTax();
            CalculateNetTotal();
        }
        public void CalculateDiscPercentTotal()
        {
            decimal subTotalAmount = subTotalKryptonNumericUpDown.Value;
            decimal discPercentTotal = discPercentKryptonNumericUpDown.Value;
            decimal disc = Utils.CalculateDiscountPercent(subTotalAmount, discPercentTotal, 2);
            discAfterAmountKryptonNumericUpDown.Value = disc;
            CalculateNetTotal();
        }
        public void CalculateNetTotal()
        {
            decimal subTotalAmount = subTotalKryptonNumericUpDown.Value;
            decimal discPercentAmount = discAfterAmountKryptonNumericUpDown.Value;
            decimal discAmount = discAmountkryptonNumericUpDown.Value;
            decimal taxAmount = taxAfterAmountkryptonNumericUpDown.Value;
            decimal expense = otherExpensekryptonNumericUpDown.Value;
            decimal netTotal = Utils.CalculateNetTotal(subTotalAmount, discPercentAmount, discAmount, taxAmount, expense, 2);
            nettotalAmountkryptonNumericUpDown.Value = netTotal;
        }
        public void CalculateTax()
        {
            Tax tax = (Tax)taxKryptonComboBox.SelectedItem;
            decimal taxAmount = 0m;
            if (tax == null)
                taxAmount = 0m;
            else
            {
                decimal subTotalAmount = subTotalKryptonNumericUpDown.Value;
                decimal discPercentAmount = discAfterAmountKryptonNumericUpDown.Value;
                decimal discAmount = discAmountkryptonNumericUpDown.Value;
                decimal netafterdisc = Utils.CalculateNetTotalWithoutTaxExpense(subTotalAmount, discPercentAmount, discAmount, 2);
                taxAmount = Utils.CalculateNetTotalTax(netafterdisc, Convert.ToDecimal(tax.RATE),2 );
            }
            taxAfterAmountkryptonNumericUpDown.Value = taxAmount;
            CalculateNetTotal();
        }
        private double splitDiscString(string abc, int d)
        {
            if (abc == "") return 0;
            string[] discs = abc.Split('+');
            if ((discs.Length < 3) || (discs.Length > 3))
            {
                throw new Exception("Not Valid Step Discount");
            }
            return Convert.ToDouble(discs[d]);
        }
        void dataItemskryptonDataGridView_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (m_editMode == EditMode.View) return;
            itemsDataGrid.Rows[e.RowIndex].ErrorText = "";
            if (!itemsDataGrid[e.ColumnIndex, e.RowIndex].IsInEditMode) return;
            if (e.ColumnIndex == scanColumn.Index)
            {
                if (!((DataGridViewTextBoxCell)itemsDataGrid[scanColumn.Index, e.RowIndex]).IsInEditMode)return;
                if (e.FormattedValue.ToString() == "")return;
                IList result = r_part.SearchActivePart(e.FormattedValue.ToString(),true);
                if (result.Count == 1)
                {
                    Part p = (Part)result[0];
                    for (int i = 0; i < itemsDataGrid.Rows.Count; i++)
                    {
                        if (i == e.RowIndex) continue;
                        Part pi = (Part)itemsDataGrid[codeColumn.Index, i].Tag;
                        if (pi == null) continue;
                        if (pi.ID == p.ID)
                        {
                            itemsDataGrid.Rows[e.RowIndex].ErrorText = "Part : "+p.NAME+" already add.";
                            e.Cancel = true;
                            return;
                        }
                    }
                    itemsDataGrid[codeColumn.Index, e.RowIndex].Tag = p;
                    itemsDataGrid[scanColumn.Index, e.RowIndex].Value = p.BARCODE;
                    itemsDataGrid[codeColumn.Index, e.RowIndex].Value = p.CODE;
                    itemsDataGrid[nameColumn.Index, e.RowIndex].Value = p.NAME;
                    //dataItemskryptonDataGridView[QtyColumn.Index, e.RowIndex].Value = 0;
                    //unitColumn.Items.Clear();
                   // IList units = r_part.GetAllUnit(p.ID, p.UNIT.ID);
                    //Utils.GetListCode(unitColumn.Items, units);
                    p.UNIT = (Unit)r_unit.GetById(p.UNIT);
                    itemsDataGrid[unitColumn.Index, e.RowIndex].Value = p.UNIT.ToString();
                    itemsDataGrid[priceColumn.Index, e.RowIndex].Value = r_po.GetTheLatestPOPrice(((Customer)supplierkryptonComboBox.SelectedItem).ID, p.ID, p.UNIT.ID);
                    //dataItemskryptonDataGridView[totalAmountColumn.Index, e.RowIndex].Value = 0;
                    itemsDataGrid[warehouseColumn.Index, e.RowIndex].Value = m_warehouses[0].ToString();

                }
                if ((result.Count == 0) || (result.Count > 1))
                {
                    using (SearchPartForm fr = new SearchPartForm(e.FormattedValue.ToString(), result))
                    {
                        fr.ShowDialog();
                        Part p = fr.PART;
                        if (p == null)
                        {
                            p = (Part)itemsDataGrid[codeColumn.Index, e.RowIndex].Tag;
                            if (p == null)
                            {
                                e.Cancel = true;
                                return;
                            }
                        }
                        else
                        {
                            for (int i = 0; i < itemsDataGrid.Rows.Count; i++)
                            {
                                if (i == e.RowIndex) continue;
                                Part pi = (Part)itemsDataGrid[codeColumn.Index, i].Tag;
                                if (pi == null) continue;
                                if (pi.ID == p.ID)
                                {
                                    itemsDataGrid.Rows[e.RowIndex].ErrorText = "Part : " + p.NAME + " already add.";
                                    e.Cancel = true;
                                    return;
                                }
                            }
                            itemsDataGrid[codeColumn.Index, e.RowIndex].Tag = p;
                            itemsDataGrid[scanColumn.Index, e.RowIndex].Value = p.BARCODE;
                            itemsDataGrid[codeColumn.Index, e.RowIndex].Value = p.CODE;
                            itemsDataGrid[nameColumn.Index, e.RowIndex].Value = p.NAME;
                            //dataItemskryptonDataGridView[QtyColumn.Index, e.RowIndex].Value = 0;
                            //unitColumn.Items.Clear();
                            //IList units = r_part.GetAllUnit(p.ID, p.UNIT.ID);
                            //Utils.GetListCode(unitColumn.Items, units);
                            p.UNIT = (Unit)r_unit.GetById(p.UNIT);
                            itemsDataGrid[unitColumn.Index, e.RowIndex].Value = p.UNIT.ToString();
                            itemsDataGrid[priceColumn.Index, e.RowIndex].Value = r_po.GetTheLatestPOPrice(((Customer)supplierkryptonComboBox.SelectedItem).ID, p.ID, p.UNIT.ID);
                           // dataItemskryptonDataGridView[totalAmountColumn.Index, e.RowIndex].Value = 0;
                            itemsDataGrid[warehouseColumn.Index, e.RowIndex].Value = m_warehouses[0].ToString();
                        }
                    }

                }
            }
            if (e.ColumnIndex == discabcColumn.Index)
            {
                if (e.FormattedValue.ToString() == "")
                    return;
                else
                {
                    try
                    {
                        splitDiscString(e.FormattedValue.ToString(), 0);
                        splitDiscString(e.FormattedValue.ToString(), 1);
                        splitDiscString(e.FormattedValue.ToString(), 2);
                    }
                    catch (Exception x)
                    {
                        itemsDataGrid.Rows[e.RowIndex].ErrorText = x.Message;
                        e.Cancel = true;
                        return;
                    }
                }
            }
        }
        private void InitializeDataSource()
        {
            employeeKryptonComboBox.DataSource = r_employee.GetAllSalesman();
            currencyKryptonComboBox.DataSource = r_ccy.GetAll();
            divisionKryptonComboBox.DataSource = r_division.GetAll();
            termofpaymentKryptonComboBox.DataSource = r_top.GetAll();
            supplierkryptonComboBox.DataSource = r_cus.GetAllActive();
            taxKryptonComboBox.DataSource = r_tax.GetAll();
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
                if (m_po.POSTED)
                {
                    r_po.Revise(m_po.ID);
                    m_po.POSTED = false;
                    KryptonMessageBox.Show("Transaction has been UNPOSTED", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    r_po.Confirm(m_po.ID);
                    m_po.POSTED = true;
                    KryptonMessageBox.Show("Transaction has been POSTED", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                //m_po = (SalesOrder)r_po.Get(m_po.ID);
                //m_po.EMPLOYEE = (Employee)r_employee.GetById(m_po.EMPLOYEE);
                //m_po.WAREHOUSE = (Warehouse)r_warehouse.GetById(m_po.WAREHOUSE);
                //m_po.CURRENCY = (Currency)r_ccy.GetById(m_po.CURRENCY);
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
                if (Valid())
                {
                    this.Cursor = Cursors.WaitCursor;
                    UpdateEntity();
                    if (m_po.ID == 0)
                    {
                        r_po.Save(m_po);
                    }
                    else
                    {
                        r_po.Update(m_po);
                    }
                    KryptonMessageBox.Show("Transaction '" + m_po.CODE + "' Record has been saved", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //ClearForm();
                    textBoxCode.Text = m_po.CODE;
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
            bool a = textBoxCode.Text == "" && !r_po.IsAutoNumber();
            bool b = employeeKryptonComboBox.SelectedItem == null;
            bool c = divisionKryptonComboBox.SelectedItem == null;
            bool d = currencyKryptonComboBox.SelectedItem == null;
            bool h = termofpaymentKryptonComboBox.SelectedItem == null;
            bool k = supplierkryptonComboBox.SelectedItem == null;
            bool e = false;
            bool f = m_po.ID > 0 ? false : r_po.IsCodeExist(textBoxCode.Text);
            
            if (a) errorProvider1.SetError(textBoxCode, "Code Can not Empty");
            if (b) errorProvider1.SetError(employeeKryptonComboBox, "Employee Can not Empty");
            if (c) errorProvider1.SetError(divisionKryptonComboBox, "Division Can not Empty");
            if (d) errorProvider1.SetError(currencyKryptonComboBox, "Currency Can not Empty");
            if (h) errorProvider1.SetError(termofpaymentKryptonComboBox, "TOP Can not Empty");
            if (k) errorProvider1.SetError(supplierkryptonComboBox, "Customer Can not Empty");
            if (f) errorProvider1.SetError(textBoxCode, a ? "Code Can not Empty & Code already used" : "Code already used");

            int j = 0;
            for (int i = 0; i < itemsDataGrid.Rows.Count; i++)
            {
                Part p = (Part)itemsDataGrid[codeColumn.Index, i].Tag;
              //  if (dataItemskryptonDataGridView[unitColumn.Index, i].Value == null)
              //      continue;
               // Unit u = (Unit)Utils.FindEntityInList(dataItemskryptonDataGridView[unitColumn.Index, i].Value.ToString(), m_units);
                if (p == null)
                    continue;
                if (itemsDataGrid[unitColumn.Index, i].Value == null)
                {
                    itemsDataGrid.Rows[i].ErrorText = "Please choose unit.";
                    e = true;
                }
                if (itemsDataGrid[warehouseColumn.Index, i].Value == null)
                {
                    itemsDataGrid.Rows[i].ErrorText = "Please choose warehouse.";
                    e = true;
                }
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
            return !a && !b && !c && !d && !e && !f && !g;
        }
        private void UpdateEntity()
        {
            itemsDataGrid.RefreshEdit();
            m_po.CODE = textBoxCode.Text.Trim();
            m_po.TRANSACTION_DATE = dateKryptonDateTimePicker.Value;
            m_po.EMPLOYEE = (Employee)employeeKryptonComboBox.SelectedItem;
            m_po.NOTES = notesKryptonTextBox.Text;
            m_po.DIVISION = (Division)divisionKryptonComboBox.SelectedItem;
            m_po.TOP = (TermOfPayment)termofpaymentKryptonComboBox.SelectedItem;
            m_po.DUE_DATE = duedateKryptonDateTimePicker.Value;
            m_po.CURRENCY = (Currency)currencyKryptonComboBox.SelectedItem;
            m_po.SUB_TOTAL = Convert.ToDouble(subTotalKryptonNumericUpDown.Value);
            m_po.DISC_PERCENT = Convert.ToDouble(discPercentKryptonNumericUpDown.Value);
            m_po.DISC_AFTER_AMOUNT = Convert.ToDouble(discAfterAmountKryptonNumericUpDown.Value);
            m_po.DISC_AMOUNT = Convert.ToDouble(discAmountkryptonNumericUpDown.Value);
            m_po.TAX = (Tax)taxKryptonComboBox.SelectedItem;
            m_po.TAX_AFTER_AMOUNT = Convert.ToDouble(taxAfterAmountkryptonNumericUpDown.Value);
            m_po.OTHER_EXPENSE = Convert.ToDouble(otherExpensekryptonNumericUpDown.Value);
            m_po.NET_TOTAL = Convert.ToDouble(nettotalAmountkryptonNumericUpDown.Value);
            m_po.CUSTOMER = (Customer)supplierkryptonComboBox.SelectedItem;
            m_po.DOCUMENT_DATE = docdatekryptonDateTimePicker.Value;
            m_po.DOCUMENT_NO = docnokryptonTextBox.Text;
            m_po.MODIFIED_BY = m_mainForm.CurrentUser.NAME;
            m_po.MODIFIED_COMPUTER_NAME = Environment.MachineName;
            m_po.EVENT_ITEMS = getItems();
        }

        private IList getItems()
        {
            IList items = new ArrayList();
            for (int i = 0; i < itemsDataGrid.Rows.Count; i++)
            {
                Part p = (Part)itemsDataGrid[codeColumn.Index, i].Tag;
                if (itemsDataGrid[unitColumn.Index, i].Value == null)
                    continue;
                Unit u = (Unit)Utils.FindEntityInList(itemsDataGrid[unitColumn.Index, i].Value.ToString(), m_units);
                if ((p == null) || (u == null))
                    continue;
                SalesOrderItem st=(SalesOrderItem)itemsDataGrid.Rows[i].Tag;
                if(st==null)
                    st = new SalesOrderItem();
                itemsDataGrid.Rows[i].Tag = st;
                st.EVENT = m_po;
                st.PART = p;
                st.WAREHOUSE = (Warehouse)Utils.FindEntityInList(itemsDataGrid[warehouseColumn.Index, i].Value.ToString(), m_warehouses);
                st.QYTAMOUNT = Convert.ToDouble(itemsDataGrid[QtyColumn.Index, i].Value);
                st.UNIT = u;
                st.PRICE = Convert.ToDouble(itemsDataGrid[priceColumn.Index,i].Value);
                st.DISC_PERCENT = Convert.ToDouble(itemsDataGrid[discpercentColumn.Index, i].Value);
                st.DISC_AMOUNT = Convert.ToDouble(itemsDataGrid[discAmountColumn.Index, i].Value);
                st.TOTAL_DISCOUNT = Convert.ToDouble(itemsDataGrid[totalDiscColumn.Index, i].Value);
                st.NOTES = itemsDataGrid[notesColumn.Index, i].Value == null ? "" : itemsDataGrid[notesColumn.Index, i].Value.ToString();
                st.DISC_ABC = itemsDataGrid[discabcColumn.Index, i].Value == null ? "" : itemsDataGrid[discabcColumn.Index, i].Value.ToString();
                st.DISC_A = splitDiscString(st.DISC_ABC, 0);
                st.DISC_B = splitDiscString(st.DISC_ABC, 1);
                st.DISC_C = splitDiscString(st.DISC_ABC, 2);
                st.SUBTOTAL = Convert.ToDouble(itemsDataGrid[totalAmountColumn.Index, i].Value);
                if (st.QYTAMOUNT == 0) continue;
                items.Add(st);
            }
            return items;
        }
        public void ClearForm()
        {
            try
            {
                m_po = new SalesOrder();
                textBoxCode.Text = "";
                dateKryptonDateTimePicker.Value = DateTime.Today;
                employeeKryptonComboBox.SelectedIndex = 0;
                currencyKryptonComboBox.SelectedIndex = 0;
                nettotalAmountkryptonNumericUpDown.Value = 0m;
                notesKryptonTextBox.Text = "";
                divisionKryptonComboBox.SelectedIndex = 0;
                termofpaymentKryptonComboBox.SelectedIndex = 0;
                duedateKryptonDateTimePicker.Value = DateTime.Today;
                subTotalKryptonNumericUpDown.Value = 0m;
                discPercentKryptonNumericUpDown.Value = 0m;
                discAmountkryptonNumericUpDown.Value = 0m;
                discAfterAmountKryptonNumericUpDown.Value = 0m;
                taxKryptonComboBox.SelectedIndex = 0;
                taxAfterAmountkryptonNumericUpDown.Value = 0m;
                otherExpensekryptonNumericUpDown.Value = 0m;
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
            textBoxCode.ReadOnly = r_po.IsAutoNumber()?true:!enable;
            dateKryptonDateTimePicker.Enabled = enable;
            employeeKryptonComboBox.Enabled = enable;
            currencyKryptonComboBox.Enabled = enable;
            notesKryptonTextBox.ReadOnly = !enable;

            divisionKryptonComboBox.Enabled = enable;
            termofpaymentKryptonComboBox.Enabled = enable;
            duedateKryptonDateTimePicker.Enabled = enable;
            //subTotalKryptonNumericUpDown.Enabled = enable;
            discPercentKryptonNumericUpDown.Enabled = enable;
            discAmountkryptonNumericUpDown.Enabled = enable;
            //discAfterAmountKryptonNumericUpDown.Enabled = enable;
            taxKryptonComboBox.Enabled = enable;
            //taxAfterAmountkryptonNumericUpDown.Enabled = enable;
            otherExpensekryptonNumericUpDown.Enabled = enable;
            supplierkryptonComboBox.Enabled = enable;
            docnokryptonTextBox.ReadOnly = !enable;
            docdatekryptonDateTimePicker.Enabled = enable;

            itemsDataGrid.AllowUserToDeleteRows = enable;
            itemsDataGrid.AllowUserToAddRows = enable;

            scanColumn.ReadOnly = !enable;
            QtyColumn.ReadOnly = !enable;
            unitColumn.ReadOnly = !enable;
            priceColumn.ReadOnly = !enable;
            warehouseColumn.ReadOnly = !enable;
            discpercentColumn.ReadOnly = !enable;
            discAmountColumn.ReadOnly = !enable;
            notesColumn.ReadOnly = !enable;
            discabcColumn.ReadOnly = !enable;

            m_enable = enable;
        }
        private void setEditMode(EditMode editmode)
        {
            toolStripButtonSave.Enabled = (editmode == EditMode.New || editmode == EditMode.Update) && !m_po.POSTED && m_mainForm.CurrentUser.FORM_ACCESS_LIST[Name].SAVE;
            toolStripButtonEdit.Enabled = (editmode == EditMode.View) && !m_po.POSTED && m_mainForm.CurrentUser.FORM_ACCESS_LIST[Name].SAVE;
            toolStripButtonDelete.Enabled = (editmode == EditMode.View) && !m_po.POSTED && m_mainForm.CurrentUser.FORM_ACCESS_LIST[Name].DELETE;
            toolStripButtonClear.Enabled = m_mainForm.CurrentUser.FORM_ACCESS_LIST[Name].SAVE;
            toolStripButtonPrint.Enabled = m_po.POSTED && m_mainForm.CurrentUser.FORM_ACCESS_LIST[Name].PRINT;
            postToolStripButton.Enabled = (m_po.ID > 0) && (editmode == EditMode.View) && m_mainForm.CurrentUser.FORM_ACCESS_LIST[Name].POST;
            postToolStripButton.Text = m_po.POSTED ? "Unpost" : "Post";
            statusKryptonLabel.Text = m_po.POSTED ? "POSTED" : "ENTRY";
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
                if (m_po.ID > 0)
                {
                    this.Cursor = Cursors.WaitCursor;
                    if (KryptonMessageBox.Show("Are you sure to delete this record?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No) { this.Cursor = Cursors.Default; return; }
                    r_po.Delete(m_po);
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
            textBoxCode.Text = m_po.CODE;
            dateKryptonDateTimePicker.Value = m_po.TRANSACTION_DATE;
            employeeKryptonComboBox.Text = m_po.EMPLOYEE.ToString();
            currencyKryptonComboBox.Text = m_po.CURRENCY.ToString();
            nettotalAmountkryptonNumericUpDown.Value = Convert.ToDecimal(m_po.NET_TOTAL);
            notesKryptonTextBox.Text = m_po.NOTES;
            divisionKryptonComboBox.Text = m_po.DIVISION.ToString();
            termofpaymentKryptonComboBox.Text = m_po.TOP.ToString();
            duedateKryptonDateTimePicker.Value = m_po.DUE_DATE;
            subTotalKryptonNumericUpDown.Value = Convert.ToDecimal(m_po.SUB_TOTAL);
            discPercentKryptonNumericUpDown.Value = Convert.ToDecimal(m_po.DISC_PERCENT);
            discAfterAmountKryptonNumericUpDown.Value = Convert.ToDecimal(m_po.DISC_AFTER_AMOUNT);
            discAmountkryptonNumericUpDown.Value = Convert.ToDecimal(m_po.DISC_AMOUNT);
            supplierkryptonComboBox.Text = m_po.CUSTOMER.ToString();
            taxKryptonComboBox.Text = m_po.TAX == null ? "" : m_po.TAX.ToString();
            taxAfterAmountkryptonNumericUpDown.Value = Convert.ToDecimal(m_po.TAX_AFTER_AMOUNT);
            otherExpensekryptonNumericUpDown.Value = Convert.ToDecimal(m_po.OTHER_EXPENSE);
            docdatekryptonDateTimePicker.Value = m_po.DOCUMENT_DATE;
            docnokryptonTextBox.Text = m_po.DOCUMENT_NO;

            itemsDataGrid.Rows.Clear();
            foreach (SalesOrderItem item in m_po.EVENT_ITEMS)
            {
                item.UNIT = (Unit)r_unit.GetById(item.UNIT);
                int i = itemsDataGrid.Rows.Add();
                itemsDataGrid.Rows[i].Tag = item;
                itemsDataGrid[scanColumn.Index, i].Value = item.PART.BARCODE;
                itemsDataGrid[codeColumn.Index, i].Tag = item.PART;
                itemsDataGrid[codeColumn.Index, i].Value = item.PART.CODE;
                itemsDataGrid[nameColumn.Index, i].Value = item.PART.NAME;
                itemsDataGrid[QtyColumn.Index, i].Value = item.QYTAMOUNT;

                itemsDataGrid[warehouseColumn.Index, i].Value = r_warehouse.GetById(item.WAREHOUSE).ToString();
                itemsDataGrid[discpercentColumn.Index, i].Value = item.DISC_PERCENT;
                itemsDataGrid[discAmountColumn.Index, i].Value = item.DISC_AMOUNT;
                itemsDataGrid[totalDiscColumn.Index, i].Value = item.TOTAL_DISCOUNT;
                itemsDataGrid[notesColumn.Index, i].Value = item.NOTES;
                itemsDataGrid[discabcColumn.Index, i].Value = item.DISC_ABC;
                unitColumn.Items.Clear();
                IList units = r_part.GetAllUnit(item.PART.ID, item.PART.UNIT.ID);
                Utils.GetListCode(unitColumn.Items, units);
                itemsDataGrid[unitColumn.Index, i].Value = item.UNIT.ToString(); ;
                itemsDataGrid[priceColumn.Index, i].Value = item.PRICE;
                itemsDataGrid[totalAmountColumn.Index, i].Value = item.SUBTOTAL;
            }
            updateOutstandingReceived();
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
            IList result = searchToolStripTextBox.Text == string.Empty ? new ArrayList() : r_po.Search(searchToolStripTextBox.Text);
            if (result.Count == 1)
            {
                m_po = (SalesOrder)result[0];
                m_po = (SalesOrder)r_po.Get(m_po.ID);
                m_po.EMPLOYEE = (Employee)r_employee.GetById(m_po.EMPLOYEE);
                m_po.CURRENCY = (Currency)r_ccy.GetById(m_po.CURRENCY);
                m_po.DIVISION = (Division)r_division.GetById(m_po.DIVISION);
                m_po.TOP = (TermOfPayment)r_top.GetById(m_po.TOP);
                m_po.TAX = m_po.TAX == null ? null : (Tax)r_tax.GetById(m_po.TAX);
                m_po.CUSTOMER = (Customer)r_cus.GetById(m_po.CUSTOMER);
                setEditMode(EditMode.View);
                loadData();
                setEnableForm(false);
            }
            else
            {
                using (SearchSalesOrderForm frm = new SearchSalesOrderForm(searchToolStripTextBox.Text, result))
                {
                    frm.ShowDialog();
                    if (frm.SALES_ORDER == null)
                    {
                        return;
                    }
                    else
                    {
                        m_po = frm.SALES_ORDER;
                        m_po = (SalesOrder)r_po.Get(m_po.ID);
                        m_po.EMPLOYEE = (Employee)r_employee.GetById(m_po.EMPLOYEE);
                        m_po.CURRENCY = (Currency)r_ccy.GetById(m_po.CURRENCY);
                        m_po.DIVISION = (Division)r_division.GetById(m_po.DIVISION);
                        m_po.TOP = (TermOfPayment)r_top.GetById(m_po.TOP);
                        m_po.TAX = m_po.TAX == null ? null : (Tax)r_tax.GetById(m_po.TAX);
                        m_po.CUSTOMER = (Customer)r_cus.GetById(m_po.CUSTOMER);
                        setEditMode(EditMode.View);
                        loadData();
                        setEnableForm(false);
                        //setEditMode(EditMode.View);
                    }
                }
            }
        }

        private void discPercentKryptonNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            if ((m_editMode == EditMode.New) || (m_editMode ==EditMode.Update))
            {
                if (m_enable)
                {
                    CalculateDiscPercentTotal();
                    CalculateTax();
                    CalculateNetTotal();
                }
            }
        }

        private void fieldChooserTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FieldChooserForm cm = new FieldChooserForm(m_mainForm.CurrentUser.ID, this.Name,itemsDataGrid);
            cm.ShowDialog();
        }

        private void SalesOrderForm_Load(object sender, EventArgs e)
        {
            UserSetting.LoadSetting(itemsDataGrid, m_mainForm.CurrentUser.ID, this.Name);
        }

        private void SalesOrderForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            UserSetting.SaveSetting(itemsDataGrid, m_mainForm.CurrentUser.ID, this.Name);
        }

        private void supplierkryptonComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Customer em = (Customer)supplierkryptonComboBox.SelectedItem;
            supplierKryptonTextBox.Text = em == null ? "" : em.NAME;
            contactPersonKryptonTextBox.Text = em == null ? "" : em.CONTACT;
            if ((m_editMode == EditMode.New) || (m_editMode == EditMode.Update))
            {
                if (m_enable)
                {
                    em.TERM_OF_PAYMENT = (TermOfPayment)r_top.GetById(em.TERM_OF_PAYMENT);
                    termofpaymentKryptonComboBox.Text = em.TERM_OF_PAYMENT.ToString();

                    em.EMPLOYEE = (Employee)r_employee.GetById(em.EMPLOYEE);
                    employeeKryptonComboBox.Text = em.EMPLOYEE.ToString();
                }
            }
            addressKryptonTextBox.Text = em == null ? "" : em.ADDRESS;
        }

        private void termofpaymentKryptonComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((m_editMode == EditMode.New) || (m_editMode == EditMode.Update))
            {
                if (m_enable)
                {
                    TermOfPayment top = (TermOfPayment)termofpaymentKryptonComboBox.SelectedItem;
                    if(top==null)return;
                    duedateKryptonDateTimePicker.Value = dateKryptonDateTimePicker.Value.AddDays(top.DAYS);
                }
            }
            TermOfPayment tp = (TermOfPayment)termofpaymentKryptonComboBox.SelectedItem;
            topkryptonTextBox.Text = tp == null ? "" : tp.NAME;
        }

        private void itemsDataGrid_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            for (int count = 0; (count <= (itemsDataGrid.Rows.Count - 2)); count++)
            {
                itemsDataGrid.Rows[count].HeaderCell.Value = string.Format((count + 1).ToString(), "0");
                itemsDataGrid.Rows[count].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
        }

        private void kryptonPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void divisionKryptonComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Division d = (Division)divisionKryptonComboBox.SelectedItem;
            divisionKryptonTextBox.Text = d == null ? "" : d.NAME;
        }

        private void updateOutstandingReceived()
        {
            for (int i = 0; i < itemsDataGrid.Rows.Count; i++)
            {
                SalesOrderItem st = (SalesOrderItem)itemsDataGrid.Rows[i].Tag;
                if (st == null) continue;
                Part p = (Part)itemsDataGrid[codeColumn.Index, i].Tag;
                if (itemsDataGrid[unitColumn.Index, i].Value == null) continue;
                p.UNIT = (Unit)r_unit.GetById(p.UNIT);
                itemsDataGrid[receivedColumn.Index, i].Value = r_po.GetReceived(st.ID);
                itemsDataGrid[outstandingColumn.Index, i].Value = r_po.GetOutstandingDelivered(st.ID);
                itemsDataGrid[receivedunitColumn.Index, i].Value = p.UNIT.ToString();
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
