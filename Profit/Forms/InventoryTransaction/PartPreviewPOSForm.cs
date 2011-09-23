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
    public partial class PartPreviewPOSForm : KryptonForm
    {
        Timer mtimer = new Timer();
        public PartPreviewPOSForm(Part part)
        {
            InitializeComponent();
            mtimer.Interval = 500;
            mtimer.Tick += new EventHandler(mtimer_Tick);
            desckryptonLabel3.Text = part == null ? "Item not found" : part.NAME;
            mtimer.Start();
        }

        void mtimer_Tick(object sender, EventArgs e)
        {
            mtimer.Stop();
            this.Close();
        }
    }
}
