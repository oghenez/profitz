using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

namespace Profit.Server
{
    public class PurchaseReturn : Event
    {
        public Supplier SUPPLIER = null;
        public double TOTAL_AMOUNT_FROM_PO = 0;
        public PurchaseReturn()
            : base()
        { }
        public PurchaseReturn(int id)
            : base()
        {
            ID = id;
        }
        public override string GetInsertSQL()
        {
            return String.Format(@"insert into table_purchasereturn 
                (   prn_date,
                    prn_noticedate,
                    prn_scentrytype,
                    emp_id,
                    prn_notes,
                    prn_posted,
                    prn_eventstatus,
                    prn_code,
                    sup_id,
                    prn_docno,
                    prn_docdate
                ) 
                VALUES ('{0}','{1}','{2}',{3},'{4}',{5},'{6}','{7}',{8},'{9}','{10}')",
                TRANSACTION_DATE.ToString(Utils.DATE_FORMAT),
                NOTICE_DATE.ToString(Utils.DATE_FORMAT),
                StockCardEntryType.PurchaseReturn.ToString(),
                EMPLOYEE.ID,
                NOTES,
                POSTED,
                EVENT_STATUS.ToString(),
                CODE,
                SUPPLIER == null ? 0 : SUPPLIER.ID,
                DOCUMENT_NO,
                DOCUMENT_DATE.ToString(Utils.DATE_FORMAT)
                );
        }
        public override string GetUpdateSQL()
        {
            return String.Format(@"update table_purchasereturn set 
                    prn_date = '{0}',
                    prn_noticedate= '{1}',
                    prn_scentrytype= '{2}',
                    emp_id= {3},
                    prn_notes= '{4}',
                    prn_posted= {5},
                    prn_eventstatus= '{6}',
                    prn_code = '{7}',
                    sup_id = {8},
                    prn_docno = '{9}',
                    prn_docdate ='{10}'
                where prn_id = {11}",
                TRANSACTION_DATE.ToString(Utils.DATE_FORMAT),
                NOTICE_DATE.ToString(Utils.DATE_FORMAT),
                StockCardEntryType.PurchaseReturn.ToString(),
                EMPLOYEE.ID,
                NOTES,
                POSTED,
                EVENT_STATUS.ToString(),
                CODE,
                SUPPLIER == null ? 0 : SUPPLIER.ID,
                 DOCUMENT_NO,
                DOCUMENT_DATE.ToString(Utils.DATE_FORMAT),
                ID);
        }
        public static PurchaseReturn TransformReader(OdbcDataReader aReader)
        {
            PurchaseReturn transaction = null;
            if (aReader.HasRows)
            {
                aReader.Read();
                transaction = new PurchaseReturn();
                transaction.ID = Convert.ToInt32(aReader["prn_id"]);
                transaction.TRANSACTION_DATE = Convert.ToDateTime(aReader["prn_date"]);
                transaction.NOTICE_DATE = Convert.ToDateTime(aReader["prn_noticedate"]);
                transaction.STOCK_CARD_ENTRY_TYPE = (StockCardEntryType)Enum.Parse(typeof(StockCardEntryType), aReader["prn_scentrytype"].ToString());
                transaction.EMPLOYEE = new Employee(Convert.ToInt32(aReader["emp_id"]));
                transaction.NOTES = aReader["prn_notes"].ToString();
                transaction.POSTED = Convert.ToBoolean(aReader["prn_posted"]);
                transaction.EVENT_STATUS = (EventStatus)Enum.Parse(typeof(EventStatus), aReader["prn_eventstatus"].ToString());
                transaction.CODE = aReader["prn_code"].ToString();
                transaction.SUPPLIER = new Supplier(Convert.ToInt32(aReader["sup_id"]));
                transaction.DOCUMENT_NO = aReader["prn_docno"].ToString();
                transaction.DOCUMENT_DATE = Convert.ToDateTime(aReader["prn_docdate"]);
            }
            return transaction;
        }
        public static IList TransformReaderList(OdbcDataReader aReader)
        {
            IList result = new ArrayList();
            while (aReader.Read())
            {
                PurchaseReturn transaction = new PurchaseReturn();
                transaction.ID = Convert.ToInt32(aReader["prn_id"]);
                transaction.TRANSACTION_DATE = Convert.ToDateTime(aReader["prn_date"]);
                transaction.NOTICE_DATE = Convert.ToDateTime(aReader["prn_noticedate"]);
                transaction.STOCK_CARD_ENTRY_TYPE = (StockCardEntryType)Enum.Parse(typeof(StockCardEntryType), aReader["prn_scentrytype"].ToString());
                transaction.EMPLOYEE = new Employee(Convert.ToInt32(aReader["emp_id"]));
                transaction.NOTES = aReader["prn_notes"].ToString();
                transaction.POSTED = Convert.ToBoolean(aReader["prn_posted"]);
                transaction.EVENT_STATUS = (EventStatus)Enum.Parse(typeof(EventStatus), aReader["prn_eventstatus"].ToString());
                transaction.CODE = aReader["prn_code"].ToString();
                transaction.SUPPLIER = new Supplier(Convert.ToInt32(aReader["sup_id"]));
                transaction.DOCUMENT_NO = aReader["prn_docno"].ToString();
                transaction.DOCUMENT_DATE = Convert.ToDateTime(aReader["prn_docdate"]);
                result.Add(transaction);
            }
            return result;
        }
        public static string SelectMaxIDSQL()
        {
            return String.Format("SELECT max(prn_id) from table_purchasereturn");
        }
        public static new string GetByIDSQL(int id)
        {
            return String.Format("SELECT * from table_purchasereturn where prn_id ={0}", id);
        }
        public static string DeleteSQL(int id)
        {
            return String.Format("Delete from table_purchasereturn where prn_id ={0}", id);
        }
        public static string GetEventStatus(int id)
        {
            return String.Format("SELECT prn_eventstatus from table_purchasereturn where prn_id ={0}", id);
        }
        public static string GetUpdateStatusSQL(Event e)
        {
            return String.Format(@"update table_purchasereturn set 
                    prn_posted= {0},
                    prn_eventstatus= '{1}'
                where prn_id = {2}",
                e.POSTED,
                e.EVENT_STATUS.ToString(),
                e.ID);
        }
        public static string GetSearch(string find)
        {
            return String.Format(@"select * from table_purchasereturn p
                INNER JOIN table_employee e on e.emp_id = p.emp_id
                where concat(p.prn_code, e.emp_code, e.emp_name)
                like '%{0}%'", find);
        }
        public static string SelectCountByCode(string code)
        {
            return String.Format("SELECT count(*) from table_purchasereturn where prn_code ='{0}'", code);
        }
        public static string FindLastCodeAndTransactionDate(string code)
        {
            return String.Format(@"select * from table_purchasereturn p where p.prn_code like '%{0}%' ORDER BY p.prn_id DESC", code);
        }
        public static string RecordCount()
        {
            return @"select Count(*) from table_purchasereturn p";
        }
        public static string GetSearchPRNoForAPDN(string find, int supplierID, string poi, DateTime trdate)
        {
            return String.Format(@"SELECT p.*
                FROM table_purchasereturn p
                where p.prn_posted = true
                and p.prn_code like '%{0}%'
                and p.sup_id = {1}
                and p.prn_date <= '{2}'
               {3}", find, supplierID, trdate.ToString(Utils.DATE_FORMAT), poi != "" ? " and p.prn_id not in (" + poi + ")" : "");
        }
    }
}
