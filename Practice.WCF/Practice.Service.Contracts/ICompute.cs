using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Practice.Service.Contracts
{
    [ServiceContract(CallbackContract = typeof(ICallBack))]
    public interface ICompute
    {
        [OperationContract(IsOneWay = true)]
        void Add(double x, double y);
    }
}
