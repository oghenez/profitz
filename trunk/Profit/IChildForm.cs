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
    public interface IPOSChildForm
    {
        void Items(object sender, EventArgs e);
        void Barcode(object sender, EventArgs e);
        void Help(object sender, EventArgs e);
        void Refresh(object sender, EventArgs e);
        void Member(object sender, EventArgs e);
        void New(object sender, EventArgs e);
        void Save(object sender, EventArgs e);
        void Post(object sender, EventArgs e);
        void Print(object sender, EventArgs e);
        void Antrian(object sender, EventArgs e);
        void Exit(object sender, EventArgs e);
    }
}
