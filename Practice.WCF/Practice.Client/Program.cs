using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Practice.Service.Contracts;
using Practice.Service.Proxy;

namespace Practice.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            //InstanceContext instanceContext = new InstanceContext(new CallbackImpl());
            //DuplexChannelFactory<ICompute> channelFactory =
            //    new DuplexChannelFactory<ICompute>(instanceContext, "Compute_Endpoint");
            //var proxy = channelFactory.CreateChannel();
            //using ( proxy as IDisposable)
            //{

            //    proxy.Add(2, 1);
            //}

            InstanceContext instanceContext = new InstanceContext(new CallbackImpl());
            using (var proxy=new ComputeClient(instanceContext))
            {
                proxy.Add(2, 1);
                //proxy.Add(1.0,2.0);//通信关闭
            }
            using (var proxy = new ComputeClient(instanceContext))
            {
                proxy.Add(1.0,2.0);
            }


            Console.ReadKey();
        }
    }
}
