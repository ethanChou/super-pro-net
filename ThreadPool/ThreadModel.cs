using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadPool
{
    /// <summary>
    /// 线程模型
    /// </summary>    
    public class ThreadModel 
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ID;

        public ThreadModel()
            : this("无名")
        {
            
        }

        public ThreadModel(string name)
        {
            System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(Run));
            thread.Name = name;
            thread.Start();
        }

        /// <summary>
        /// 任务队列
        /// </summary>
        protected System.Collections.Concurrent.ConcurrentQueue<TaskBase> taskQueue = new System.Collections.Concurrent.ConcurrentQueue<TaskBase>();

        /// <summary>
        /// 加入任务
        /// </summary>
        /// <param name="t"></param>
        public virtual void AddTask(TaskBase t)
        {
            taskQueue.Enqueue(t);
            ///防止线程正在阻塞时添加进入了新任务
            are.Set();
        }

        protected System.Threading.AutoResetEvent are = new System.Threading.AutoResetEvent(true);

        /// <summary>
        /// 会无限阻塞，直到拿到新任务
        /// </summary>
        /// <returns></returns>
        protected virtual TaskBase GetTask()
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

        protected virtual void Run()
        {
            while (true)
            {
                TaskBase t = GetTask();//无限阻塞，直到拿到新任务
                t.TaskRun();//执行任务
            }
        }
    }
}
