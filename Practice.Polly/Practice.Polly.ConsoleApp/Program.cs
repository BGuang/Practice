using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Polly;

namespace Practice.Polly.ConsoleApp
{
    class Program
    {
        public static int count = 0;
        static void Main(string[] args)
        {

            Console.WriteLine();

            //NormalDeal();
            //PollyTest();
            string YearMonth = "201910";

            string _y = YearMonth.Substring(0, 4);
            string _m = YearMonth.Substring(4, YearMonth.Length - 4);
           var _yearmonth = _y + _m.PadLeft(2, '0');

           Console.WriteLine(_yearmonth);
            Console.ReadKey();
        }


        #region 常规处理

        private static void NormalDeal()
        {
            var counttime = 0;
            while (counttime < 5)
            {
                counttime++;
                Console.WriteLine($"第{counttime}次重试。;{DateTime.Now}");
                try
                {
                    var ss = test();
                    Console.WriteLine("正常：" + ss);
                    break;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"第{counttime}次异常。{counttime * counttime}秒后重试;{DateTime.Now}");

                    Thread.Sleep(counttime * counttime * 1000);
                    continue;
                }
            }

            Console.WriteLine("结束");
        }


        private static int test()
        {
            count++;
            if (count == 2)
            {
                Console.WriteLine($"第{count}正常");
                return 666;
            }
            else
            {
                Console.WriteLine($"第{count}错误");
                throw new Exception("错误" + count);
            }
        }
        #endregion


        #region MyRegion

        private static void PollyTest()
        {
            Policy
                // 1. 指定要处理什么异常
                .Handle<HttpRequestException>()
                //    或者指定需要处理什么样的错误返回
                .OrResult<HttpResponseMessage>(r => r.StatusCode == HttpStatusCode.BadGateway)
                // 2. 指定重试次数和重试策略
                .Retry(3, (exception, retryCount, context) =>
                {
                    Console.WriteLine($"开始第 {retryCount} 次重试：");
                })
                // 3. 执行具体任务
                .Execute(ExecuteMockRequest);
        }
        static HttpResponseMessage ExecuteMockRequest()
        {
            // 模拟网络请求
            Console.WriteLine("正在执行网络请求...");
            Thread.Sleep(3000);
            // 模拟网络错误
            return new HttpResponseMessage(HttpStatusCode.BadGateway);
        }

        #endregion


    }
}
