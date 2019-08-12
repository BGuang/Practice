using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace LogsApp
{
    class Program
    {
        private static readonly ILog _Log = LogManager.GetLogger(typeof(Program));

        static void Main(string[] args)
        {
            Console.WriteLine("1");
            _Log.Info("1.Info");

            Console.WriteLine("2");
            var time = DateTime.Now.ToString();
            _Log.InfoFormat($"2.Infoformat:{time}");

            Console.WriteLine("3");
            _Log.Error($"3.Error:{time}");


            Console.WriteLine("4");
            _Log.ErrorFormat($"4.ErrorFormat:{time}");


            Test.log();

        }
    }

    public class Test
    {
        private static readonly ILog _Log = LogManager.GetLogger(typeof(Test));
        public static void log()
        {
            var time = DateTime.Now.ToString();
            Console.WriteLine("5");
            _Log.InfoFormat($"5.InfoFormat:{time}");
        }
    }



}
