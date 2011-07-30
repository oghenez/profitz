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
    public partial class SearchForm : KryptonForm
    {
        Repository r_rep;

        public SearchForm(Point p, Repository rep)
        {
            InitializeComponent();
            this.Location = p;
            r_rep = rep;
            searchText.Focus();
        }

        private void buttonSpecAny1_Click(object sender, EventArgs e)
        {
            resultData.Rows.Clear();
            IList result = r_rep.GetConcatSearch(searchText.Text);
            foreach (IEntity p in result)
            {
                resultData.Rows.Add(p.GetCode(), "");
            }
        }

        private void searchText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                resultData.Rows.Clear();
                IList result = r_rep.GetConcatSearch(searchText.Text);
                foreach (IEntity p in result)
                {
                    resultData.Rows.Add(p.GetCode(), "");
                }
            }
        }
    }
}
