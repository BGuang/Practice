using System;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Practice.RabbitMQ.Producer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("生产者启动！");

            //SimpleSender();
            //FanoutSender();
            //DirectSender();
            TopicSender();
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

        /// <summary>
        /// Direct模式
        /// </summary>
        private static void DirectSender()
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

                    channel.ExchangeDeclare("hiDirect", "direct");

                    Console.WriteLine("\nRabbitMQ-direct连接成功，请输入发布消息，输入exit退出！");

                    string input;
                    do
                    {
                        input = Console.ReadLine();
                        var body = Encoding.UTF8.GetBytes(input);
                        //发布消息、绑定exchange、必须设置routingKey
                        channel.BasicPublish("hiDirect", "onlyone", null, body: body);
                    } while (input.Trim().ToLower() != "exit");
                }
            }
        }


        /// <summary>
        /// Topic模式
        /// </summary>
        private static void TopicSender()
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

                    channel.ExchangeDeclare("hiTopic", "topic");

                    Console.WriteLine("\nRabbitMQ-topic连接成功，请输入发布消息，输入exit退出！");

                    string input;
                    do
                    {
                        input = Console.ReadLine();
                        var body = Encoding.UTF8.GetBytes(input);
                        //发布消息、绑定exchange、必须设置routingKey
                        channel.BasicPublish("hiTopic", "A.test.send.001", null, body: body);
                    } while (input.Trim().ToLower() != "exit");
                }
            }
        }


        /// <summary>
        /// RPC模式
        /// </summary>
        private static void RPCServer()
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

                    channel.ExchangeDeclare("hiTopic", "topic");

                    Console.WriteLine("\nRabbitMQ-topic连接成功，请输入发布消息，输入exit退出！");

                    //事件基本消费者
                    EventingBasicConsumer consumer = new EventingBasicConsumer(channel);

                    //接收到消息事件
                    consumer.Received += (ch, ea) =>
                    {
                        var message = Encoding.UTF8.GetString(ea.Body);
                        Console.WriteLine($"收到消息： {message}");
                        channel.BasicAck(ea.DeliveryTag, false);
                    };
                    channel.BasicConsume(queueName, false, consumer);

                    //保持通道
                    Console.ReadKey();
                    
                }
            }
        }

    }
}
