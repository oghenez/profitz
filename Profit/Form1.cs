using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using ComponentFactory.Krypton.Navigator;

namespace WindowsFormsApplication1
{
    public partial class Form1 : KryptonForm
    {
        public Form1()
        {
            InitializeComponent();
            kryptonManager1.GlobalPaletteMode = PaletteModeManager.Office2010Blue;
            toolStripComboBox1.Items.AddRange(Enum.GetNames(typeof(PaletteModeManager)));
        }

        private void buttonSpecExpandCollapse_Click(object sender, EventArgs e)
        {
            if (kryptonNavigatorMain.NavigatorMode == NavigatorMode.OutlookFull)
            {
                kryptonNavigatorMain.NavigatorMode = NavigatorMode.OutlookMini;
                splitter1.Enabled = false;
                kryptonNavigatorMain.Width = 35;
                buttonSpecExpandCollapse.TypeRestricted = PaletteNavButtonSpecStyle.ArrowRight;
            }
            else
            {
                kryptonNavigatorMain.NavigatorMode = NavigatorMode.OutlookFull;
                splitter1.Enabled = true;
                kryptonNavigatorMain.Width = 194;
                buttonSpecExpandCollapse.TypeRestricted = PaletteNavButtonSpecStyle.ArrowLeft;
            }

        }
        private void toolStripComboBox1_TextChanged(object sender, EventArgs e)
        {
            string c= sender.ToString();;
            kryptonManager1.GlobalPaletteMode = (PaletteModeManager)Enum.Parse(typeof(PaletteModeManager), toolStripComboBox1.SelectedItem.ToString());
        }
    }
}
