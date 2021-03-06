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
    public partial class SearchAPDebitNoteForm : KryptonForm
    {
        APDebitNoteRepository r_soinv = (APDebitNoteRepository)RepositoryFactory.GetInstance().GetJournalRepository(RepositoryFactory.APDEBITNOTE_REPOSITORY);

        Repository r_warehouse = RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.WAREHOUSE_REPOSITORY);
        Repository r_employee = RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.EMPLOYEE_REPOSITORY);
        PartRepository r_part = (PartRepository)RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.PART_REPOSITORY);
        Repository r_ccy = RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.CURRENCY_REPOSITORY);
        Repository r_unit = RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.UNIT_REPOSITORY);
        Repository r_sup = RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.SUPPLIER_REPOSITORY);
        public APDebitNote APDEBIT_NOTE = null;
        User m_user;
        IList m_listLastresult = new ArrayList();

        public SearchAPDebitNoteForm(string textfind, IList result, User p)
        {
            InitializeComponent();
            m_listLastresult = result;
            searchText.Text = textfind;
            m_user = p;
            if (result.Count > 0)
            {
                loadResult(result);
                gridData.Focus();
            }
            else
                searchText.Focus();
        }

        private void loadResult(IList records)
        {
            foreach (APDebitNote d in records)
            {
                d.EMPLOYEE = (Employee)r_employee.GetById(d.EMPLOYEE);
                //d.WAREHOUSE = (Warehouse)r_warehouse.GetById(d.WAREHOUSE);
                d.VENDOR = (Supplier)r_sup.GetById((Supplier)d.VENDOR);
                int row = gridData.Rows.Add(d.CODE, d.TRANSACTION_DATE.ToString("dd-MM-yyyy"),d.VENDOR.NAME, d.EMPLOYEE.CODE,d.POSTED);
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
                IList records = r_soinv.Search(searchText.Text.Trim());
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
            if (gridData.SelectedRows.Count > 0)
            {
                APDEBIT_NOTE = (APDebitNote)gridData.SelectedRows[0].Tag;
                this.Close();
            }
        }

        private void OKkryptonButton_Click(object sender, EventArgs e)
        {
            if (gridData.SelectedRows.Count > 0)
            {
                APDEBIT_NOTE = (APDebitNote)gridData.SelectedRows[0].Tag;
                this.Close();
            }
        }

        private void CANCELkryptonButton_Click(object sender, EventArgs e)
        {
            APDEBIT_NOTE = null;
            this.Close();
        }

        private void gridData_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                OKkryptonButton_Click(sender, null);
        }

        private void SearchPurchaseReturnForm_Load(object sender, EventArgs e)
        {
            UserSetting.LoadSetting(gridData, m_user.ID, this.Name);
        }

        private void SearchPurchaseReturnForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            UserSetting.SaveSetting(gridData, m_user.ID, this.Name);
        }
    }
}
