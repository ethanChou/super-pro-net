
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadPool
{
    internal sealed class TimerTaskManager
    {

        public TimerTaskManager()
        {
            System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(Run));
            thread.IsBackground = true;
            thread.Start();
        }

        /// <summary>
        /// 任务队列
        /// </summary>
        private List<TimerEventTaskBase> taskQueue = new List<TimerEventTaskBase>();

        /// <summary>
        /// 加入任务
        /// </summary>
        /// <param name="t"></param>
        public void AddTask(TimerEventTaskBase t)
        {
            if (t.IsStartAction)
            {
                t.TaskRun();
            }
            lock (taskQueue)
            {
                taskQueue.Add(t);
            }
            ///防止线程正在阻塞时添加进入了新任务
            are.Set();
        }

        private System.Threading.AutoResetEvent are = new System.Threading.AutoResetEvent(true);


        /// <summary>
        /// 重构函数执行器
        /// </summary>
        private void Run()
        {
            ///无限循环执行函数器
            while (true)
            {
                if (taskQueue.Count > 0)
                {
                    IEnumerable<TimerEventTaskBase> collections = null;
                    lock (taskQueue)
                    {
                        collections = new List<TimerEventTaskBase>(taskQueue);
                        //collections = taskQueue.Where((TimerEventTaskBase) => { return true; });
                    }
                    foreach (TimerEventTaskBase tet in collections)
                    {
                        int actionCount = tet.TempAttribute.getintValue("actionCount");
                        if ((tet.EndTime > 0 && GlobalConfig.GetDate() > tet.EndTime) || (tet.ActionCount > 0 && actionCount > tet.ActionCount))
                        {
                            //任务过期
                            lock (taskQueue)
                            {
                                taskQueue.Remove(tet);
                            }
                            continue;
                        }
                        long lastactiontime = tet.TempAttribute.getlongValue("lastactiontime");
                        if (lastactiontime != 0 && Math.Abs(GlobalConfig.GetDate() - lastactiontime) < tet.IntervalTime)
                        {
                            continue;
                        }
                        tet.TempAttribute.setValue("actionCount", ++actionCount);
                        tet.TempAttribute.setValue("lastactiontime", GlobalConfig.GetDate());
                        ThreadManager.GetInstance.AddBackTask(tet);
                    }
                    are.WaitOne(10);
                }
                else
                {
                    ///队列为空等待200毫秒继续
                    are.WaitOne(200);
                }

            }
        }
    }
}
