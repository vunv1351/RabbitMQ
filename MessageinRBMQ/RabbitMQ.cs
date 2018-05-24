using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageinRBMQ
{
    class RabbitMQ
    {

        public string Node { get; set; }
        public string Name { get; set; }
        public Backing_Queue_Status Backing_Queue_Status;
    }
}
