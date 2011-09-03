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
    public partial class FieldChooserForm : KryptonForm
    {
        DataGridView m_grid = null;
        int userID = 0;
        string formName = "";
        UserSettingsRepository r_setting = RepositoryFactory.GetInstance().UserSetting();


        public FieldChooserForm(int userid, string frmNAme,  DataGridView grid)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
            m_grid = grid;
            foreach (DataGridViewColumn v in m_grid.Columns)
            {
                int t = columnKryptonCheckedListBox.Items.Add(new ColumnChooser(v));
                columnKryptonCheckedListBox.SetItemChecked(t, v.Visible);
            }
            userID = userid;
            formName = frmNAme;
        }

        private class ColumnChooser
        {
            public DataGridViewColumn COLUMN;
            public ColumnChooser(DataGridViewColumn c)
            {
                COLUMN = c;
            }
            public override string ToString()
            {
                return COLUMN.HeaderText;
            }
        }

        private void columnKryptonCheckedListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            ColumnChooser cc = (ColumnChooser)columnKryptonCheckedListBox.Items[e.Index];
            cc.COLUMN.Visible = e.NewValue == CheckState.Checked;
            Type p = columnKryptonCheckedListBox.Items[e.Index].GetType();
 
        }

        private void FieldChooserForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //foreach (ColumnChooser cc in columnKryptonCheckedListBox.Items)
            //{
            //    r_setting.SaveSettings(userID, formName+cc.COLUMN.Name, typeof(bool), cc.COLUMN.Visible.ToString());
            //}
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
