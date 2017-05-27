using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadPool
{
    public class GlobalConfig
    {
        private static long id = long.MinValue;

        private static int serverID = 2;

        /// <summary>
        /// 生成无重复ID
        /// </summary>
        /// <returns></returns>
        public static long GetID()
        {
            lock (typeof(GlobalConfig))
            {
                id += 1;
                if (id == long.MaxValue)
                {
                    id = long.MinValue;
                }
                return (serverID & 0xFFFF) << 48 | ((new DateTime().ToBinary()) / 1000L & 0xFFFFFFFF) >> 16 | id & 0xFFFF;
            }
        }

        public static long GetDate()
        {
            return Convert.ToInt64(System.DateTime.Now.ToString("yyyyMMddHHmmssfff"));
        }
    }
}
