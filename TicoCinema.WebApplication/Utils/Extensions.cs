using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TicoCinema.WebApplication.Utils
{
    public static class Extensions
    {
        public static bool IsInRange(this DateTime dateToCheck, DateTime startDate, DateTime endDate)
        {
            return dateToCheck >= startDate && dateToCheck < endDate;
        }

        public static bool IsInSameWeek(this DateTime dateToCheck, DateTime date)
        {
            return (dateToCheck.Date - date).TotalDays < 7;
        }

        public static int GetYearsBetweenDateAndNow(this DateTime date)
        {
            DateTime today = DateTime.Today;
            int age = today.Year - date.Year;
            if (date > today.AddYears(-age))
                age--;
            return age;
        }
    }
}