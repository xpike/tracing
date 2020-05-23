using System;
using XPike.Logging;
using zipkin4net;

namespace Xpike.Tracing.Zipkin
{
    public class ZipkinXpikeTracer : ITracer
    {
        private ITraceContextAccessor xpikeTraceContextAccessor;
        
        public ZipkinXpikeTracer(string name, ITraceContextAccessor xpikeTraceContextAccessor)
        {
            this.Name = name;
            this.xpikeTraceContextAccessor = xpikeTraceContextAccessor;
        }
        
        public string Name { get; }
        
        public ISpan CreateSpan(string name)
        {
            if (CurrentSpan != null)
                return CreateSpan(name, CurrentSpan);

            return CurrentSpan = new ZipkinSpan(name);
        }

        public ISpan CreateSpan(string name, ISpan parentSpan)
        {
            return CurrentSpan = new ZipkinSpan(name, (ZipkinSpan)parentSpan);
        }

        public ISpan ActivateSpan(SpanId spanId)
        {
            throw new System.NotImplementedException();
        }

        public ISpan ActivateSpan(ISpan span)
        {
            throw new System.NotImplementedException();
        }

        public ISpan CurrentSpan { get; private set; }

        public void EndServerTrace(Span span)
        {
            span.End();
            trace.Record(Annotations.ServerSend(), span.EndTime ?? DateTime.UtcNow);
        }
        
        private void BeginServerTrace(Span span)
        {
            // Get the zipkin ids into to logs/xpike trace context...
            xpikeTraceContextAccessor.TraceContext.Set("zipkin-trace-id", trace.CurrentSpan.TraceId.ToString());
            xpikeTraceContextAccessor.TraceContext.Set("zipkin-span-id", trace.CurrentSpan.SpanId.ToString());
            xpikeTraceContextAccessor.TraceContext.Set("zipkin-parent-span-id", trace.CurrentSpan.ParentSpanId.ToString());
            xpikeTraceContextAccessor.TraceContext.Set("zipkin-correlation-id", trace.CorrelationId.ToString());

            // Put the vendor specific spanId into tracestate...
            span.SpanContext.TraceState.TryAdd("zipkin", trace.CurrentSpan.SpanId.ToString());
            
            // Annotate the zipkin trace
            trace.Record(Annotations.ServerRecv(), span.StartTime);
            trace.Record(Annotations.ServiceName(""));
            trace.Record(Annotations.Rpc(span.Name));
            trace.Record(Annotations.Tag("open-telemetry-trace-id", span.SpanContext.TraceId.Value));
            trace.Record(Annotations.Tag("open-telemetry-parent-id", span.Parent?.SpanId.Value ?? span.SpanContext.SpanId.Value));
            trace.Record(Annotations.Tag("open-telemetry-span-id", span.SpanContext.SpanId.Value));
        }
    }
}