using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLApp
{
    class Program
    {
        static void Main(string[] args)
        {

            List<CustomRoleConfig> ss=new List<CustomRoleConfig>()
            {
                new CustomRoleConfig(){OrgCode = "A01",RoleIDs = "2001"},
                new CustomRoleConfig(){OrgCode = "A02",RoleIDs = "2001,1001"}
            };

            XmlUtil.Save(ss,"D:\\1.xml");
            var s=XmlUtil.Load(typeof(List<CustomRoleConfig>), "D:\\2.xml");
            Console.ReadKey();
        }
    }

    /// <summary>
    ///自定义角色附加
    /// </summary>
    public class CustomRoleConfig
    {
        /// <summary>
        /// 组织编码
        /// </summary>
        public string OrgCode { get; set; }
        /// <summary>
        /// 附加的角色ID
        /// </summary>
        public string RoleIDs { get; set; }

    }

}
