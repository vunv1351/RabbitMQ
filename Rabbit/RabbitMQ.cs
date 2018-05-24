using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.MessagePatterns;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rabbit
{
    class RabbitMQ
    {

        private ConnectionFactory govConnectionFactory;
        private ConnectionFactory govConnectionFactory1;
        private int Port = 5672;
        private IConnection govConnection;
        private static IModel govChannel;
        private IConnection govConnection1;
        private static IModel govChannel1;
        private EventingBasicConsumer consumer;
        private object _syncRoot = new object();
        string Track = null;
        public RabbitMQ(string track, string group) { }
        public RabbitMQ()
        {

        }
        public RabbitMQ(string track)
        {
            StartConnect();
            this.Track = track;

        }
        public void StartConnect()
        {

            StartConnectRabbit();
            //  this.govConnectionFactory = new ConnectionFactory();
            //  this.govConnection = this.govConnectionFactory.CreateConnection();
            //  govChannel = this.govConnection.CreateModel();


            //check
            Console.WriteLine(govConnectionFactory.HostName);

        }
        private void StartConnectRabbit()
        {
            try
            {
                // check connection 				
                if ((this.govConnection == null) || !this.govConnection.IsOpen)
                {
                    // try to connect 
                    lock (_syncRoot)
                    {
                        this.govConnectionFactory = new ConnectionFactory();
                        this.govConnectionFactory1 = new ConnectionFactory();
                        this.govConnectionFactory.VirtualHost = "/gov/";
                        this.govConnectionFactory1.VirtualHost = "/";
                        this.govConnectionFactory.Protocol = Protocols.AMQP_0_9_1;
                        //this.govConnectionFactory.UserName = "vunv";
                        //this.govConnectionFactory.Password = "vunv@123456";
                        //this.govConnectionFactory.HostName = "ec2-13-229-226-59.ap-southeast-1.compute.amazonaws.com";
                        this.govConnectionFactory.UserName = "guest";
                        this.govConnectionFactory.Password = "guest";
                        this.govConnectionFactory.HostName = "localhost";
                        this.govConnectionFactory.HostName = "localhost";
                        this.govConnectionFactory1.UserName = "guest";
                        this.govConnectionFactory1.Password = "guest";
                        // this.govConnectionFactory1.HostName = "localhost";
                        // this.govConnectionFactory1.Port = Port;
                        //  if ((this.govConnection == null) || !this.govConnection.IsOpen || (this.govConnection1 == null) || !this.govConnection1.IsOpen)
                        if ((this.govConnection == null) || !this.govConnection.IsOpen)
                        {
                            try
                            {
                                // connect cloud
                                this.govConnection = this.govConnectionFactory.CreateConnection();
                                govChannel = this.govConnection.CreateModel();
                                //connect localhost
                                this.govConnection1 = this.govConnectionFactory1.CreateConnection();
                                govChannel1 = this.govConnection1.CreateModel();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public void SendMessage()
        {
            string data1 = "BGTVT,0,1522911593,43A21174,7E08C175500104E0,,0,108.231391666667,16.0751416666667,0,290,0,0,100";
            byte[] bytes = Encoding.ASCII.GetBytes(data1);
            PutRabbitMQ(bytes, Track);
            int i = 1;
            if (i == 100)
            {
                i = 1;
                System.Threading.Thread.Sleep(40);
            }

        }
        public void PutRabbitMQ(byte[] bytearr, string track)
        {
            //govChannel.BasicPublish("gps.government", track, null, bytearr);
            //govChannel.BasicPublish("gps.government", "gov.track0", null, bytearr);
            govChannel1.BasicPublish("trackvnet.com", "track1", null, bytearr);
            Console.WriteLine(govConnectionFactory.HostName + ":" + govConnectionFactory.Port + "---" + govConnectionFactory.Protocol + "---" + govConnectionFactory.Password + "-----" + govConnectionFactory.VirtualHost + "----" + govConnectionFactory.UserName);

        }
        public void PutRabbitMQ(byte[] bytearr)
        {

            govChannel1.BasicPublish("amq.direct", "track2", null, bytearr);
            //govChannel.BasicPublish("trackvnet.com", "gov.track1", null, bytearr);
            Console.WriteLine(govConnectionFactory.HostName + ":" + govConnectionFactory.Port + "---" + govConnectionFactory.Protocol + "---" + govConnectionFactory.Password + "-----" + govConnectionFactory.VirtualHost + "----" + govConnectionFactory.UserName);

        }
        public void PutRabbitMQ1(byte[] bytearr, string track)
        {

            govChannel.BasicPublish("amq.direct", track, null, bytearr);
            //govChannel.BasicPublish("trackvnet.com", "gov.track1", null, bytearr);
          //  Console.WriteLine(govConnectionFactory.HostName + ":" + govConnectionFactory.Port + "---" + govConnectionFactory.Protocol + "---" + govConnectionFactory.Password + "-----" + govConnectionFactory.VirtualHost + "----" + govConnectionFactory.UserName);

        }
        private byte[] ObjectToByteArray(object obj)
        {
            if (obj == null)
                return null;

            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, obj);

            return ms.ToArray();
        }

        string date = null;
        public void countMessage(object obj)
        {
            lock (_syncRoot)
            {
                Dictionary<string, object> args = new Dictionary<string, object>()
{
    { "x-queue-mode", "lazy" }

                    //count message
}; QueueDeclareOk result = govChannel.QueueDeclare(queue: "gov.track0",
                                           durable: true,
                                           exclusive: false,
                                           autoDelete: false,
                                           arguments: args);
                uint count = result.MessageCount;
                Console.WriteLine("count message: " + count);
                WriteAppendLog(count.ToString(), "D:\\count.txt");
            }

        }
        // bool exits = true;
        private ManualResetEvent mrse = new ManualResetEvent(false);
        public void signaled()
        {
            mrse.Set();
        }
        int count = 0;
        private static QueueingBasicConsumer consumerss;
        // BasicDeliverEventArgs deliveryArguments;
        private static bool global = false;
        public void getdatatest(object obj)
        {
            WriteAppendLog("message cout at: " + DateTime.Now.ToString("MMddyyy HH:mm:ss") + "-" + count, "D:\\test.txt");
            count = 0;
            
            if (global == false)
            {
                govChannel.BasicQos(0, 100, true); //basic quality of service
                global = true;                                //   govChannel.BasicQos(); //basic quality of service

                consumerss = new QueueingBasicConsumer(govChannel);
                govChannel.BasicConsume("track11", false, consumerss);
            }
 
            while (true)
            {

                count++;
                var deliveryArguments = (BasicDeliverEventArgs)consumerss.Queue.Dequeue();
                String message = Encoding.UTF8.GetString(deliveryArguments.Body);
                Console.WriteLine("Message received: {0} ", count);
                PutRabbitMQ(deliveryArguments.Body);
                //Console.WriteLine("Message received: {0} , {1}",count, message);
                if (count == 200)
                {
                    govChannel.BasicAck(deliveryArguments.DeliveryTag, false);

                    // govChannel.BasicCancel(consumerss.ConsumerTag);


                    break;
                    //return;
                }
                govChannel.BasicAck(deliveryArguments.DeliveryTag, false);
            }

        }

        public void ReceiveHeadersMessageReceiverTwo()
        {
            if (global == false)
            {
                govChannel.BasicQos(0, 100, true); //basic quality of service
                global = true;

            }
            //  govChannel.BasicQos(0, 1, false);
            Subscription subscription = new Subscription(govChannel, "track6", false);
            while (true)
            {
                count++;
                BasicDeliverEventArgs deliveryArguments = subscription.Next();
                StringBuilder messageBuilder = new StringBuilder();
                String message = Encoding.UTF8.GetString(deliveryArguments.Body);
                // messageBuilder.Append("Message from queue: ").Append(message).Append(". ");
                PutRabbitMQ(deliveryArguments.Body);

                if (count == 200)
                {
                    count = 0;
                    subscription.Ack(deliveryArguments);
                    govChannel.BasicCancel(subscription.ConsumerTag);
                    return;
                }
                ///Console.WriteLine("count {0},  message {1} ",count,messageBuilder.ToString());
                Console.WriteLine("count {0}", count);
                subscription.Ack(deliveryArguments);
            }
        }
        public void get(object obj)
        {

            //exits = true;
            date = Convert.ToString(DateTime.Now.ToString("HHmmss"));
            // StartConnect();
            List<string> data = new List<string>();


            try
            {

                Dictionary<string, object> args = new Dictionary<string, object>()
{
    { "queue-mode", "lazy" }
};

                ///
                govChannel.QueueDeclare(queue: "track6",
                                            durable: true,
                                            exclusive: false,
                                            autoDelete: false,
                                            arguments: args);

                consumer = new EventingBasicConsumer(govChannel);
                //var consumer = new EventingBasicConsumer();
                consumer.Model = (IModel)govChannel;
                govChannel.BasicQos(0, 100, false);
                //  if (mrse.WaitOne()==true)
                //  {
                //      mrse.Set();
                //  }
                //
                // consumer.Received += ConsumerOnReceived;


                consumer.Received += (model, ea) =>
                {
                    count++;
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine("count {0}, message {1}", count, message);
                    //ThreadPool.QueueUserWorkItem(processstr, message);
                    //PutRabbitMQ(Encoding.UTF8.GetBytes(message), "gov.track1");
                    data.Add(message);
                    if (count == 200)
                    {
                        // ThreadPool.QueueUserWorkItem(process, data);
                        // process(data);
                        count = 1;

                        //  Thread.Sleep(5000);


                        //  mrse.
                        if (consumer != null)
                        {

                            //govChannel.BasicCancel(consumer.ConsumerTag);
                            data = new List<string>();

                        }
                        //govChannel.BasicAck(ea.DeliveryTag, false);

                        //   mrse.WaitOne();

                    }
                    govChannel.BasicAck(ea.DeliveryTag, false);
                };


                govChannel.BasicConsume(queue: "track6",
                             // noAck: true,
                             autoAck: false,
                             consumer: consumer);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
        public virtual void ConsumerOnReceived(object sender, BasicDeliverEventArgs ea)
        {
            count++;
            var body = ea.Body;
            var message = Encoding.UTF8.GetString(body);

            Console.WriteLine(message);
            if (count == 200)
            {
                count = 0;
                return;
            }
            govChannel.BasicAck(ea.DeliveryTag, false);


        }
        List<string> data = null;
        public List<string> getL()
        {
            data = new List<string>();
            try
            {
                govChannel.QueueDeclare(queue: "gov.track0",
                                        durable: true,
                                        exclusive: false,
                                        autoDelete: false,
                                        arguments: null);

                var consumer = new EventingBasicConsumer(govChannel);
                govChannel.BasicQos(0, 100, true);
                consumer.Received += (model, ea) =>
                {

                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    object ob = ByteArrayToObject(body);
                    data.Add(message);

                    if (data.Count == 150)
                    {
                        ThreadPool.QueueUserWorkItem(process, data);
                    }
                    // ThreadPool.QueueUserWorkItem(process, (object)message);
                    govChannel.BasicAck(ea.DeliveryTag, false);
                };
                govChannel.BasicConsume(queue: "gov.track0",
                     // noAck: true,
                     autoAck: false,
                     consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        List<object> dataIOBJ = null;
        public List<object> getobj()
        {
            dataIOBJ = new List<object>();
            try
            {

                govChannel.QueueDeclare(queue: "track1",
                                        durable: true,
                                        exclusive: false,
                                        autoDelete: false,
                                        arguments: null);

                var consumer = new EventingBasicConsumer(govChannel);
                govChannel.BasicQos(0, 100, true);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    object ob = ByteArrayToObject(body);
                    // data.Add(message);
                    dataIOBJ.Add(ob);
                    if (data.Count == 150)
                    {
                        ThreadPool.QueueUserWorkItem(Process, data);
                    }
                    // ThreadPool.QueueUserWorkItem(process, (object)message);
                    govChannel.BasicAck(ea.DeliveryTag, false);
                };
                govChannel.BasicConsume(queue: "track1",
                     // noAck: true,
                     autoAck: false,
                     consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dataIOBJ;
        }
        public void Process(object data)
        {

            List<object> l = (List<object>)data;
            foreach (string dta in l)
            {

                Console.WriteLine(" [x] Received {0} " + dta.ToString());
                WriteAppendLog(dta.ToString(), "D:\\revices1.txt");
                //Thread.Sleep(500);
                Console.WriteLine(" Wakeup at " + DateTime.Now);
            }
            WriteAppendLog("--------------------------------------------", "D:\\revices1.txt");
        }
        public void processstr(object data)
        {

            string l = (string)data;
            PutRabbitMQ(Encoding.UTF8.GetBytes(l), "gov.track1");
            Console.WriteLine(" [x] Received {0} " + data.ToString());
            // WriteAppendLog(data.ToString(), "D:\\revices1.txt");
            //Thread.Sleep(500);
            Console.WriteLine(" Wakeup at " + DateTime.Now);

            // WriteAppendLog("--------------------------------------------", "D:\\revices1.txt");
        }
        public void process(object data)
        {

            //exits = false;
            List<string> l = (List<string>)data;
            foreach (string dta in l.ToList())
            {
                PutRabbitMQ(Encoding.UTF8.GetBytes(dta), "track1");
                Console.WriteLine(" [x] Received {0} " + dta.ToString());
                // WriteAppendLog(dta.ToString(), "D:\\" + date + ".txt");
                //Thread.Sleep(500);
            }
            //mrse.Set();
            WriteAppendLog("--------------------------------------------", "D:\\revices1.txt");
        }
        public static void WriteAppendLog(string message, string path)
        {
            try
            {
                // Create directory first if it not exist.
                Directory.CreateDirectory(Path.GetDirectoryName(path));

                using (StreamWriter writer = File.AppendText(path))
                {
                    writer.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + message);
                }
            }
            catch
            {
            }
        }
        private Object ByteArrayToObject(byte[] arrBytes)
        {
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            Object obj = (Object)binForm.Deserialize(memStream);
            return obj;
        }

    }
}
