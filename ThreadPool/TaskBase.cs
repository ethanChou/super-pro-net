using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadPool
{
    public abstract class TaskBase
    {
        public TaskBase()
        {
            TempAttribute = new ObjectAttribute();
        }
        public ObjectAttribute TempAttribute { get; set; }

        public abstract void TaskRun();
    }
}
