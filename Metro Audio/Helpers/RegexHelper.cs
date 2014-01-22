using Experia.Framework.Debug;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Experia.Framework
{
    /// <summary>
    /// Responsible for assisting in Regular Expressions in the .NET Environment
    /// </summary>
    public static class RegexHelper
    {
        private static Logger m_Logger = Logger.CreateLoggerFromClass();

        public static String GetMatchedValue(String regexPattern, String text)
        {
            Match match = Regex.Match(text, regexPattern);

            if (match.Success)
            {
                m_Logger.Info("Running GetMatchedValue - Returned {0}", match.Groups[1].Value);
                return match.Groups[1].Value;
            }

            return String.Empty;
        }
    }
}
