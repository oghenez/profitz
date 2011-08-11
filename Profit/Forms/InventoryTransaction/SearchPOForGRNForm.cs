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
    public partial class SearchPOForGRNForm : KryptonForm
    {
        IList m_listAdded = new ArrayList();
        PurchaseOrderRepository r_po = (PurchaseOrderRepository)RepositoryFactory.GetInstance().GetTransactionRepository(RepositoryFactory.PURCHASEORDER_REPOSITORY);
        public IList RESULT = new ArrayList();
        User m_user;
        int m_supplierID;

        public SearchPOForGRNForm(string textfind, int supplier, IList added, User user)
        {
            InitializeComponent();
            m_user = user;
            searchText.Text = textfind;
            m_supplierID = supplier;
            m_listAdded = added;
            searchText.Focus();
        }

        private void loadResult(IList records)
        {
            foreach (PurchaseOrderItem d in records)
            {
                int row = gridData.Rows.Add();
                gridData[checkColumn.Index, row].Value = false;
                gridData[purchaseorderNoColumn.Index, row].Value = d.EVENT.CODE;
                gridData[poDateColumn.Index, row].Value = d.EVENT.TRANSACTION_DATE.ToString("dd-MM-yyyy");
                gridData[topColumn.Index, row].Value = ((PurchaseOrder)d.EVENT).TOP.CODE;
                gridData[dueDateColumn.Index, row].Value = ((PurchaseOrder)d.EVENT).DUE_DATE.ToString("dd-MM-yyyy");
                gridData[codeColumn.Index, row].Value = d.PART.CODE;
                gridData[nameColumn.Index, row].Value = d.PART.NAME;
                gridData[qtyColumn.Index, row].Value = d.OUTSTANDING_AMOUNT_TO_GRN;
                gridData[unitColumn.Index, row].Value = d.PART.UNIT.CODE;
                gridData[warehouseColumn.Index, row].Value = d.WAREHOUSE.CODE;
                gridData.Rows[row].Tag = d;
            }
            gridData.ClearSelection();
            if (gridData.Rows.Count > 0) gridData.Rows[0].Selected = true; ;
            gridData.Focus();
        }

        private void buttonSpecAny1_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                gridData.Rows.Clear();

                IList records = r_po.FindPObyPartAndPONo(searchText.Text.Trim(), m_listAdded, m_supplierID);
                loadResult(records);
                this.Cursor = Cursors.Default;
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

        private void searchText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                buttonSpecAny1_Click(sender, null);
            }
        }

        private void gridData_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //if (gridData.SelectedRows.Count > 0)
            //{
            //    PART = (Part)gridData.SelectedRows[0].Tag;
            //    this.Close();
            //}
        }

        private void OKkryptonButton_Click(object sender, EventArgs e)
        {
            AddResult();
            this.Close();
        }

        private void AddResult()
        {
            for (int i = 0; i < gridData.Rows.Count; i++)
            {
                bool check = Convert.ToBoolean(gridData[checkColumn.Index, i].Value);
                if (!check) continue;
                PurchaseOrderItem pi = (PurchaseOrderItem)gridData.Rows[i].Tag;
                RESULT.Add(pi);
            }
        }

        private void CANCELkryptonButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SearchPOForGRNForm_Load(object sender, EventArgs e)
        {
            UserSetting.LoadSetting(gridData, m_user.ID, this.Name);
        }

        private void SearchPOForGRNForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            UserSetting.SaveSetting(gridData, m_user.ID, this.Name);
        }

        private void SearchPOForGRNForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();
        }
        private void gridData_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                OKkryptonButton_Click(sender, null);
        }
    }
}
