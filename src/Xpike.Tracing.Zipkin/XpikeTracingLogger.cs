using XPike.Logging;

namespace Xpike.Tracing.Zipkin
{
    /// <summary>
    /// Implementation of Zipkin ILogger that writes the xPike logging system.
    /// </summary>
    internal class XpikeTracingLogger : zipkin4net.ILogger
    {
        private ILog<ZipkinTracingProvider> logger;

        internal XpikeTracingLogger(ILog<ZipkinTracingProvider> logger)
        {
            this.logger = logger;
        }

        public void LogInformation(string message)
        {
            logger.Info(message);
        }

        public void LogWarning(string message)
        {
            logger.Warn(message);
        }

        public void LogError(string message)
        {
            logger.Error(message);
        }
    }
}