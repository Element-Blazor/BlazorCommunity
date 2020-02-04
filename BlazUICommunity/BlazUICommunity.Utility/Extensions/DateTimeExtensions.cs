﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Blazui.Community.Utility
{
    public static class DateTimeExtensions
    {
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
        public static string GetTimeStamp(this DateTime time , int length = 13)
        {
            long ts = ConvertDateTimeToInt(time);
            return length == 10? ts.ToString().Substring(0 , 10): ts.ToString();
        }
        /// <summary>  
        /// 将c# DateTime时间格式转换为Unix时间戳格式  
        /// </summary>  
        /// <param name="time">时间</param>  
        /// <returns>long</returns>  
        public static long ConvertDateTimeToInt(this DateTime time)
        {
            DateTime startTime = TimeZoneInfo.ConvertTimeFromUtc(new DateTime(1970 , 1 , 1) , TimeZoneInfo.Local);
            long t = ( time.Ticks - startTime.Ticks ) / 10000;   //除10000调整为13位      
            return t;
        }
        /// <summary>        
        /// 时间戳转为C#格式时间        
        /// </summary>        
        /// <param name=”timeStamp”></param>        
        /// <returns></returns>        
        public static DateTime ConvertStringToDateTime(string timeStamp)
        {
            if ( timeStamp is null )
            {
                throw new ArgumentNullException(nameof(timeStamp));
            }
            if ( timeStamp.Length < 10 )
                throw new InvalidTimeZoneException(nameof(timeStamp));
            timeStamp = timeStamp.Substring(0 , 10);
            DateTime dtStart = TimeZoneInfo.ConvertTimeFromUtc(new DateTime(1970 , 1 , 1) , TimeZoneInfo.Local);
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
            if ( curSeconds.ToString().Length > 10 )
                curSeconds = long.Parse(curSeconds.ToString().Substring(0 , 10));
            DateTime dtStart = TimeZoneInfo.ConvertTimeFromUtc(new DateTime(1970 , 1 , 1) , TimeZoneInfo.Local);
            return dtStart.AddSeconds(curSeconds);
        }



    }
}
