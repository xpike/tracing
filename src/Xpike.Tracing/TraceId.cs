using System;
using System.Text.RegularExpressions;

namespace Xpike.Tracing
{
    /// <summary>
    /// Implementation of the trace-id defined in https://www.w3.org/TR/trace-context
    /// </summary>
    public sealed class TraceId : IEquatable<TraceId>
    {
        private readonly string _value;
        
        private TraceId(string value)
        {
            _value = value;
        }
        
        public string Value => _value;
        
        public bool IsValid
        {
            get { return !string.IsNullOrWhiteSpace(_value) && _value != "00000000000000000000000000000000"; }
        }
        
        public static TraceId NewId()
        {
            return Parse(Guid.NewGuid());
        }
        
        public static TraceId Parse(Guid fromGuid)
        {
            return new TraceId(fromGuid.ToString().Replace("-", "").ToLowerInvariant());
        }

        public static TraceId Parse(string fromString)
        {
            if (fromString.Length != 32)
                throw new FormatException($"Invalid value. A traceId must be 16 bytes. For example: e4fa528cbb3742c29531ed98a4022c39. Actual length of value provided is {fromString.Length}");
            
            if (fromString == "00000000000000000000000000000000") 
                throw new FormatException("Invalid value. A traceId cannot be all zero bytes. For example: e4fa528cbb3742c29531ed98a4022c39");

            if (Regex.IsMatch(fromString, "[^0-9a-f]"))
                throw new FormatException("Invalid value. A traceId must be 32 hex digits in lower case. For example: e4fa528cbb3742c29531ed98a4022c39.");
            
            return new TraceId(fromString);
        }
        
        public override string ToString()
        {
            return _value;
        }

        public bool Equals(TraceId other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return _value == other._value;
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || obj is TraceId other && Equals(other);
        }

        public override int GetHashCode()
        {
            return (_value != null ? _value.GetHashCode() : 0);
        }

        public static bool operator ==(TraceId left, TraceId right)
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

        public static bool operator !=(TraceId left, TraceId right)
        {
            return !(left == right);
        }
    }
}