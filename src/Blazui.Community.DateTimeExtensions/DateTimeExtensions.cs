using System;

namespace Blazui.Community.DateTimeExtensions
{
    public static class DateTimeExtensions
    {
        public enum DateDiffType
        {
            //Year,
            //Month,
            Day,

            Hour,
            Minute,
            Second
        }

        public static string ConvertToDateDiffStr(this DateTime time)
        {
            var day = time.DateDayDiff();
            string timeString;
            if (day > 0)
            {
                if (day > 30)
                {
                    if (day > 365)
                    {
                        if(day>365*3)
                        {
                            timeString = $"久远前";
                        }else
                            timeString = $"{Convert.ToInt32(day / 365)}年前";
                    }
                    else
                    {
                        timeString = $"{day % 30}个月前";
                    }
                }
                else
                {
                    timeString = $"{day}天前";
                }
            }
            else
            {
                var hours = time.DateHourDiff();
                if (hours > 0)
                {
                    timeString = $"{hours}小时前";
                }
                else
                {
                    var minutes = time.DateMinuteDiff();
                    if (minutes > 0)
                        timeString = $"{minutes}分钟前";
                    else
                    {
                        timeString = $"刚刚";
                    }
                }
            }
            return timeString;
        }

        public static int DateDayDiff(this DateTime dateStart)
        {
            return DateDiff(dateStart, DateTime.Now, DateDiffType.Day);
        }

        public static int DateHourDiff(this DateTime dateStart)
        {
            return DateDiff(dateStart, DateTime.Now, DateDiffType.Hour);
        }

        public static int DateMinuteDiff(this DateTime dateStart)
        {
            return DateDiff(dateStart, DateTime.Now, DateDiffType.Minute);
        }

        public static int DateSecondDiff(this DateTime dateStart)
        {
            return DateDiff(dateStart, DateTime.Now, DateDiffType.Second);
        }

        public static int DateDayDiff(this DateTime dateStart, DateTime dateEnd)
        {
            return DateDiff(dateStart, dateEnd, DateDiffType.Day);
        }

        public static int DateHourDiff(this DateTime dateStart, DateTime dateEnd)
        {
            return DateDiff(dateStart, dateEnd, DateDiffType.Hour);
        }

        public static int DateMinuteDiff(this DateTime dateStart, DateTime dateEnd)
        {
            return DateDiff(dateStart, dateEnd, DateDiffType.Minute);
        }

        public static int DateSecondDiff(this DateTime dateStart, DateTime dateEnd)
        {
            return DateDiff(dateStart, dateEnd, DateDiffType.Second);
        }

        /// <summary>
        /// 时间差
        /// </summary>
        /// <param name="dateStart"></param>
        /// <param name="dateEnd"></param>
        /// <param name="type">0：天数，1：月数，2：年数，</param>
        /// <returns></returns>
        private static int DateDiff(this DateTime dateStart, DateTime dateEnd, DateDiffType dateDiffType = DateDiffType.Day)
        {
            TimeSpan sp = dateEnd.Subtract(dateStart);
            return dateDiffType switch
            {
                //DateDiffType.Year => sp.Minutes,
                //DateDiffType.Month => sp.Minutes,
                DateDiffType.Day => sp.Days,
                DateDiffType.Hour => sp.Hours,
                DateDiffType.Minute => sp.Minutes,
                DateDiffType.Second => sp.Seconds,
                _ => sp.Days,
            };
        }

        public static DateTimeOffset ToDateTimeOffset(this DateTime dateTime)
        {
            return dateTime.ToUniversalTime() <= DateTimeOffset.MinValue.UtcDateTime
                       ? DateTimeOffset.MinValue
                       : new DateTimeOffset(dateTime);
        }

        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        public static string GetTimeStamp(this DateTime time, int length = 13)
        {
            long ts = ConvertDateTimeToInt(time);
            return length == 10 ? ts.ToString().Substring(0, 10) : ts.ToString();
        }

        /// <summary>
        /// 将c# DateTime时间格式转换为Unix时间戳格式
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns>long</returns>
        public static long ConvertDateTimeToInt(this DateTime time)
        {
            DateTime startTime = TimeZoneInfo.ConvertTimeFromUtc(new DateTime(1970, 1, 1), TimeZoneInfo.Local);
            long t = (time.Ticks - startTime.Ticks) / 10000;   //除10000调整为13位
            return t;
        }

        /// <summary>
        /// 时间戳转为C#格式时间
        /// </summary>
        /// <param name=”timeStamp”></param>
        /// <returns></returns>
        public static DateTime ConvertStringToDateTime(string timeStamp)
        {
            if (timeStamp is null)
            {
                throw new ArgumentNullException(nameof(timeStamp));
            }
            if (timeStamp.Length < 10)
                throw new InvalidTimeZoneException(nameof(timeStamp));
            timeStamp = timeStamp.Substring(0, 10);
            DateTime dtStart = TimeZoneInfo.ConvertTimeFromUtc(new DateTime(1970, 1, 1), TimeZoneInfo.Local);
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }

        /// <summary>
        /// 时间戳转为C#格式时间10位
        /// </summary>
        /// <param name="timeStamp">Unix时间戳格式</param>
        /// <returns>C#格式时间</returns>
        public static DateTime GetDateTimeFrom1970Ticks(long curSeconds)
        {
            if (curSeconds.ToString().Length > 10)
                curSeconds = long.Parse(curSeconds.ToString().Substring(0, 10));
            DateTime dtStart = TimeZoneInfo.ConvertTimeFromUtc(new DateTime(1970, 1, 1), TimeZoneInfo.Local);
            return dtStart.AddSeconds(curSeconds);
        }
    }
}