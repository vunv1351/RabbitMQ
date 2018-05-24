using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageinRBMQ
{
    class Program
    {
        static void Main(string[] args)
        {
            
            CheckQueue checkQueue = new CheckQueue();
            // System.Threading.Thread t = new System.Threading.Thread();          
            // t.Start();
            checkQueue.Start();
            Console.ReadLine();
            //checkQueue.CheckListRabbit(null);
        }
    }
}
