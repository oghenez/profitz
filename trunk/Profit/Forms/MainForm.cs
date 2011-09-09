﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using System.Collections;
using Profit.Server;

namespace Profit
{
    public partial class MainForm : KryptonForm, IMainForm
    {
        const string BANK_FORM = "BankForm";
        const string CURRENCY_FORM = "CurrencyForm";
        const string DIVISION_FORM = "DivisionForm";
        const string EMPLOYEE_FORM = "EmployeeForm";
        const string TOP_FORM = "TOPForm";
        const string UNIT_FORM = "UnitForm";
        const string CUSTOMER_CATEGORY_FORM = "CustomerCategoryForm";
        const string SUPPLIER_CATEGORY_FORM = "SupplierCategoryForm";
        const string PRICE_CATEGORY_FORM = "PriceCategoryForm";
        const string TAX_FORM = "TaxForm";
        const string PART_GROUP_FORM = "PartGroupForm";
        const string WAREHOUSE_FORM = "WarehouseForm";
        const string PART_CATEGORY_FORM = "PartCategoryForm";
        const string DOC_TYPE_FORM = "DocumentTypeForm";
        const string EXCHANGE_RATE_FORM = "ExchangeRateForm";
        const string CUSTOMER_FORM = "CustomerForm";
        const string SUPPLIER_FORM = "SupplierForm";
        const string YEAR_FORM = "YearForm";
        const string PART_FORM = "PartForm";
        const string STOCK_TAKING_FORM = "StockTakingForm";
        const string USER_FORM = "UserForm";
        const string GENERALSETUP_FORM = "GeneralSetupForm";
        const string PURCHASE_ORDER_FORM = "PurchaseOrderForm";
        const string GOOD_RECEIVE_NOTE_FORM = "GoodReceiveNoteForm";
        const string PURCHASE_RETURN_FORM = "PurchaseReturnForm";
        const string SUPPLIER_INVOICE_FORM = "SupplierInvoiceForm";
        const string SUPPLIER_OUTSTANDING_INVOICE_FORM = "SupplierOutstandingInvoiceForm";
        const string PAYMENT_FORM = "PaymentForm";
        const string AP_DEBIT_NOTE_FORM = "APDebitNoteForm";
        const string PROCESS_TRANSACTION_FORM = "ProcessTransactionForm";
        const string SALES_ORDER_FORM = "SalesOrderForm";
        const string DELIVERY_ORDER_FORM = "DeliveryOrderForm";
        const string CUSTOMER_OS_INVOICE_FORM = "CustomerOutStandingInvoiceForm";
        const string CUSTOMER_INVOICE_FORM = "CustomerInvoiceForm";
        const string RECEIPT_FORM = "ReceiptInvoiceForm";
        const string SALES_RETURN_FORM = "SalesReturnForm";
        const string AR_CREDIT_NOTE_FORM = "ARCreditNoteForm";

        IList m_listForm = new ArrayList();
        User m_currentUser = null;
        Period m_currentPeriod = null;

