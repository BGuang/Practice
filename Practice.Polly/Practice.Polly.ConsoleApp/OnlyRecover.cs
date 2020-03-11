using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice.Polly.ConsoleApp
{
    public  class OnlyRecover
    {
        public OnlyRecover Retry(Action action)
        {
            action();
            return this;
        }

        //,T3,T5,T6,T7
        public OnlyRecover Retry<T1, T2>(Action<T1,T2> action,T1 arg1,T2 arg2)
        {
            action(arg1,arg2);
            return this;
        }
    }
}
