using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapsLibrary
{
    public static partial class SnapsEngine
    {
        public static int GetHourValue()
        {
            return DateTime.Now.Hour;
        }
        public static int GetMinuteValue()
        {
            return DateTime.Now.Minute;
        }
        public static int GetDayValue()
        {
            return DateTime.Now.Day;
        }
        public static int GetMonthValue()
        {
            return DateTime.Now.Month;
        }
        public static int GetYearValue()
        {
            return DateTime.Now.Year;
        }
        public static string GetDayOfWeekName()
        {
            return DateTime.Now.DayOfWeek.ToString();
        }
        public static int GetSecondsValue()
        {
            return DateTime.Now.Second;
        }
    }
}
