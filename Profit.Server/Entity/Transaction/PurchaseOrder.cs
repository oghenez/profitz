using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Profit.Server
{
    public class PurchaseOrder : Event
    {
        public Division DIVISION = null;
        public TermOfPayment TOP = null;
        public DateTime DUE_DATE = DateTime.Today;
        public Currency CURRENCY = null;
        public double SUB_TOTAL = 0;
        public double DISC_PERCENT = 0;
        public double DISC_AFTER_AMOUNT = 0;
        public double DISC_AMOUNT = 0;
        public Tax TAX = null;
        public double TAX_AFTER_AMOUNT = 0;
        public double OTHER_EXPENSE = 0;
        public double NET_TOTAL = 0;
        public AgainstStatus m_againstGRNStatus = AgainstStatus.Open;

        public PurchaseOrder()
            : base()
        { }
        public PurchaseOrder(int id)
            : base()
        {
            ID = id;
        }
        public override string GetInsertSQL()
        {
            return String.Format(@"insert into table_purchaseorder 
                (   po_date,
                    po_noticedate,
                    po_scentrytype,
                    emp_id,
                    po_notes,
                    po_posted,
                    po_eventstatus,
                    div_id,
                    top_id,
                    po_duedate,
                    ccy_id,
                    po_subtotal,
                    po_discpercent,
                    po_discafteramount,
                    po_discamount,
                    tax_id,
                    po_taxafteramount,
                    po_otherexpense,
                    po_nettotal,
                    po_againsgrnstatus
                ) 
                VALUES ('{0}','{1}','{2}',{3},'{4}',{5},'{6}',{7},{8},{9},'{10}')",
                TRANSACTION_DATE.ToString(Utils.DATE_FORMAT),
                NOTICE_DATE.ToString(Utils.DATE_FORMAT),
                STOCK_CARD_ENTRY_TYPE.ToString(),
                EMPLOYEE.ID,
                NOTES,
                POSTED,
                EVENT_STATUS.ToString(),
                DIVISION == null?0:DIVISION.ID,
                TOP == null?0:TOP.ID,
         DUE_DATE = DateTime.Today;
         CURRENCY = null;
         SUB_TOTAL = 0;
          DISC_PERCENT = 0;
          DISC_AFTER_AMOUNT = 0;
          DISC_AMOUNT = 0;
          TAX = null;
          TAX_AFTER_AMOUNT = 0;
          OTHER_EXPENSE = 0;
          NET_TOTAL = 0;
          m_againstGRNStatus = AgainstStatus.Open;
                );
        }
        public override string GetUpdateSQL()
        {
            return String.Format(@"update table_stocktaking set 
                    stk_date = '{0}',
                    stk_noticedate= '{1}',
                    stk_scentrytype= '{2}',
                    emp_id= {3},
                    stk_notes= '{4}',
                    stk_posted= {5},
                    stk_eventstatus= '{6}',
                    warehouse_id= {7},
                    stk_amount= {8},
                    ccy_id= {9}
                    stk_stocktakingtype = '{10}'
                where stk_id = {11}",
                TRANSACTION_DATE.ToString(Utils.DATE_FORMAT),
                NOTICE_DATE.ToString(Utils.DATE_FORMAT),
                STOCK_CARD_ENTRY_TYPE.ToString(),
                EMPLOYEE.ID,
                NOTES,
                POSTED,
                EVENT_STATUS.ToString(),
                WAREHOUSE.ID,
                AMOUNT,
                CURRENCY.ID,
                STOCK_TAKING_TYPE.ToString(),
                ID);
        }
        public static StockTaking TransformReader(OdbcDataReader aReader)
        {
            StockTaking transaction = null;
            while (aReader.Read())
            {
                transaction = new StockTaking();
                transaction.ID = Convert.ToInt32(aReader["stk_id"]);
                transaction.TRANSACTION_DATE = Convert.ToDateTime(aReader["stk_date"]);
                transaction.NOTICE_DATE = Convert.ToDateTime(aReader["stk_noticedate"]);
                transaction.STOCK_CARD_ENTRY_TYPE = (StockCardEntryType)Enum.Parse(typeof(StockCardEntryType), aReader["stk_scentrytype"].ToString());
                transaction.EMPLOYEE = new Employee(Convert.ToInt32(aReader["emp_id"]));
                transaction.NOTES = aReader["stk_notes"].ToString();
                transaction.POSTED = Convert.ToBoolean(aReader["stk_posted"]);
                transaction.EVENT_STATUS = (EventStatus)Enum.Parse(typeof(EventStatus), aReader["stk_eventstatus"].ToString());
                transaction.WAREHOUSE = new Warehouse(Convert.ToInt32(aReader["warehouse_id"]));
                transaction.AMOUNT = Convert.ToDouble(Convert.ToInt32(aReader["stk_amount"]));
                transaction.CURRENCY = new Currency(Convert.ToInt32(aReader["ccy_id"]));
                transaction.STOCK_TAKING_TYPE = (StockTakingType)Enum.Parse(typeof(StockTakingType), aReader["stk_stocktakingtype"].ToString());
            }
            return transaction;
        }
        public static IList TransformReaderList(OdbcDataReader aReader)
        {
            IList result = new ArrayList();
            while (aReader.Read())
            {
                StockTaking transaction = new StockTaking();
                transaction.ID = Convert.ToInt32(aReader["stk_id"]);
                transaction.TRANSACTION_DATE = Convert.ToDateTime(aReader["stk_date"]);
                transaction.NOTICE_DATE = Convert.ToDateTime(aReader["stk_noticedate"]);
                transaction.STOCK_CARD_ENTRY_TYPE = (StockCardEntryType)Enum.Parse(typeof(StockCardEntryType), aReader["stk_scentrytype"].ToString());
                transaction.EMPLOYEE = new Employee(Convert.ToInt32(aReader["emp_id"]));
                transaction.NOTES = aReader["stk_notes"].ToString();
                transaction.POSTED = Convert.ToBoolean(aReader["stk_posted"]);
                transaction.EVENT_STATUS = (EventStatus)Enum.Parse(typeof(EventStatus), aReader["stk_eventstatus"].ToString());
                transaction.WAREHOUSE = new Warehouse(Convert.ToInt32(aReader["warehouse_id"]));
                transaction.AMOUNT = Convert.ToDouble(Convert.ToInt32(aReader["stk_amount"]));
                transaction.CURRENCY = new Currency(Convert.ToInt32(aReader["ccy_id"]));
                transaction.STOCK_TAKING_TYPE = (StockTakingType)Enum.Parse(typeof(StockTakingType), aReader["stk_stocktakingtype"].ToString());
                result.Add(transaction);
            }
            return result;
        }
        public static string SelectMaxIDSQL()
        {
            return String.Format("SELECT max(stk_id) from table_stocktaking");
        }
        public static string GetByIDSQL(int id)
        {
            return String.Format("SELECT * from table_stocktaking where stk_id ={0}", id);
        }
        public static string DeleteSQL(int id)
        {
            return String.Format("Delete from table_stocktaking where stk_id ={0}", id);
        }
        public static string GetEventStatus(int id)
        {
            return String.Format("SELECT stk_eventstatus from table_stocktaking where stk_id ={0}", id);
        }
        public static string GetUpdateStatusSQL(int id, bool posted)
        {
            return String.Format(@"update table_stocktaking set 
                    stk_posted= {0},
                    stk_eventstatus= '{1}'
                where stk_id = {2}",
                posted,
                posted ? EventStatus.Confirm: EventStatus.Entry,
                id);
        }
    }
}
