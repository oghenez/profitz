using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Profit.Server;

namespace Profit.Test
{
    public class StockTakingTest
    {
        StockTaking m_stockTaking = new StockTaking();
        StockTakingRepository m_stRep = new StockTakingRepository();

        public StockTakingTest()
        {
            //Create();
            //Get();
           // Delete();
            //Post();
            Unpost();
        }

        private void Unpost()
        {
            m_stRep.Revise(4);
        }

        private void Post()
        {
            m_stRep.Confirm(4);
        }

        private void Delete()
        {
            m_stRep.Delete(new StockTaking(3));
        }

        private void Get()
        {
            m_stockTaking = (StockTaking)m_stRep.Get(2);
        }

        private void Create()
        {
            m_stockTaking.AMOUNT = 500000;
            m_stockTaking.CURRENCY = new Currency(1);
            m_stockTaking.EMPLOYEE = new Employee(1);
            m_stockTaking.EVENT_STATUS = EventStatus.Entry;
            m_stockTaking.NOTES = "test insert";
            m_stockTaking.NOTICE_DATE = DateTime.Today;
            m_stockTaking.POSTED = false;
            m_stockTaking.STOCK_CARD_ENTRY_TYPE = StockCardEntryType.StockTaking;
            m_stockTaking.STOCK_TAKING_TYPE = StockTakingType.Adjustment;
            m_stockTaking.TRANSACTION_DATE = DateTime.Today;
            m_stockTaking.WAREHOUSE = new Warehouse(1);

            StockTakingItems it1 = new StockTakingItems();
            it1.AMOUNT = 3;
            it1.EVENT = m_stockTaking;
            it1.PART = new Part(9068);
            it1.PRICE = 100000;
            it1.STOCK_CARD_ENTRY_TYPE = StockCardEntryType.StockTaking;
            it1.TOTAL_AMOUNT = 300000;
            it1.UNIT = new Unit(1);
            it1.WAREHOUSE = new Warehouse(1);

            StockTakingItems it2 = new StockTakingItems();
            it2.AMOUNT = 2;
            it2.EVENT = m_stockTaking;
            it2.PART = new Part(9068);
            it2.PRICE = 50000;
            it2.STOCK_CARD_ENTRY_TYPE = StockCardEntryType.StockTaking;
            it2.TOTAL_AMOUNT = 200000;
            it2.UNIT = new Unit(1);
            it2.WAREHOUSE = new Warehouse(1);

            m_stockTaking.EVENT_ITEMS.Add(it1);
            m_stockTaking.EVENT_ITEMS.Add(it2);

            m_stRep.Save(m_stockTaking);
        }
    }
}
