using Accountool.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace Accountool.Utils
{
    public static class Constants
    {
        public static DateTime FirstDayCurrentMonth => new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        public static DateTime LastDayCurrentMonth => new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1);
        public static int FirstMonth => 1;
        public static int LastMonth => 12;
        public static int PageSize => 300;
    }
}
