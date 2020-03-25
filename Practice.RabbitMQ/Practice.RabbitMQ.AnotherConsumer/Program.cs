using System;
using RabbitMQ.Client;

namespace Practice.RabbitMQ.AnotherConsumer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("另一个消费者!");



            Console.ReadKey();
        }

        private static void FanoutConsumer()
        {

            //创建连接工厂，默认情况下为“ guest” /“ guest”，仅限本地主机连接
            ConnectionFactory factory = new ConnectionFactory
            {
                UserName = "admin", //用户名
                Password = "admin", //密码
                VirtualHost = "/", //虚拟目录
                HostName = "192.168.11.201", //rabbitmq ip
                Port = 32296
            };

            using (IConnection connection=factory.CreateConnection())
            {
                using (IModel channel= connection.CreateModel())
                {
                    


                    //保持通道
                    Console.ReadKey();

                }

                
            }


        }
    }
}
