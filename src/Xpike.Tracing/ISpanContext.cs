namespace Xpike.Tracing
{
    public interface ISpanContext
    {
        TraceParent TraceParent { get; }
        TraceId TraceId { get; }
        SpanId SpanId { get; }
        SpanId ParentSpanId { get; }
        bool IsValid { get; }
        bool IsRemote { get; }
        TraceState TraceState { get; }
    }
}