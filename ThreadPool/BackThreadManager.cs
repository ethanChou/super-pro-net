using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadPool
{
    /// <summary>
    /// 后台线程池
    /// </summary>
    internal sealed class BackThread
    {       

        /// <summary>
        /// 初始化线程模型，并且启动线程
        /// </summary>
        internal BackThread(int count)
        {
            for (int i = 0; i < count; i++)
            {
                System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(Run));
                //thread.IsBackground = true;
                thread.Start();
            }
        }

        /// <summary>
        /// 任务队列
        /// </summary>
        System.Collections.Concurrent.ConcurrentQueue<TaskBase> taskQueue = new System.Collections.Concurrent.ConcurrentQueue<TaskBase>();

        /// <summary>
        /// 加入任务
        /// </summary>
        /// <param name="t"></param>
        internal void AddTask(TaskBase t)
        {
            taskQueue.Enqueue(t);
            ///防止线程正在阻塞时添加进入了新任务
            are.Set();
        }

        System.Threading.AutoResetEvent are = new System.Threading.AutoResetEvent(true);

        /// <summary>
        /// 会无限阻塞，直到拿到新任务
        /// </summary>
        /// <returns></returns>
        private TaskBase GetTask()
        {
            TaskBase t = default(TaskBase);
            while (true)
            {
                if (!taskQueue.IsEmpty && taskQueue.TryDequeue(out t))
                {
                    break;
                }
                else
                {
                    ///队列为空等待200毫秒继续
                    are.WaitOne(200);
                }
            }
            return t;
        }

        private void Run()
        {
            while (true)
            {
                try
                {
                    TaskBase t = GetTask();
                    t.TaskRun();
                    //log.Info("后台线程完成任务");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("后台线程执行任务 exception " + ex.ToString());
                }
            }
        }
    }
}
