using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;

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


        public MainForm()
        {
            InitializeComponent();
            kryptonManager1.GlobalPaletteMode = PaletteModeManager.Office2010Blue;
            toolStripComboBox1.Items.AddRange(Enum.GetNames(typeof(PaletteModeManager)));
            toolStripComboBox1.Text = PaletteModeManager.Office2010Blue.ToString();
            toolStripComboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void buttonSpecHeaderGroup1_Click(object sender, EventArgs e)
        {
            kryptonHeaderGroup1.Size = kryptonHeaderGroup1.Collapsed ? new Size(30, 30) : new Size(230, 324);
            //kryptonHeaderGroup2.Size = kryptonHeaderGroup2.Collapsed ? new Size(30, 30) : new Size(230, 392);
        }
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
            treeView8.Visible = buttonSpecAny1.Type == PaletteButtonSpecStyle.ArrowDown;
            buttonSpecAny1.Type = buttonSpecAny1.Type == PaletteButtonSpecStyle.ArrowUp ? PaletteButtonSpecStyle.ArrowDown : PaletteButtonSpecStyle.ArrowUp;
            kryptonHeader7.StateCommon.Border.DrawBorders = treeView8.Visible ? PaletteDrawBorders.TopBottom : PaletteDrawBorders.Top;
        }
        private void kryptonHeader7_MouseClick(object sender, MouseEventArgs e)
        {
            buttonSpecAny1_Click(sender, null);
        }
        private void buttonSpecAny2_Click(object sender, EventArgs e)
        {
            treeView2.Visible = buttonSpecAny2.Type == PaletteButtonSpecStyle.ArrowDown;
            buttonSpecAny2.Type = buttonSpecAny2.Type == PaletteButtonSpecStyle.ArrowUp ? PaletteButtonSpecStyle.ArrowDown : PaletteButtonSpecStyle.ArrowUp;
            kryptonHeader2.StateCommon.Border.DrawBorders = treeView2.Visible ? PaletteDrawBorders.TopBottom : PaletteDrawBorders.Top;
        }
        private void kryptonHeader2_Click(object sender, EventArgs e)
        {
            buttonSpecAny2_Click(sender, null);
        }
        private void buttonSpecAny3_Click(object sender, EventArgs e)
        {
            kryptonPanel4.Tag = kryptonHeader8.Orientation == VisualOrientation.Top ? kryptonPanel4.Width : (int)kryptonPanel4.Tag;
            kryptonHeader8.Orientation = kryptonHeader8.Orientation == VisualOrientation.Top ? VisualOrientation.Left : VisualOrientation.Top;
            kryptonPanel4.Width = kryptonHeader8.Orientation == VisualOrientation.Left ? 33 : (int)kryptonPanel4.Tag;
            buttonSpecAny3.Type = kryptonHeader8.Orientation == VisualOrientation.Left ? PaletteButtonSpecStyle.ArrowRight : PaletteButtonSpecStyle.ArrowLeft;
            kryptonPanel1.Visible = kryptonHeader8.Orientation != VisualOrientation.Left;
            kryptonHeader8.StateCommon.Border.DrawBorders = kryptonHeader8.Orientation == VisualOrientation.Left ? PaletteDrawBorders.All : PaletteDrawBorders.TopLeftRight;
        }

        private void buttonSpecAny4_Click(object sender, EventArgs e)
        {
            treeView1.Visible = buttonSpecAny4.Type == PaletteButtonSpecStyle.ArrowDown;
            buttonSpecAny4.Type = buttonSpecAny4.Type == PaletteButtonSpecStyle.ArrowUp ? PaletteButtonSpecStyle.ArrowDown : PaletteButtonSpecStyle.ArrowUp;
            kryptonHeader1.StateCommon.Border.DrawBorders = treeView1.Visible ? PaletteDrawBorders.TopBottom : PaletteDrawBorders.Top;
        }

        private void kryptonHeader1_Click(object sender, EventArgs e)
        {
            buttonSpecAny4_Click(sender, null);
        }

        private void buttonSpecAny5_Click(object sender, EventArgs e)
        {
            treeView7.Visible = buttonSpecAny5.Type == PaletteButtonSpecStyle.ArrowDown;
            buttonSpecAny5.Type = buttonSpecAny5.Type == PaletteButtonSpecStyle.ArrowUp ? PaletteButtonSpecStyle.ArrowDown : PaletteButtonSpecStyle.ArrowUp;
            kryptonHeader6.StateCommon.Border.DrawBorders = treeView7.Visible ? PaletteDrawBorders.TopBottom : PaletteDrawBorders.Top;
        }

        private void kryptonHeader6_Click(object sender, EventArgs e)
        {
            buttonSpecAny5_Click(sender, null);
        }

        private void kryptonHeader9_Click(object sender, EventArgs e)
        {
            kryptonHeader9.HeaderStyle = kryptonHeader9.HeaderStyle == HeaderStyle.DockActive ? HeaderStyle.DockInactive : HeaderStyle.DockActive;
            kryptonHeader8.Visible = kryptonHeader9.HeaderStyle == HeaderStyle.DockActive;
            kryptonPanel1.Visible = kryptonHeader8.Orientation != VisualOrientation.Left ? kryptonHeader9.HeaderStyle == HeaderStyle.DockActive : kryptonHeader8.Orientation != VisualOrientation.Left;
        }

       
    }
}
