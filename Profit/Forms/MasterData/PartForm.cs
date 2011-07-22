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
    public partial class PartForm : KryptonForm, IChildForm
    {
        Part m_part = new Part();
        IMainForm m_mainForm;

        public PartForm(IMainForm mainForm, string formName)
        {
            InitializeComponent();
            InitializeButtonClick();
            InitializeDataSource();
            InitializeGridEvent();
            this.MdiParent = (Form)mainForm;
            this.Name = formName;
            m_mainForm = mainForm;
            loadRecords();
        }

        private void InitializeGridEvent()
        {
            dataGridViewUOM.CellValidating += new DataGridViewCellValidatingEventHandler(multiUnitkryptonDataGridView1_CellValidating);
            dataGridViewUOM.CellValidated += new DataGridViewCellEventHandler(multiUnitkryptonDataGridView1_CellValidated);
        }

        void multiUnitkryptonDataGridView1_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            dataGridViewUOM.Rows[e.RowIndex].ErrorText = "";
            Unit unit = (Unit)unitkryptonComboBox2.SelectedItem;
            if (unit == null) return;
            dataGridViewUOM[OrigUnit.Index, e.RowIndex].Value = unit.CODE;
        }

        void multiUnitkryptonDataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if ((e.ColumnIndex == ConversionQTy.Index) ||
               (e.ColumnIndex == CostPrice.Index) ||
               (e.ColumnIndex == OrigQty.Index) ||
               (e.ColumnIndex == SellPrice.Index)
               )
            {
                try
                {
                    Convert.ToDecimal(e.FormattedValue.ToString());
                }
                catch (Exception x)
                {
                    dataGridViewUOM.Rows[e.RowIndex].ErrorText =
                                  x.Message;
                    e.Cancel = true;
                }
            }
            if (e.ColumnIndex == ConvUnit.Index)
            {
                foreach (DataGridViewRow v in dataGridViewUOM.Rows)
                {
                    if (e.RowIndex == v.Index) continue;
                    if (dataGridViewUOM[ConvUnit.Index, v.Index].Value == null) continue;
                    string unit = dataGridViewUOM[ConvUnit.Index, v.Index].Value.ToString();
                    if (unit == e.FormattedValue.ToString())
                    {
                        dataGridViewUOM.Rows[e.RowIndex].ErrorText = "Unit has been add";
                        e.Cancel = true;
                    }
                }
            }
        }

        private void InitializeDataSource()
        {
            partGroupkryptonComboBox1.DataSource = RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.PART_GROUP_REPOSITORY).GetAll();
            unitkryptonComboBox2.DataSource = RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.UNIT_REPOSITORY).GetAll();
            currencykryptonComboBox3.DataSource = RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.CURRENCY_REPOSITORY).GetAll();
            costMethodekryptonComboBox4.DataSource = Enum.GetValues(typeof(CostMethod));
            partCategorykryptonComboBox5.DataSource = RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.PART_CATEGORY_REPOSITORY).GetAll();
            Utils.GetListCode(ConvUnit.Items, RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.UNIT_REPOSITORY).GetAll());
        }
        private void InitializeButtonClick()
        {
            toolStripButtonSave.Click += new EventHandler(Save);
            toolStripButtonEdit.Click += new EventHandler(Edit);
            toolStripButtonDelete.Click += new EventHandler(Delete);
            toolStripButtonClear.Click += new EventHandler(Clear);
            toolStripButtonRefresh.Click+=new EventHandler(Refresh);
        }
        private void loadRecords()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                gridData.Rows.Clear();
                IList records = RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.PART_REPOSITORY).GetAll();
                foreach (Part d in records)
                {
                    int row = gridData.Rows.Add(d.CODE, d.NAME);
                    d.PART_GROUP = (PartGroup)RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.PART_GROUP_REPOSITORY).GetById(d.PART_GROUP);
                    d.UNIT = (Unit)RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.UNIT_REPOSITORY).GetById(d.UNIT);
                    d.CURRENCY = (Currency)RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.CURRENCY_REPOSITORY).GetById(d.CURRENCY);
                    d.PART_CATEGORY = (PartCategory)RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.PART_GROUP_REPOSITORY).GetById(d.PART_CATEGORY);
                    gridData.Rows[row].Tag = d;
                }
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
        public void Save(object sender, EventArgs e)
        {
            try
            {
                if (Valid())
                {
                    this.Cursor = Cursors.WaitCursor;
                    UpdateEntity();
                    if (m_part.ID == 0)
                    {
                        RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.PART_REPOSITORY).Save(m_part);
                        Part bank = (Part)RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.PART_REPOSITORY).GetByCode(m_part);
                        int r = gridData.Rows.Add(bank.CODE, bank.NAME);
                        gridData.Rows[r].Tag = bank;
                    }
                    else
                    {
                        RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.PART_REPOSITORY).Update(m_part);
                        updateRecord();
                    }
                    KryptonMessageBox.Show("Record has been saved","Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    gridData.ClearSelection();
                    ClearForm();
                    textBoxCode.Focus();
                    this.Cursor = Cursors.Default;
                }
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

        private void updateRecord()
        {
            foreach (DataGridViewRow item in gridData.Rows)
            {
                Part dep = (Part)item.Tag;
                if (dep.ID == m_part.ID)
                {
                    gridData[0, item.Index].Value = m_part.CODE;
                    gridData[1, item.Index].Value = m_part.NAME;
                    break;
                }
            }
        }
        public bool Valid()
        {
            bool a = textBoxCode.Text == "";
            bool b = textBoxName.Text == "";
            if (a) errorProvider1.SetError(textBoxCode, "Code Can not Empty");
            if (b) errorProvider1.SetError(textBoxName, "Name Can not Empty");
            return !a && !b;
        }
        private void UpdateEntity()
        {
            m_part.CODE = textBoxCode.Text.Trim();
            m_part.NAME = textBoxName.Text.Trim();
            m_part.ACTIVE = activekryptonCheckBox1.Checked;
            m_part.BARCODE = barcodekryptonTextBox1.Text;
            m_part.COST_METHOD = (CostMethod)Enum.Parse(typeof(CostMethod), costMethodekryptonComboBox4.SelectedItem.ToString());
            m_part.COST_PRICE = Convert.ToDouble(costPricekryptonNumericUpDown3.Value);
            m_part.CURRENCY = (Currency)currencykryptonComboBox3.SelectedItem;
            m_part.CURRENT_STOCK = Convert.ToDouble(currentStockkryptonNumericUpDown5.Value);
            m_part.MAXIMUM_STOCK = Convert.ToDouble(maximumStockkryptonNumericUpDown2.Value);
            m_part.MINIMUM_STOCK = Convert.ToDouble(minimumStockkryptonNumericUpDown1.Value);
            m_part.PART_CATEGORY = (PartCategory)partCategorykryptonComboBox5.SelectedItem;
            m_part.PART_GROUP = (PartGroup)partGroupkryptonComboBox1.SelectedItem;
            m_part.SELL_PRICE = Convert.ToDouble(sellPricekryptonNumericUpDown4.Value);
            m_part.TAXABLE = taxkryptonCheckBox2.Checked;
            m_part.UNIT = (Unit)unitkryptonComboBox2.SelectedItem;
            m_part.UNIT_CONVERSION_LIST.Clear();
            IList unitConversionlist = GetListUom();
            foreach (UnitConversion uc in unitConversionlist)
            {
                m_part.UNIT_CONVERSION_LIST.Add(uc);
            }
        }
        public void ClearForm()
        {
            try
            {
                textBoxCode.Text = "";
                textBoxName.Text = "";
                activekryptonCheckBox1.Checked = false;
                barcodekryptonTextBox1.Text ="";
                costMethodekryptonComboBox4.SelectedIndex = 0;
                costPricekryptonNumericUpDown3.Value = 0;
                currencykryptonComboBox3.SelectedIndex = 0;
                currentStockkryptonNumericUpDown5.Value  = 0;
                maximumStockkryptonNumericUpDown2.Value  = 0;
                minimumStockkryptonNumericUpDown1.Value  = 0;
                partCategorykryptonComboBox5.SelectedIndex = 0;
                partGroupkryptonComboBox1.SelectedIndex = 0;
                sellPricekryptonNumericUpDown4.Value = 0;
                taxkryptonCheckBox2.Checked = false;
                unitkryptonComboBox2.SelectedIndex = 0;
                dataGridViewUOM.Rows.Clear();
                m_part = new Part();
                errorProvider1.Clear();
            }
            catch (Exception x)
            {
                KryptonMessageBox.Show(x.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        public void Edit(object sender, EventArgs e)
        {
            setEnableForm(true);
            setEditMode(EditMode.Update);
            textBoxCode.Focus();
        }
        public void setEnableForm(bool enable)
        {
            textBoxCode.ReadOnly = !enable;
            textBoxName.ReadOnly = !enable;
            activekryptonCheckBox1.Enabled = enable;
            barcodekryptonTextBox1.ReadOnly = !enable;
            costMethodekryptonComboBox4.Enabled = enable;
            costPricekryptonNumericUpDown3.Enabled = enable;
            currencykryptonComboBox3.Enabled = enable;
            currentStockkryptonNumericUpDown5.Enabled = enable;
            maximumStockkryptonNumericUpDown2.Enabled = enable;
            minimumStockkryptonNumericUpDown1.Enabled = enable;
            partCategorykryptonComboBox5.Enabled = enable;
            partGroupkryptonComboBox1.Enabled = enable;
            sellPricekryptonNumericUpDown4.Enabled = enable;
            taxkryptonCheckBox2.Enabled = enable;
            unitkryptonComboBox2.Enabled = enable;

            dataGridViewUOM.AllowUserToAddRows = enable;
            ConvUnit.ReadOnly = !enable;
            CostPrice.ReadOnly = !enable;
            SellPrice.ReadOnly = !enable;
            OrigQty.ReadOnly = !enable;
        }
        private void setEditMode(EditMode editmode)
        {
            toolStripButtonSave.Enabled = (editmode == EditMode.New || editmode == EditMode.Update);// && m_mainForm.CurrentUser.FORM_CCY_SAVE;
            toolStripButtonEdit.Enabled = (editmode == EditMode.View);//&& m_mainForm.CurrentUser.FORM_CCY_SAVE;
            toolStripButtonDelete.Enabled = (editmode == EditMode.View);//&& m_mainForm.CurrentUser.FORM_CCY_DELETE;
            toolStripButtonClear.Enabled = true;//m_mainForm.CurrentUser.FORM_CCY_SAVE;
            ReloadMainFormButton();
        }
        private void ReloadMainFormButton()
        {
            m_mainForm.EnableButtonSave(toolStripButtonSave.Enabled);
            m_mainForm.EnableButtonEdit(toolStripButtonEdit.Enabled);
            m_mainForm.EnableButtonDelete(toolStripButtonDelete.Enabled);
            m_mainForm.EnableButtonClear(true);
        }
        public void Delete(object obj, EventArgs e)
        {
            try
            {
                if (m_part.ID > 0)
                {
                    this.Cursor = Cursors.WaitCursor;
                    if (KryptonMessageBox.Show("Are you sure to delete this record?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No) { this.Cursor = Cursors.Default; return; }
                    RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.PART_REPOSITORY).Delete(m_part);
                    removeRecord(m_part.ID);
                    ClearForm();
                    setEnableForm(true);
                    setEditMode(EditMode.New);
                    textBoxCode.Focus();
                    this.Cursor = Cursors.Default;
                }

            }
            catch (Exception x)
            {
                KryptonMessageBox.Show(x.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
        private void removeRecord(int id)
        {
            foreach (DataGridViewRow item in gridData.Rows)
            {
                Part dep = (Part)item.Tag;
                if (dep.ID == id)
                {
                    gridData.Rows.Remove(item);
                    break;
                }
            }
            gridData.ClearSelection();
        }
        public void Clear(object sender, EventArgs e)
        {
            gridData.ClearSelection();
            ClearForm();
            setEnableForm(true);
            setEditMode(EditMode.New);
            textBoxCode.Focus();
        }

        private void gridData_SelectionChanged(object sender, EventArgs e)
        {
            if (gridData.SelectedRows.Count == 0) return;
            ClearForm();
            m_part = (Part)gridData.SelectedRows[0].Tag;
            if (m_part == null) return;
            loadData();
            setEnableForm(false);
            setEditMode(EditMode.View);
        }
        private void loadData()
        {
            textBoxCode.Text = m_part.CODE;
            textBoxName.Text = m_part.NAME;

            activekryptonCheckBox1.Checked = m_part.ACTIVE;
            barcodekryptonTextBox1.Text = m_part.BARCODE;
            costMethodekryptonComboBox4.Text = m_part.COST_METHOD.ToString();
            costPricekryptonNumericUpDown3.Value = Convert.ToDecimal(m_part.COST_PRICE);
            currencykryptonComboBox3.Text = m_part.CURRENCY.ToString();
            currentStockkryptonNumericUpDown5.Value = Convert.ToDecimal(m_part.CURRENT_STOCK);
            maximumStockkryptonNumericUpDown2.Value = Convert.ToDecimal(m_part.MAXIMUM_STOCK);
            minimumStockkryptonNumericUpDown1.Value = Convert.ToDecimal(m_part.MINIMUM_STOCK);
            partCategorykryptonComboBox5.Text = m_part.PART_CATEGORY.ToString();
            partGroupkryptonComboBox1.Text = m_part.PART_GROUP.ToString();
            sellPricekryptonNumericUpDown4.Value = Convert.ToDecimal(m_part.SELL_PRICE);
            taxkryptonCheckBox2.Checked = m_part.TAXABLE;
            unitkryptonComboBox2.Text = m_part.UNIT.ToString();

            dataGridViewUOM.Rows.Clear();
            IList l = ((PartRepository)RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.PART_REPOSITORY)).GetUnitConversions(m_part.ID);
            foreach (UnitConversion u in l)
                AddUOM(u);
        }

        #region IChildForm Members


        public void Refresh(object sender, EventArgs e)
        {
            InitializeDataSource();
            loadRecords(); 
            gridData.ClearSelection(); 
        }

        public void Print(object sender, EventArgs e)
        {
            
        }

        #endregion

        private void BankForm_Activated(object sender, EventArgs e)
        {
            ReloadMainFormButton();
        }

        private void kryptonComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        public IList GetListUom()
        {
            IList list = new ArrayList();
            foreach (DataGridViewRow rw in dataGridViewUOM.Rows)
            {
                //if (rw.Tag == null) continue;
                if (dataGridViewUOM[ConvUnit.Index, rw.Index].Value == null) continue;
                UnitConversion unit = (UnitConversion)rw.Tag;
                if (unit == null)
                {
                    unit = new UnitConversion();
                    rw.Tag = unit;
                }
                unit.PART = m_part;
                unit.CONVERSION_QTY = Convert.ToDouble(dataGridViewUOM[OrigQty.Index, rw.Index].Value);
                unit.CONVERSION_UNIT = (Unit)Utils.FindEntityInList(dataGridViewUOM[ConvUnit.Index, rw.Index].Value.ToString(), (IList)unitkryptonComboBox2.DataSource);
                unit.ORIGINAL_QTY = Convert.ToDouble(dataGridViewUOM[OrigQty.Index, rw.Index].Value);
                unit.COST_PRICE = Convert.ToDouble(dataGridViewUOM[CostPrice.Index, rw.Index].Value);
                unit.SELL_PRICE = Convert.ToDouble(dataGridViewUOM[SellPrice.Index, rw.Index].Value);
                if (unit.CONVERSION_UNIT == null) continue;
                list.Add(unit);
            }
            return list;
        }
        public void AddUOM(UnitConversion u)
        {
            u.CONVERSION_UNIT = (Unit)RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.UNIT_REPOSITORY).GetById(u.CONVERSION_UNIT);
            int index = dataGridViewUOM.Rows.Add(1, u.CONVERSION_UNIT.CODE, u.CONVERSION_QTY,
                m_part.UNIT.CODE, u.COST_PRICE, u.SELL_PRICE);
            dataGridViewUOM.Rows[index].Tag = u;
        }
        private void deleteUomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridViewUOM.Rows.Remove(dataGridViewUOM.CurrentRow);
        }

        private void dataGridViewUOM_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Delete)
            {
                if (dataGridViewUOM.CurrentRow != null) 
                    deleteUomToolStripMenuItem_Click(sender, null);
            }
        }
    }
}
