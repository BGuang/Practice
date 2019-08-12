using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Practice.Service.Contracts;

namespace Practice.Service.Proxy
{
    public class ComputeClient : DuplexClientBase<ICompute>, ICompute
    {
        public ComputeClient(System.ServiceModel.InstanceContext callbackInstance) :
            base(callbackInstance)
        {
        }

        public ComputeClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName) :
            base(callbackInstance, endpointConfigurationName)
        {
        }

        public ComputeClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, string remoteAddress) :
            base(callbackInstance, endpointConfigurationName, remoteAddress)
        {
        }

        public ComputeClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) :
            base(callbackInstance, endpointConfigurationName, remoteAddress)
        {
        }

        public ComputeClient(System.ServiceModel.InstanceContext callbackInstance, System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) :
            base(callbackInstance, binding, remoteAddress)
        {
        }

        public void Add(double x, double y)
        {
            base.Channel.Add(x, y);
        }
    }
}
