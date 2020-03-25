using System;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace Practice.RabbitMQ.Producer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("生产者启动！");

            //SimpleSender();
            FanoutSender();

            Console.ReadKey();
        }
        /// <summary>
        /// 简单发布者
        /// </summary>
        private static void SimpleSender()
        {
            //创建连接工厂
            ConnectionFactory factory = new ConnectionFactory
            {
                UserName = "admin", //用户名
                Password = "admin", //密码
                HostName = "192.168.11.201", //rabbitmq ip
                Port = 32296
            };

            //创建连接,手动释放：channel.Close();
            using (var connection = factory.CreateConnection())
            {
                //创建通道，可收到释放：connection.Close();
                using (var channel = connection.CreateModel())
                {


                    //声明一个队列
                    channel.QueueDeclare(
                        queue: "hiqueue", //队列名
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

                    Console.WriteLine("\nRabbitMQ连接成功，请输入发布消息，输入exit退出！");

                    string input;
                    do
                    {
                        input = Console.ReadLine();
                        //Body是一堆字节码
                        var body = Encoding.UTF8.GetBytes(input);
                        //发布消息
                        channel.BasicPublish("", "hiqueue", null, body: body);
                    } while (input.Trim().ToLower() != "exit");
                }
            }

        }

        /// <summary>
        /// Fanout模式
        /// </summary>
        private static void FanoutSender()
        {
            //创建连接工厂
            ConnectionFactory factory = new ConnectionFactory
            {
                UserName = "admin", //用户名
                Password = "admin", //密码
                HostName = "192.168.11.201", //rabbitmq ip
                Port = 32296
            };

            //创建连接,手动释放：channel.Close();
            using (var connection = factory.CreateConnection())
            {
                //创建通道，可收到释放：connection.Close();
                using (var channel = connection.CreateModel())
                {


                    //声明一个队列
                    //channel.QueueDeclare(queue: "hiqueue",durable: false,exclusive: false,autoDelete: false,arguments: null);

                    channel.ExchangeDeclare("hiFanout","fanout");

                    Console.WriteLine("\nRabbitMQ连接成功，请输入发布消息，输入exit退出！");

                    string input;
                    do
                    {
                        input = Console.ReadLine();
                        //Body是一堆字节码
                        var body = Encoding.UTF8.GetBytes(input);
                        //发布消息
                        channel.BasicPublish("hiFanout", "", null, body: body);
                    } while (input.Trim().ToLower() != "exit");
                }
            }
        }

    }
}
