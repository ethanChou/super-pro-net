using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadPool
{
    public abstract class TimerEventTaskBase : TaskBase
    {

        /// <summary>
        /// 开始执行的时间
        /// </summary>
        public long StartTime { get; set; }

        /// <summary>
        /// 是否一开始执行一次
        /// </summary>
        public bool IsStartAction { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public long EndTime { get; set; }

        /// <summary>
        /// 执行次数
        /// </summary>
        public int ActionCount { get; set; }

        /// <summary>
        /// 间隔执行时间
        /// </summary>
        public int IntervalTime { get; set; }

        /// <summary>
        /// 制定执行次数的定时任务
        /// </summary>
        /// <param name="startTime">0表示立即执行,否则延迟执行,填写开始时间</param>
        /// <param name="intervalTime">执行间隔时间,小于10毫秒,当10毫秒处理</param>
        /// <param name="isStartAction">是否一开始执行一次</param>
        /// <param name="actionCount">需要执行的次数</param>
        public TimerEventTaskBase(long startTime, int intervalTime, bool isStartAction, int actionCount)
        {
            this.StartTime = startTime;
            this.IntervalTime = intervalTime;
            this.IsStartAction = isStartAction;
            this.ActionCount = actionCount;
            this.EndTime = 0;
        }

        /// <summary>
        /// 制定结束时间的定时任务
        /// </summary>
        /// <param name="startTime">0表示立即执行,否则延迟执行,填写开始时间</param>
        /// <param name="intervalTime">执行间隔时间,小于10毫秒,当10毫秒处理</param>
        /// <param name="endTime">执行结束时间</param>
        /// <param name="isStartAction">是否一开始执行一次</param>
        public TimerEventTaskBase(long startTime, int intervalTime, long endTime, bool isStartAction)
        {
            this.StartTime = startTime;
            this.IntervalTime = intervalTime;
            this.IsStartAction = isStartAction;
            this.ActionCount = 0;
            this.EndTime = endTime;
        }

        /// <summary>
        /// 制定开始时间,无限执行任务
        /// </summary>
        /// <param name="startTime">0表示立即执行,否则延迟执行,填写开始时间</param>
        /// <param name="intervalTime">执行间隔时间,小于10毫秒,当 10 毫秒处理 建议 10 毫秒的倍数</param>
        /// <param name="isStartAction">是否一开始执行一次</param>
        public TimerEventTaskBase(long startTime, int intervalTime, bool isStartAction)
        {
            this.StartTime = startTime;
            this.IntervalTime = intervalTime;
            this.IsStartAction = isStartAction;
            this.ActionCount = 0;
            this.EndTime = 0;
        }

        public TimerEventTaskBase()
        {
            // TODO: Complete member initialization
        }

    }
}
