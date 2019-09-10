using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Practice.Service.Contracts;

namespace Practice.Service.Web
{
    //[ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant)]
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class ComputeService : ICompute
    {
        public void Add(double x, double y)
        {
            var result = x + y;
            ICallBack callBack = OperationContext.Current.GetCallbackChannel<ICallBack>();
            callBack.Show($"Double:{x}+{y}={result}");
        }

        public void Add(int x, int y)
        {
            var result = x + y;
            ICallBack callBack = OperationContext.Current.GetCallbackChannel<ICallBack>();
            callBack.Show($"INT：{x}+{y}={result}");
        }
    }
}