        UserRepository r_user = (UserRepository)RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.USER_REPOSITORY);
        PeriodRepository r_period = (PeriodRepository)RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.PERIOD_REPOSITORY);

        public MainForm()
        {
            InitializeComponent();
            kryptonManager1.GlobalPaletteMode = PaletteModeManager.ProfessionalSystem;
            toolStripComboBox1.Items.AddRange(Enum.GetNames(typeof(PaletteModeManager)));
            InitFormAccessList();
            m_currentUser = (User)r_user.getUser("ADMIN");
            m_currentPeriod = r_period.FindCurrentPeriod();
        }

        //private void buttonSpecHeaderGroup1_Click(object sender, EventArgs e)
        //{
        //    kryptonHeaderGroup1.Size = kryptonHeaderGroup1.Collapsed ? new Size(30, 30) : new Size(230, 324);
        //    //kryptonHeaderGroup2.Size = kryptonHeaderGroup2.Collapsed ? new Size(30, 30) : new Size(230, 392);
        //}
        void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            if (e.Node.Name == "NodeBank")
            {
                if (isChild(BANK_FORM)) { this.Cursor = Cursors.Default; return; }
                BankForm user = new BankForm(this, BANK_FORM);
                user.WindowState = FormWindowState.Maximized;
                user.Show();
            }
            if (e.Node.Name == "NodeCurrency")
            {
                if (isChild(CURRENCY_FORM)) { this.Cursor = Cursors.Default; return; }
                CurrencyForm user = new CurrencyForm(this, CURRENCY_FORM);
                user.WindowState = FormWindowState.Maximized;
                user.Show();
            }
            if (e.Node.Name == "NodeDivision")
            {
                if (isChild(DIVISION_FORM)) { this.Cursor = Cursors.Default; return; }
                DivisionForm user = new DivisionForm(this, DIVISION_FORM);
                user.WindowState = FormWindowState.Maximized;
                user.Show();
            }
            if (e.Node.Name == "NodeEmployee")
            {
                if (isChild(EMPLOYEE_FORM)) { this.Cursor = Cursors.Default; return; }
                EmployeeForm user = new EmployeeForm(this, EMPLOYEE_FORM);
                user.WindowState = FormWindowState.Maximized;
                user.Show();
            } 
            if (e.Node.Name == "NodeTOP")
            {
                if (isChild(TOP_FORM)) { this.Cursor = Cursors.Default; return; }
                TOPForm user = new TOPForm(this, TOP_FORM);
                user.WindowState = FormWindowState.Maximized;
                user.Show();
            } 
            if (e.Node.Name == "NodeUnit")
            {
                if (isChild(UNIT_FORM)) { this.Cursor = Cursors.Default; return; }
                UnitForm user = new UnitForm(this, UNIT_FORM);
                user.WindowState = FormWindowState.Maximized;
                user.Show();
            }
            if (e.Node.Name == "NodeCustomerCategory")
            {
                if (isChild(CUSTOMER_CATEGORY_FORM)) { this.Cursor = Cursors.Default; return; }
                CustomerCategoryForm user = new CustomerCategoryForm(this, CUSTOMER_CATEGORY_FORM);
                user.WindowState = FormWindowState.Maximized;
                user.Show();
            }
            if (e.Node.Name == "NodeSupplierCategory")
            {
                if (isChild(SUPPLIER_CATEGORY_FORM)) { this.Cursor = Cursors.Default; return; }
                SupplierCategoryForm user = new SupplierCategoryForm(this, SUPPLIER_CATEGORY_FORM);
                user.WindowState = FormWindowState.Maximized;
                user.Show();
            }
            if (e.Node.Name == "NodePriceCategory")
            {
                if (isChild(PRICE_CATEGORY_FORM)) { this.Cursor = Cursors.Default; return; }
                PriceCategoryForm user = new PriceCategoryForm(this, PRICE_CATEGORY_FORM);
                user.WindowState = FormWindowState.Maximized;
                user.Show();
            }
            if (e.Node.Name == "NodeTax")
            {
                if (isChild(TAX_FORM)) { this.Cursor = Cursors.Default; return; }
                TaxForm user = new TaxForm(this, TAX_FORM);
                user.WindowState = FormWindowState.Maximized;
                user.Show();
            }
            if (e.Node.Name == "NodePartGroup")
            {
                if (isChild(PART_GROUP_FORM)) { this.Cursor = Cursors.Default; return; }
                PartGroupForm user = new PartGroupForm(this, PART_GROUP_FORM);
                user.WindowState = FormWindowState.Maximized;
                user.Show();
            }
            if (e.Node.Name == "NodeWarehouse")
            {
                if (isChild(WAREHOUSE_FORM)) { this.Cursor = Cursors.Default; return; }
                WarehouseForm user = new WarehouseForm(this, WAREHOUSE_FORM);
                user.WindowState = FormWindowState.Maximized;
                user.Show();
            } 
            if (e.Node.Name == "NodePartCategory")
            {
                if (isChild(PART_CATEGORY_FORM)) { this.Cursor = Cursors.Default; return; }
                PartCategoryForm user = new PartCategoryForm(this, PART_CATEGORY_FORM);
                user.WindowState = FormWindowState.Maximized;
                user.Show();
            } 
            if (e.Node.Name == "NodeDocumentType")
            {
                if (isChild(DOC_TYPE_FORM)) { this.Cursor = Cursors.Default; return; }
                DocumentTypeForm user = new DocumentTypeForm(this, DOC_TYPE_FORM);
                user.WindowState = FormWindowState.Maximized;
                user.Show();
            }
            if (e.Node.Name == "NodeExchangeRate")
            {
                if (isChild(EXCHANGE_RATE_FORM)) { this.Cursor = Cursors.Default; return; }
                ExchangeRateForm user = new ExchangeRateForm(this, EXCHANGE_RATE_FORM);
                user.WindowState = FormWindowState.Maximized;
                user.Show();
            } if (e.Node.Name == "NodeCustomer")
            {
                if (isChild(CUSTOMER_FORM)) { this.Cursor = Cursors.Default; return; }
                CustomerForm user = new CustomerForm(this, CUSTOMER_FORM);
                user.WindowState = FormWindowState.Maximized;
                user.Show();
            }
            if (e.Node.Name == "NodeSupplier")
            {
                if (isChild(SUPPLIER_FORM)) { this.Cursor = Cursors.Default; return; }
                SupplierForm user = new SupplierForm(this, SUPPLIER_FORM);
                user.WindowState = FormWindowState.Maximized;
                user.Show();
            }
            if (e.Node.Name == "NodeYear")
            {
                if (isChild(YEAR_FORM)) { this.Cursor = Cursors.Default; return; }
                YearForm user = new YearForm(this, YEAR_FORM);
                user.WindowState = FormWindowState.Maximized;
                user.Show();
            }
            if (e.Node.Name == "NodePart")
            {
                if (isChild(PART_FORM)) { this.Cursor = Cursors.Default; return; }
                PartForm user = new PartForm(this, PART_FORM);
                user.WindowState = FormWindowState.Maximized;
                user.Show();
            }
            if (e.Node.Name == "NodeStockTaking")
            {
                if (isChild(STOCK_TAKING_FORM)) { this.Cursor = Cursors.Default; return; }
                StockTakingForm user = new StockTakingForm(this, STOCK_TAKING_FORM);
                user.WindowState = FormWindowState.Maximized;
                user.Show();
            }
            if (e.Node.Name == "NodePurchaseOrder")
            {
                if (isChild(PURCHASE_ORDER_FORM)) { this.Cursor = Cursors.Default; return; }
                PurchaseOrderForm user = new PurchaseOrderForm(this, PURCHASE_ORDER_FORM);
                user.WindowState = FormWindowState.Maximized;
                user.Show();
            }
            if (e.Node.Name == "NodeGoodReceiveNote")
            {
                if (isChild(GOOD_RECEIVE_NOTE_FORM)) { this.Cursor = Cursors.Default; return; }
                GoodReceiptNoteForm user = new GoodReceiptNoteForm(this, GOOD_RECEIVE_NOTE_FORM);
                user.WindowState = FormWindowState.Maximized;
                user.Show();
            }
            if (e.Node.Name == "NodePurchaseReturn")
            {
                if (isChild(PURCHASE_RETURN_FORM)) { this.Cursor = Cursors.Default; return; }
                PurchaseReturnForm user = new PurchaseReturnForm(this, PURCHASE_RETURN_FORM);
                user.WindowState = FormWindowState.Maximized;
                user.Show();
            }
            if (e.Node.Name == "NodeSupplierInvoice")
            {
                if (isChild(SUPPLIER_INVOICE_FORM)) { this.Cursor = Cursors.Default; return; }
                SupplierInvoiceForm user = new SupplierInvoiceForm(this, SUPPLIER_INVOICE_FORM);
                user.WindowState = FormWindowState.Maximized;
                user.Show();
            }
            if (e.Node.Name == "NodeSupplierOutstandingInvoice")
            {
                if (isChild(SUPPLIER_OUTSTANDING_INVOICE_FORM)) { this.Cursor = Cursors.Default; return; }
                SupplierOutstandingInvoiceForm user = new SupplierOutstandingInvoiceForm(this, SUPPLIER_OUTSTANDING_INVOICE_FORM);
                user.WindowState = FormWindowState.Maximized;
                user.Show();
            }
            if (e.Node.Name == "NodePayment")
            {
                if (isChild(PAYMENT_FORM)) { this.Cursor = Cursors.Default; return; }
                PaymentForm user = new PaymentForm(this, PAYMENT_FORM);
                user.WindowState = FormWindowState.Maximized;
                user.Show();
            }
            if (e.Node.Name == "NodeAPDebitNote")
            {
                if (isChild(AP_DEBIT_NOTE_FORM)) { this.Cursor = Cursors.Default; return; }
                APDebitNoteForm user = new APDebitNoteForm(this, AP_DEBIT_NOTE_FORM);
                user.WindowState = FormWindowState.Maximized;
                user.Show();
            }
            if (e.Node.Name == "NodeSalesOrder")
            {
                if (isChild(SALES_ORDER_FORM)) { this.Cursor = Cursors.Default; return; }
                SalesOrderForm user = new SalesOrderForm(this, SALES_ORDER_FORM);
                user.WindowState = FormWindowState.Maximized;
                user.Show();
            }
            if (e.Node.Name == "NodeDeliveryOrder")
            {
                if (isChild(DELIVERY_ORDER_FORM)) { this.Cursor = Cursors.Default; return; }
                DeliveryOrderForm user = new DeliveryOrderForm(this, DELIVERY_ORDER_FORM);
                user.WindowState = FormWindowState.Maximized;
                user.Show();
            }
            if (e.Node.Name == "NodeCustomerOutstandingInvoice")
            {
                if (isChild(CUSTOMER_OS_INVOICE_FORM)) { this.Cursor = Cursors.Default; return; }
                CustomerOutstandingInvoiceForm user = new CustomerOutstandingInvoiceForm(this, CUSTOMER_OS_INVOICE_FORM);
                user.WindowState = FormWindowState.Maximized;
                user.Show();
            }
            if (e.Node.Name == "NodeCustomerInvoice")
            {
                if (isChild(CUSTOMER_INVOICE_FORM)) { this.Cursor = Cursors.Default; return; }
                CustomerInvoiceForm user = new CustomerInvoiceForm(this, CUSTOMER_INVOICE_FORM);
                user.WindowState = FormWindowState.Maximized;
                user.Show();
            }
            if (e.Node.Name == "NodeReceipt")
            {
                if (isChild(RECEIPT_FORM)) { this.Cursor = Cursors.Default; return; }
                ReceiptForm user = new ReceiptForm(this, RECEIPT_FORM);
                user.WindowState = FormWindowState.Maximized;
                user.Show();
            }
            if (e.Node.Name == "NodeSalesReturn")
            {
                if (isChild(SALES_RETURN_FORM)) { this.Cursor = Cursors.Default; return; }
                SalesReturnForm user = new SalesReturnForm(this, SALES_RETURN_FORM);
                user.WindowState = FormWindowState.Maximized;
                user.Show();
            }
            if (e.Node.Name == "NodeARCreditNote")
            {
                if (isChild(AR_CREDIT_NOTE_FORM)) { this.Cursor = Cursors.Default; return; }
                ARCreditNoteForm user = new ARCreditNoteForm(this, AR_CREDIT_NOTE_FORM);
                user.WindowState = FormWindowState.Maximized;
                user.Show();
            }
            this.Cursor = Cursors.Default;
        }
        bool isChild(string name)
        {
            bool result = false;
            foreach (Form ch in this.MdiChildren)
            {
                if (ch.Name == name)
                {
                    result = true;
                    ch.Activate();
                    break;
                }
            }
            return result;
        }


        #region IMainForm Members

        public void EnableButtonSave(bool enable)
        {
            saveToolStripMenuItem.Enabled = enable;
        }

        public void EnableButtonEdit(bool enable)
        {
            editToolStripMenuItem.Enabled = enable;
        }

        public void EnableButtonDelete(bool enable)
        {
            deleteToolStripMenuItem.Enabled = enable;
        }

        public void EnableButtonClear(bool enable)
        {
            clearToolStripMenuItem.Enabled = enable;
        }
        public void EnableButtonRefresh(bool enable)
        {
            refreshToolStripMenuItem.Enabled = enable;
        }

        #endregion

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IChildForm frm = (IChildForm)this.ActiveMdiChild;
            if (frm == null) return;
            frm.Save(sender, e);
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IChildForm frm = (IChildForm)this.ActiveMdiChild;
            if (frm == null) return;
            frm.Clear(sender, e);
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IChildForm frm = (IChildForm)this.ActiveMdiChild;
            if (frm == null) return;
            frm.Print(sender, e);
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IChildForm frm = (IChildForm)this.ActiveMdiChild;
            if (frm == null) return;
            frm.Edit(sender, e);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IChildForm frm = (IChildForm)this.ActiveMdiChild;
            if (frm == null) return;
            frm.Delete(sender, e);
        }

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            kryptonManager1.GlobalPaletteMode = (PaletteModeManager)Enum.Parse(typeof(PaletteModeManager), toolStripComboBox1.SelectedItem.ToString());
        }

        private void buttonSpecAny1_Click(object sender, EventArgs e)
        {
            financeTreeView.Visible = buttonSpecAny1.Type == PaletteButtonSpecStyle.ArrowDown;
            buttonSpecAny1.Type = buttonSpecAny1.Type == PaletteButtonSpecStyle.ArrowUp ? PaletteButtonSpecStyle.ArrowDown : PaletteButtonSpecStyle.ArrowUp;
            financeKryptonHeader.StateCommon.Border.DrawBorders = financeTreeView.Visible ? PaletteDrawBorders.Bottom : PaletteDrawBorders.None;//PaletteDrawBorders.TopBottom : PaletteDrawBorders.Top;
        }
        private void kryptonHeader7_MouseClick(object sender, MouseEventArgs e)
        {
            buttonSpecAny1_Click(sender, null);
        }
        private void buttonSpecAny2_Click(object sender, EventArgs e)
        {
            generalMenuTreeView.Visible = buttonSpecAny2.Type == PaletteButtonSpecStyle.ArrowDown;
            buttonSpecAny2.Type = buttonSpecAny2.Type == PaletteButtonSpecStyle.ArrowUp ? PaletteButtonSpecStyle.ArrowDown : PaletteButtonSpecStyle.ArrowUp;
            generalMenuKryptonHeader.StateCommon.Border.DrawBorders = generalMenuTreeView.Visible ? PaletteDrawBorders.TopBottom : PaletteDrawBorders.Top;
        }
        private void kryptonHeader2_Click(object sender, EventArgs e)
        {
            buttonSpecAny2_Click(sender, null);
        }
        private void buttonSpecAny3_Click(object sender, EventArgs e)
        {
            kryptonPanel4.Tag = MainHeaderkryptonHeader8.Orientation == VisualOrientation.Top ? kryptonPanel4.Width : (int)kryptonPanel4.Tag;
            MainHeaderkryptonHeader8.Orientation = MainHeaderkryptonHeader8.Orientation == VisualOrientation.Top ? VisualOrientation.Left : VisualOrientation.Top;
            kryptonPanel4.Width = MainHeaderkryptonHeader8.Orientation == VisualOrientation.Left ? 35 : (int)kryptonPanel4.Tag;
            buttonSpecAny3.Type = MainHeaderkryptonHeader8.Orientation == VisualOrientation.Left ? PaletteButtonSpecStyle.ArrowRight : PaletteButtonSpecStyle.ArrowLeft;
            MasterDatakryptonPanel1.Visible = MasterDatakryptonCheckButton1.Checked && (MainHeaderkryptonHeader8.Orientation != VisualOrientation.Left);
            TransactionSkryptonPanel.Visible = TransactionkryptonCheckButton2.Checked && (MainHeaderkryptonHeader8.Orientation != VisualOrientation.Left);
            MainHeaderkryptonHeader8.StateCommon.Border.DrawBorders = MainHeaderkryptonHeader8.Orientation == VisualOrientation.Left ? PaletteDrawBorders.All : PaletteDrawBorders.TopLeftRight;
            splitter1.Visible = !(MainHeaderkryptonHeader8.Orientation == VisualOrientation.Left);
        }

        private void buttonSpecAny4_Click(object sender, EventArgs e)
        {
            inventoryTreeView.Visible = buttonSpecAny4.Type == PaletteButtonSpecStyle.ArrowDown;
            buttonSpecAny4.Type = buttonSpecAny4.Type == PaletteButtonSpecStyle.ArrowUp ? PaletteButtonSpecStyle.ArrowDown : PaletteButtonSpecStyle.ArrowUp;
            inventoryKryptonHeader.StateCommon.Border.DrawBorders = inventoryTreeView.Visible ? PaletteDrawBorders.TopBottom : PaletteDrawBorders.Top;
        }

        private void kryptonHeader1_Click(object sender, EventArgs e)
        {
            buttonSpecAny4_Click(sender, null);
        }

        private void buttonSpecAny5_Click(object sender, EventArgs e)
        {
            distributionTreeView.Visible = buttonSpecAny5.Type == PaletteButtonSpecStyle.ArrowDown;
            buttonSpecAny5.Type = buttonSpecAny5.Type == PaletteButtonSpecStyle.ArrowUp ? PaletteButtonSpecStyle.ArrowDown : PaletteButtonSpecStyle.ArrowUp;
            distributionKryptonHeader.StateCommon.Border.DrawBorders = distributionTreeView.Visible ? PaletteDrawBorders.TopBottom : PaletteDrawBorders.Top;
        }

        private void kryptonHeader6_Click(object sender, EventArgs e)
        {
            buttonSpecAny5_Click(sender, null);
        }

        private void treeView8_AfterSelect(object sender, TreeViewEventArgs e)
        {
            treeView1_NodeMouseDoubleClick(sender, new TreeNodeMouseClickEventArgs(e.Node, MouseButtons.Right, 1, 1, 1));
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            SetAuthorityFormAccess();
            toolStripComboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            toolStripComboBox1.Text = UserSetting.GetStringValue("theme", CurrentUser.ID, this.Name, PaletteModeManager.ProfessionalSystem.ToString());
            kryptonPanel4.Width = UserSetting.GetIntValue("menuwidth", CurrentUser.ID, this.Name, 240);
            if (kryptonPanel4.Width == 35)
            {
                kryptonPanel4.Width = 217;
                buttonSpecAny3_Click(null, null);
            }
            this.Width = UserSetting.GetIntValue("mainformwidth", CurrentUser.ID, this.Name, 1024);
            this.Height = UserSetting.GetIntValue("mainformheight", CurrentUser.ID, this.Name, 600);
        }

        private void SetAuthorityFormAccess()
        {
            if (!m_currentUser.FORM_ACCESS_LIST.ContainsKey(BANK_FORM)) financeTreeView.Nodes["NodeBank"].Remove();
            if (!m_currentUser.FORM_ACCESS_LIST.ContainsKey(CURRENCY_FORM)) generalMenuTreeView.Nodes["NodeCurrency"].Remove();
            if (!m_currentUser.FORM_ACCESS_LIST.ContainsKey(DIVISION_FORM)) generalMenuTreeView.Nodes["NodeDivision"].Remove();
            if (!m_currentUser.FORM_ACCESS_LIST.ContainsKey(EMPLOYEE_FORM)) generalMenuTreeView.Nodes["NodeEmployee"].Remove();
            if (!m_currentUser.FORM_ACCESS_LIST.ContainsKey(TOP_FORM)) distributionTreeView.Nodes["NodeTOP"].Remove();
            if (!m_currentUser.FORM_ACCESS_LIST.ContainsKey(CUSTOMER_CATEGORY_FORM)) distributionTreeView.Nodes["NodeCustomerCategory"].Remove();
            if (!m_currentUser.FORM_ACCESS_LIST.ContainsKey(SUPPLIER_CATEGORY_FORM)) distributionTreeView.Nodes["NodeSupplierCategory"].Remove();
            if (!m_currentUser.FORM_ACCESS_LIST.ContainsKey(PRICE_CATEGORY_FORM)) distributionTreeView.Nodes["NodePriceCategory"].Remove();
            if (!m_currentUser.FORM_ACCESS_LIST.ContainsKey(TAX_FORM)) distributionTreeView.Nodes["NodeTax"].Remove();
            if (!m_currentUser.FORM_ACCESS_LIST.ContainsKey(PART_GROUP_FORM)) inventoryTreeView.Nodes["NodePartGroup"].Remove();
            if (!m_currentUser.FORM_ACCESS_LIST.ContainsKey(WAREHOUSE_FORM)) inventoryTreeView.Nodes["NodeWarehouse"].Remove();
            if (!m_currentUser.FORM_ACCESS_LIST.ContainsKey(PART_CATEGORY_FORM)) inventoryTreeView.Nodes["NodePartCategory"].Remove();
            if (!m_currentUser.FORM_ACCESS_LIST.ContainsKey(DOC_TYPE_FORM)) financeTreeView.Nodes["NodeDocumentType"].Remove();
            if (!m_currentUser.FORM_ACCESS_LIST.ContainsKey(EXCHANGE_RATE_FORM)) financeTreeView.Nodes["NodeExchangeRate"].Remove();
            if (!m_currentUser.FORM_ACCESS_LIST.ContainsKey(CUSTOMER_FORM)) distributionTreeView.Nodes["NodeCustomer"].Remove();
            if (!m_currentUser.FORM_ACCESS_LIST.ContainsKey(SUPPLIER_FORM)) distributionTreeView.Nodes["NodeSupplier"].Remove();
            if (!m_currentUser.FORM_ACCESS_LIST.ContainsKey(YEAR_FORM)) financeTreeView.Nodes["NodeYear"].Remove();
            if (!m_currentUser.FORM_ACCESS_LIST.ContainsKey(PART_FORM)) internalTreeView.Nodes["NodePart"].Remove();
            if (!m_currentUser.FORM_ACCESS_LIST.ContainsKey(STOCK_TAKING_FORM)) internalTreeView.Nodes["NodeStockTaking"].Remove();
            if (!m_currentUser.FORM_ACCESS_LIST.ContainsKey(UNIT_FORM)) inventoryTreeView.Nodes["NodeUnit"].Remove();
            if (!m_currentUser.FORM_ACCESS_LIST.ContainsKey(PURCHASE_ORDER_FORM)) purchaseTreeView.Nodes["NodePurchaseOrder"].Remove();
            if (!m_currentUser.FORM_ACCESS_LIST.ContainsKey(GOOD_RECEIVE_NOTE_FORM)) purchaseTreeView.Nodes["NodeGoodReceiveNote"].Remove();
            if (!m_currentUser.FORM_ACCESS_LIST.ContainsKey(PURCHASE_RETURN_FORM)) purchaseTreeView.Nodes["NodePurchaseReturn"].Remove();
            if (!m_currentUser.FORM_ACCESS_LIST.ContainsKey(SUPPLIER_INVOICE_FORM)) purchaseTreeView.Nodes["NodeSupplierInvoice"].Remove();
            if (!m_currentUser.FORM_ACCESS_LIST.ContainsKey(SUPPLIER_OUTSTANDING_INVOICE_FORM)) purchaseTreeView.Nodes["NodeSupplierOutstandingInvoice"].Remove();
            if (!m_currentUser.FORM_ACCESS_LIST.ContainsKey(PAYMENT_FORM)) purchaseTreeView.Nodes["NodePayment"].Remove();
            if (!m_currentUser.FORM_ACCESS_LIST.ContainsKey(AP_DEBIT_NOTE_FORM)) purchaseTreeView.Nodes["NodeAPDebitNote"].Remove();
            if (!m_currentUser.FORM_ACCESS_LIST.ContainsKey(SALES_ORDER_FORM)) SalesTreeView.Nodes["NodeSalesOrder"].Remove();
            if (!m_currentUser.FORM_ACCESS_LIST.ContainsKey(DELIVERY_ORDER_FORM)) SalesTreeView.Nodes["NodeDeliveryOrder"].Remove();
            if (!m_currentUser.FORM_ACCESS_LIST.ContainsKey(CUSTOMER_OS_INVOICE_FORM)) SalesTreeView.Nodes["NodeCustomerOutstandingInvoice"].Remove();
            if (!m_currentUser.FORM_ACCESS_LIST.ContainsKey(CUSTOMER_INVOICE_FORM)) SalesTreeView.Nodes["NodeCustomerInvoice"].Remove();
            if (!m_currentUser.FORM_ACCESS_LIST.ContainsKey(RECEIPT_FORM)) SalesTreeView.Nodes["NodeReceipt"].Remove();
            if (!m_currentUser.FORM_ACCESS_LIST.ContainsKey(SALES_RETURN_FORM)) SalesTreeView.Nodes["NodeSalesReturn"].Remove();
            if (!m_currentUser.FORM_ACCESS_LIST.ContainsKey(AR_CREDIT_NOTE_FORM)) SalesTreeView.Nodes["NodeARCreditNote"].Remove();


            inventoryKryptonHeader.Visible = inventoryTreeView.Visible = inventoryTreeView.Nodes.Count > 0;
            financeKryptonHeader.Visible = financeTreeView.Visible = financeTreeView.Nodes.Count > 0;
            generalMenuKryptonHeader.Visible = generalMenuTreeView.Visible = generalMenuTreeView.Nodes.Count > 0;
            distributionKryptonHeader.Visible = distributionTreeView.Visible = distributionTreeView.Nodes.Count > 0;
            internalKryptonHeader.Visible = internalTreeView.Visible = internalTreeView.Nodes.Count > 0;
            purchaseKryptonHeader.Visible = purchaseTreeView.Visible = purchaseTreeView.Nodes.Count > 0;


            userMaintenanceToolStripMenuItem.Visible = m_currentUser.FORM_ACCESS_LIST.ContainsKey(USER_FORM);
            generalSetupToolStripMenuItem.Visible = m_currentUser.FORM_ACCESS_LIST.ContainsKey(GENERALSETUP_FORM);
            processTransactionToolStripMenuItem.Visible = m_currentUser.FORM_ACCESS_LIST.ContainsKey(PROCESS_TRANSACTION_FORM);


        }

        private void kryptonCheckSet1_CheckedButtonChanged(object sender, EventArgs e)
        {
            TransactionSkryptonPanel.Visible = TransactionkryptonCheckButton2.Checked && (MainHeaderkryptonHeader8.Orientation != VisualOrientation.Left);
            MasterDatakryptonPanel1.Visible = MasterDatakryptonCheckButton1.Checked && (MainHeaderkryptonHeader8.Orientation != VisualOrientation.Left);
            TransactionSkryptonPanel.Dock = DockStyle.Fill;
            MasterDatakryptonPanel1.Dock = DockStyle.Fill;
            if (TransactionkryptonCheckButton2.Checked)
                MainHeaderkryptonHeader8.Text = TransactionkryptonCheckButton2.Text;
            if (MasterDatakryptonCheckButton1.Checked)
                MainHeaderkryptonHeader8.Text = MasterDatakryptonCheckButton1.Text;
        }

        private void buttonSpecAny6_Click(object sender, EventArgs e)
        {
            internalTreeView.Visible = buttonSpecAny6.Type == PaletteButtonSpecStyle.ArrowDown;
            buttonSpecAny6.Type = buttonSpecAny6.Type == PaletteButtonSpecStyle.ArrowUp ? PaletteButtonSpecStyle.ArrowDown : PaletteButtonSpecStyle.ArrowUp;
            internalKryptonHeader.StateCommon.Border.DrawBorders = internalTreeView.Visible ? PaletteDrawBorders.Bottom : PaletteDrawBorders.None;
        }

        private void buttonSpecAny7_Click(object sender, EventArgs e)
        {
            SalesTreeView.Visible = buttonSpecAny7.Type == PaletteButtonSpecStyle.ArrowDown;
            buttonSpecAny7.Type = buttonSpecAny7.Type == PaletteButtonSpecStyle.ArrowUp ? PaletteButtonSpecStyle.ArrowDown : PaletteButtonSpecStyle.ArrowUp;
            kryptonHeader9.StateCommon.Border.DrawBorders = SalesTreeView.Visible ? PaletteDrawBorders.TopBottom : PaletteDrawBorders.Top;
        }

        private void buttonSpecAny8_Click(object sender, EventArgs e)
        {
            purchaseTreeView.Visible = buttonSpecAny8.Type == PaletteButtonSpecStyle.ArrowDown;
            buttonSpecAny8.Type = buttonSpecAny8.Type == PaletteButtonSpecStyle.ArrowUp ? PaletteButtonSpecStyle.ArrowDown : PaletteButtonSpecStyle.ArrowUp;
            //kryptonHeader10.StateCommon.Border.DrawBorders = SalesTreeView.Visible ? PaletteDrawBorders.TopBottom : PaletteDrawBorders.Top;
        }

        private void kryptonHeader4_MouseClick(object sender, MouseEventArgs e)
        {
            buttonSpecAny6_Click(null, null);
        }

        private void kryptonHeader9_MouseClick(object sender, MouseEventArgs e)
        {
            buttonSpecAny7_Click(null, null);
        }

        private void kryptonHeader10_MouseClick(object sender, MouseEventArgs e)
        {
            buttonSpecAny8_Click(null, null);
        }

        public IList GetFormAccessList()
        {
            return m_listForm;
        }
        public void InitFormAccessList()
        {
            m_listForm.Add(new FormAccess(0, MainForm.BANK_FORM.ToString(), "MSTF001 - Bank"));
            m_listForm.Add(new FormAccess(0, MainForm.DOC_TYPE_FORM.ToString(), "MSTF002 - Document Type"));
            m_listForm.Add(new FormAccess(0, MainForm.EXCHANGE_RATE_FORM.ToString(), "MSTF003 - Exchange Rate"));
            m_listForm.Add(new FormAccess(0, MainForm.YEAR_FORM.ToString(), "MSTF004 - Year"));
            m_listForm.Add(new FormAccess(0, MainForm.CURRENCY_FORM.ToString(), "MSTG001 - Currency"));
            m_listForm.Add(new FormAccess(0, MainForm.EMPLOYEE_FORM.ToString(), "MSTG002 - Employee"));
            m_listForm.Add(new FormAccess(0, MainForm.DIVISION_FORM.ToString(), "MSTG003 - Division"));
            m_listForm.Add(new FormAccess(0, MainForm.PART_GROUP_FORM.ToString(), "MSTI001 - Part Group"));
            m_listForm.Add(new FormAccess(0, MainForm.PART_CATEGORY_FORM.ToString(), "MSTI002 - Part Category"));
            m_listForm.Add(new FormAccess(0, MainForm.UNIT_FORM.ToString(), "MSTI003 - Unit"));
            m_listForm.Add(new FormAccess(0, MainForm.WAREHOUSE_FORM.ToString(), "MSTI004 - Warehouse"));
            m_listForm.Add(new FormAccess(0, MainForm.CUSTOMER_FORM.ToString(), "MSTD001 - Customer"));
            m_listForm.Add(new FormAccess(0, MainForm.SUPPLIER_FORM.ToString(), "MSTD002 - Supplier"));
            m_listForm.Add(new FormAccess(0, MainForm.CUSTOMER_CATEGORY_FORM.ToString(), "MSTD003 - Customer Category"));
            m_listForm.Add(new FormAccess(0, MainForm.SUPPLIER_CATEGORY_FORM.ToString(), "MSTD004 - Supplier Category"));
            m_listForm.Add(new FormAccess(0, MainForm.PRICE_CATEGORY_FORM.ToString(), "MSTD005 - Price Category"));
            m_listForm.Add(new FormAccess(0, MainForm.TAX_FORM.ToString(), "MSTD006 - Tax"));
            m_listForm.Add(new FormAccess(0, MainForm.TOP_FORM.ToString(), "MSTD007 - Term Of Payment"));
            m_listForm.Add(new FormAccess(0, MainForm.STOCK_TAKING_FORM.ToString(), "TRCI001 - Stock Taking"));
            m_listForm.Add(new FormAccess(0, MainForm.PART_FORM.ToString(), "TRCI002 - Part Master"));
            m_listForm.Add(new FormAccess(0, MainForm.USER_FORM.ToString(), "GSTP001 - User"));
            m_listForm.Add(new FormAccess(0, MainForm.GENERALSETUP_FORM.ToString(), "GSTP002 - General Setup"));
            m_listForm.Add(new FormAccess(0, MainForm.PROCESS_TRANSACTION_FORM.ToString(), "GSTP003 - Process Transaction"));
            m_listForm.Add(new FormAccess(0, MainForm.PURCHASE_ORDER_FORM.ToString(), "TRCP001 - Purchase Order"));
            m_listForm.Add(new FormAccess(0, MainForm.GOOD_RECEIVE_NOTE_FORM.ToString(), "TRCP002 - Good Receive Note"));
            m_listForm.Add(new FormAccess(0, MainForm.SUPPLIER_INVOICE_FORM.ToString(), "TRCP003 - Supplier Invoice"));
            m_listForm.Add(new FormAccess(0, MainForm.SUPPLIER_OUTSTANDING_INVOICE_FORM.ToString(), "TRCP007 - Supplier Outstanding Invoice"));
            m_listForm.Add(new FormAccess(0, MainForm.PURCHASE_RETURN_FORM.ToString(), "TRCP005 - Purchase Return"));
            m_listForm.Add(new FormAccess(0, MainForm.PAYMENT_FORM.ToString(), "TRCP004 - Payment"));
            m_listForm.Add(new FormAccess(0, MainForm.AP_DEBIT_NOTE_FORM.ToString(), "TRCP006 - APDebitNote"));
            m_listForm.Add(new FormAccess(0, MainForm.SALES_ORDER_FORM.ToString(), "TRCS001 - Sales Order"));
            m_listForm.Add(new FormAccess(0, MainForm.DELIVERY_ORDER_FORM.ToString(), "TRCS002 - Delivery Order"));
            m_listForm.Add(new FormAccess(0, MainForm.CUSTOMER_OS_INVOICE_FORM.ToString(), "TRCS007 - Customer Outstanding Invoice"));
            m_listForm.Add(new FormAccess(0, MainForm.CUSTOMER_INVOICE_FORM.ToString(), "TRCS003 - Customer Invoice"));
            m_listForm.Add(new FormAccess(0, MainForm.RECEIPT_FORM.ToString(), "TRCS004 - Receipt"));
            m_listForm.Add(new FormAccess(0, MainForm.SALES_RETURN_FORM.ToString(), "TRCS005 - Sales Return"));
            m_listForm.Add(new FormAccess(0, MainForm.AR_CREDIT_NOTE_FORM.ToString(), "TRCS006 - AR Credit Note"));
        }

        private void userMaintenanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isChild(USER_FORM)) { this.Cursor = Cursors.Default; return; }
            UserForm user = new UserForm(this, USER_FORM);
            user.WindowState = FormWindowState.Maximized;
            user.Show();
        }
        private void processTransactionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isChild(PROCESS_TRANSACTION_FORM)) { this.Cursor = Cursors.Default; return; }
            ProcessTransactionForm user = new ProcessTransactionForm(this, PROCESS_TRANSACTION_FORM);
            user.WindowState = FormWindowState.Maximized;
            user.Show();
        }

        public User CurrentUser
        {
            get { return m_currentUser; }
            set { m_currentUser = value; }
        }
        public Period CurrentPeriod
        {
            get { return m_currentPeriod; }
            set { m_currentPeriod = value; }
        }

        private void generalSetupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isChild(GENERALSETUP_FORM)) { this.Cursor = Cursors.Default; return; }
            GeneralSetupForm user = new GeneralSetupForm(this, GENERALSETUP_FORM);
            user.WindowState = FormWindowState.Maximized;
            user.Show();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            UserSetting.SaveSetting("theme", toolStripComboBox1.SelectedItem.ToString(), CurrentUser.ID, this.Name, typeof(string));
            UserSetting.SaveSetting("menuwidth", kryptonPanel4.Width.ToString(), CurrentUser.ID, this.Name, typeof(int));
            UserSetting.SaveSetting("mainformwidth", this.Width.ToString(), CurrentUser.ID, this.Name, typeof(int));
            UserSetting.SaveSetting("mainformheight", this.Height.ToString(), CurrentUser.ID, this.Name, typeof(int));

        }

       
    }
}
