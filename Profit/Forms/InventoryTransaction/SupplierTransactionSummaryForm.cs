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
    public partial class SupplierTransactionSummaryForm : KryptonForm//, IChildForm
    {
        Bank m_bank = new Bank();
        IMainForm m_mainForm;

        public SupplierTransactionSummaryForm(IMainForm mainForm, string formName)
        {
            InitializeComponent();
           
        }
    }
}
