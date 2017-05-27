using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadPool
{
    public sealed class ObjectAttribute
    {

        Dictionary<String, Object> attributesMap = new Dictionary<string, object>();

        /// <summary>
        /// 如果未找到也返回 null
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Object getValue(String key)
        {
            if (attributesMap.ContainsKey(key))
            {
                return attributesMap[key];
            }
            return null;
        }


        public void setValue(String key, Object attribute)
        {
            this.attributesMap[key] = attribute;
        }

        /// <summary>
        /// 如果未找到也返回 null
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public String getStringValue(String key)
        {
            if (attributesMap.ContainsKey(key))
            {
                return attributesMap[key].ToString();
            }
            return null;
        }

        /// <summary>
        /// 如果未找到也返回 0
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public int getintValue(String key)
        {
            if (attributesMap.ContainsKey(key))
            {
                return (int)(attributesMap[key]);
            }
            return 0;
        }

        /// <summary>
        /// 如果未找到也返回 0
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public long getlongValue(String key)
        {
            if (attributesMap.ContainsKey(key))
            {
                return (long)(attributesMap[key]);
            }
            return 0;
        }


        /// <summary>
        /// 如果未找到也返回 0
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public float getfloatValue(String key)
        {
            if (attributesMap.ContainsKey(key))
            {
                return (float)(attributesMap[key]);
            }
            return 0;
        }

        /// <summary>
        /// 如果未找到也返回 false
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool getbooleanValue(String key)
        {
            if (attributesMap.ContainsKey(key))
            {
                return (bool)(attributesMap[key]);
            }
            return false;
        }
    }
}
