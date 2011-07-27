using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Profit.Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void kryptonButton1_Click(object sender, EventArgs e)
        {
            try
            {
                PurchaseOrderTest stTest = new PurchaseOrderTest();
            }
            catch (Exception x)
            {
                ComponentFactory.Krypton.Toolkit.KryptonMessageBox.Show(x.Message);
            }
        }
    }
}
