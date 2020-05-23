using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Xpike.Tracing
{
    /// <summary>
    /// Implementation of the parent-id defined in https://www.w3.org/TR/trace-context
    /// </summary>
    public sealed class SpanId : IEquatable<SpanId>
    {
       
        private readonly string _value;

        private SpanId(string value)
        {
            _value = value;
        }
        
        public string Value => _value;
        
        public bool IsValid
        {
            get { return !string.IsNullOrWhiteSpace(_value) && _value != "0000000000000000"; }
        }
        
        public static SpanId NewId()
        {
            return Parse(Guid.NewGuid());
        }
        
        public static SpanId Parse(Guid fromGuid)
        {
            return new SpanId(fromGuid.ToString().Replace("-", "").ToLowerInvariant().Substring(15, 16));
        }

        public static SpanId Parse(string fromString)
        {
            if (fromString.Length != 16)
                throw new FormatException($"Invalid value. A span-id must be 8 bytes. For example: a9ded08a9a59720c. Actual length of value provided is {fromString.Length}");
            
            if (fromString == "0000000000000000") 
                throw new FormatException("Invalid value. A span-id cannot be all zero bytes. For example: a9ded08a9a59720c");

            if (Regex.IsMatch(fromString, "[^0-9a-f]"))
                throw new FormatException("Invalid value. A span-id must be 16 hex digits (8 bytes) in lower case. For example: a9ded08a9a59720c.");
            
            return new SpanId(fromString);
        }
        
        public override string ToString()
        {
            return _value;
        }

        public bool Equals(SpanId other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return _value == other._value;
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || obj is SpanId other && Equals(other);
        }

        public override int GetHashCode()
        {
            return (_value != null ? _value.GetHashCode() : 0);
        }

        public static bool operator ==(SpanId left, SpanId right)
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

        public static bool operator !=(SpanId left, SpanId right)
        {
            return !(left == right);
        }
    }
}