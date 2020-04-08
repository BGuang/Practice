using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Practice.Consul.DemoService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,IApplicationLifetime appLifeTime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseMvc();


            string ip = Configuration["ip"];
            string port = Configuration["port"];
            string serviceName = "DemoService";
            string serviceId = serviceName + Guid.NewGuid();
            //consul地址
            var client = new ConsulClient(p=>
            {
                p.Address = new Uri("http://192.168.11.201:31319/");
            });
            //服务注册
            var result = client.Agent.ServiceRegister(new AgentServiceRegistration()
            {
                ID= serviceId,
                Name = "DemoService",
                Address = ip,//服务ip
                Port = Convert.ToInt32(port),//服务端口
                Check = new AgentServiceCheck()//健康检查
                {
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(6),//服务启动多久后反注册
                    Interval = TimeSpan.FromSeconds(10),//监控检查间隔时间
                    HTTP = $"http://{ip}:{port}/api/Health",//健康检查地址
                    Timeout = TimeSpan.FromSeconds(5)//超时时间

                }

            });

            //注销服务
            appLifeTime.ApplicationStopped.Register(() =>
            {
                using (var consulClient = new ConsulClient(p =>
                {
                    p.Address = new Uri("http://192.168.11.201:31319/");
                }))
                {
                    consulClient.Agent.ServiceDeregister(serviceId).Wait();  //从consul集群中移除服务
                }
            });




        }
    }
}
