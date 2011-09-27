using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;
using System.Collections;

namespace Profit.Server
{
    public class Period : Entity, IEntity
    {
        string DATE_FORMAT = "yyyy/MM/dd";
        public int ID = 0;
        public string CODE = "B001";
        public PeriodStatus PERIOD_STATUS;
        public Year YEAR = new Year();
        public DateTime START_DATE;
        public DateTime END_DATA;
        public DateTime CLOSED_DATE;

        public Period()
        {
        }
        public Period(string pCode, PeriodStatus period, Year year, DateTime startDate, DateTime end)
        {
            CODE = pCode;
            PERIOD_STATUS = period;
            YEAR = year;
            START_DATE = startDate;
            END_DATA = end;
        }
        public Period(int id)
        {
            ID = id;
        }
        public Period(int id, string code)
        {
            ID = id;
            CODE = code;
        }
        public static Period TransformReader(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            Period period = null;
            while (aReader.Read())
            {
                period = new Period();
                period.ID = Convert.ToInt32(aReader[0]);
                period.CODE = aReader[1].ToString();
                period.PERIOD_STATUS = (PeriodStatus)Enum.Parse(typeof(PeriodStatus), aReader[2].ToString());
                period.YEAR = new Year(Convert.ToInt32(aReader[3]));
                period.START_DATE = Convert.ToDateTime(aReader[4]);
                period.END_DATA = Convert.ToDateTime(aReader[5]);
                period.CLOSED_DATE = Convert.ToDateTime(aReader[6]);
                period.MODIFIED_BY = aReader["modified_by"].ToString();
                period.MODIFIED_DATE = Convert.ToDateTime(aReader["modified_date"].ToString());
                period.MODIFIED_COMPUTER_NAME = aReader["modified_computer"].ToString();
            }
            return period;
        }
        public string GetInsertSQL()
        {
            return String.Format(@"insert into table_period 
                (period_code,
                period_status,
                year_id,
                period_start,
                period_end,
                period_close, modified_by, modified_date, modified_computer
                ) 
                VALUES ('{0}','{1}',{2},'{3}','{4}','{5}','{6}','{7}','{8}')",
                CODE, 
                PERIOD_STATUS.ToString(),
                YEAR.ID,
                START_DATE.ToString(DATE_FORMAT),
                END_DATA.ToString(DATE_FORMAT),
                CLOSED_DATE.ToString(DATE_FORMAT), 
                MODIFIED_BY, 
                DateTime.Now.ToString(Utils.DATE_FORMAT), 
                MODIFIED_COMPUTER_NAME
                );
        }
        public string GetDeleteSQL()
        {
            return "delete from table_period where period_id = " + ID;
        }
        public string GetUpdateSQL()
        {
            return String.Format(@"update table_period set 
                period_code = '{0}', 
                period_status= '{1}',
                year_id= {2},
                period_start= '{3}',
                period_end= '{4}',
                period_close= '{5}',
                modified_by='{6}', 
                modified_date='{7}',
                modified_computer='{8}'
                where period_id = {9}",
                CODE,
                PERIOD_STATUS.ToString(),
                YEAR.ID,
                START_DATE.ToString(DATE_FORMAT),
                END_DATA.ToString(DATE_FORMAT),
                CLOSED_DATE.ToString(DATE_FORMAT), 
                MODIFIED_BY, 
                DateTime.Now.ToString(Utils.DATE_FORMAT), 
                MODIFIED_COMPUTER_NAME, 
                ID);
        }
        public string GetByIDSQL(int ID)
        {
            return String.Format("select * from table_period where period_id = {0}", ID);
        }
        public string GetByCodeSQL(string code)
        {
            return String.Format("select * from table_period where period_code = '{0}'", code);
        }
        public string GetByCodeLikeSQL(string text)
        {
            return String.Format("select * from table_period where period_code like '%{0}%'", text);
        }
        public string GetByNameLikeSQL(string text)
        {
            return String.Format("select * from table_period where period_name like '%{0}%'", text);
        }
        public string GetAllSQL()
        {
            return String.Format("select * from table_period");
        }
        public static string GetAllSQLStatic()
        {
            return String.Format("select * from table_period");
        }
        public string GetByPeriodByYearID(int ID)
        {
            return String.Format("select * from table_period where year_id = {0}", ID);
        }
        public string GetConcatSearch(string find)
        {
            return "";
        }
        public static IList TransformReaderList(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            IList result = new ArrayList();
            while (aReader.Read())
            {
                Period period = new Period();
                period.ID = Convert.ToInt32(aReader["period_id"]);
                period.CODE = aReader["period_code"].ToString();
                period.PERIOD_STATUS = (PeriodStatus)Enum.Parse(typeof(PeriodStatus), aReader["period_status"].ToString());
                period.YEAR = new Year(Convert.ToInt32(aReader["year_id"]));
                period.START_DATE = Convert.ToDateTime(aReader["period_start"]);
                period.END_DATA = Convert.ToDateTime(aReader["period_end"]);
                period.CLOSED_DATE = Convert.ToDateTime(aReader["period_close"]);
                period.MODIFIED_BY = aReader["modified_by"].ToString();
                period.MODIFIED_DATE = Convert.ToDateTime(aReader["modified_date"].ToString());
                period.MODIFIED_COMPUTER_NAME = aReader["modified_computer"].ToString();
                result.Add(period);
            }
            return result;
        }
        public IList GetAll(MySql.Data.MySqlClient.MySqlDataReader aReader, IEntity year)
        {
            IList result = new ArrayList();
            while (aReader.Read())
            {
                Period period = new Period();
                period.ID = Convert.ToInt32(aReader["period_id"]);
                period.CODE = aReader["period_code"].ToString();
                period.PERIOD_STATUS = (PeriodStatus)Enum.Parse(typeof(PeriodStatus), aReader["period_status"].ToString());
                period.YEAR = new Year(Convert.ToInt32(aReader["year_id"]));
                period.START_DATE = Convert.ToDateTime(aReader["period_start"]);
                period.END_DATA = Convert.ToDateTime(aReader["period_end"]);
                period.CLOSED_DATE = Convert.ToDateTime(aReader["period_close"]);
                period.MODIFIED_BY = aReader["modified_by"].ToString();
                period.MODIFIED_DATE = Convert.ToDateTime(aReader["modified_date"].ToString());
                period.MODIFIED_COMPUTER_NAME = aReader["modified_computer"].ToString();
                result.Add(period);
            }
            return result;
        }
        public int GetID()
        {
            return ID;
        }
        public void SetID(int id)
        {
            ID = id;
        }
        public override string ToString()
        {
            return CODE;
        }
        public string GetCode()
        {
            return CODE;
        }
        public void SetCode(string code)
        {
            CODE = code;
        }
        public string GetMaximumIDSQL()
        {
            return String.Format("SELECT max(period_id) from table_period");
        }
        public bool IsInRange(DateTime date)
        {
            bool a = date >= START_DATE;
            bool b = date <= END_DATA;
            return a && b;
        }
        public override bool Equals(object obj)
        {
            IEntity ob = (IEntity)obj;
            if (ob == null) return false;
            return this.GetID() == ob.GetID();
        }

