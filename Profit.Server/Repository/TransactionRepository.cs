﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

namespace Profit.Server
{
    public abstract class TransactionRepository
    {
        protected MySql.Data.MySqlClient.MySqlConnection m_connection = new MySql.Data.MySqlClient.MySqlConnection("server=localhost;database=profit_db;uid=root;pwd=1234");
        protected MySql.Data.MySqlClient.MySqlCommand m_command = new MySql.Data.MySqlClient.MySqlCommand();
        abstract protected void doConfirm(Event events, Period p);
        abstract protected void doRevise(Event events, Period p);
        abstract protected IList doSearch(string find);
        abstract protected bool doIsCodeExist(string code);

        //abstract protected void doInitEventRepository();
        //protected EventRepository m_eventRepository;

        public TransactionRepository()
        {
            m_connection.Open();
            m_command.Connection = m_connection;
        }
        public void Confirm(int id)
        {
            MySql.Data.MySqlClient.MySqlTransaction trc = m_connection.BeginTransaction();
            m_command.Transaction = trc;
            try
            {
                Event events = this.Get(id);
                if (events.POSTED) //.EVENT_STATUS == EventStatus.Confirm)
                    throw new Exception("Status is already Posted/Confirm");
                Period p = AssertValidPeriod(events.TRANSACTION_DATE);
                doConfirm(events, p);
                events.ProcessConfirm();
                this.UpdateStatus(events, true);
                updateStockCards(events.EVENT_ITEMS);
                trc.Commit();
            }
            catch (Exception x)
            {
                trc.Rollback();
                throw x;
            }
        }
        public void Revise(int id)
        {
            MySql.Data.MySqlClient.MySqlTransaction trc = m_connection.BeginTransaction();
            m_command.Transaction = trc;
            try
            {
                Event events = this.Get(id);
                if (events.EVENT_STATUS == EventStatus.Entry)
                    throw new Exception("Status is already Unposted/Entry");
                Period p = AssertValidPeriod(events.TRANSACTION_DATE);
                doRevise(events, p);
                events.ProcessRevised();
                this.UpdateStatus(events, false);
                deleteStockCardEntry(events.DELETED_STOCK_CARD_ENTRY);
                updateStockCards(events.EVENT_ITEMS);
                trc.Commit();
            }
            catch (Exception x)
            {
                trc.Rollback();
                throw x;
            }
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
                    StockCardRepository.UpdateHeader(m_command, sc);
                else if (sc.ID == 0)
                    StockCardRepository.SaveHeader(m_command, sc);
                if (item.STOCK_CARD_ENTRY != null)
                {
                    StockCardEntryRepository.Save(m_command, item.STOCK_CARD_ENTRY);
                }
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
            StockCard sc = StockCardRepository.FindStockCardHeader(m_command, item.PART.ID, item.WAREHOUSE.ID, period, item.EVENT.TRANSACTION_DATE);
            item.WAREHOUSE = StockCardRepository.FindWarehouse(m_command, item.WAREHOUSE.ID);
            item.PART.UNIT = StockCardRepository.FindUnit(m_command, item.PART.UNIT.ID);
            if (sc == null)
            {
                sc = StockCard.CreateStockCard(item, period);
            }
            item.STOCK_CARD = sc;
            item.STOCK_CARD.PART = item.PART;
        }

        abstract protected void doSave(Event e);
        abstract protected void doUpdate(Event e);
        abstract protected void doDelete(Event e);
        abstract protected Event doGet(int ID);
        abstract protected void doUpdateStatus(Event e, bool p);

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
        public bool IsCodeExist(string code)
        {
            return doIsCodeExist(code);
        }
        public IList Search(string find)
        {
            return doSearch(find);
        }
        public void UpdateStatus(Event e, bool p)
        {
            doUpdateStatus(e, p);
        }
        public virtual Event FindLastCodeAndTransactionDate(string codesample)
        {
            throw new Exception("FindLastCodeAndTransactionDate is not implemented");
        }
    }
}
