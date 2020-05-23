using System;

namespace Xpike.Tracing
{
    /// <summary>
    /// Implementation of the trace-context as defined by https://www.w3.org/TR/trace-context.
    /// </summary>
    public sealed class TraceParent : IEquatable<TraceParent>
    {
        public static readonly string HEADER_NAME = "traceparent";
        
        public static readonly string SUPPORTED_VERSION = "00";
        
        private string version;
        private TraceId traceId;
        private SpanId spanId;
        private TraceFlags traceFlags;

        private TraceParent()
        {
        }

        public string Version => version;
        public TraceId TraceId => traceId;
        public SpanId SpanId => spanId;
        public TraceFlags TraceFlags => traceFlags;
        public bool IsSampled => traceFlags.IsSampled;
        
        public static TraceParent NewTraceParent(bool isSampled = false)
        {
            TraceParent traceParent = new TraceParent
            {
                version = SUPPORTED_VERSION,
                traceId = TraceId.NewId(),
                spanId = SpanId.NewId(),
                traceFlags = TraceFlags.NewFlags()
            };

            traceParent.TraceFlags.IsSampled = isSampled;
            
            return traceParent;
        }

        public static TraceParent NewForTrace(string traceParentAsString)
        {
            return NewForTrace(Parse(traceParentAsString));
        }

        public static TraceParent NewForTrace(TraceParent traceParent)
        {
            TraceParent newTraceParent = new TraceParent
            {
                traceId = traceParent.TraceId,
                spanId = SpanId.NewId(),
                traceFlags = traceParent.traceFlags
            };
            
            return newTraceParent;
        }
        
        public static TraceParent Parse(string traceParentAsString)
        {
            if (traceParentAsString.Length != 55)
                throw new FormatException($"Invalid traceparent value. Version {SUPPORTED_VERSION} header length should be 55 characters, but received a value with a length of {traceParentAsString.Length}.");
            
            string[] parts = traceParentAsString.Split('-');
            
            if (parts.Length != 4)
                            throw new FormatException($"Invalid traceparent value. Version {SUPPORTED_VERSION} expects 4 parts [version-traceId-parentId-traceFlags], but value provided only contains {parts.Length} parts.");
                        
            if (parts[0] != SUPPORTED_VERSION)
                throw new FormatException($"Invalid traceparent value. Expecting version {SUPPORTED_VERSION}, but received version {parts[0]}.");

            TraceParent traceParent = new TraceParent
            {
                version = SUPPORTED_VERSION,
                traceId = TraceId.Parse(parts[1]),
                spanId = SpanId.Parse(parts[2]),
                traceFlags = TraceFlags.Parse(parts[3])
            };

            return traceParent;
        }
      
        public override string ToString()
        {
            return $"{version}-{traceId}-{spanId}-{traceFlags}";
        }
        
        public bool Equals(TraceParent other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return ToString() == other.ToString();
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || obj is TraceParent other && Equals(other);
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public static bool operator ==(TraceParent left, TraceParent right)
        {
            if (object.ReferenceEquals(left, null))
            {
                if (object.ReferenceEquals(right, null))
                {
                    return true; // null == null
                }

                return false;
            }
            return left.Equals(right);
        }

        public static bool operator !=(TraceParent left, TraceParent right)
        {
            return !(left == right);
        }
    }
}