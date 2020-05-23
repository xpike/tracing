using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Xpike.Tracing.Tests
{
    [TestClass]
    public class TraceParentTests
    {
        [TestMethod]
        public void Parsing_from_a_ToString_result()
        {
            TraceParent expected = TraceParent.NewTraceParent();

            Console.WriteLine(expected.ToString());
            
            TraceParent parsed = TraceParent.Parse(expected.ToString());

            Console.WriteLine(parsed.ToString());
            
            Assert.AreEqual(expected, parsed);
            Assert.AreEqual("00-1f29ce6b7be546c6b312aef0ae672a6d-4ac1a44ed5960f94-00".Length, parsed.ToString().Length);
        }
        
        [TestMethod]
        public void IsSampledFlag()
        {
            TraceParent traceParent = TraceParent.NewTraceParent(isSampled: true);

            Console.WriteLine(traceParent.ToString());
            
            Assert.IsTrue(traceParent.IsSampled);
        }

        [TestMethod]
        public void foo()
        {
            Activity.DefaultIdFormat = ActivityIdFormat.W3C;
            Activity activity = new Activity("test");

            activity.Start();

        }
    }
}