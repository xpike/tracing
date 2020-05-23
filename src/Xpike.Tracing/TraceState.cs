using System.Collections.Concurrent;
using System.Linq;

namespace Xpike.Tracing
{
    public sealed class TraceState : ConcurrentDictionary<string, string>
    {
        public override string ToString()
        {
            return string.Join(",", this.Select(kvp => $"{kvp.Key}={kvp.Value}"));
        }
    }
}