using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Practice.Service.Contracts;

namespace Practice.Client
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class CallbackImpl : ICallBack
    {
        public void Show(string result)
        {
            Console.WriteLine(result);
        }

    }

}
