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
    }
}