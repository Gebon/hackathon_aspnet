using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace DepenedcyInjection.Infrastructure
{
    public class WeekProvider : IWeekProvider
    {
        public int GetWeek()
        {
            return new GregorianCalendar().GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
        }
    }

    public interface IWeekProvider
    {
        int GetWeek();
    }
}