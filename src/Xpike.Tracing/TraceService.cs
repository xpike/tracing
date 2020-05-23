using System;
using XPike.Logging;
using XPike.RequestContext;

namespace Xpike.Tracing
{
    public class TraceService : ITraceService
    {
        private readonly ITraceContext traceContext;
        private readonly IRequestContext requestContext;
        private readonly ILogService logger;
        private ITracingProvider[] tracingProviders;
        private TraceParent callerTraceParentHeader;        
        private TraceParent myTraceParentHeader;
        
        public TraceService(ITraceContext traceContext, IRequestContext requestContext, ILogService logger, ITracingProvider[] tracingProviders)
        {
            this.traceContext = traceContext;
            this.requestContext = requestContext;
            this.logger = logger;
            this.tracingProviders = tracingProviders;

            ParseTraceParentHeader();
            myTraceParentHeader = TraceParent.NewForTrace(callerTraceParentHeader);
        }

        private void ParseTraceParentHeader()
        {
            if (requestContext.Headers.ContainsKey(TraceParent.HEADER_NAME))
            {
                try
                {
                    callerTraceParentHeader = TraceParent.Parse(requestContext.Headers[TraceParent.HEADER_NAME]);
                }
                catch (Exception ex)
                {
                    logger.Warn(
                        "Unable to parse provided traceparent header. Starting a new trace from here. See inner exception for more details.",
                        ex);
                    callerTraceParentHeader = TraceParent.NewTraceParent();
                }
            }
            else
            {
                callerTraceParentHeader = TraceParent.NewTraceParent();
            }            
        }

        private void AddTraceParentToTraceContext()
        {
            traceContext.Set(TraceContext.TRACE_ID_KEY, callerTraceParentHeader.TraceId.Value);
            traceContext.Set("parentSpanId", callerTraceParentHeader.SpanId.Value);
            traceContext.Set("spanId", myTraceParentHeader.SpanId.Value);
        }
    }
}