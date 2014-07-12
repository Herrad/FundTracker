using System;

namespace FundTracker.Web.Http
{
    public interface IWriteCookies
    {
        void SetCookie(string key, string value);
    }
}