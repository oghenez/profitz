using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;

namespace Profit.Server
{
    public class AutoNumberSetupRepository
    {
        public static string GetCodeSampleByDomainName(MySql.Data.MySqlClient.MySqlCommand cmd, string p)
        {
            AutoNumberSetup ans = GetAutoNumberSetup(cmd, p);
            if (ans == null) throw new Exception("AutoNumber Setup (" + p + ") not set");
            string code = ans.PREFIX;
            code = code.Replace("#", getUnderScore(ans.DIGIT));
            return getDateCodeUnderScore(code);
        }

        internal static AutoNumberSetup GetAutoNumberSetup(MySql.Data.MySqlClient.MySqlCommand cmd,string domainName)
        {
            cmd.CommandText = AutoNumberSetup.FindByDomainName(domainName);
            MySql.Data.MySqlClient.MySqlDataReader r = cmd.ExecuteReader();
            AutoNumberSetup res = AutoNumberSetup.GetTransform(r);
            r.Close();
            if (res == null) throw new Exception("AutoNumber Setup (" + domainName + ") not set");
            return res;
        }
        internal static string getUnderScore(int count)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < count; i++)
            {
                sb.Append("_");
            }
            return sb.ToString();
        }
        internal static string getDateCodeUnderScore(string prefix)
        {
            if (prefix.Contains("MMMM"))
            {
                prefix = prefix.Replace("MMMM", "____");
            }
            if (prefix.Contains("MMM"))
            {
                prefix = prefix.Replace("MMM", "___");
            }
            if (prefix.Contains("MM"))
            {
                prefix = prefix.Replace("MM", "__");
            }
            if (prefix.Contains("yyyy"))
            {
                prefix = prefix.Replace("yyyy", "____");
            }
            if (prefix.Contains("yyy"))
            {
                prefix = prefix.Replace("yyy", "___");
            }
            if (prefix.Contains("yy"))
            {
                prefix = prefix.Replace("yy", "__");
            }
            return prefix;
        }
        public static string GetAutoNumberByDomainName(MySql.Data.MySqlClient.MySqlCommand cmd, string domainName, string manualCode, string lastCode, DateTime lastTransactionDate,
            DateTime transactionDate, bool isFirstTransaction)
        {
            AutoNumberSetup ans = GetAutoNumberSetup(cmd, domainName);
            if (ans == null) return manualCode;
            int number = 0;
            string prefix = ans.PREFIX;
            string seqNoBefore = ans.PREFIX;
            string seqNo = string.Empty;
            bool istr = ans.IS_TRANSACTION;
            if (istr)
            {
                if (ans.AUTONUMBER_SETUP_TYPE.Equals(AutoNumberSetupType.Manual)) return manualCode;
                if (ans.INITIAL_AUTO_NUMBER.Equals(InitialAutoNumberSetup.None)) return manualCode;

                if (isFirstTransaction)
                {
                    number = ans.START;
                }
                else
                {
                    if (lastCode.Trim().Equals(string.Empty))
                    {
                        number = ans.START;
                    }
                    if (!lastCode.Trim().Equals(string.Empty))
                    {
                        try
                        {
                            number = seqNoBefore.IndexOf("#");
                            if (number == 0)
                            {
                                lastCode = lastCode.Remove(ans.DIGIT - 1, ans.PREFIX.Length);
                            }
                            if (number == ans.PREFIX.Length - 1)
                            {
                                lastCode = lastCode.Remove(0, number);
                            }
                            if (number > 0)
                            {
                                lastCode = lastCode.Remove(0, number);
                                lastCode = lastCode.Remove(ans.DIGIT, lastCode.Length - ans.DIGIT);
                            }

                            number = Convert.ToInt32(lastCode);
                            if (number < 0)
                                throw new Exception("minus autonumber");
                        }
                        catch (Exception x)
                        {
                            number = ans.START;
                        }

                    }
                    switch (ans.INITIAL_AUTO_NUMBER)
                    {
                        case InitialAutoNumberSetup.Monthly:
                            if (transactionDate.Month > lastTransactionDate.Month)
                            {
                                number = 1;
                            }
                            else
                            {
                                if (isFirstTransaction || !lastCode.Trim().Equals(string.Empty))
                                    number++;
                            }
                            break;
                        case InitialAutoNumberSetup.Yearly:
                            if (transactionDate.Year > lastTransactionDate.Year)
                            {
                                number = 1;
                            }
                            else
                            {
                                if (!isFirstTransaction || !lastCode.Trim().Equals(string.Empty))
                                    number++;
                            }
                            break;
                    }
                }
                seqNo = number.ToString().PadLeft(ans.DIGIT, '0');
                prefix = prefix.Replace("#", seqNo);
                prefix = getDateCode(prefix, transactionDate);
            }
            return prefix;
        }
        private static string getDateCode(string prefix, DateTime transactionDate)
        {
            if (prefix.Contains("MMMM"))
            {
                prefix = prefix.Replace("MMMM", transactionDate.ToString("MMMM"));
            }
            if (prefix.Contains("MMM"))
            {
                prefix = prefix.Replace("MMM", transactionDate.ToString("MMM"));
            }
            if (prefix.Contains("MM"))
            {
                prefix = prefix.Replace("MM", transactionDate.ToString("MM"));
            }
            if (prefix.Contains("yyyy"))
            {
                prefix = prefix.Replace("yyyy", transactionDate.ToString("yyyy"));
            }
            if (prefix.Contains("yyy"))
            {
                prefix = prefix.Replace("yyy", transactionDate.ToString("yyy"));
            }
            if (prefix.Contains("yy"))
            {
                prefix = prefix.Replace("yy", transactionDate.ToString("yy"));
            }
            return prefix;
        }
    }
}
