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
    public partial class SearchARCRForReceiptForm : KryptonForm
    {
        ARCreditNoteRepository r_soinv = (ARCreditNoteRepository)RepositoryFactory.GetInstance().GetJournalRepository(RepositoryFactory.ARCREDITNOTE_REPOSITORY);

        Repository r_warehouse = RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.WAREHOUSE_REPOSITORY);
        Repository r_employee = RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.EMPLOYEE_REPOSITORY);
        PartRepository r_part = (PartRepository)RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.PART_REPOSITORY);
        Repository r_ccy = RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.CURRENCY_REPOSITORY);
        Repository r_unit = RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.UNIT_REPOSITORY);
        public ARCreditNote ARCREDIT_NOTE = null;
        User m_user;
        IList m_listLastresult = new ArrayList();
        Customer m_sup;
        DateTime m_date;
        IList m_notIn;

        public SearchARCRForReceiptForm(string textfind, Customer supID, IList result, User p, DateTime date, IList notIN)
        {
            InitializeComponent();
            m_listLastresult = result;
            searchText.Text = textfind;
            m_user = p;
            m_sup = supID;
            m_date = date;
            m_notIn = notIN;
            if (result.Count > 0)
            {
                loadResult(result);
                gridData.Focus();
            }
            else
                searchText.Focus();
            this.Text += " [" + supID.CODE + "-" + supID.NAME + "]";
        }

        private void loadResult(IList records)
        {
            foreach (ARCreditNote d in records)
            {
                d.EMPLOYEE = (Employee)r_employee.GetById(d.EMPLOYEE);
                d.CURRENCY = (Currency)r_ccy.GetById(d.CURRENCY);
                int row = gridData.Rows.Add(d.CODE, 
                    d.TRANSACTION_DATE.ToString("dd-MM-yyyy"),
                    d.CURRENCY.CODE,
                    d.NET_AMOUNT,
                    d.EMPLOYEE.CODE,
                    d.POSTED);
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
                IList records = r_soinv.FindARCRForReceipt(m_sup.ID, m_date, searchText.Text.Trim(), m_notIn);
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
                ARCREDIT_NOTE = (ARCreditNote)gridData.SelectedRows[0].Tag;
                this.Close();
            }
        }

        private void OKkryptonButton_Click(object sender, EventArgs e)
        {
            if (gridData.SelectedRows.Count > 0)
            {
                ARCREDIT_NOTE = (ARCreditNote)gridData.SelectedRows[0].Tag;
                this.Close();
            }
        }

        private void CANCELkryptonButton_Click(object sender, EventArgs e)
        {
            ARCREDIT_NOTE = null;
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
