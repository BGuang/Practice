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
            //TopicSender();
            RPCServer();
            Console.ReadKey();
        }
        /// <summary>
        /// 简单发布者
        /// </summary>
        private static void SimpleSender()
        {
            //1.创建连接工厂
            ConnectionFactory factory = new ConnectionFactory
            {
                UserName = "admin", //用户名
                Password = "admin", //密码
                HostName = "192.168.11.201", //rabbitmq ip
                Port = 32296
            };

            //2.创建连接,手动释放：channel.Close();
            using (var connection = factory.CreateConnection())
            {
                //3.创建通道，可收到释放：connection.Close();
                using (var channel = connection.CreateModel())
                {
                    //4.声明一个队列
                    channel.QueueDeclare(
                        queue: "hiqueue", //队列名
                        durable: false,//持久化
                        exclusive: false,//是否互斥，互斥则只有这个连接可以访问此队列
                        autoDelete: false,//连接断开后是否删除队列
                        arguments: null);

                    Console.WriteLine("\nRabbitMQ发送端连接成功，请输入发布消息，输入exit退出！");

                    string input;
                    do
                    {
                        input = Console.ReadLine();
                        var body = Encoding.UTF8.GetBytes(input);//Body是一堆字节码
                        //5.发布消息
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
        /// Direct模式、单播
        /// </summary>
        private static void DirectSender()
        {
            //1.创建连接工厂
            ConnectionFactory factory = new ConnectionFactory
            {
                UserName = "admin", //用户名
                Password = "admin", //密码
                HostName = "192.168.11.201", //rabbitmq ip
                Port = 32296
            };

            //2.创建连接,手动释放：channel.Close();
            using (var connection = factory.CreateConnection())
            {
                //3.创建通道，可收到释放：connection.Close();
                using (var channel = connection.CreateModel())
                {
                    //4.交换器
                    channel.ExchangeDeclare("hiDirect", "direct");

                    Console.WriteLine("\nRabbitMQ-direct连接成功，请输入发布消息，输入exit退出！");

                    string input;
                    do
                    {
                        input = Console.ReadLine();
                        var body = Encoding.UTF8.GetBytes(input);
                        //5.发布消息、Direct绑定exchange、必须设置routingKey
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

            //创建连接
            using (var connection = factory.CreateConnection())
            {
                //创建通道
                using (var channel = connection.CreateModel())
                {
                    //1.声明交换机
                    channel.ExchangeDeclare("hiTopic", "topic");

                    Console.WriteLine("\nRabbitMQ-topic连接成功，请输入发布消息，输入exit退出！");

                    string input;
                    do
                    {
                        input = Console.ReadLine();
                        var body = Encoding.UTF8.GetBytes(input);
                        //2.发布消息、绑定exchange、设置routingKey
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

            //创建连接
            using (var connection = factory.CreateConnection())
            {
                //创建通道
                using (var channel = connection.CreateModel())
                {
                    //服务质量
                    channel.BasicQos(0, 1, false);

                    //1.声明服务端队列-接收客户端请求
                    string serverQueueName = "rpc_server";
                    channel.QueueDeclare(queue: serverQueueName,durable: false,exclusive: false,autoDelete: false,arguments: null);
                    
                    Console.WriteLine("RPC_Server连接成功，等待处理消息！");

                    //2.事件基本消费者
                    EventingBasicConsumer consumer = new EventingBasicConsumer(channel);

                    //3.接收到消息事件
                    consumer.Received += (ch, ea) =>
                    {
                        //3.1服务端调用本地服务
                        var message = Encoding.UTF8.GetString(ea.Body);
                        var result = $"【响应】服务端已处理消息：Id:{ea.BasicProperties.CorrelationId}-内容：{message}";
                        Console.WriteLine($"服务端收到客户端消息： {message}并处理");

                        //3.2服务端响应结果
                        var props = ea.BasicProperties;
                        var replyprops=channel.CreateBasicProperties();
                        replyprops.CorrelationId = props.CorrelationId;
                        //发送：服务端处理结果
                        channel.BasicPublish("",props.ReplyTo,replyprops, Encoding.UTF8.GetBytes(result.ToString()));

                        //消费应答
                        channel.BasicAck(ea.DeliveryTag, false);
                    };
                    //4.消费消息
                    channel.BasicConsume(serverQueueName, false, consumer);

                    //保持通道
                    Console.ReadKey();
                    
                }
            }
        }

    }
}
