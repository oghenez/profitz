using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Profit.Server;

namespace Profit
{
    public interface IMainForm
    {
        IList GetFormAccessList();
        void EnableButtonSave(bool enable);
        void EnableButtonEdit(bool enable);
        void EnableButtonDelete(bool enable);
        void EnableButtonClear(bool enable);
        void EnableButtonRefresh(bool enable);
        User CurrentUser { get; set; }
        Period CurrentPeriod { get; set; }
        GeneralSetup GeneralSetup { get; }
        //void SetUser(User user);
        //User CurrentUser { get; }
    }
}
