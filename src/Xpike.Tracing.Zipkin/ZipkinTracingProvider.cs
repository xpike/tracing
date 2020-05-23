using XPike.Logging;
using zipkin4net;
using zipkin4net.Tracers.Zipkin;

namespace Xpike.Tracing.Zipkin
{
    /// <summary>
    /// This class should be a singleton.
    /// </summary>
    public class ZipkinTracingProvider
    {
        private ILog<ZipkinTracingProvider> logger;
        private ITraceContextAccessor xpikeTraceContextAccessor;
        
        public ZipkinTracingProvider(ILog<ZipkinTracingProvider> logger, IZipkinSender zipkinSender, ITraceContextAccessor xpikeTraceContextAccessor)
        {
            this.logger = logger;
            this.xpikeTraceContextAccessor = xpikeTraceContextAccessor;
            
            //IZipkinSender httpSender = new HttpZipkinSender("http://localhost:9411", "application/json");
            var tracer = new ZipkinTracer(zipkinSender, new JSONSpanSerializer());
            
            TraceManager.RegisterTracer(tracer);
        }

        public void Start()
        {
            ILogger zipkinLogger = new XpikeTracingLogger(logger);
            TraceManager.Start(zipkinLogger);
        }

        public void Stop()
        {
            TraceManager.Stop();
        }

        public ITracer GetTracer()
        {
            return new ZipkinXpikeTracer("zipkin", xpikeTraceContextAccessor);
        }
    }
}