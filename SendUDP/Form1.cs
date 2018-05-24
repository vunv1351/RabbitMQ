using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SendUDP
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void send()
        {
            UdpClient udpClient = new UdpClient();
            udpClient.Connect("192.168.107.105", 8080);
            Byte[] senddata = Encoding.ASCII.GetBytes(txtsend.Text);
            udpClient.Send(senddata, senddata.Length);
        }

        private void btnsend_Click(object sender, EventArgs e)
        {
            send();
        }

        private void txtsend_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
