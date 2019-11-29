using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Practice.TPL
{
    public class ParallelTest
    {
        public void Run()
        {
            #region 1 Parallel.Invoke

            #endregion

            #region 2 Parallel For / Parallel ForEach
            //---------【错误写法】
            List<Product> list1 = new List<Product>();
            this.ParallelFor(list1, 500);
            //异步，无法确认最后结果
            Console.WriteLine("1-错误：完成初始化：数量" + list1.Count);

            //-----------【正确写法】
            List<Product> list2 = new List<Product>();
            //正确写法1、无返回
            Task tq = new Task(() => ParallelFor(list2, 1000));//无返回值的
            tq.Start();
            Task.WaitAll(tq);
            Console.WriteLine("1-正确1：完成初始化：数量" + list2.Count);

            //正确写法2、有返回
            Task<int> tq2 = Task<int>.Factory.StartNew(() => ParallelForWithReturn(list2, 1000));
            //Task.WaitAll(tq2);
            //Console.WriteLine("1.1-正确2-完成初始化：数量" + list2.Count); //如果上面没有waitall，则这里不会执行
            //或者
            Console.WriteLine("1.2-正确3-完成初始化：数量" + tq2.Result);//先调用Result
            Console.WriteLine("1.1-正确2-完成初始化：数量" + list2.Count);

            //----------【List非线程安全对象，Lock】
            List<Product> list3 = new List<Product>();
            ParallelForEach(list2, list3);
            Console.WriteLine("4-锁-遍历后：数量" + list3.Count);

            //-----------【正确写法】
            ConcurrentBag<Product> list4 = new ConcurrentBag<Product>();
            Task tq4 = new Task(() => ParallelForEach(list2, list4));
            tq4.Start();
            Task.WaitAll(tq4);
            Console.WriteLine("4-线程对象-遍历后：数量" + list4.Count);

            #endregion

            #region ParallelLoopState
            //loopState.Stop() 立刻停止
            //loopState.Break() 仍会执行完当前条件

            //Parallel.For(0, productList.Count, (i, loopState) =>
            //Parallel.ForEach(productList, (model, loopState) =>{})
            ConcurrentBag<Product> list5 = new ConcurrentBag<Product>();
            //ParallelForEachState(list2, list5);
            Task tq5 = new Task(() => ParallelForEachState(list2, list5));
            tq5.Start();
            Task.WaitAll(tq5);
            Console.WriteLine("5-ParallelLoopState-Break-遍历后：数量" + list5.Count);


            #endregion



        }

        #region Parallel For / Parallel ForEach
        /// <summary>
        /// 1、并行顺序无法保证、2、只支持int类型
        /// </summary>
        private void ParallelFor(List<Product> list, int times = 1000)
        {
            list.Clear();
            Parallel.For(1, times, index =>
            {

                Product model = new Product();
                model.Category = "Category" + index;
                model.Name = "Name" + index;
                model.SellPrice = index;
                list.Add(model);

                Console.WriteLine("ForSetProcuct SetProcuct index: {0}", index);
            });

        }
        private int ParallelForWithReturn(List<Product> list, int times = 1000)
        {
            list.Clear();
            Parallel.For(0, times, index =>
            {

                Product model = new Product();
                model.Category = "Category" + index;
                model.Name = "Name" + index;
                model.SellPrice = index;
                list.Add(model);
                Console.WriteLine("ForSetProcuct SetProcuct index: {0}", index);
            });
            return list.Count;
        }

        #region Lock与线程安全
        private void ParallelForEach(List<Product> list, List<Product> list2)
        {
            list2.Clear();
            Parallel.ForEach(list, (model) =>
            {
                lock (list2)
                {

                    list2.Add(model);
                }
            });

        }
        private void ParallelForEach(List<Product> list, ConcurrentBag<Product> list2)
        {
            list2.Clear();
            Parallel.ForEach(list, (model) =>
            {
                list2.Add(model);
            });

        }
        #endregion

        private void ParallelForEachState(List<Product> list, ConcurrentBag<Product> list2)
        {
            list2.Clear();
            Parallel.ForEach(list, (model,loopstate) =>
            {
                if (model.SellPrice<100)
                {
                    list2.Add(model);
                }
                else
                {
                    //loopstate.Stop();
                    loopstate.Break();
                }
                
            });

        }
        #endregion


    }
}
