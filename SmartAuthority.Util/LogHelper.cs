/*技术支持QQ群：226090960*/
using System;
using log4net;
using System.Reflection;

namespace SmartAuthority.Util
{
    public class LogHelper
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static void Error(string message)
        {
            log.Error(message);
        }

        public static void Error(string message, params object[] args)
        {
            log.Error(string.Format(message, args));
        }

        public static void Exception(Exception ex)
        {
            log.Error(ex);
        }

        public static void Exception(Exception ex, string message, params object[] args)
        {
            log.Error(string.Format(message, args), ex);
        }
    }
}
