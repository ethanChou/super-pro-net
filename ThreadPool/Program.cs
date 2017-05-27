using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadPool
{
    class Program
    {
        static void Main(string[] args)
        {
            ThreadManager.GetInstance.AddTimerTask(new TestTimerEvent(0, 100, false));
            ThreadManager.GetInstance.AddTimerTask(new TestTimerEvent(0, 700, false));
            ThreadManager.GetInstance.AddTimerTask(new TestTimerEvent(0, 10, false));
            ThreadManager.GetInstance.AddTimerTask(new TestTimerEvent(0, 500, false));
            ThreadManager.GetInstance.AddTimerTask(new TestTimerEvent(0, 1000, false));
            ThreadManager.GetInstance.AddTimerTask(new TestTimerEvent(0, 100, false));
            ThreadManager.GetInstance.AddTimerTask(new TestTimerEvent(0, 5000, false));
            ThreadManager.GetInstance.AddTimerTask(new TestTimerEvent(0, 10, false));
            ThreadManager.GetInstance.AddTimerTask(new TestTimerEvent(0, 100, false));
            ThreadManager.GetInstance.AddTimerTask(new TestTimerEvent(0, 300, false));
            ThreadManager.GetInstance.AddTimerTask(new TestTimerEvent(0, 220, false));
        }
    }
}