        #region IEntity Members


        public IEntity Get(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            Period period = null;
            while (aReader.Read())
            {
                period = new Period();
                period.ID = Convert.ToInt32(aReader[0]);
                period.CODE = aReader[1].ToString();
                period.PERIOD_STATUS = (PeriodStatus)Enum.Parse(typeof(PeriodStatus), aReader[2].ToString());
                period.YEAR = new Year(Convert.ToInt32(aReader[3]));
                period.START_DATE = Convert.ToDateTime(aReader[4]);
                period.END_DATA = Convert.ToDateTime(aReader[5]);
                period.CLOSED_DATE = Convert.ToDateTime(aReader[6]);
            }
            return period;
        }

        public IList GetAll(MySql.Data.MySqlClient.MySqlDataReader aReader)
        {
            IList result = new ArrayList();
            while (aReader.Read())
            {
                Period period = new Period();
                period.ID = Convert.ToInt32(aReader[0]);
                period.CODE = aReader[1].ToString();
                period.PERIOD_STATUS = (PeriodStatus)Enum.Parse(typeof(PeriodStatus), aReader[2].ToString());
                period.YEAR = new Year(Convert.ToInt32(aReader[3]));
                period.START_DATE = Convert.ToDateTime(aReader[4]);
                period.END_DATA = Convert.ToDateTime(aReader[5]);
                period.CLOSED_DATE = Convert.ToDateTime(aReader[6]);
                result.Add(period);
            }
            return result;
        }

        #endregion
    }
}
