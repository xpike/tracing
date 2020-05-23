using System;
using System.Text.RegularExpressions;

namespace Xpike.Tracing
{
    /// <summary>
    /// Implementation of trace-flags defined in https://www.w3.org/TR/trace-context
    /// </summary>
    public sealed class TraceFlags : IEquatable<TraceFlags>
    {
        private char[] flags;

        private TraceFlags()
        {
        }

        public bool IsSampled
        {
            get => flags[7] == '1';
            set => flags[7] = value ? '1' : '0';
        }

        public static TraceFlags NewFlags()
        {
            return new TraceFlags
            {
                flags = new[] {'0','0','0','0','0','0','0','0'}
            };
        }
        
        public static TraceFlags Parse(string flags)
        {
            if (flags.Length != 2)
                throw new FormatException($"Invalid value. Trace flags must be 1 byte.");
            
            if (Regex.IsMatch(flags, "[^0-9a-f]"))
                throw new FormatException("Invalid value. A traceflags must be 2 hex digits in lower case. For example: 01.");

            return new TraceFlags
            {
                flags = Convert.ToString(byte.Parse(flags), toBase: 2).PadLeft(8, '0').ToCharArray()
            };
        }
        
        public override string ToString()
        {
            return Convert.ToString(byte.Parse(new string(flags)) ,toBase: 2).PadLeft(2, '0');
        }
        
        public bool Equals(TraceFlags other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return ToString() == other.ToString();
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || obj is TraceFlags other && Equals(other);
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public static bool operator ==(TraceFlags left, TraceFlags right)
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

        public static bool operator !=(TraceFlags left, TraceFlags right)
        {
            return !(left == right);
        }
    }
}