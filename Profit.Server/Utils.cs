using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Specialized;

namespace Profit.Server
{
    public class Utils
    {
        //public const string DATE_FORMAT = "yyyy/MM/dd";
        public const string DATE_FORMAT = "yyyy/MM/dd hh:mm:ss";
        public static void GetListCode(StringCollection list, IList dtos)
        {
            foreach (IEntity cod in dtos)
            {
                list.Add(cod.ToString());
            }
        }
        public static IEntity FindEntityInList(string value, IList list)
        {
            foreach (IEntity cod in list)
            {
                if (cod.ToString() == value)
                    return cod;
            }
            return null;
        }
        public static IEntity FindEntityInList(int value, IList list)
        {
            foreach (IEntity cod in list)
            {
                if (cod.GetID() == value)
                    return cod;
            }
            return null;
        }
        public static double CalculateTotalDiscount(double q, double p, double a, double b, double c, double d, double e, int rounding)
        {
            q = Math.Round(q, rounding);
            p = Math.Round(p, rounding);
            a = Math.Round(a, rounding);
            b = Math.Round(b, rounding);
            c = Math.Round(c, rounding);
            d = Math.Round(d, rounding);
            e = Math.Round(e, rounding);
            double n = Math.Round(1 * p, rounding);
            double d1 = Math.Round(n * a / 100, rounding);
            double d2 = Math.Round(b, rounding);
            double d3 = Math.Round(n * c / 100, rounding);
            double m1 = Math.Round(n - d3, rounding);
            double d4 = Math.Round(m1 * d / 100, rounding);
            double m2 = Math.Round(m1 - d4, rounding);
            double d5 = Math.Round(m2 * e / 100, rounding);
            return Convert.ToDouble(Math.Round(d1 + d2 + d3 + d4 + d5, rounding));
        }
        public static double CalculateSubTotal(double q, double p, double totalDisc, int rounding)
        {
            q = Math.Round(q, rounding);
            p = Math.Round(p, rounding);
            totalDisc = Math.Round(totalDisc, rounding);
            double subtotal = Math.Round((q * p) - (totalDisc * q), rounding);
            return  Convert.ToDouble(subtotal);
        }
        public static double CalculateSumList(IList listAmount, int rounding)
        {
            double result = 0;
            for (int i = 0; i < listAmount.Count; i++)
            {
                double a = Math.Round(Convert.ToDouble(listAmount[i]), rounding);
                result = Math.Round(result + a, rounding);
            }
            return Math.Round(result, rounding);
        }
        public static decimal CalculateDiscountPercent(decimal subTotal, decimal discPercent, int rounding)
        {
            subTotal = Math.Round(subTotal, rounding);
            discPercent = Math.Round(discPercent, rounding);
            return Math.Round(subTotal * discPercent / 100, rounding);
        }
        public static decimal CalculateNetTotal(decimal subTotalAmount, decimal discPercentAmount, decimal discAmount, decimal taxAmount, decimal expense, int rounding)
        {
            subTotalAmount = Math.Round(subTotalAmount, rounding);
            discPercentAmount = Math.Round(discPercentAmount, rounding);
            discAmount = Math.Round(discAmount, rounding);
            taxAmount = Math.Round(taxAmount, rounding);
            expense = Math.Round(expense, rounding);
            return Math.Round(subTotalAmount - discPercentAmount - discAmount + taxAmount + expense, rounding);
        }
        public static decimal CalculateNetTotalWithoutTaxExpense(decimal subTotalAmount, decimal discPercentAmount, decimal discAmount, int rounding)
        {
            subTotalAmount = Math.Round(subTotalAmount, rounding);
            discPercentAmount = Math.Round(discPercentAmount, rounding);
            discAmount = Math.Round(discAmount, rounding);
            return Math.Round(subTotalAmount - discPercentAmount - discAmount, rounding);
        }
        public static decimal CalculateNetTotalTax(decimal netafterdisc, decimal taxPercent, int rounding)
        {
            netafterdisc = Math.Round(netafterdisc, rounding);
            taxPercent = Math.Round(taxPercent, rounding);
            return Math.Round(netafterdisc * taxPercent / 100, rounding);
        }
    }
}
