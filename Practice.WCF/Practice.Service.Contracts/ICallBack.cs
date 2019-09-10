using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Practice.Service.Contracts
{
    [ServiceContract]
    public interface ICallBack
    {
        [OperationContract(IsOneWay = true)]
        void Show(string result);
    }
}
