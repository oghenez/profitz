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
    public partial class WarehouseForm : KryptonForm, IChildForm
    {
        Warehouse m_warehouse = new Warehouse();
        IMainForm m_mainForm;

        public WarehouseForm(IMainForm mainForm, string formName)
        {
            InitializeComponent();
            InitializeButtonClick();
            this.MdiParent = (Form)mainForm;
            this.Name = formName;
            m_mainForm = mainForm;
            loadRecords();
        }
        private void InitializeButtonClick()
        {
            toolStripButtonSave.Click += new EventHandler(Save);
            toolStripButtonEdit.Click += new EventHandler(Edit);
            toolStripButtonDelete.Click += new EventHandler(Delete);
            toolStripButtonClear.Click += new EventHandler(Clear);
        }
        private void loadRecords()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                gridData.Rows.Clear();
                IList records = RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.WAREHOUSE_REPOSITORY).GetAll();
                foreach (Warehouse d in records)
                {
                    int row = gridData.Rows.Add(d.CODE, d.NAME);
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
                    if (m_warehouse.ID == 0)
                    {
                        RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.WAREHOUSE_REPOSITORY).Save(m_warehouse);
                        Warehouse bank = (Warehouse)RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.WAREHOUSE_REPOSITORY).GetByCode(m_warehouse);
                        int r = gridData.Rows.Add(bank.CODE, bank.NAME);
                        gridData.Rows[r].Tag = bank;
                    }
                    else
                    {
                        RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.WAREHOUSE_REPOSITORY).Update(m_warehouse);
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
                Warehouse dep = (Warehouse)item.Tag;
                if (dep.ID == m_warehouse.ID)
                {
                    gridData[0, item.Index].Value = m_warehouse.CODE;
                    gridData[1, item.Index].Value = m_warehouse.NAME;
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
            m_warehouse.CODE = textBoxCode.Text.Trim();
            m_warehouse.NAME = textBoxName.Text.Trim();
        }
        public void ClearForm()
        {
            try
            {
                textBoxCode.Text = "";
                textBoxName.Text = "";
                m_warehouse = new Warehouse();
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
                if (m_warehouse.ID > 0)
                {
                    this.Cursor = Cursors.WaitCursor;
                    if (KryptonMessageBox.Show("Are you sure to delete this record?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No) { this.Cursor = Cursors.Default; return; }
                    RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.WAREHOUSE_REPOSITORY).Delete(m_warehouse);
                    removeRecord(m_warehouse.ID);
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
                Warehouse dep = (Warehouse)item.Tag;
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
            m_warehouse = (Warehouse)gridData.SelectedRows[0].Tag;
            if (m_warehouse == null) return;
            loadData();
            setEnableForm(false);
            setEditMode(EditMode.View);
        }
        private void loadData()
        {
            textBoxCode.Text = m_warehouse.CODE;
            textBoxName.Text = m_warehouse.NAME;
        }

        #region IChildForm Members


        public void Refresh(object sender, EventArgs e)
        {
           
        }

        public void Print(object sender, EventArgs e)
        {
            
        }

        #endregion

        private void BankForm_Activated(object sender, EventArgs e)
        {
            ReloadMainFormButton();
        }
    }
}
