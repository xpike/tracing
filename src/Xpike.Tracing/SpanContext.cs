using System.Collections.Concurrent;
using System.Diagnostics;

namespace Xpike.Tracing
{
    /// <summary>
    /// A SpanContext represents the portion of a Span which must be serialized and propagated along side of a
    /// distributed context. SpanContexts are immutable.
    /// https://github.com/open-telemetry/opentelemetry-specification/blob/master/specification/api-tracing.md#spancontext
    /// </summary>
    public sealed class SpanContext : ISpanContext
    {
        public SpanContext()
        {
            this.TraceParent = Tracing.TraceParent.NewTraceParent();
            this.SpanId = this.TraceParent.SpanId;
            this.IsRemote = false;

            Activity.DefaultIdFormat = ActivityIdFormat.W3C;
            Activity activity = new Activity("");
            
        }

        public SpanContext(SpanContext parentContext)
        {
            this.TraceParent = Tracing.TraceParent.NewForTrace(parentContext.TraceParent);
            this.SpanId = this.TraceParent.SpanId;
            this.IsRemote = parentContext.IsRemote;
        }
        
        public TraceParent TraceParent { get; }

        public TraceId TraceId => TraceParent.TraceId;

        public SpanId SpanId { get; }
        
        public SpanId ParentSpanId { get; }

        public bool IsValid => (TraceId.IsValid && SpanId.IsValid);

        public bool IsRemote { get; }
        
        public TraceState TraceState { get; } = new TraceState();
    }
}