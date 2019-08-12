using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Practice.Service.Contracts;

namespace Practice.Client
{
    public class CallbackImpl : ICallBack
    {
        public void Show(double x, double y, double result)
        {
            Console.WriteLine($"{x}+{y}={result}");
        }
    }
}
