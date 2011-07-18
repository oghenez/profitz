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

        public MainForm()
        {
            InitializeComponent();
            kryptonManager1.GlobalPaletteMode = PaletteModeManager.Office2010Black;
            toolStripComboBox1.Items.AddRange(Enum.GetNames(typeof(PaletteModeManager)));
        }
        private void toolStripComboBox1_TextChanged(object sender, EventArgs e)
        {
            string c = sender.ToString(); ;
            kryptonManager1.GlobalPaletteMode = (PaletteModeManager)Enum.Parse(typeof(PaletteModeManager), toolStripComboBox1.SelectedItem.ToString());
        }

        private void buttonSpecHeaderGroup1_Click(object sender, EventArgs e)
        {
            kryptonHeaderGroup1.Size = kryptonHeaderGroup1.Collapsed ? new Size(30, 30) : new Size(230, 324);
            kryptonHeaderGroup2.Size = kryptonHeaderGroup2.Collapsed ? new Size(30, 30) : new Size(230, 392);
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


    }
}
