using System;

namespace Xpike.Tracing
{
    public sealed class Span : ISpan
    {
        public Span(string name, ISpanContext mySpanContext)
        {
            StartTime = DateTime.UtcNow;
            SpanContext = mySpanContext;
            Name = name;
        }
        
        public Span(string name, SpanContext mySpanContext, SpanContext parentSpanContext)
        {
            StartTime = DateTime.UtcNow;
            SpanContext = mySpanContext;
            Parent = parentSpanContext;
            Name = name;
        }

        public Span(string name, Span parentSpan) : this(name, parentSpan.SpanContext)
        {
        }
        
        public string Name { get; }
        
        public ISpanContext SpanContext { get; }
        
        public ISpanContext Parent { get; }

        public DateTime StartTime { get; }

        public DateTime? EndTime { get; private set; }

        public void End()
        {
            if (!EndTime.HasValue)
                EndTime = DateTime.UtcNow;
        }
    }
}