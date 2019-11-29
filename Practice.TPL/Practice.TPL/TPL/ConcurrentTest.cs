using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Practice.TPL
{
    /// <summary>
    /// 
    /// </summary>
    public class ConcurrentTest
    {
        //五大线程安全对象

        //1.BlockingCollection 经典的阻塞队列集合

        //2.ConcurrentBag  无序集合


        //3.ConcurrentDictionary 键值对字典
        private ConcurrentDictionary<string, int> count = new ConcurrentDictionary<string, int> { ["CurrentCount"] = 0 };
        private int count2 ;

        public void ConcurrentDictionaryTest()
        {
            for (int i = 0; i < 100; i++)
            {
                object locks = new object();
                Task.Factory.StartNew(() =>
                {
                    //开始action
                    Interlocked.Increment(ref count2);
                    var sss = count2;
                    Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
                    Console.WriteLine("结果：" + sss);
                    //GlobalConfig.AddCurrentCount();

                    //响应
                });
            }

            Task.WaitAll();

            //Console.WriteLine(count["CurrentCount"]);
            //Console.WriteLine(GlobalConfig.CurrentCount);
            //Console.WriteLine(GlobalConfig._currentCount);
            
        }
        //4.ConcurrentQueue 先进先出集合

        //5.ConcurrentStack 后进先出集合







    }
}
