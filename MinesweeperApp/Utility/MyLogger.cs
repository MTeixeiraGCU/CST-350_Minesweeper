using Microsoft.Extensions.Logging;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinesweeperApp.Utility
{
    public class MyLogger : ILogger
    {
        private static MyLogger instance;
        private static Logger logger;

        /// <summary>
        /// Returns the instance of MyLogger if it exists, 
        /// otherwise creates a new instance of the MyLogger class
        /// </summary>
        /// <returns></returns>
        public static MyLogger GetInstance()
        {
            if (instance == null)
                instance = new MyLogger();
            return instance;
        }

        /// <summary>
        /// Returns the NLog logger rule
        /// </summary>
        /// <returns></returns>
        private Logger GetLogger()
        {
            if (MyLogger.logger == null)
                MyLogger.logger = LogManager.GetLogger("MinesweeperLoggerrule");
            return MyLogger.logger;
        }

        /// <summary>
        /// Provides Debug message
        /// </summary>
        /// <param name="message"></param>
        public void Debug(string message)
        {
            GetLogger().Debug(message);
        }

        /// <summary>
        /// Provides Error message
        /// </summary>
        /// <param name="message"></param>
        public void Error(string message)
        {
            GetLogger().Error(message);
        }

        /// <summary>
        /// Provides Info message
        /// </summary>
        /// <param name="message"></param>
        public void Info(string message)
        {
            GetLogger().Info(message);
        }

        /// <summary>
        /// Provides Warning message
        /// </summary>
        /// <param name="message"></param>
        public void Warning(string message)
        {
            GetLogger().Warn(message);
        }
    }
}
