using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MessageinRBMQ
{
    public class CheckQueue
    {

        public void CheckListRabbit(object obj)
        {
           // Console.WriteLine("ahihi");
            var ListObject = new List<RabbitMQ>();
            try
            {
                string queuesUrl = "http://ec2-13-229-226-59.ap-southeast-1.compute.amazonaws.com:15672/api/queues/%2F";
                WebClient webClient = new WebClient { Credentials = new NetworkCredential("vunv", "vunv@123456") };
                var response = webClient.DownloadString(queuesUrl);
                ListObject = Newtonsoft.Json.JsonConvert.DeserializeObject<List<RabbitMQ>>(response);
                ProcessCheck(ListObject);
                //ThreadPool.QueueUserWorkItem(ProcessCheck, ListObject);
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
        private void ProcessCheck(object obj)
        {
            List<RabbitMQ> listrabbit = (List<RabbitMQ>)obj;
            foreach (RabbitMQ item in listrabbit)
            {
                if (item.Backing_Queue_Status.Len >= 2000)
                {
                    Console.WriteLine("node : " + item.Node + "\t name : " + item.Name + "\tlen : " + item.Backing_Queue_Status.Len);
                }
                Console.WriteLine("node : " + item.Node + "\t name : " + item.Name + "\tlen : " + item.Backing_Queue_Status.Len);
            }
            Console.ReadLine();

        }
        System.Threading.Timer TimmerStart;
        public void Start()
        {
            System.Threading.TimerCallback callback = new System.Threading.TimerCallback(CheckListRabbit);
            TimmerStart = new System.Threading.Timer(callback, null, 10, 1000);
        }
    }
}
