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
    public partial class StockReportForm : KryptonForm, IChildForm
    {
        IMainForm m_mainForm;
        SupplierRepository r_sup = (SupplierRepository)RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.SUPPLIER_REPOSITORY);
        Repository r_emp = RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.EMPLOYEE_REPOSITORY);
        Repository r_top = RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.TOP_REPOSITORY);
        Repository r_ccy = RepositoryFactory.GetInstance().GetRepository(RepositoryFactory.CURRENCY_REPOSITORY);

        ArrayList l_supplier = new ArrayList();

        public StockReportForm(IMainForm mainForm, string formName)
        {
            InitializeComponent();
            InitialDataSource();
            this.MdiParent = (Form)mainForm;
            this.Name = formName;
            m_mainForm = mainForm;
        }

        private void InitialDataSource()
        {
            l_supplier.Add(new Supplier(0, "ALL"));
            l_supplier.AddRange(r_sup.GetAllActive());

            IList status = new ArrayList();
            status.Add("ALL");
            status.Add("TRUE");
            status.Add("FALSE");
            

            //supplierkryptonComboBox.DataSource = l_supplier;
            //trtypekryptonComboBox1.DataSource = Enum.GetValues(typeof(StockCardEntryTypeSupplier));
           // statuskryptonComboBox2.DataSource = status;
        }
       
      

        private void SupplierTransactionSummaryForm_Load(object sender, EventArgs e)
        {
           // UserSetting.LoadSetting(transactionkryptonDataGridView, m_mainForm.CurrentUser.ID, this.Name);
        }

        private void SupplierTransactionSummaryForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //UserSetting.SaveSetting(transactionkryptonDataGridView, m_mainForm.CurrentUser.ID, this.Name);
        }

        private void toolStripButtonClear_Click(object sender, EventArgs e)
        {
            //InitialDataSource();
           // transactionkryptonDataGridView.Rows.Clear();
        }

        #region IChildForm Members

        public void Save(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public void Edit(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public void Delete(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public void Clear(object sender, EventArgs e)
        {
            toolStripButtonClear_Click(null, null);
        }

        public void Refresh(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public void Print(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion

        private void SupplierTransactionSummaryForm_Activated(object sender, EventArgs e)
        {
            m_mainForm.EnableButtonSave(toolStripButtonSave.Enabled);
            m_mainForm.EnableButtonEdit(toolStripButtonEdit.Enabled);
            m_mainForm.EnableButtonDelete(toolStripButtonDelete.Enabled);
            m_mainForm.EnableButtonRefresh(toolStripButtonRefresh.Enabled);
            m_mainForm.EnableButtonClear(true);
        }

        private void transactionkryptonDataGridView_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
           // UserSetting.AddNumberToGrid(transactionkryptonDataGridView);
        }

        private void transactionkryptonDataGridView_Sorted(object sender, EventArgs e)
        {
          //  UserSetting.AddNumberToGrid(transactionkryptonDataGridView);
        }
    }
}
