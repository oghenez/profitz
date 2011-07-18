using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Profit
{
    public interface IChildForm
    {
        void Save(object sender, EventArgs e);
        void Edit(object sender, EventArgs e);
        void Delete(object sender, EventArgs e);
        void Clear(object sender, EventArgs e);
        void Refresh(object sender, EventArgs e);
        void Print(object sender, EventArgs e);
    }
}
