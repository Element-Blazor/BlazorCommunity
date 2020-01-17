using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlazUICommunity.Utility.Logger
{
    public class EFLogger : ILogger
    {
        public IDisposable BeginScope<TState>(TState state) => null;


        public bool IsEnabled(LogLevel logLevel) => true;

        private readonly string categoryName;

        public EFLogger(string categoryName) => this.categoryName = categoryName;
        public void Log<TState>(LogLevel logLevel , EventId eventId , TState state , Exception exception , Func<TState , Exception , string> formatter)
        {
            //ef core执行数据库查询时的categoryName为Microsoft.EntityFrameworkCore.Database.Command,日志级别为Information
            if ( categoryName == "Microsoft.EntityFrameworkCore.Database.Command"
                    && logLevel == LogLevel.Information )
            {
                var logContent = formatter(state , exception);
                //TODO: 拿到日志内容想怎么玩就怎么玩吧
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(logContent);
                Console.ResetColor();
            }
        }
    }
}
