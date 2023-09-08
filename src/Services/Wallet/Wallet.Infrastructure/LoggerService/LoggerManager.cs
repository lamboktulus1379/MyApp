using NLog;
using System;

namespace Wallet.Infrastructure.LoggerService
{
    public class LoggerManager : ILoggerManager
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();
        public void LogDebug(string message)
        {
            Console.WriteLine("[DEBUG] {0}", message);
            logger.Debug(message);
        }

        public void LogError(string message)
        {
            Console.WriteLine("[ERROR] {0}", message);
            logger?.Error(message);
        }

        public void LogInfo(string message)
        {
            Console.WriteLine("[INFO] {0}", message);
            logger.Info(message);
        }

        public void LogWarn(string message)
        {
            Console.WriteLine("[WARN] {0}", message);
            logger.Warn(message);
        }
    }


}
