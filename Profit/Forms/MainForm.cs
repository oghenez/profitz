using System;
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
        IList m_listForm = new ArrayList();
        User m_currentUser = null;

        public MainForm()
        {
            InitializeComponent();
            kryptonManager1.GlobalPaletteMode = PaletteModeManager.ProfessionalSystem;
            toolStripComboBox1.Items.AddRange(Enum.GetNames(typeof(PaletteModeManager)));
            toolStripComboBox1.Text = PaletteModeManager.ProfessionalSystem.ToString();
            toolStripComboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            InitFormAccessList();

            m_currentUser = (User)((UserRepository)RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.USER_REPOSITORY)).getUser("ADMIN");
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
            string c = sender.ToString(); ;
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

        //private void kryptonHeader9_Click(object sender, EventArgs e)
        //{
        //    kryptonHeader9.HeaderStyle = kryptonHeader9.HeaderStyle == HeaderStyle.DockActive ? HeaderStyle.DockInactive : HeaderStyle.DockActive;
        //    kryptonHeader8.Visible = kryptonHeader9.HeaderStyle == HeaderStyle.DockActive;
        //    kryptonPanel1.Visible = kryptonHeader8.Orientation != VisualOrientation.Left ? kryptonHeader9.HeaderStyle == HeaderStyle.DockActive : kryptonHeader8.Orientation != VisualOrientation.Left;
        //}

        private void treeView8_AfterSelect(object sender, TreeViewEventArgs e)
        {
            treeView1_NodeMouseDoubleClick(sender, new TreeNodeMouseClickEventArgs(e.Node, MouseButtons.Right, 1, 1, 1));
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            SetAuthorityFormAccess();
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

            inventoryKryptonHeader.Visible = inventoryTreeView.Visible = inventoryTreeView.Nodes.Count > 0;
            financeKryptonHeader.Visible = financeTreeView.Visible = financeTreeView.Nodes.Count > 0;
            generalMenuKryptonHeader.Visible = generalMenuTreeView.Visible = generalMenuTreeView.Nodes.Count > 0;
            distributionKryptonHeader.Visible = distributionTreeView.Visible = distributionTreeView.Nodes.Count > 0;
            internalKryptonHeader.Visible = internalTreeView.Visible = internalTreeView.Nodes.Count > 0;
            userMaintenanceToolStripMenuItem.Visible = m_currentUser.FORM_ACCESS_LIST.ContainsKey(USER_FORM);
            generalSetupToolStripMenuItem.Visible = m_currentUser.FORM_ACCESS_LIST.ContainsKey(GENERALSETUP_FORM);

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
            PurchaseTreeView.Visible = buttonSpecAny8.Type == PaletteButtonSpecStyle.ArrowDown;
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

        private void kryptonHeader7_Paint(object sender, PaintEventArgs e)
        {

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
        }

        private void userMaintenanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isChild(USER_FORM)) { this.Cursor = Cursors.Default; return; }
            UserForm user = new UserForm(this, USER_FORM);
            user.WindowState = FormWindowState.Maximized;
            user.Show();
        }

        public User CurrentUser
        {
            get { return m_currentUser; }
            set { m_currentUser = value; }
        }

        private void kryptonPanel4_SizeChanged(object sender, EventArgs e)
        {
            //if (kryptonPanel4.Width > 35)
            //    buttonSpecAny3_Click(sender, e);
        }

        private void generalSetupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isChild(GENERALSETUP_FORM)) { this.Cursor = Cursors.Default; return; }
            GeneralSetupForm user = new GeneralSetupForm(this, GENERALSETUP_FORM);
            user.WindowState = FormWindowState.Maximized;
            user.Show();
        }
    }
}
