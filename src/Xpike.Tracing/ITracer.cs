namespace Xpike.Tracing
{
    public interface ITracer
    {
        string Name { get; }

        ISpan CreateSpan(string name);

        ISpan CreateSpan(string name, Span parentSpan);

        ISpan CreateSpan(string name, SpanContext parentSpanContext);
        
        ISpan ActivateSpan(SpanId spanId);
        
        ISpan ActivateSpan(Span span);
        
        ISpan CurrentSpan { get; }
    }
}