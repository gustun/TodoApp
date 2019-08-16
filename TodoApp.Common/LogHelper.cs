using System;
using System.Text;
using TodoApp.Common.Interface;

namespace TodoApp.Common
{
    public class LoggerHelper : ILoggerHelper
    {

        public LoggerHelper()
        {
        }

        public void LogDebug(string message)
        {
        }

        public void LogError(string message)
        {
        }

        public void LogError(Exception e, string message = "")
        {
            LogError($"Message: {message}, Exception: {e.Message}, Stacktrace: {e.StackTrace}");
        }

        public void LogInfo(string message)
        {
        }

        public void LogWarn(string message)
        {
        }
    }
}
