using System;
using System.Globalization;

namespace FundTracker.Web.Controllers.ParameterParsers
{
    public class DateParser : IParseDates
    {
        public DateTime ParseDateOrUseToday(string date)
        {
            DateTime selectedDate;
            var couldParse = DateTime.TryParseExact(date, "yyyy-MM-dd", new DateTimeFormatInfo(), DateTimeStyles.None,
                out selectedDate);
            if (!couldParse)
            {
                selectedDate = DateTime.Today;
            }
            return selectedDate;
        }
    }
}