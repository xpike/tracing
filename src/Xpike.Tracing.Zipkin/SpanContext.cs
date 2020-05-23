using zipkin4net;
using zipkin4net.Utils;

namespace Xpike.Tracing.Zipkin
{
    internal class SpanContext : ISpanContext
    {
        internal SpanContext(ITraceContext zipkinTraceContext)
        {
            string hexTraceIdHigh =
                (TraceManager.Trace128Bits ? NumberUtils.EncodeLongToLowerHexString(zipkinTraceContext.TraceIdHigh) : "")
                .PadLeft(8,'0');
            string hexTraceIdLow = NumberUtils.EncodeLongToLowerHexString(zipkinTraceContext.TraceId).PadLeft(8, '0');
            string hexSpanId = NumberUtils.EncodeLongToLowerHexString(zipkinTraceContext.SpanId).PadLeft(8, '0');
            string hexFlags = zipkinTraceContext.Sampled.GetValueOrDefault() ? "01" : "00";
            Tracing.TraceParent.Parse($"{TraceParent.SUPPORTED_VERSION}-{hexTraceIdHigh}{hexTraceIdLow}-{hexSpanId}-{hexFlags}");
            this.ParentSpanId = SpanId.Parse(NumberUtils.EncodeLongToLowerHexString(zipkinTraceContext.ParentSpanId.GetValueOrDefault()).PadLeft(8, '0'));
            this.TraceState = new TraceState();
            this.TraceState.TryAdd("zipkin", hexSpanId);
        }
        
        public TraceParent TraceParent { get; }
        public TraceId TraceId
        {
            get => TraceParent.TraceId;
        }
        public SpanId SpanId
        {
            get => TraceParent.SpanId;
        }
        public SpanId ParentSpanId { get; }
        public bool IsValid
        {
            get => this.TraceId.IsValid && this.SpanId.IsValid;
        }
        public bool IsRemote { get; }
        public TraceState TraceState { get; }
    }
}