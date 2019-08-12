using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Practice.Service.Contracts;

namespace Practice.Service.Web
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“ComputeService”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 ComputeService.svc 或 ComputeService.svc.cs，然后开始调试。
    public class ComputeService : ICompute
    {
        public void Add(double x, double y)
        {
            var result = x + y;
            ICallBack callBack = OperationContext.Current.GetCallbackChannel<ICallBack>();
            callBack.Show(x, y, result);
        }
    }
}
