using APIAccessProDependencies.Helpers.DTOs.Global;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static APIAccessProDependencies.Helpers.Common.Utils;

namespace APIAccessProDependencies.Helpers.Logger
{
    public static class LogWriter
    {
        private static ILogger _Logger;

        public static ILogger Logger
        {
            set
            {
                _Logger = value;
            }
        }

        /// <summary>
        /// Write Log to the Configured Sink
        /// </summary>
        /// <param name="logs">List of Log Objects to write to Sink.</param>
        /// <returns>Initiated Token Details</returns>
        /// <response code="200">Returns the Initiated Token Details</response>
        public static void WriteLog(List<Log> logs)
        {
            foreach (var log in logs)
            {
                MainLogWriter(log.MessageLog, (LogType)log.LogType, log.ExceptionLog);
            }
        }

        /// <summary>
        /// Write Log to the Configured Sink
        /// </summary>
        /// <param name="messageLog">Message Log as Text</param>
        /// <param name="loMBype">Logging Type to Use</param>
        /// <param name="exceptionLog">Exception Object if any.</param>
        /// <returns>Initiated Token Details</returns>
        /// <response code="200">Returns the Initiated Token Details</response>
        public static void WriteLog(string messageLog, LogType logType, Exception exceptionLog = null)
        {
            MainLogWriter(messageLog, logType, exceptionLog);
        }

        private static void MainLogWriter(string messageLog, LogType loMBype, Exception exceptionLog = null)
        {
            try
            {
                switch (loMBype)
                {
                    case LogType.LOG_DEBUG:

                        _Logger.LogDebug(exceptionLog, messageLog);

                        break;
                    case LogType.LOG_INFORMATION:

                        _Logger.LogInformation(messageLog);

                        break;
                    case LogType.LOG_ERROR:

                        _Logger.LogError(messageLog);

                        break;
                    default:
                        //DO NOTHING
                        break;
                }
            }
            catch (Exception ex)
            {
                var eventLog = new EventLog();
                eventLog.Source = "APIAccessPro";
                //eventLog.WriteEntry("Error in: " + MyHttpContextAccessor.GetHttpContext()?.Request?.GetEncodedUrl());
                eventLog.WriteEntry(ex.Message, EventLogEntryType.Information);
            }
        }

        //[Obsolete]
        public static void AddLogAndClearLogBuilderOnException(ref StringBuilder logBuilder, LogType loMBype, ref List<Log> logs, Exception exception, string exceptionMessage = null)
        {
            logs.Add(new Log()
            {
                LogType = (int)loMBype,
                MessageLog = logBuilder.ToString()
            });
            logBuilder.Clear();

            logs.Add(new Log()
            {
                LogType = (int)LogType.LOG_DEBUG,
                MessageLog = exceptionMessage,
                ExceptionLog = exception
            });
        }
    }
}
