using System;

namespace FundTracker.Web.Controllers.ParameterParsers
{
    public interface IParseDates
    {
        DateTime ParseDateOrUseToday(string date);
    }
}