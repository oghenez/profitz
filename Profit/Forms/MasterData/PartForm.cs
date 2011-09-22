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
using System.IO;
using System.Drawing.Drawing2D;

namespace Profit
{
    public partial class PartForm : KryptonForm, IChildForm
    {
        Part m_part = new Part();
        PartRepository r_part = (PartRepository)RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.PART_REPOSITORY);
        IMainForm m_mainForm;
        IList m_partGroupList = new ArrayList();
        IList m_unitList = new ArrayList();
        IList m_currencyList = new ArrayList();
        IList m_taxList = new ArrayList();
        IList m_priceCatList = new ArrayList();
        IList m_partCategoryList = new ArrayList();
        Repository r_unit = RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.UNIT_REPOSITORY);
        Repository r_tax = RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.TAX_REPOSITORY);
        Repository r_ccy = RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.CURRENCY_REPOSITORY);
        Repository r_prtCat = RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.PART_CATEGORY_REPOSITORY);
        Repository r_prtGrp = RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.PART_GROUP_REPOSITORY);
        Repository r_priceCat = RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.PRICE_CATEGORY_REPOSITORY);
        Repository r_sup = RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.SUPPLIER_REPOSITORY);
        CustomerRepository r_cus = (CustomerRepository)RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.CUSTOMER_REPOSITORY);
        SupplierInvoiceRepository r_sir = (SupplierInvoiceRepository)RepositoryFactory.GetInstance().GetTransactionRepository(RepositoryFactory.SUPPLIERINVOICE_REPOSITORY);

        public PartForm(IMainForm mainForm, string formName)
        {
            InitializeComponent();
            InitializeButtonClick();
            InitializeDataSource();
            InitializeGridEvent();
            this.MdiParent = (Form)mainForm;
            this.Name = formName;
            m_mainForm = mainForm;
            Clear(null, null);
            //r_part.UpdateUnitkeUnitConversion();
            //loadRecords();
        }

        private void InitializeGridEvent()
        {
            dataGridViewUOM.CellValidating += new DataGridViewCellValidatingEventHandler(multiUnitkryptonDataGridView1_CellValidating);
            dataGridViewUOM.CellValidated += new DataGridViewCellEventHandler(multiUnitkryptonDataGridView1_CellValidated);
        }

        void multiUnitkryptonDataGridView1_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            dataGridViewUOM.Rows[e.RowIndex].ErrorText = "";
            Unit unit = (Unit)unitkryptonComboBox2.SelectedItem;
            if (unit == null) return;
            dataGridViewUOM[OrigUnit.Index, e.RowIndex].Value = unit.CODE;
        }

        void multiUnitkryptonDataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if ((e.ColumnIndex == ConversionQTy.Index) ||
               (e.ColumnIndex == CostPrice.Index) ||
               (e.ColumnIndex == OrigQty.Index) ||
               (e.ColumnIndex == SellPrice.Index)
               )
            {
                try
                {
                    Convert.ToDecimal(e.FormattedValue.ToString());
                }
                catch (Exception x)
                {
                    dataGridViewUOM.Rows[e.RowIndex].ErrorText =
                                  x.Message;
                    e.Cancel = true;
                }
            }
            if (e.ColumnIndex == ConvUnit.Index)
            {
                foreach (DataGridViewRow v in dataGridViewUOM.Rows)
                {
                    if (e.RowIndex == v.Index) continue;
                    if (dataGridViewUOM[ConvUnit.Index, v.Index].Value == null) continue;
                    string unit = dataGridViewUOM[ConvUnit.Index, v.Index].Value.ToString();
                    if (unit == e.FormattedValue.ToString())
                    {
                        dataGridViewUOM.Rows[e.RowIndex].ErrorText = "Unit has been add";
                        e.Cancel = true;
                    }
                }
                if (unitkryptonComboBox2.SelectedItem.ToString() == e.FormattedValue.ToString())
                {
                    dataGridViewUOM.Rows[e.RowIndex].ErrorText = "Unit Conversion tidak bisa sama dengan Unit Master";
                    e.Cancel = true;
                }
            }
        }

        private void InitializeDataSource()
        {
            m_partGroupList = r_prtGrp.GetAll();
            partGroupkryptonComboBox1.DataSource = m_partGroupList;

            m_unitList = r_unit.GetAll();
            unitkryptonComboBox2.DataSource = m_unitList;

            m_currencyList = r_ccy.GetAll();
            currencykryptonComboBox3.DataSource = m_currencyList;

            costMethodekryptonComboBox4.DataSource = Enum.GetValues(typeof(CostMethod));

            m_partCategoryList = r_prtCat.GetAll();
            partCategorykryptonComboBox5.DataSource = m_partCategoryList;

            Utils.GetListCode(ConvUnit.Items, r_unit.GetAll());

            m_taxList = r_tax.GetAll();
            taxkryptonComboBox1.DataSource = m_taxList;

            m_priceCatList = r_priceCat.GetAll();
            pricecatkryptonComboBox1.DataSource = m_priceCatList;
        }
        private void InitializeButtonClick()
        {
            toolStripButtonSave.Click += new EventHandler(Save);
            toolStripButtonEdit.Click += new EventHandler(Edit);
            toolStripButtonDelete.Click += new EventHandler(Delete);
            toolStripButtonClear.Click += new EventHandler(Clear);
            toolStripButtonRefresh.Click+=new EventHandler(Refresh);
        }
        private void loadRecords()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                gridData.Rows.Clear();
                IList records = r_part.GetAll();
                foreach (Part d in records)
                {
                    int row = gridData.Rows.Add(d.CODE, d.NAME, d.ACTIVE, d.BARCODE);
                    d.PART_GROUP = (PartGroup)Utils.FindEntityInList(d.PART_GROUP.ID, m_partGroupList); //(PartGroup)RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.PART_GROUP_REPOSITORY).GetById(d.PART_GROUP);
                    d.UNIT = (Unit)Utils.FindEntityInList(d.UNIT.ID, m_unitList);//RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.UNIT_REPOSITORY).GetById(d.UNIT);
                    d.CURRENCY = (Currency)Utils.FindEntityInList(d.CURRENCY.ID, m_currencyList);//RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.CURRENCY_REPOSITORY).GetById(d.CURRENCY);
                    d.PART_CATEGORY = (PartCategory)Utils.FindEntityInList(d.PART_CATEGORY.ID, m_partCategoryList);//RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.PART_GROUP_REPOSITORY).GetById(d.PART_CATEGORY);
                    gridData.Rows[row].Tag = d;
                }
                if (gridData.Rows.Count > 0) gridData.Rows[0].Selected = true;
                gridData_SelectionChanged(null, null);
                this.Cursor = Cursors.Default;
                foundtoolStripLabel.Text = "Found " + gridData.Rows.Count.ToString() + " item(s)";

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
                    if (m_part.ID == 0)
                    {
                        r_part.Save(m_part);
                        //Part d = (Part)RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.PART_REPOSITORY).GetByCode(m_part);
                        //int r = gridData.Rows.Add(d.CODE, d.NAME, d.ACTIVE, d.BARCODE);
                        //gridData.Rows[r].Tag = d;
                    }
                    else
                    {
                        r_part.Update(m_part);
                        updateRecord();
                    }
                    KryptonMessageBox.Show("Record has been saved","Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //gridData.ClearSelection();
                    //ClearForm();
                    //textBoxCode.Focus();
                    //this.Cursor = Cursors.Default;
                    setEnableForm(false);
                    setEditMode(EditMode.View);
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
                Part dep = (Part)item.Tag;
                if (dep.ID == m_part.ID)
                {
                    gridData[0, item.Index].Value = m_part.CODE;
                    gridData[1, item.Index].Value = m_part.NAME;
                    gridData[2, item.Index].Value = m_part.ACTIVE   ;
                    gridData[3, item.Index].Value = m_part.BARCODE;
                    break;
                }
            }
        }
        public bool Valid()
        {
            bool a = textBoxCode.Text == "";
            bool b = textBoxName.Text == "";
            bool c = false;
            foreach (DataGridViewRow rw in dataGridViewUOM.Rows)
            {
                if (dataGridViewUOM[ConvUnit.Index, rw.Index].Value == null) continue;
                if (dataGridViewUOM[barcodeColumn.Index, rw.Index].Value == null)
                {
                    c = true;
                    rw.ErrorText = "Please fill Barcode";
                }
                if (Convert.ToDouble(dataGridViewUOM[OrigQty.Index, rw.Index].Value) < 1)
                {
                    c = true;
                    rw.ErrorText += " Please fill Conversion Qty";
                }
                if (Convert.ToDouble(dataGridViewUOM[CostPrice.Index, rw.Index].Value) == 0)
                {
                    c = true;
                    rw.ErrorText += " Please fill CostPrice";
                }
                if(Convert.ToDouble(dataGridViewUOM[SellPrice.Index, rw.Index].Value) == 0)
                {
                    c = true;
                    rw.ErrorText += " Please fill Sell Price";
                }
            }
            if (a) errorProvider1.SetError(textBoxCode, "Code Can not Empty");
            if (b) errorProvider1.SetError(textBoxName, "Name Can not Empty");
            return !a && !b && !c;
        }
        private void UpdateEntity()
        {
            m_part.CODE = textBoxCode.Text.Trim();
            m_part.NAME = textBoxName.Text.Trim();
            m_part.ACTIVE = activekryptonCheckBox1.Checked;
            m_part.BARCODE = barcodekryptonTextBox1.Text;
            m_part.COST_METHOD = (CostMethod)Enum.Parse(typeof(CostMethod), costMethodekryptonComboBox4.SelectedItem.ToString());
            m_part.COST_PRICE = Convert.ToDouble(costPricekryptonNumericUpDown3.Value);
            m_part.CURRENCY = (Currency)currencykryptonComboBox3.SelectedItem;
            //m_part.CURRENT_STOCK = Convert.ToDouble(currentStockkryptonNumericUpDown5.Value);
            m_part.MAXIMUM_STOCK = Convert.ToDouble(maximumStockkryptonNumericUpDown2.Value);
            m_part.MINIMUM_STOCK = Convert.ToDouble(minimumStockkryptonNumericUpDown1.Value);
            m_part.PART_CATEGORY = (PartCategory)partCategorykryptonComboBox5.SelectedItem;
            m_part.PART_GROUP = (PartGroup)partGroupkryptonComboBox1.SelectedItem;
            m_part.SELL_PRICE = Convert.ToDouble(sellPricekryptonNumericUpDown4.Value);
            m_part.TAXABLE = taxkryptonCheckBox2.Checked;
            m_part.UNIT = (Unit)unitkryptonComboBox2.SelectedItem;
            m_part.TAX = (Tax)taxkryptonComboBox1.SelectedItem;
            m_part.PRICE_CATEGORY = (PriceCategory)pricecatkryptonComboBox1.SelectedItem;
            m_part.UNIT_CONVERSION_LIST.Clear();
            //if (m_part.PICTURE != null) m_part.PICTURE.Dispose();
            //m_part.PICTURE = null; 
            m_part.PICTURE = pictureBox.Image == null ? null : imageToByteArray(pictureBox.Image);
            IList unitConversionlist = GetListUom();
            foreach (UnitConversion uc in unitConversionlist)
            {
                m_part.UNIT_CONVERSION_LIST.Add(uc);
            }
        }
        public byte[] imageToByteArray(Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            return ms.ToArray();
        }
        public Image byteArrayToImage(byte[] byteArrayIn)
        {
            if (byteArrayIn == null) return null;
            if (byteArrayIn.Length == 0) return null;
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }
        public void ClearForm()
        {
            try
            {
                textBoxCode.Text = "";
                textBoxName.Text = "";
                activekryptonCheckBox1.Checked = false;
                barcodekryptonTextBox1.Text ="";
                costMethodekryptonComboBox4.SelectedIndex = 0;
                costPricekryptonNumericUpDown3.Value = 0;
                currencykryptonComboBox3.SelectedIndex = 0;
                //currentStockkryptonNumericUpDown5.Value  = 0;
                maximumStockkryptonNumericUpDown2.Value  = 0;
                minimumStockkryptonNumericUpDown1.Value  = 0;
                partCategorykryptonComboBox5.SelectedIndex = 0;
                partGroupkryptonComboBox1.SelectedIndex = 0;
                sellPricekryptonNumericUpDown4.Value = 0;
                taxkryptonCheckBox2.Checked = false;
                unitkryptonComboBox2.SelectedIndex = 0;
                taxkryptonComboBox1.SelectedIndex = 0;
                pricecatkryptonComboBox1.SelectedIndex = 0;
                dataGridViewUOM.Rows.Clear();
                balanceKryptonTextBox.Text = "0";
                bookedKryptonTextBox.Text = "0";
                BackOrderKryptonTextBox.Text = "0";
                pictureBox.Image = null;
                m_part = new Part();
                errorProvider1.Clear();
                movemntkryptonDataGridView.Rows.Clear();
                pricemovementkryptonDataGridView1.Rows.Clear();
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
            barcodekryptonTextBox1.ReadOnly = !enable;
            costMethodekryptonComboBox4.Enabled = enable;
            costPricekryptonNumericUpDown3.Enabled = enable;
            currencykryptonComboBox3.Enabled = enable;
            //currentStockkryptonNumericUpDown5.Enabled = enable;
            maximumStockkryptonNumericUpDown2.Enabled = enable;
            minimumStockkryptonNumericUpDown1.Enabled = enable;
            partCategorykryptonComboBox5.Enabled = enable;
            partGroupkryptonComboBox1.Enabled = enable;
            sellPricekryptonNumericUpDown4.Enabled = enable;
            taxkryptonCheckBox2.Enabled = enable;
            unitkryptonComboBox2.Enabled = enable;
            taxkryptonComboBox1.Enabled = enable;
            pricecatkryptonComboBox1.Enabled = enable;

            dataGridViewUOM.AllowUserToAddRows = enable;
            dataGridViewUOM.AllowUserToDeleteRows = enable;
            ConvUnit.ReadOnly = !enable;
            CostPrice.ReadOnly = !enable;
            SellPrice.ReadOnly = !enable;
            //OrigQty.ReadOnly = !enable;
            barcodeColumn.ReadOnly = !enable;
        }
        private void setEditMode(EditMode editmode)
        {
            toolStripButtonSave.Enabled = (editmode == EditMode.New || editmode == EditMode.Update) && m_mainForm.CurrentUser.FORM_ACCESS_LIST[Name].SAVE;
            toolStripButtonEdit.Enabled = (editmode == EditMode.View) && m_mainForm.CurrentUser.FORM_ACCESS_LIST[Name].SAVE;
            toolStripButtonDelete.Enabled = (editmode == EditMode.View) && m_mainForm.CurrentUser.FORM_ACCESS_LIST[Name].DELETE;
            toolStripButtonClear.Enabled = m_mainForm.CurrentUser.FORM_ACCESS_LIST[Name].SAVE;
            openingstocktoolStripButton.Enabled = toolStripButtonSave.Enabled;
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
                if (m_part.ID > 0)
                {
                    this.Cursor = Cursors.WaitCursor;
                    if (KryptonMessageBox.Show("Are you sure to delete this record?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No) { this.Cursor = Cursors.Default; return; }
                    r_part.Delete(m_part);
                    removeRecord(m_part.ID);
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
                Part dep = (Part)item.Tag;
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
            m_part = (Part)gridData.SelectedRows[0].Tag;
            if (m_part == null) return;
            loadData();
            setEnableForm(false);
            setEditMode(EditMode.View);
        }
        private void loadData()
        {
            m_part.CURRENCY = (Currency)r_ccy.GetById(m_part.CURRENCY);
            m_part.PART_CATEGORY = (PartCategory)r_prtCat.GetById(m_part.PART_CATEGORY);
            m_part.PART_GROUP = (PartGroup)r_prtGrp.GetById(m_part.PART_GROUP);
            m_part.PRICE_CATEGORY = (PriceCategory)r_priceCat.GetById(m_part.PRICE_CATEGORY);
            m_part.TAX = (Tax)r_tax.GetById(m_part.TAX);
            m_part.UNIT = (Unit)r_unit.GetById(m_part.UNIT);
            

            textBoxCode.Text = m_part.CODE;
            textBoxName.Text = m_part.NAME;

            activekryptonCheckBox1.Checked = m_part.ACTIVE;
            barcodekryptonTextBox1.Text = m_part.BARCODE;
            costMethodekryptonComboBox4.Text = m_part.COST_METHOD.ToString();
            costPricekryptonNumericUpDown3.Value = Convert.ToDecimal(m_part.COST_PRICE);
            currencykryptonComboBox3.Text = m_part.CURRENCY.ToString();
            //currentStockkryptonNumericUpDown5.Value = Convert.ToDecimal(m_part.CURRENT_STOCK);
            maximumStockkryptonNumericUpDown2.Value = Convert.ToDecimal(m_part.MAXIMUM_STOCK);
            minimumStockkryptonNumericUpDown1.Value = Convert.ToDecimal(m_part.MINIMUM_STOCK);
            partCategorykryptonComboBox5.Text = m_part.PART_CATEGORY.ToString();
            partGroupkryptonComboBox1.Text = m_part.PART_GROUP.ToString();
            sellPricekryptonNumericUpDown4.Value = Convert.ToDecimal(m_part.SELL_PRICE);
            taxkryptonCheckBox2.Checked = m_part.TAXABLE;
            unitkryptonComboBox2.Text = m_part.UNIT.ToString();
            taxkryptonComboBox1.Text = m_part.TAX.ToString();
            pricecatkryptonComboBox1.Text = m_part.PRICE_CATEGORY.ToString();
            StockCardInfo sci = r_part.GetStockCardInfo(m_part.ID);
            balanceKryptonTextBox.Text = sci.BALANCE.ToString();
            BackOrderKryptonTextBox.Text = sci.BACKORDER.ToString();
            bookedKryptonTextBox.Text = sci.BOOKED.ToString();
            pictureBox.Image = byteArrayToImage(r_part.GetImage(m_part.CODE));//m_part.PICTURE;// m_part.PICTURE == null ? null : byteArrayToImage(m_part.PICTURE);
            dataGridViewUOM.Rows.Clear();
            IList l = r_part.GetUnitConversions(m_part.ID);
            foreach (UnitConversion u in l)
            {
                if (u.CONVERSION_UNIT.ID == m_part.UNIT.ID) continue;
                AddUOM(u);
            }
            //loadMovement();
        }

        private void loadMovement()
        {
            movemntkryptonDataGridView.Rows.Clear();
            if (m_part.ID == 0) return;
            IList movs = r_part.GetAllEvents(m_part.ID);
            foreach (EventItem itm in movs)
            {
                int r = movemntkryptonDataGridView.Rows.Add();
                movemntkryptonDataGridView[dateMovementColumn.Index, r].Value = itm.EVENT.TRANSACTION_DATE;
                movemntkryptonDataGridView[eventCodeMovementColumn.Index, r].Value = itm.EVENT.CODE;
                movemntkryptonDataGridView[eventTypeMovementColumn.Index, r].Value = itm.STOCK_CARD_ENTRY_TYPE.ToString();
                movemntkryptonDataGridView[QtyMovementColumn.Index, r].Value = itm.GetAmountInSmallestUnit();
                movemntkryptonDataGridView[unitMovementColumn.Index, r].Value = m_part.UNIT.CODE;
                movemntkryptonDataGridView[vendorMovementColumn.Index, r].Value = "-";
                switch (itm.STOCK_CARD_ENTRY_TYPE)
                {
                    case StockCardEntryType.PurchaseOrder:
                        PurchaseOrderItem pi = (PurchaseOrderItem)itm;
                        PurchaseOrder p = (PurchaseOrder)pi.EVENT;
                        p.SUPPLIER = (Supplier)r_sup.GetById(p.SUPPLIER);
                        movemntkryptonDataGridView[vendorMovementColumn.Index, r].Value = p.SUPPLIER.NAME;
                        break;
                    case StockCardEntryType.SalesOrder:
                        SalesOrderItem soi = (SalesOrderItem)itm;
                        SalesOrder so = (SalesOrder)soi.EVENT;
                        so.CUSTOMER = (Customer)r_cus.GetById(so.CUSTOMER);
                        movemntkryptonDataGridView[vendorMovementColumn.Index, r].Value = so.CUSTOMER.NAME;
                        break;
                    case StockCardEntryType.GoodReceiveNote:
                        GoodReceiveNoteItem grni = (GoodReceiveNoteItem)itm;
                        GoodReceiveNote grn = (GoodReceiveNote)grni.EVENT;
                        grn.SUPPLIER = (Supplier)r_sup.GetById(grn.SUPPLIER);
                        movemntkryptonDataGridView[vendorMovementColumn.Index, r].Value = grn.SUPPLIER.NAME;
                        break;
                    case StockCardEntryType.DeliveryOrder:
                        DeliveryOrderItem doi = (DeliveryOrderItem)itm;
                        DeliveryOrder dor = (DeliveryOrder)doi.EVENT;
                        dor.CUSTOMER = (Customer)r_cus.GetById(dor.CUSTOMER);
                        movemntkryptonDataGridView[vendorMovementColumn.Index, r].Value = dor.CUSTOMER.NAME;
                        break;
                    case StockCardEntryType.SupplierInvoice:
                        SupplierInvoiceItem sii = (SupplierInvoiceItem)itm;
                        SupplierInvoice si = (SupplierInvoice)sii.EVENT;
                        si.SUPPLIER = (Supplier)r_sup.GetById(si.SUPPLIER);
                        movemntkryptonDataGridView[vendorMovementColumn.Index, r].Value = si.SUPPLIER.NAME;
                        break;
                    case StockCardEntryType.CustomerInvoice:
                        CustomerInvoiceItem cii = (CustomerInvoiceItem)itm;
                        CustomerInvoice ci = (CustomerInvoice)cii.EVENT;
                        ci.CUSTOMER = (Customer)r_cus.GetById(ci.CUSTOMER);
                        movemntkryptonDataGridView[vendorMovementColumn.Index, r].Value = ci.CUSTOMER.NAME;
                        break;
                    case StockCardEntryType.PurchaseReturn:
                        PurchaseReturnItem pri = (PurchaseReturnItem)itm;
                        PurchaseReturn pr = (PurchaseReturn)pri.EVENT;
                        pr.SUPPLIER = (Supplier)r_sup.GetById(pr.SUPPLIER);
                        movemntkryptonDataGridView[vendorMovementColumn.Index, r].Value = pr.SUPPLIER.NAME;
                        break;
                    case StockCardEntryType.SalesReturn:
                        SalesReturnItem sri = (SalesReturnItem)itm;
                        SalesReturn sr = (SalesReturn)sri.EVENT;
                        sr.CUSTOMER = (Customer)r_cus.GetById(sr.CUSTOMER);
                        movemntkryptonDataGridView[vendorMovementColumn.Index, r].Value = sr.CUSTOMER.NAME;
                        break;
                }
                movemntkryptonDataGridView[statusMovementColumn.Index, r].Value = itm.EVENT.POSTED.ToString();
            }
            UserSetting.AddNumberToGrid(movemntkryptonDataGridView);
        }

        #region IChildForm Members


        public void Refresh(object sender, EventArgs e)
        {
            if(toolStripButtonSave.Enabled) InitializeDataSource();
            //loadRecords(); 
            StockCardInfo sci = r_part.GetStockCardInfo(m_part.ID);
            balanceKryptonTextBox.Text = sci.BALANCE.ToString();
            BackOrderKryptonTextBox.Text = sci.BACKORDER.ToString();
            bookedKryptonTextBox.Text = sci.BOOKED.ToString();

           // gridData.Rows.Clear();
           // gridData.ClearSelection(); 
        }

        public void Print(object sender, EventArgs e)
        {
            
        }

        #endregion

        private void BankForm_Activated(object sender, EventArgs e)
        {
            ReloadMainFormButton();
        }

        private void kryptonComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            PartGroup p = (PartGroup)partGroupkryptonComboBox1.SelectedItem;
            partGroupkryptonTextBox.Text = p == null ? "" : p.NAME;
        }
        public IList GetListUom()
        {
            IList list = new ArrayList();
            foreach (DataGridViewRow rw in dataGridViewUOM.Rows)
            {
                //if (rw.Tag == null) continue;
                if (dataGridViewUOM[ConvUnit.Index, rw.Index].Value == null) continue;
                UnitConversion unit = (UnitConversion)rw.Tag;
                if (unit == null)
                {
                    unit = new UnitConversion();
                    rw.Tag = unit;
                }
                unit.PART = m_part;
                unit.BARCODE = dataGridViewUOM[barcodeColumn.Index, rw.Index].Value.ToString();
                unit.CONVERSION_QTY = Convert.ToDouble(dataGridViewUOM[OrigQty.Index, rw.Index].Value);
                unit.CONVERSION_UNIT = (Unit)Utils.FindEntityInList(dataGridViewUOM[ConvUnit.Index, rw.Index].Value.ToString(), (IList)unitkryptonComboBox2.DataSource);
                unit.ORIGINAL_QTY = Convert.ToDouble(dataGridViewUOM[OrigQty.Index, rw.Index].Value);
                unit.COST_PRICE = Convert.ToDouble(dataGridViewUOM[CostPrice.Index, rw.Index].Value);
                unit.SELL_PRICE = Convert.ToDouble(dataGridViewUOM[SellPrice.Index, rw.Index].Value);
                if (unit.CONVERSION_UNIT == null) continue;
                list.Add(unit);
            }
            return list;
        }
        public void AddUOM(UnitConversion u)
        {
            u.CONVERSION_UNIT = (Unit)r_unit.GetById(u.CONVERSION_UNIT);
            int index = dataGridViewUOM.Rows.Add(u.BARCODE, 1, u.CONVERSION_UNIT.CODE, u.CONVERSION_QTY,
                m_part.UNIT.CODE, u.COST_PRICE, u.SELL_PRICE);
            dataGridViewUOM.Rows[index].Tag = u;
        }
        private void deleteUomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridViewUOM.Rows.Remove(dataGridViewUOM.CurrentRow);
        }

        //private void dataGridViewUOM_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (ConvUnit.ReadOnly) return;
        //    if (e.KeyData == Keys.Delete)
        //    {
        //        if (dataGridViewUOM.CurrentRow != null) 
        //            deleteUomToolStripMenuItem_Click(sender, null);
        //    }
        //}

        private void toolStripButtonLoadAll_Click(object sender, EventArgs e)
        {
            loadRecords();
        }

        private void toolStripButtonSearch_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                gridData.Rows.Clear();
                IList records = r_part.SearchActivePart(searchtoolStripTextBox.Text.Trim(), false);
                foreach (Part d in records)
                {
                    int row = gridData.Rows.Add(d.CODE, d.NAME, d.ACTIVE,d.BARCODE);
                    d.PART_GROUP = (PartGroup)Utils.FindEntityInList(d.PART_GROUP.ID, m_partGroupList); //(PartGroup)RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.PART_GROUP_REPOSITORY).GetById(d.PART_GROUP);
                    d.UNIT = (Unit)Utils.FindEntityInList(d.UNIT.ID, m_unitList);//RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.UNIT_REPOSITORY).GetById(d.UNIT);
                    d.CURRENCY = (Currency)Utils.FindEntityInList(d.CURRENCY.ID, m_currencyList);//RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.CURRENCY_REPOSITORY).GetById(d.CURRENCY);
                    d.PART_CATEGORY = (PartCategory)Utils.FindEntityInList(d.PART_CATEGORY.ID, m_partCategoryList);//RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.PART_GROUP_REPOSITORY).GetById(d.PART_CATEGORY);
                    gridData.Rows[row].Tag = d;
                }
                gridData.ClearSelection();
                if (gridData.Rows.Count > 0) gridData.Rows[0].Selected = true; ;
                this.Cursor = Cursors.Default;
                foundtoolStripLabel.Text = "Found " + gridData.Rows.Count.ToString() + " item(s)";
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

        private void searchtoolStripTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                toolStripButtonSearch_Click(sender, null);
            }
        }

        private void unitkryptonComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Unit p = (Unit)unitkryptonComboBox2.SelectedItem;
            unitKryptonTextBox.Text = p == null ? "" : p.NAME;
            unitKryptonTextBox.Text = p == null ? "" : p.CODE;
        }

        private void currencykryptonComboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            Currency p = (Currency)currencykryptonComboBox3.SelectedItem;
            currencyKryptonTextBox.Text = p == null ? "" : p.NAME;
        }

        private void partCategorykryptonComboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            PartCategory p = (PartCategory)partCategorykryptonComboBox5.SelectedItem;
            partCategorykryptonTextBox.Text = p == null ? "" : p.NAME;
        }

        private void kryptonPanel1_DoubleClick(object sender, EventArgs e)
        {
           
            
        }
        private Image resizeImage(Image imgToResize, Size size)
        {
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;
            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;
            nPercentW = ((float)size.Width / (float)sourceWidth);
            nPercentH = ((float)size.Height / (float)sourceHeight);
            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;
            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);
            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();
            return (Image)b;
        }

        private void pictureBox_DoubleClick(object sender, EventArgs e)
        {
            if (!partCategorykryptonComboBox5.Enabled) return;
            openFileDialog1.Multiselect = false;
            openFileDialog1.Filter = "Images (*.JPG)|*.JPG|All files (*.*)|*.*";
            openFileDialog1.ShowDialog();
            if (!File.Exists(openFileDialog1.FileName)) return;
            Image image = Image.FromFile(openFileDialog1.FileName);
            image = resizeImage(image, new Size(225, 225));
            pictureBox.Image = image;
        }

        private void kryptonLabel16_Paint(object sender, PaintEventArgs e)
        {

        }

        private void PartForm_Load(object sender, EventArgs e)
        {
            UserSetting.LoadSetting(movemntkryptonDataGridView, m_mainForm.CurrentUser.ID, this.Name);
            UserSetting.LoadSetting(pricemovementkryptonDataGridView1, m_mainForm.CurrentUser.ID, this.Name);
        }

        private void PartForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            UserSetting.SaveSetting(movemntkryptonDataGridView, m_mainForm.CurrentUser.ID, this.Name);
            UserSetting.SaveSetting(pricemovementkryptonDataGridView1, m_mainForm.CurrentUser.ID, this.Name);
        }

        private void refreshMovementkryptonButton_Click(object sender, EventArgs e)
        {
            loadMovement();
        }

        private void openingstocktoolStripButton_Click(object sender, EventArgs e)
        {
            if (m_part.ID == 0)
            {
                KryptonMessageBox.Show("Please save first.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            OpeningStockForm frm = new OpeningStockForm(m_mainForm, "OpeningStockForm");
            frm.WizardFromPart(m_part);
            frm.Show();
        }

        private void markupdownselltoolStripButton1_Click(object sender, EventArgs e)
        {
            MarkDownSellingPriceForm frm = new MarkDownSellingPriceForm();
            frm.ShowDialog();
        }

        private void pricemovkryptonButton1_Click(object sender, EventArgs e)
        {
            pricemovementkryptonDataGridView1.Rows.Clear();
            if (m_part.ID == 0) return;
            IList movs = r_sir.GetSupplierInvoiceItem(m_part.ID);
            foreach (EventItem itm in movs)
            {
                int r = pricemovementkryptonDataGridView1.Rows.Add();
                pricemovementkryptonDataGridView1[dateprcmovColumn1.Index, r].Value = itm.EVENT.TRANSACTION_DATE;
                pricemovementkryptonDataGridView1[codeprcmovColumn2.Index, r].Value = itm.EVENT.CODE;
                pricemovementkryptonDataGridView1[typeprcmovColumn1.Index, r].Value = itm.STOCK_CARD_ENTRY_TYPE.ToString();
                pricemovementkryptonDataGridView1[qtyprcmovColumn1.Index, r].Value = itm.GetAmountInSmallestUnit();
                pricemovementkryptonDataGridView1[unitprcmovColumn3.Index, r].Value = m_part.UNIT.CODE;
                switch (itm.STOCK_CARD_ENTRY_TYPE)
                {
                    case StockCardEntryType.SupplierInvoice:
                        SupplierInvoiceItem sii = (SupplierInvoiceItem)itm;
                        SupplierInvoice si = (SupplierInvoice)sii.EVENT;
                        si.SUPPLIER = (Supplier)r_sup.GetById(si.SUPPLIER);
                        pricemovementkryptonDataGridView1[vendorprcmovColumn4.Index, r].Value = si.SUPPLIER.NAME;
                        pricemovementkryptonDataGridView1[priceprcmovColumn.Index, r].Value = sii.SUBTOTAL / sii.GetAmountInSmallestUnit();
                        break;
                    case StockCardEntryType.StockTaking:
                        StockTakingItems stk = (StockTakingItems)itm;
                        pricemovementkryptonDataGridView1[priceprcmovColumn.Index, r].Value = stk.TOTAL_AMOUNT / stk.GetAmountInSmallestUnit();
                        break;
                    case StockCardEntryType.OpeningStock:
                        OpeningStockItem opn = (OpeningStockItem)itm;
                        pricemovementkryptonDataGridView1[priceprcmovColumn.Index, r].Value = opn.TOTAL_AMOUNT / opn.GetAmountInSmallestUnit();
                        break;
                }
                pricemovementkryptonDataGridView1[statusMovementColumn.Index, r].Value = itm.EVENT.POSTED.ToString();
            }
            UserSetting.AddNumberToGrid(pricemovementkryptonDataGridView1);
            updatePriceMovement();

        }
        private void updatePriceMovement()
        {
            int r = 0;
            for (int i = 1; i < pricemovementkryptonDataGridView1.Rows.Count; i++)
            {
                r = i - 1;
                double p1 = Convert.ToDouble(pricemovementkryptonDataGridView1[priceprcmovColumn.Index, r].Value);
                double p2 = Convert.ToDouble(pricemovementkryptonDataGridView1[priceprcmovColumn.Index, i].Value);
                pricemovementkryptonDataGridView1[pricemovementColumn.Index, i].Value = (p1 + p2) / 2;

            }
        }

        private void kryptonComboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }
        //private void toolStripButtonMigrate_Click(object sender, EventArgs e)
        //{
        //    StreamReader p = new StreamReader(@"part.csv");
        //    string line = p.ReadLine();
        //    int i = 1;
        //    while (!p.EndOfStream)
        //    {
        //        line = p.ReadLine();
        //        i++;
        //    }
        //    progressBar1.Maximum = i;
        //    progressBar1.Value = 0;
        //    p = new StreamReader(@"part.csv");
        //    line = p.ReadLine();
        //    while (!p.EndOfStream)
        //    {
        //        progressBar1.Value++;
        //        line = p.ReadLine();
        //        ((PartRepository)RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.PART_REPOSITORY)).Import(line);
        //    }
        //}
    }
}
