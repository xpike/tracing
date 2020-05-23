namespace Xpike.Tracing
{
    public interface ITracingProvider
    {
        ITracer GetTracer();
    }
}