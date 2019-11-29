using System;

namespace Practice.TPL
{
    class Program
    {
        static void Main(string[] args)
        {
            //ParallelTest pt=new ParallelTest();
            //pt.Run();

            ConcurrentTest ct=new ConcurrentTest();
            ct.ConcurrentDictionaryTest();


            Console.ReadKey();
        }
    }
}
