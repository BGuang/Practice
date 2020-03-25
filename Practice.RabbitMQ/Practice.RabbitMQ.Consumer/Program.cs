using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Practice.RabbitMQ.Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("消费者已启动!");
            #region 简单模式

            //Task.Factory.StartNew(() =>
            //{
            //    try
            //    {

            //        SimpleConsumer(20,"A");
            //    }
            //    catch (Exception e)
            //    {
            //        Console.WriteLine("模拟中断");
            //    }
            //});
            //Task.Factory.StartNew(() =>
            //{
            //    SimpleConsumer(1,"B");
            //}); 
            #endregion

            #region 手动应答\服务质量
            //WorkerConsumer();
            #endregion

            #region Exchange

            #endregion


            Console.ReadKey();
        }

        

        private static void SimpleConsumer(int secounds,string name)
        {
            ConnectionFactory connectionFactory = new ConnectionFactory()
            {
                HostName = "192.168.11.201",
                Port = 32296,
                UserName = "admin",
                Password = "admin",
                VirtualHost = "/"
            };

            using (IConnection connection = connectionFactory.CreateConnection())
            {
                using (IModel channel = connection.CreateModel())
                {
                    //声明队列，防止消费者先启动，生产者后启动
                    channel.QueueDeclare("hiqueue", false, false, false, null);

                    EventingBasicConsumer consumer=new EventingBasicConsumer(channel);

                    consumer.Received += (ch, ea) =>
                    {
                        var message = Encoding.UTF8.GetString(ea.Body);
                        //模拟耗时逻辑
                        Thread.Sleep(secounds*1000);
                        if (secounds>5)
                        {
                            //Thread.CurrentThread.Interrupt();
                            throw new Exception("错误");

                        }

                        Console.WriteLine($"消费者{name}收到消息： {message}");
                    };
                    //消费消息
                    channel.BasicConsume("hiqueue", true, consumer);
                    Console.ReadKey();
                }

            }
        }

        private static void WorkerConsumer()
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
                    //声明一个队列,因为消费者可能先启动，然后再启动消费者
                    channel.QueueDeclare(queue: "hiqueue",
                        durable: false, //持久化方式，ture写在磁盘上，false写内存中
                        exclusive: false,
                        autoDelete: false, //所有消费者断开连接时候，是否删除队列；true删除，会丢失宕机期间的消息；fasle不删除，恢复后继续处理。
                        arguments: null);

                    //事件基本消费者
                    EventingBasicConsumer consumer = new EventingBasicConsumer(channel);

                    //接收到消息事件
                    consumer.Received += (ch, ea) =>
                    {
                        Thread.Sleep(5*1000);
                        var message = Encoding.UTF8.GetString(ea.Body);
                        Console.WriteLine($"收到消息： {message}");
                        //手动应答，默认消费者从队列获取消息就算成功，手动应当开启后则需要消费者确认成功
                        //应答：确认该消息已被消费，手动确认模式下如果执行这个方法前，消费者异常退出；该消息会被重新投递
                        //默认情况下，多个消费者消费一个队列，每个消息只会被处理一次，也就是A消费了，B就不会消费；触发A应答前down掉
                        channel.BasicAck(ea.DeliveryTag, false);
                    };
                    //启动消费者 设置为手动应答消息，将autoAck设置false 关闭自动确认，和channel.BasicAck(ea.DeliveryTag, false);配合使用，防止多消费者造成的丢失消息
                    channel.BasicConsume("hiqueue", false, consumer);

                    //保持通道
                    Console.ReadKey();

                }
            }
            
            Console.WriteLine("消费者已启动");
            Console.ReadKey();
            
        }
    }
}
