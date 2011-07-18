using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Profit
{
    public interface IMainForm
    {
        void EnableButtonSave(bool enable);
        void EnableButtonEdit(bool enable);
        void EnableButtonDelete(bool enable);
        void EnableButtonClear(bool enable);
        void EnableButtonRefresh(bool enable);
        //void SetUser(User user);
        //User CurrentUser { get; }
    }
}
