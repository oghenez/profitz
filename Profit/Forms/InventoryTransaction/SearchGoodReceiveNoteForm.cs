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
    public partial class SearchGoodReceiveNoteForm : KryptonForm
    {
        GoodReceiveNoteRepository r_grn = (GoodReceiveNoteRepository)RepositoryFactory.GetInstance().GetTransactionRepository(RepositoryFactory.GOODRECEIVENOTE_REPOSITORY);

        Repository r_warehouse = RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.WAREHOUSE_REPOSITORY);
        Repository r_employee = RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.EMPLOYEE_REPOSITORY);
        PartRepository r_part = (PartRepository)RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.PART_REPOSITORY);
        Repository r_ccy = RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.CURRENCY_REPOSITORY);
        Repository r_unit = RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.UNIT_REPOSITORY);
        Repository r_sup = RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.SUPPLIER_REPOSITORY);
        public GoodReceiveNote GOOD_RECEIVE_NOTE = null;
        IList m_listLastresult = new ArrayList();

        public SearchGoodReceiveNoteForm(string textfind, IList result)//Point p)
        {
            InitializeComponent();
            m_listLastresult = result;
            searchText.Text = textfind;
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
            foreach (GoodReceiveNote d in records)
            {
                d.EMPLOYEE = (Employee)r_employee.GetById(d.EMPLOYEE);
                d.SUPPLIER = (Supplier)r_sup.GetById(d.SUPPLIER);
                //d.WAREHOUSE = (Warehouse)r_warehouse.GetById(d.WAREHOUSE);
                int row = gridData.Rows.Add(d.CODE, d.TRANSACTION_DATE.ToString("dd-MM-yyyy"), d.SUPPLIER.NAME, d.EMPLOYEE.CODE,d.POSTED);
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
                IList records = r_grn.Search(searchText.Text.Trim());
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
                GOOD_RECEIVE_NOTE = (GoodReceiveNote)gridData.SelectedRows[0].Tag;
                this.Close();
            }
        }

        private void OKkryptonButton_Click(object sender, EventArgs e)
        {
            if (gridData.SelectedRows.Count > 0)
            {
                GOOD_RECEIVE_NOTE = (GoodReceiveNote)gridData.SelectedRows[0].Tag;
                this.Close();
            }
        }

        private void CANCELkryptonButton_Click(object sender, EventArgs e)
        {
            GOOD_RECEIVE_NOTE = null;
            this.Close();
        }

        private void gridData_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                OKkryptonButton_Click(sender, null);
        }

        private void SearchGoodReceiveNoteForm_Load(object sender, EventArgs e)
        {
            UserSetting.LoadSetting(gridData, 1, this.Name);
        }

        private void SearchGoodReceiveNoteForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            UserSetting.SaveSetting(gridData, 1, this.Name);
        }
    }
}
