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

namespace Profit
{
    public partial class POSPaymentForm : KryptonForm
    {
        POS m_POS = null;
        double m_netTotalAmount = 0;
        double m_cashPayAmount = 0;
        double m_changeAmount = 0;

        public POSPaymentForm(POS pos)
        {
            InitializeComponent();
            m_POS = pos;
            m_netTotalAmount = pos.NET_TOTAL;
            nettotalAmountkryptonNumericUpDown.Value = Convert.ToDecimal(m_netTotalAmount);
            TextBox t = (TextBox)cashkryptonNumericUpDown1.Controls[0].Controls[1];
            t.SelectAll();
        }

        private void kryptonNumericUpDown1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                m_cashPayAmount = Convert.ToDouble(cashkryptonNumericUpDown1.Value);
                m_changeAmount = m_cashPayAmount - m_netTotalAmount;
                changekryptonNumericUpDown2.Value = Convert.ToDecimal(m_changeAmount);
            }
        }

        private void POSPaymentForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.F4)
            {
                TextBox t = (TextBox)cashkryptonNumericUpDown1.Controls[0].Controls[1];
                t.SelectAll();
            }
            if (e.KeyData == Keys.F9)
            {
                double cashpay = Convert.ToDouble(cashkryptonNumericUpDown1.Value);
                double changepay = Convert.ToDouble(changekryptonNumericUpDown2.Value);
                if (cashpay > 0 && changepay >= 0 && (cashpay - m_netTotalAmount)>= 0)
                {
                    m_POS.CASH_PAY_AMOUNT = Convert.ToDouble(cashkryptonNumericUpDown1.Value);
                    m_POS.CHANGE_AMOUNT = Convert.ToDouble(changekryptonNumericUpDown2.Value);
                    this.DialogResult = DialogResult.OK;
                }
            }
            if (e.KeyData == Keys.Escape)
            {
                this.DialogResult = DialogResult.Cancel;
            }
        }
    }
}
