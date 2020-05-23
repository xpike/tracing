using System;
using zipkin4net;

namespace Xpike.Tracing.Zipkin
{
    internal class ZipkinSpan : ISpan
    {
        private Trace trace;
        
        internal ZipkinSpan(string name)
        {
            this.Name = name;
            this.trace = Trace.Create();
            this.SpanContext = new SpanContext(trace.CurrentSpan);
        }

        internal ZipkinSpan(string name, ZipkinSpan parentSpan)
        {
            this.Name = name;
            this.trace = parentSpan.trace.Child();
            this.SpanContext = new SpanContext(trace.CurrentSpan);
            this.Parent = (ISpanContext)parentSpan;
        }
        
        public string Name { get; }
        public ISpanContext SpanContext { get; }
        public ISpanContext Parent { get; }
        public DateTime StartTime { get; } = DateTime.UtcNow;
        public DateTime? EndTime { get; private set; }
        
        public void End()
        {
            if (!EndTime.HasValue)
                EndTime = DateTime.UtcNow;
        }

        private void BeginSpan()
        {
            trace.Record(Annotations.ServerRecv(), this.StartTime);
            trace.Record(Annotations.ServiceName(""));
            trace.Record(Annotations.Rpc(this.Name));
        }
        
    }
}