using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

namespace Profit.Server
{
    public abstract class TransactionRepository
    {
        protected OdbcCommand m_command;
        abstract protected void doConfirm(Event events, Period p);
        abstract protected void doRevise(Event events, Period p);
        abstract protected void do();
        protected EventRepository m_eventRepository;

        public void Confirm(int id)
        {
            Event events = m_eventRepository.Get(id);
            Period p = AssertValidPeriod(events.TRANSACTION_DATE);
            doConfirm(events, p);
            events.ProcessConfirm();
            m_eventRepository.Update(events);
            updateStockCards(events.EVENT_ITEMS);
        }
        public void Revise(int id)
        {
            Event events = m_eventRepository.Get(id);
            Period p = AssertValidPeriod(events.TRANSACTION_DATE);
            doRevise(events, p);
            events.ProcessRevised();
            m_eventRepository.Update(events);
            deleteStockCardEntry(events.DELETED_STOCK_CARD_ENTRY);
            updateStockCards(events.EVENT_ITEMS);
        }
        protected Period AssertValidPeriod(DateTime transactionDate)
        {
            Period transactionPeriod = PeriodRepository.FindPeriodByDate(m_command, transactionDate);
            if (transactionPeriod == null)
                throw new Exception("Period not set");
            if (transactionPeriod.PERIOD_STATUS.Equals(PeriodStatus.Close) || transactionPeriod.PERIOD_STATUS.Equals(PeriodStatus.Open))
                throw new Exception("Transaction is not in Current Period");
            return transactionPeriod;
        }
        protected void updateStockCards(IList items)
        {
            foreach (EventItem item in items)
            {
                StockCard sc = item.STOCK_CARD;
                if (sc.ID > 0)
                    StockCardRepository.Update(m_command, sc);
                else if (sc.ID == 0)
                    StockCardRepository.Save(m_command, sc);
            }
        }
        protected void deleteStockCardEntry(IList sces)
        {
            foreach (StockCardEntry sce in sces)
            {
                StockCardEntryRepository.Delete(m_command, sce);
            }
        }
        protected void SetStockCard(EventItem item, Period period)
        {
            StockCard sc = StockCardRepository.FindStockCard(m_command, item.PART.ID, item.WAREHOUSE.ID, period.ID);
            if (sc == null)
            {
                sc = StockCard.CreateStockCard(item, period);
            }
            item.STOCK_CARD = sc;
        }

    }
}
