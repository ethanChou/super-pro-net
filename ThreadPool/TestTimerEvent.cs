using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadPool
{
    public class TestTimerEvent : TimerEventTaskBase
    {

        public TestTimerEvent(long startTime, int intervalTime, bool isStartAction)
            : base(startTime, intervalTime, isStartAction)
        {

        }

        public override void TaskRun()
        {
            Console.WriteLine("test WriteLine   ");
        }
    }
}
