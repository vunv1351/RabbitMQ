using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.MessagePatterns;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rabbit
{
    class Program
    {
        public static RabbitMQ put_get_Rabbit = new RabbitMQ();
        public static Dictionary<string, string> Tracklist;
        static void Main(string[] args)
        {
          

            // RabbitMQ put_get_Rabbit = new RabbitMQ();
            //string strget = "track1,track2,track3,track4,track5,track6,track7,track8,track9,track10,track11,track12,dangkiem";
            //string strput = "track1,track2,track3,track4,track5,track6,track7,track8,track9,track10,dangkiem";
            //for (int i = 0; i < strget.Length; i++)
            //{
            //    string strgettrack = strget[i].ToString();
            //    if (i < strput.Length - 1)
            //    {

            //        for ()
            //        {

            //        }
            //    }
            //}
            System.Threading.Timer TimmerStart;
            System.Threading.Timer TimmerStartCountMessage;
            
            Program p = new Program();
            Tracklist = p.TrackListS();

            try
            {
                put_get_Rabbit.StartConnect();
                Console.WriteLine("connect ok!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            // get 
            //put_get_Rabbit.getdatatest(null);
            // p.StartTimercallbackgetrabbit();
            // Console.ReadLine();
            //PUT
            //t tracknum = 0;
            string data1 = "BGTVT,0,1522911593,43A21174,7E08C175500104E0,,0,108.231391666667,16.0751416666667,0,290,0,0,100";
            //while (true)
            //{ put_get_Rabbit.PutRabbitMQ(Encoding.ASCII.GetBytes(data1)); }
            var fileStream = new FileStream(@"C:\Users\vNet\Desktop\abbb.txt", FileMode.Open, FileAccess.Read);
            int lines = File.ReadAllLines(@"C:\Users\vNet\Desktop\abbb.txt").Length;
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    if (line.Length >= 20)
                    {

                        if (line.Contains("insert") || line.Contains("insert:") || line.Contains("insert :") || line.Contains("insert :"))
                        {
                            string[] b = line.Split(new string[] { "'TCDB','BGTVT','" }, StringSplitOptions.None);
                            string[] b2 = b[1].Split(new string[] { "'," }, StringSplitOptions.None);
                            if (b2.Length > 1 && (b2[0] != "" || b2[0] != null))
                            {
                                string data = b2[0].ToString();
                               // byte[] bytes = Encoding.ASCII.GetBytes(data);
                               // string Verhiclefinal = p.Verhiclefinal(data);
                               // p.PutRabbitMQCus(Verhiclefinal, data);
                                p.PutRabbitMQCus(data);
                            }
                        }
                        else
                        {
                            string data2 = line.Substring(20);
                            //byte[] bytes = Encoding.ASCII.GetBytes(data2);
                            //string Verhiclefinal = p.Verhiclefinal(data2);
                            //p.PutRabbitMQCus(Verhiclefinal, data2);
                            p.PutRabbitMQCus(data2);
                        }

                        //Thread.Sleep(1);
                    }
                }
            }

        }
        private System.Timers.Timer getdatarabbit;
        private void StartTimercallbackgetrabbit()
        {
            //  Timer timer = new Timer(new TimerCallback(put_get_Rabbit.getdatatest), null, 0, 5000);
            this.getdatarabbit = new System.Timers.Timer(5 * 1000.0); // 20 seconds
            this.getdatarabbit.Elapsed += new System.Timers.ElapsedEventHandler(this.GetDataRabbit);
            this.getdatarabbit.AutoReset = true;
            this.getdatarabbit.Enabled = true;
        }
        
        private void GetDataRabbit(object sender, System.Timers.ElapsedEventArgs e)
        {
            //put_get_Rabbit.ReceiveHeadersMessageReceiverTwo();
            put_get_Rabbit.getdatatest(null);
            // put_get_Rabbit.get(null);
        }
        private static void GetData(object obj)
        {
            // put_get_Rabbit.get((string)obj);
        }
        private static void getData(object obj)
        {

            List<string> data = new List<string>();
            //data = put_get_Rabbit.getL();
            //foreach (string d in data)
            //{
            //    Console.WriteLine(d);
            // 
            //
            //}
        }
        int numbertrack = 15;
        public Dictionary<string, string> TrackListS()
        {
            var dic = new Dictionary<string, string>();
            int phannguyen, phandu;
            phannguyen = (100-numbertrack) / numbertrack;
            phandu = (100 - numbertrack) - phannguyen * numbertrack;
            Console.WriteLine("phan nguyen:  {0}, phan du:  {1}", phannguyen, phandu);
            int k = 0;
            for (int i = 0; i <= numbertrack; i++)
            {
                k++;
                if (k < numbertrack + 1)
                {
                    string track = "track" + k;
                    string grouptrack = i.ToString();
                    if (grouptrack.Length == 1)
                    {
                        grouptrack = string.Format("{0:00}", i);
                    }
                    grouptrack = grouptrack + ",";
                    dic.Add(track,grouptrack.Substring(0, grouptrack.Length - 1));
                }
            }
            int k2 = 0;
            for (int j=numbertrack;j<100;j+= phannguyen)
            {
                k2++;
                if (k2 <= numbertrack)
                {
                    string track = "track" + k2;
                    string grouptrack = "";

                    for (int x = 0; x < phannguyen; x++)
                    {
                        if (dic.ContainsKey(track))
                        {
                            dic[track] = dic[track] + "," + (j + x).ToString();
                        }
                        else
                        {
                            Console.Write("Loi track");
                        }
                      
                    }
                }
                else
                {
                    int track2 = 0;
                    for (int ji = 0; ji < phandu; ji++)
                    {
                        track2++;
                        string trackj = "track" + track2;
                        if (dic.ContainsKey(trackj))
                        {
                            dic[trackj] = dic[trackj] + "," + ( j+ ji).ToString();
                        }
                        else
                        {
                            Console.Write("Loi track du");
                        }

                        Console.WriteLine("track {0} so {1} ", track2, j+ji);
                    }
                    return dic;
                }
            }
            return dic;
        }
        private void PutRabbitMQ(string grouptr, string bytes)
        {

            byte[] data = Encoding.ASCII.GetBytes(bytes);
            string[] str = bytes.Split(',');
            // Log.WriteData(str[1]+"\t"+ bytes);
            if ((str[1] != null && str[1] == "1") || (str[1] != "" && str[1] == "1") || (str[1] != "" && str[1].Equals("1")) || (str[1] != null && str[1].Equals("1")))
            {
                put_get_Rabbit.PutRabbitMQ1(data, "dangkiem");
                // Log.WriteData("put  dangkiem: " + str[1] + "\t" + bytes);
                //  return;
            }
            else
            {

                //Log.WriteData("put  track : " + numbertrack.ToString() + "\t" + str[1] + "\t" + bytes);
                switch (grouptr)
                {

                    case "1":
                        put_get_Rabbit.PutRabbitMQ1(data, "track" + grouptr.ToString());
                        break;
                    case "2":
                        put_get_Rabbit.PutRabbitMQ1(data, "track" + grouptr.ToString());
                        break;
                    case "3":
                        put_get_Rabbit.PutRabbitMQ1(data, "track" + grouptr.ToString());
                        break;
                    case "4":
                        put_get_Rabbit.PutRabbitMQ1(data, "track" + grouptr.ToString());
                        break;
                    case "5":
                        put_get_Rabbit.PutRabbitMQ1(data, "track" + grouptr.ToString());
                        break;
                    case "6":
                        put_get_Rabbit.PutRabbitMQ1(data, "track" + grouptr.ToString());
                        break;
                    case "7":
                        put_get_Rabbit.PutRabbitMQ1(data, "track" + grouptr.ToString());
                        break;
                    case "8":
                        put_get_Rabbit.PutRabbitMQ1(data, "track" + grouptr.ToString());
                        break;
                    case "9":
                        put_get_Rabbit.PutRabbitMQ1(data, "track" + grouptr.ToString());
                        break;
                    case "0":
                        put_get_Rabbit.PutRabbitMQ1(data, "track10");
                        break;
                    default:
                        put_get_Rabbit.PutRabbitMQ1(data, "track1");
                        break;
                }
            }

        }
        private void PutRabbitMQCus(string bytes)
        {


            string[] str = bytes.Split(',');
            if (str.Length < 15)
            {
                this.PutRB(bytes);
            }
            else
            {
                
                string[] t = bytes.Split(new string[] { "BGTVT" }, StringSplitOptions.None);
                string data2 = "";
                for (int i = 1; i < t.Length; i++)
                {
                    string dt = "BGTVT" + t[i];
                    this.PutRB(dt);
                }

            }


        }
        private void PutRB(string datas)
        {
            string Verhiclefinal = this.Verhiclefinal(datas);
            byte[] data = Encoding.ASCII.GetBytes(datas);
            string[] str = datas.Split(',');
            if (str.Length < 4)
                return;
            if (str[3].Length > 14)
            {
               Console.WriteLine(str[3].ToString());
                return;
            }
            if ((str[1] != null && str[1] == "1") || (str[1] != "" && str[1] == "1") || (str[1] != "" && str[1].Equals("1")) || (str[1] != null && str[1].Equals("1")))
            {
                put_get_Rabbit.PutRabbitMQ1(data, "dangkiem");
                // Log.WriteData("put  dangkiem: " + str[1] + "\t" + bytes);
                //  return;
            }
            else
            {
                int Countcontains = 0;
                foreach (KeyValuePair<string, string> pair in Tracklist)
                {
                    if (pair.Value.Contains(Verhiclefinal))
                    {
                        Countcontains++;
                        put_get_Rabbit.PutRabbitMQ1(data, pair.Key);
                    }
                }
                if (Countcontains == 0)
                {

                    if (str.Length > 14)
                        put_get_Rabbit.PutRabbitMQ1(data, "track10");
                    else
                        Console.WriteLine(str[3].ToString());


                    //_rabbitMQ_Vnet.PutDataRabbuitCusto(data, "track10");
                }

                Countcontains = 0;


                //   this.TrackList();

            }
        }
        private static void getData()
        {
            var connectionFactory = new ConnectionFactory
            {
                HostName = "ec2-13-229-239-65.ap-southeast-1.compute.amazonaws.com",
                Port = 5672,
                UserName = "vunv",
                Password = "vunv@123456",
                Protocol = Protocols.AMQP_0_9_1,
                RequestedFrameMax = UInt32.MaxValue,
                RequestedHeartbeat = UInt16.MaxValue
            };

            using (var connection = connectionFactory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                Dictionary<string, object> args = new Dictionary<string, object>()
{
    { "x-queue-mode", "lazy" }
};
                // This instructs the channel not to prefetch more than one message
                channel.BasicQos(0, 100, false);

                // Create a new, durable exchange
                // channel.ExchangeDeclare("gov.track0", ExchangeType.Direct, true, false, null);
                // Create a new, durable queue
                channel.QueueDeclare("gov.track0", true, false, false, args);
                // Bind the queue to the exchange
                //channel.QueueBind("sample-queue", "sample-ex", "optional-routing-key");

                using (var subscription = new Subscription(channel, "gov.track0", true))
                {
                    Console.WriteLine("Waiting for messages...");
                    var encoding = new UTF8Encoding();
                    while (channel.IsOpen)
                    {
                        BasicDeliverEventArgs eventArgs;
                        var success = subscription.Next(2000, out eventArgs);
                        if (success == false) continue;
                        var msgBytes = eventArgs.Body;
                        var message = encoding.GetString(msgBytes);
                        Console.WriteLine(message);
                        channel.BasicAck(eventArgs.DeliveryTag, false);
                    }
                }
            }
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
        public string Verhiclefinal(string msg)
        {
            string str = "";
            try
            {
                string[] list = msg.Split(',');
                str = list[3].ToString().Substring(list[3].ToString().Length - 2, 2);
            }
            catch (Exception ex)
            {
                // Log.WriteErrorLog(ex.Message + "\n" + ex.StackTrace.ToString());
            }

            return str;
        }
    }
}
