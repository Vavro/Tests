using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public delegate void TestDelegate(object sender, EventArgs e);

    public class EventTest
    {
        public event TestDelegate TestEvent;

        [Fact]
        public void WillCallMultipleTimes()
        {
            TestEvent += OnTestEvent;
            TestEvent += OnTestEvent;

            TestEvent(this, new EventArgs());

            TestEvent -= OnTestEvent;
            Console.WriteLine("First removed!");
            TestEvent(this, new EventArgs());

            TestEvent -= OnTestEvent;
            Console.WriteLine("Second removed!");
            if (TestEvent != null)
                TestEvent(this, new EventArgs());

            TestEvent -= OnTestEvent;
            Console.WriteLine("Third removed!");
        }

        private void OnTestEvent(object sender, EventArgs eventArgs)
        {
            Console.WriteLine("called!");
        }
    }
}
