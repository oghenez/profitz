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
    public partial class SearchDOForCussInvoiceForm : KryptonForm
    {
        IList m_listAdded = new ArrayList();
        DeliveryOrderRepository r_po = (DeliveryOrderRepository)RepositoryFactory.GetInstance().GetTransactionRepository(RepositoryFactory.DELIVERY_ORDER_REPOSITORY);
        public IList RESULT = new ArrayList();
        User m_user;
        Customer m_supplierID;
        DateTime m_trDate;

        public SearchDOForCussInvoiceForm(Customer supplier, IList added, User user, DateTime trDate)
        {
            InitializeComponent();
            m_user = user;
            m_trDate = trDate;
            m_supplierID = supplier;
            m_listAdded = added;
            IList records = r_po.FindDOItemlistByCustomerDate("", supplier.ID,m_trDate, added );
            loadResult(records);
            this.Text += " [" + supplier.CODE + "-" + supplier.NAME + "]";
        }

        private void loadResult(IList records)
        {
            foreach (DeliveryOrderItem d in records)
            {
                int row = gridData.Rows.Add();
                gridData[checkColumn.Index, row].Value = false;
                gridData[purchaseorderNoColumn.Index, row].Value = d.EVENT.CODE;
                gridData[poDateColumn.Index, row].Value = d.EVENT.TRANSACTION_DATE.ToString("dd-MM-yyyy");
                //gridData[topColumn.Index, row].Value = ((DeliveryOrder)d.EVENT).TOP.CODE;
                //gridData[dueDateColumn.Index, row].Value = ((DeliveryOrder)d.EVENT).DUE_DATE.ToString("dd-MM-yyyy");
                gridData[codeColumn.Index, row].Value = d.PART.CODE;
                gridData[nameColumn.Index, row].Value = d.PART.NAME;
                gridData[qtyColumn.Index, row].Value = d.OUTSTANDING_AMOUNT_TO_SR;
                gridData[unitColumn.Index, row].Value = d.PART.UNIT.CODE;
                gridData[warehouseColumn.Index, row].Value = d.WAREHOUSE.CODE;
                gridData.Rows[row].Tag = d;
            }
            gridData.ClearSelection();
            searchText.SelectAll();
            if (gridData.Rows.Count > 0)
            {
                gridData.Rows[0].Selected = true; ;
                gridData.Focus();
            }
        }

        private void buttonSpecAny1_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                gridData.Rows.Clear();
                IList records = r_po.FindDOItemlistByCustomerDate(searchText.Text.Trim(), m_supplierID.ID, m_trDate, m_listAdded);
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
            OKkryptonButton_Click(sender, null);
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
                DeliveryOrderItem pi = (DeliveryOrderItem)gridData.Rows[i].Tag;
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

        }
        private void gridData_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                OKkryptonButton_Click(sender, null);
            if (e.KeyCode == Keys.Space)
            {
                bool t = Convert.ToBoolean(gridData[checkColumn.Index, gridData.SelectedRows[0].Index].Value);
                gridData[checkColumn.Index, gridData.SelectedRows[0].Index].Value = !t; 
            }
        }

        private void checkAllkryptonCheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < gridData.Rows.Count; i++)
            {
                gridData[checkColumn.Index, i].Value = checkAllkryptonCheckBox1.Checked;
            }
        }
    }
}
