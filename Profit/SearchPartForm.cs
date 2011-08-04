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
    public partial class SearchPartForm : KryptonForm
    {
        PartRepository r_rep = (PartRepository)RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.PART_REPOSITORY);
        public Part PART = null;
        IList m_listLastresult = new ArrayList();

        public SearchPartForm(string textfind, IList result)//Point p)
        {
            InitializeComponent();
            //this.Location = p;
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
            foreach (Part d in records)
            {
                int row = gridData.Rows.Add(d.CODE, d.NAME, d.ACTIVE, d.BARCODE);
                // d.PART_GROUP = (PartGroup)Utils.FindEntityInList(d.PART_GROUP.ID, m_partGroupList); 
                //  d.UNIT = (Unit)Utils.FindEntityInList(d.UNIT.ID, m_unitList);
                //  d.CURRENCY = (Currency)Utils.FindEntityInList(d.CURRENCY.ID, m_currencyList);
                //  d.PART_CATEGORY = (PartCategory)Utils.FindEntityInList(d.PART_CATEGORY.ID, m_partCategoryList);
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
                IList records = r_rep.SearchActivePart(searchText.Text.Trim());
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
                PART = (Part)gridData.SelectedRows[0].Tag;
                this.Close();
            }
        }

        private void OKkryptonButton_Click(object sender, EventArgs e)
        {
            if (gridData.SelectedRows.Count > 0)
            {
                PART = (Part)gridData.SelectedRows[0].Tag;
                this.Close();
            }
        }

        private void CANCELkryptonButton_Click(object sender, EventArgs e)
        {
            PART = null;
            this.Close();
        }
    }
}
