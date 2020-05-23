using System;

namespace Xpike.Tracing
{
    public interface ISpan
    {
        string Name { get; }
        ISpanContext SpanContext { get; }
        ISpanContext Parent { get; }
        DateTime StartTime { get; }
        DateTime? EndTime { get; }
        void End();
    }
}