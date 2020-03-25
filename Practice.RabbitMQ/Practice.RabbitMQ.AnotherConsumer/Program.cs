using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Practice.RabbitMQ.AnotherConsumer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("消费者B启动!");

            FanoutConsumer();


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

            //创建连接
            using (var connection = factory.CreateConnection())
            {
                //创建通道
                using (var channel = connection.CreateModel())
                {
                    channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);


                    channel.ExchangeDeclare("hiFanout", "fanout");
                    #region fanout
                    //模拟随机一个消费队列
                    //var queueName = channel.QueueDeclare().QueueName;
                    string queueName = "fanoutConsumer2";
                    channel.QueueDeclare(queueName, false, false, false, null);
                    channel.QueueBind(queue: queueName,
                                      exchange: "hiFanout",
                                      routingKey: "");

                    #endregion



                    //事件基本消费者
                    EventingBasicConsumer consumer = new EventingBasicConsumer(channel);

                    //接收到消息事件
                    consumer.Received += (ch, ea) =>
                    {
                        var message = Encoding.UTF8.GetString(ea.Body);
                        Console.WriteLine($"收到消息： {message}");
                        //手动应答，默认消费者从队列获取消息就算成功，手动应当开启后则需要消费者确认成功
                        //应答：确认该消息已被消费，手动确认模式下如果执行这个方法前，消费者异常退出；该消息会被重新投递
                        //默认情况下，多个消费者消费一个队列，每个消息只会被处理一次，也就是A消费了，B就不会消费；触发A应答前down掉
                        channel.BasicAck(ea.DeliveryTag, false);
                    };
                    //启动消费者 设置为手动应答消息，将autoAck设置false 关闭自动确认，和channel.BasicAck(ea.DeliveryTag, false);配合使用，防止多消费者造成的丢失消息
                    channel.BasicConsume(queueName, false, consumer);

                    //保持通道
                    Console.ReadKey();

                }
            }

            Console.WriteLine("消费者结束");
            Console.ReadKey();

        }

    }
}
