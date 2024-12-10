namespace MinimalApiStartupProject.Infrastructures.StringExtensions
{
    public static class LoggerExtension
    {
        /// <summary>
        /// Log extension method to log exceptions
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="serviceName"></param>
        /// <param name="methodName"></param>
        /// <param name="requestContentJson"></param>
        /// <param name="exceptionMessage"></param>
        public static void LogException(this ILogger logger, string serviceName, string methodName, string requestContentJson, string exceptionMessage)
        {
            logger.LogError("{serviceName} - {methodName} - {requestContentJson}: {exceptionMessage}", serviceName, methodName, requestContentJson, exceptionMessage);

            return;
        }
    }
}