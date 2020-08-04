using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace Util
{
    public enum PersianMonth
    {
        [Description("فروردین")] One = 01,
        [Description("اردیبهشت")] Two = 02,
        [Description("خرداد")] Three = 03,
        [Description("تیر")] Four = 04,
        [Description("مرداد")] Five = 05,
        [Description("شهریور")] Six = 06,
        [Description("مهر")] Seven = 07,
        [Description("آبان")] Eight = 08,
        [Description("آذر")] Nine = 09,
        [Description("دی")] Ten = 10,
        [Description("بهمن")] Eleven = 11,
        [Description("اسفند")] Twelve = 12
    }

    public static class DateHelperExtensions
    {
        private static readonly PersianCalendar pc = new PersianCalendar();

        public static string GetPersianMonthName(this DateTime date)
        {
            var pc = new System.Globalization.PersianCalendar();
            switch (pc.GetMonth(date))
            {
                case 1:
                    return "فروردین";
                case 2:
                    return "اردیبهشت";
                case 3:
                    return "خرداد";
                case 4:
                    return "تیر";
                case 5:
                    return "مرداد";
                case 6:
                    return "شهریور";
                case 7:
                    return "مهر";
                case 8:
                    return "آبان";
                case 9:
                    return "آذر";
                case 10:
                    return "دی";
                case 11:
                    return "بهمن";
                case 12:
                    return "اسفند";
                default:
                    return "";
            }
        }

        public static Dictionary<int, string> GetPersianMonthList()
        {
            var pc = new System.Globalization.PersianCalendar();
            var list = new Dictionary<int, string>()
            {
                {1, "فروردین"}, {2, "اردیبهشت"}, {3, "خرداد"}, {4, "تیر"}, {5, "مرداد"}, {6, "شهریور"}, {7, "مهر"},
                {8, "آبان"}, {9, "آذر"}, {10, "دی"}, {11, "بهمن"}, {12, "اسفند"}
            };
            return list;
        }

        public static Dictionary<int, string> GetPersianYearList()
        {
            var pc = new System.Globalization.PersianCalendar();
            var list = new Dictionary<int, string>()
            {
                {1397, "1397"},
                {1398, "1398"},
                {1399, "1399"},
                {1400, "1400"}

            };
            return list;
        }

        public static string GetPersianDayName(this DateTime date)
        {
            var pc = new System.Globalization.PersianCalendar();
            switch (pc.GetDayOfWeek(date))
            {
                case DayOfWeek.Saturday:
                    return "شنبه";
                case DayOfWeek.Sunday:
                    return "یکشنبه";
                case DayOfWeek.Monday:
                    return "دوشنبه";
                case DayOfWeek.Tuesday:
                    return "سه شنبه";
                case DayOfWeek.Wednesday:
                    return "چهارشنبه";
                case DayOfWeek.Thursday:
                    return "پنجشنبه";
                case DayOfWeek.Friday:
                    return "جمعه";
            }

            return "";

        }


        public static string GetPersianDate(this DateTime date)
        {
            // date = date.Add(new TimeSpan(3, 30, 0));
            //return (pc.GetDayOfMonth(date) + " " + date.GetPersianMonthName() + " " + pc.GetYear(date) + "، ساعت" + date.ToShortTimeString().Replace("PM", "ب.ظ").Replace("AM", "ق.ظ")).ToPersian();

            return (pc.GetDayOfMonth(date) + " " + date.GetPersianMonthName() + " " + pc.GetYear(date) + "، ساعت" +
                    date.ToShortTimeString().Replace("PM", "ب.ظ").Replace("AM", "ق.ظ")).ToPersian();
        }

        public static string GetShortPersianDate(this DateTime date)
        {
            try
            {
                return pc.GetDayOfMonth(date) + " " + date.GetPersianMonthName() + " " +
                       pc.GetYear(date).ToString().ToPersian();
            }
            catch
            {
                return "";
            }
        }

        public static string GetShortNumericPersianDate(this DateTime date)
        {
            return pc.GetYear(date) + "/" + pc.GetMonth(date) + "/" + pc.GetDayOfMonth(date);
        }

        public static string GetPersianShortDate(this DateTime date)
        {
            return date.GetPersianMonthName() + " " + pc.GetYear(date).ToString().ToPersian();
        }

        public static DateTime[] GetPersianMonthScope(this int month, int year)
        {


            var scope = new List<DateTime>();
            if (month > 0)
            {
                scope.Add(pc.ToDateTime(year, month, 1, 0, 0, 1, 0));
                scope.Add(pc.ToDateTime(year, month, (month <= 6 ? 31 : (month == 12 ? 29 : 30)), 23, 59, 59, 0));
            }
            else
            {
                scope.Add(pc.ToDateTime(year, 1, 1, 0, 0, 1, 0));
                scope.Add(pc.ToDateTime(year, 12, 29, 23, 59, 59, 0));
            }


            return scope.ToArray();
        }

        public static DateTime ConverToGregorianDate(this DateTime persianDate)
        {
            return pc.ToDateTime(persianDate.Year, persianDate.Month, persianDate.Day, persianDate.Hour,
                persianDate.Minute, persianDate.Second, 0);
        }

        public static DateTime? ConverToGregorianDate(this string persianDateStr, int hour = 0, int minute = 0,
            int second = 0)
        {
            try
            {
                var parts = persianDateStr.Split('/');
                return pc.ToDateTime(Convert.ToInt32(parts[2]), Convert.ToInt32(parts[1]), Convert.ToInt32(parts[0]),
                    hour, minute, second, 0);
            }
            catch
            {
            }

            return null;
        }

        public static DateTime ConverToGregorianDate(this string persianDateStr)
        {
            try
            {
                var parts = persianDateStr.Split('/');
                // return pc.ToDateTime(Convert.ToInt32(parts[2]), Convert.ToInt32(parts[1]), Convert.ToInt32(parts[0]), 0, 0, 0, 0);
                return pc.ToDateTime(Convert.ToInt32(parts[0]), Convert.ToInt32(parts[1]), Convert.ToInt32(parts[2]), 0,
                    0, 0, 0);
            }
            catch
            {
            }

            return DateTime.Now;
        }

        public static DateTime ConverToGregorianDate(this string persianDateStr, int hour, int minute)
        {
            try
            {
                var parts = persianDateStr.Split('/');
                // return pc.ToDateTime(Convert.ToInt32(parts[2]), Convert.ToInt32(parts[1]), Convert.ToInt32(parts[0]), 0, 0, 0, 0);
                return pc.ToDateTime(Convert.ToInt32(parts[0]), Convert.ToInt32(parts[1]), Convert.ToInt32(parts[2]),
                    hour, minute, 0, 0);
            }
            catch
            {
            }

            return DateTime.Now;
        }

        public static DateTime ConverToGregorianEndDate(this string persianDateStr)
        {
            try
            {
                var parts = persianDateStr.Split('/');
                return pc.ToDateTime(Convert.ToInt32(parts[0]), Convert.ToInt32(parts[1]), Convert.ToInt32(parts[2]),
                    23, 59, 59, 0);
            }
            catch
            {
            }

            return DateTime.Now;
        }

        public static DateTime ConverToGregorianDateWithDash(this string persianDateStr)
        {
            try
            {
                var parts = persianDateStr.Split('-');
                return pc.ToDateTime(Convert.ToInt32(parts[0]), Convert.ToInt32(parts[1]), Convert.ToInt32(parts[2]), 0,
                    0, 0, 0);
            }
            catch
            {
            }

            return DateTime.Now;
        }

        public static string GetPersianDateStr(this DateTime date)
        {
            try
            {
                // return pc.GetDayOfMonth(date) + "/" + pc.GetMonth(date) + "/" + pc.GetYear(date);
                if (date == DateTime.MinValue)
                {
                    date = DateTime.Now;
                }

                return pc.GetYear(date) + "/" + pc.GetMonth(date) + "/" + pc.GetDayOfMonth(date);
            }
            catch
            {
                return GetPersianDate(DateTime.Now);
            }
        }

        public static string GetPersianDateContinuousStr(DateTime date = default(DateTime))
        {
            try
            {
                // return pc.GetDayOfMonth(date) + "/" + pc.GetMonth(date) + "/" + pc.GetYear(date);
                if (date == DateTime.MinValue)
                {
                    date = DateTime.Now;
                }

                return pc.GetYear(date).ToString().Substring(2, 2) + "/" + pc.GetMonth(date) + "/" +
                       pc.GetDayOfMonth(date);
            }
            catch
            {
                return GetPersianDate(DateTime.Now);
            }
        }

        public static bool AreInSameDay(this DateTime firstDate, DateTime secondDate)
        {
            return firstDate.Year == secondDate.Year && firstDate.Month == secondDate.Month &&
                   firstDate.Day == secondDate.Day;
        }

        public static string GetTime(this DateTime date)
        {

            var time = date.ToString("HH:mm");
            return time;
        }

        public static int GetPersianYear(this DateTime date)
        {
            try
            {
                if (date == DateTime.MinValue)
                {
                    date = DateTime.Now;
                }

                return pc.GetYear(date);
            }
            catch
            {
                return pc.GetYear(DateTime.Now);
            }
        }

        public static int GetPersianMonth(this DateTime date)
        {
            try
            {
                if (date == DateTime.MinValue)
                {
                    date = DateTime.Now;
                }

                var dd = pc.GetMonth(date);
                return dd;
            }
            catch
            {
                return pc.GetMonth(DateTime.Now);
            }
        }

        public static string GetPersianMonth(this int month)
        {
            var pc = new System.Globalization.PersianCalendar();
            switch (month)
            {
                case 1:
                    return "فروردین";
                case 2:
                    return "اردیبهشت";
                case 3:
                    return "خرداد";
                case 4:
                    return "تیر";
                case 5:
                    return "مرداد";
                case 6:
                    return "شهریور";
                case 7:
                    return "مهر";
                case 8:
                    return "آبان";
                case 9:
                    return "آذر";
                case 10:
                    return "دی";
                case 11:
                    return "بهمن";
                case 12:
                    return "اسفند";
                default:
                    return "";
            }
        }

    }

    public static class DecimalExtensions
    {
        public static string ToStringDecimal(this int number)
        {



            if (number == default(int))
            {
                return "0";
            }

            return number.ToString("##,###");

        }

        public static string ToStringDecimal(this double number)
        {



            if (number == default(double))
            {
                return "0";
            }

            return number.ToString("##,###");

        }

        public static string ToStringDecimal(this decimal number)
        {



            if (number == default(decimal))
            {
                return "0";
            }

            return number.ToString("##,###");

        }

        public static string ToDecimal(this decimal number)
        {



            if (number == default(decimal))
            {
                return "0";
            }

            return number.ToString("##.00");

        }

        public static string ToStringDecimal(this decimal? number)
        {

            if (number.HasValue)
            {
                return ToStringDecimal(number.Value);
            }
            else
            {
                return "";
            }

        }

        public static decimal ToDecimalString(this string number)
        {
            if (string.IsNullOrEmpty(number) || number == "0")
            {
                return 0;
            }

            return decimal.Parse(number);
        }

        public static string ToDecimalStringWithoutComma(this string number)
        {

            return number.Replace(",", "", StringComparison.CurrentCulture);
        }

        public static string ToStringLong(this long number)
        {



            if (number == default(long))
            {
                return "0";
            }

            return number.ToString("##,###");

        }

        public static string Truncate(this float value)
        {

            var ssss = (Math.Truncate(100 * value) / 100).ToString();
            return ssss;
        }

        public static string Truncate(this decimal value)
        {

            var ssss = (Math.Truncate(100 * value) / 100).ToString();
            return ssss;
        }

        public static double Truncate1(this float value)
        {

            var ssss = (Math.Truncate(100 * value) / 100);
            return ssss;
        }

        public static decimal Truncate1(this decimal value)
        {

            var ssss = (Math.Truncate(100 * value) / 100);
            return ssss;
        }
    }


}
