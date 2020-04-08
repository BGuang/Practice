using System;
using System.Net.Http;
using System.Text;
using Consul;

namespace Practice.Consul.Client
{
    class Program
    {
        static void Main(string[] args)
        {

            using (var consul = new ConsulClient(c =>
            {
                c.Address = new Uri("http://192.168.11.201:31319"); //Consul地址
            }))
            {
                CatalogService[] services = consul.Catalog.Service("DemoService").Result.Response;

                foreach (var s1 in services)
                {
                    Console.WriteLine($"ID={s1.ServiceID},Service={s1.ServiceName},Addr={s1.ServiceAddress},Port={s1.ServicePort}");

                    using (HttpClient http = new HttpClient())
                    using (var httpContent = new StringContent("{\"value\":\"fist call\"}", Encoding.UTF8, "application/json"))
                    {

                        var result= http.PostAsync($"http://{s1.ServiceAddress}:{s1.ServicePort}/api/demo",httpContent).Result;

                        Console.WriteLine(result);

                        Console.WriteLine($"响应结果：{result.Content.ReadAsStringAsync().Result}");

                    }

                }

            }

            Console.ReadKey();
        }
    }
}
