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
    public partial class UserForm : KryptonForm, IChildForm
    {
        User m_user = new User();
        UserRepository r_userRep = (UserRepository)RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.USER_REPOSITORY);
        IMainForm m_mainForm;

        public UserForm(IMainForm mainForm, string formName)
        {
            InitializeComponent();
            InitializeButtonClick();
            this.MdiParent = (Form)mainForm;
            this.Name = formName;
            m_mainForm = mainForm;
            InitializeDataSource();
            loadRecords();
        }

        private void InitializeDataSource()
        {
            Utils.GetListCode(FormAccessCodeColumn.Items, m_mainForm.GetFormAccessList());
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
                IList records = r_userRep.GetAll();
                foreach (User d in records)
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
                    if (m_user.ID == 0)
                    {
                        r_userRep.Save(m_user);
                        User bank = (User)r_userRep.getUser(m_user.CODE);
                        int r = gridData.Rows.Add(bank.CODE, bank.NAME);
                        gridData.Rows[r].Tag = bank;
                    }
                    else
                    {
                        r_userRep.Update(m_user);
                        updateRecord();
                    }
                    KryptonMessageBox.Show("Record has been saved","Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (m_mainForm.CurrentUser.ID == m_user.ID)
                        m_mainForm.CurrentUser = m_user;
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
                User dep = (User)item.Tag;
                if (dep.ID == m_user.ID)
                {
                    gridData[0, item.Index].Value = m_user.CODE;
                    gridData[1, item.Index].Value = m_user.NAME;
                    break;
                }
            }
        }
        public bool Valid()
        {
            bool a = textBoxCode.Text == "";
            bool b = textBoxName.Text == "";
            bool c = passwordKryptonTextBox.Text == "";

            if (a) errorProvider1.SetError(textBoxCode, "Code Can not Empty");
            if (b) errorProvider1.SetError(textBoxName, "Name Can not Empty");
            if (c) errorProvider1.SetError(passwordKryptonTextBox, "Password Can not Empty");

            return !a && !b && !c;
        }
        private void UpdateEntity()
        {
            m_user.CODE = textBoxCode.Text.Trim();
            m_user.NAME = textBoxName.Text.Trim();
            m_user.PASSWORD = passwordKryptonTextBox.Text.Trim();
            m_user.ACTIVE = activekryptonCheckBox.Checked;
            m_user.FORM_ACCESS_LIST.Clear();
            for (int i = 0; i < formAccessKryptonDataGridView1.Rows.Count; i++)
            {
                if(formAccessKryptonDataGridView1[FormAccessCodeColumn.Index, i].Value==null)
                    continue;
                FormAccess c = (FormAccess)formAccessKryptonDataGridView1.Rows[i].Tag;
                if (c == null)
                    c = new FormAccess();
                FormAccess x = (FormAccess)Utils.FindEntityInList(formAccessKryptonDataGridView1[FormAccessCodeColumn.Index, i].Value.ToString(), m_mainForm.GetFormAccessList());
                c.CODE = x.CODE;
                c.NAME = x.NAME;
                c.SAVE = Convert.ToBoolean(formAccessKryptonDataGridView1[SaveColumn.Index, i].Value);
                c.DELETE = Convert.ToBoolean(formAccessKryptonDataGridView1[DeleteColumn.Index, i].Value);
                c.VIEW = Convert.ToBoolean(formAccessKryptonDataGridView1[ViewColumn.Index, i].Value);
                c.POST = Convert.ToBoolean(formAccessKryptonDataGridView1[ViewColumn.Index, i].Value);
                c.PRINT = Convert.ToBoolean(formAccessKryptonDataGridView1[ViewColumn.Index, i].Value);
                c.MODIFIED_BY = m_mainForm.CurrentUser.NAME;
                c.MODIFIED_COMPUTER_NAME = Environment.MachineName;
                c.USER = m_user;
                if(!m_user.FORM_ACCESS_LIST.ContainsKey(c.CODE))
                    m_user.FORM_ACCESS_LIST.Add(c.CODE, c);
            }
        }
        public void ClearForm()
        {
            try
            {
                textBoxCode.Text = "";
                textBoxName.Text = "";
                passwordKryptonTextBox.Text = "";
                formAccessKryptonDataGridView1.Rows.Clear();
                m_user = new User();
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
            passwordKryptonTextBox.ReadOnly = !enable;
            FormAccessCodeColumn.ReadOnly = !enable;
            SaveColumn.ReadOnly = !enable;
            DeleteColumn.ReadOnly = !enable;
            ViewColumn.ReadOnly = !enable;
            PostColumn.ReadOnly = !enable;
            PrintColumn.ReadOnly = !enable;
            formAccessKryptonDataGridView1.AllowUserToAddRows = enable;
            formAccessKryptonDataGridView1.AllowUserToDeleteRows = enable;
        }
        private void setEditMode(EditMode editmode)
        {
            toolStripButtonSave.Enabled = (editmode == EditMode.New || editmode == EditMode.Update) && m_mainForm.CurrentUser.FORM_ACCESS_LIST[Name].SAVE;
            toolStripButtonEdit.Enabled = (editmode == EditMode.View) && m_mainForm.CurrentUser.FORM_ACCESS_LIST[Name].SAVE;
            toolStripButtonDelete.Enabled = (editmode == EditMode.View) && m_mainForm.CurrentUser.FORM_ACCESS_LIST[Name].DELETE;
            toolStripButtonClear.Enabled = m_mainForm.CurrentUser.FORM_ACCESS_LIST[Name].SAVE;
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
                if (m_user.ID > 0)
                {
                    this.Cursor = Cursors.WaitCursor;
                    if (KryptonMessageBox.Show("Are you sure to delete this record?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.No) { this.Cursor = Cursors.Default; return; }
                    r_userRep.Delete(m_user);
                    removeRecord(m_user.ID);
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
                User dep = (User)item.Tag;
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
            m_user = (User)gridData.SelectedRows[0].Tag;
            if (m_user == null) return;
            loadData();
            setEnableForm(false);
            setEditMode(EditMode.View);
        }
        private void loadData()
        {
            try
            {
                m_user = r_userRep.getUser(m_user.CODE);
                textBoxCode.Text = m_user.CODE;
                textBoxName.Text = m_user.NAME;
                passwordKryptonTextBox.Text = m_user.PASSWORD;
                activekryptonCheckBox.Checked = m_user.ACTIVE;
                foreach (string kys in m_user.FORM_ACCESS_LIST.Keys)
                {
                    FormAccess f = m_user.FORM_ACCESS_LIST[kys];
                    int t = formAccessKryptonDataGridView1.Rows.Add(f.NAME, f.SAVE, f.DELETE, f.VIEW, f.POST, f.PRINT);
                    formAccessKryptonDataGridView1.Rows[t].Tag = f;
                }
            }
            catch (Exception x)
            { 
            }
        }

        #region IChildForm Members


        public void Refresh(object sender, EventArgs e)
        {
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
    }
}
