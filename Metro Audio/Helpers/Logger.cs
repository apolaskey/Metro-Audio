using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;
using NLogger = NLog.Logger;
using System.Diagnostics;

namespace Experia.Framework.Debug
{
    /// <summary>
    /// A logging facade that is currently wrapped around NLog (potentially more in the future).
    /// </summary>
    public class Logger
    {
        protected NLogger m_Logger;

        /// <summary>
        /// Determines whether the logger is enabled or not.
        /// [DO NOT call this if you want to block logging; the Logger will handle this internally]
        /// </summary>
        public static bool Enabled
        {
            get { return LogManager.IsLoggingEnabled(); }
            set
            {
                if (value == true)
                    LogManager.DisableLogging();
                else LogManager.EnableLogging();
            }
        }

        /// <summary>
        /// No-op constructor for hiding
        /// </summary>
        protected Logger() { }

        /// <summary>
        /// Create a logger based on the type of the class that calls this method.
        /// </summary>
        /// <returns>(Facade logger)</returns>
        public static Logger CreateLoggerFromClass()
        {
            Logger temp = new Logger();
            temp.m_Logger = LogManager.GetLogger(ReflectionHelper.GetCallingMethodType().FullName);
            return temp;
        }

        /// <summary>
        /// Create a logger for a given type.
        /// </summary>
        /// <param name="type">(Type) The type of class that we are making logs for</param>
        /// <returns>(Facade Logger)</returns>
        public static Logger CreateLoggerFromType(Type type)
        {
            Logger temp = new Logger();
            temp.m_Logger = LogManager.GetLogger(type.FullName);
            return temp;
        }

        /// <summary>
        /// Traditional Info logging; Mainly used for system specs and some generic info.
        /// </summary>
        /// <param name="message">(String)</param>
        public void Info(string message)
        {
            m_Logger.Log(NLog.LogLevel.Info, message);
        }
        public void Info(string message, params object[] args)
        {
            m_Logger.Log(NLog.LogLevel.Info, message, args);
        }

        /// <summary>
        /// Debug message; For production issues and tracking for pesky problems.
        /// </summary>
        /// <param name="message">(String)</param>
        public void Debug(string message)
        {
            m_Logger.Log(NLog.LogLevel.Debug, message);
        }
        public void Debug(string message, params object[] args)
        {
            m_Logger.Log(NLog.LogLevel.Debug, message, args);
        }

        /// <summary>
        /// Trace message; these shouldn't appear in production code but are for debugging purposes.
        /// </summary>
        /// <param name="message">(String)</param>
        public void Trace(string message)
        {
            m_Logger.Log(NLog.LogLevel.Trace, message);
        }
        public void Trace(string message, params object[] args)
        {
            m_Logger.Log(NLog.LogLevel.Trace, message, args);
        }

        /// <summary>
        /// Lower sev error message; most likely will not break the app but will cause game specific issues.
        /// </summary>
        /// <param name="message">(String)</param>
        public void Warn(string message)
        {
            m_Logger.Log(NLog.LogLevel.Warn, message);
        }

        public void Warn(string message, params object[] args)
        {
            m_Logger.Log(NLog.LogLevel.Warn, message, args);
        }

        /// <summary>
        /// Standard Error message; one that might cause stability problems.
        /// </summary>
        /// <param name="message">(String)</param>
        public void Error(string message)
        {
            m_Logger.Log(NLog.LogLevel.Error, message);
        }
        public void Error(string message, params object[] args)
        {
            m_Logger.Log(NLog.LogLevel.Error, message, args);
        }
        /// <summary>
        /// Fatal Error message; one that will most likely break the app.
        /// </summary>
        /// <param name="message">(String)</param>
        public void Fatal(string message)
        {
            m_Logger.Log(NLog.LogLevel.Fatal, message);
        }
        public void Fatal(string message, params object[] args)
        {
            m_Logger.Log(NLog.LogLevel.Fatal, message, args);
        }
        /**
         * EOC
         */
    }
}
