using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rabbit
{
    class Get_RabbitMQ
    {
        string track = "";
        public Get_RabbitMQ(string Track)
        {
            this.track = Track;
        }
        public void Get_data(IModel govChannel)
        {
           
            try
            {
                govChannel.QueueDeclare(queue: track,
                                        durable: true,
                                        exclusive: false,
                                        autoDelete: false,
                                        arguments: null);

                var consumer = new EventingBasicConsumer(govChannel);
                govChannel.BasicQos(0, 100, true);
                //consumer.Received += (model, ea) =>
                //{
                //
                //    var body = ea.Body;
                //    var message = Encoding.UTF8.GetString(body);
                //
                //    ThreadPool.QueueUserWorkItem(process, (object)message);
                //
                //};
                do
                {
                    BasicGetResult bgr = govChannel.BasicGet(track, true);

                    var message = Encoding.UTF8.GetString(bgr.Body);
                    // string t = ByteToHexString(bgr.Body);
                    ThreadPool.QueueUserWorkItem(process, (object)message);
                    Thread.Sleep(10);
                }
                while (true);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
        public void process(object data)
        {
            WriteAppendLog(data.ToString(), "D:\\test1.txt");
            Console.WriteLine(" [x] Received {0} " + data.ToString() + "  track: " + track);
            Thread.Sleep(500);
            Console.WriteLine(" Wakeup at " + DateTime.Now);
        }
        public static string ByteToHexString(byte[] data)
        {
            string str = "";
            for (int i = 0; i < data.Length; i++)
            {
                str = str + string.Format("{0:X2}", data[i]);
            }
            return str;
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
    }
}
