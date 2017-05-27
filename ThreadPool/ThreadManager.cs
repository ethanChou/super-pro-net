using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadPool
{
    /// <summary>
    /// 线程池
    /// </summary>
    public class ThreadManager
    {
        private static readonly BackThread backThreadManager = new BackThread(100);

        private static readonly TimerTaskManager timerTaskManager = new TimerTaskManager();

        private static readonly ThreadManager instance = new ThreadManager();

        /// <summary>
        /// 
        /// </summary>
        public static ThreadManager GetInstance { get { return instance; } }

        private ThreadManager()
        {

        }

        System.Collections.Concurrent.ConcurrentDictionary<long, ThreadModel> threads = new System.Collections.Concurrent.ConcurrentDictionary<long, ThreadModel>();

        public long GetThreadModel()
        {
            ThreadModel model = new ThreadModel();
            return model.ID;
        }

        public void AddBackTask(TaskBase taskbase)
        {
            backThreadManager.AddTask(taskbase);
        }

        public void AddTimerTask(TimerEventTaskBase taskbase)
        {
            timerTaskManager.AddTask(taskbase);
        }

    }
}
