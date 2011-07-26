using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Profit.Server
{
    public abstract class EventRepository
    {
        abstract protected void doSave(Event e);
        abstract protected void doUpdate(Event e);
        abstract protected void doDelete(Event e);
        abstract protected Event doGet(int ID);

        public void Save(Event e)
        {
            doSave(e);
        }
        public Event Get(int ID)
        {
            return doGet(ID);
        }
        public void Update(Event e)
        {
            doUpdate(e);
        }
        public void Delete(Event e)
        {
            doDelete(e);
        }
    }
}
