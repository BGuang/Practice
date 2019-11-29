using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Practice.TPL
{
    public static class GlobalConfig
    {
        //private static readonly GlobalConfig _instance = new GlobalConfig();

        private static object olock = new object();
        //private GlobalConfig()
        //{
        //}

        //public static GlobalConfig Instance()
        //{
        //    return _instance;
        //}

        /// <summary>
        /// 最大访问微信次数
        /// </summary>
        public static readonly int MaxTokenCount = 10;

        public static int _currentCount;
        /// <summary>
        /// 当前调用次数
        /// </summary>
        public static int CurrentCount
        {
            get { return _currentCount; }
            set
            {
                
                    _currentCount = value;
                
            }

            //get;
            //set;
        }
        public static void AddCurrentCount()
        {
            Interlocked.Increment(ref _currentCount);
        }




    }
}
