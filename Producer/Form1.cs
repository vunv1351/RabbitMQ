using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Producer
{
    public partial class Form1 : Form
    {
        private ConnectionFactory govConnectionFactory;
        private int Port = 15672;
        private IConnection govConnection;
        private static IModel govChannel;
        private object _syncRoot = new object();
        public Form1()
        {
            InitializeComponent();

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
                        this.govConnectionFactory.VirtualHost = "/";
                        this.govConnectionFactory.Protocol = Protocols.DefaultProtocol;
                        this.govConnectionFactory.UserName = "vunv";
                        this.govConnectionFactory.Password ="vunv@123456" ;
                        this.govConnectionFactory.HostName = txturl.Text;
                        this.govConnectionFactory.Port = Convert.ToInt32(txtport.Text);
                        if ((this.govConnection == null) || !this.govConnection.IsOpen)
                        {
                            try
                            {
                                this.govConnection = this.govConnectionFactory.CreateConnection();
                                govChannel = this.govConnection.CreateModel();
                                MessageBox.Show("connect sucess!");

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

        private void btnsend_Click(object sender, EventArgs e)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(txtstringsend.Text);
            int i = 0;
            while (true)
            {
                System.Threading.ThreadPool.QueueUserWorkItem(PutRabbitMQ, txtstringsend.Text);
               // PutRabbitMQ(bytes);
                i++;
                if (i == 100)
                {
                    i = 1;
                    System.Threading.Thread.Sleep(40);
                }
            }

        }
        private void PutRabbitMQ(object obj)
        {
            try
            {
                govChannel.BasicPublish("gps.government", txttrack.Text, null, Encoding.ASCII.GetBytes((string)obj));
                Console.WriteLine(govConnectionFactory.HostName + ":" + govConnectionFactory.Port + "---" + govConnectionFactory.Protocol + "---" + govConnectionFactory.Password + "-----" + govConnectionFactory.VirtualHost + "----" + govConnectionFactory.UserName);
            }
            catch (Exception ex)
            {
                StartConnectRabbit();
            }





        }
        private void PutRabbitMQ(byte[] bytearr)
        {
            try
            {
                govChannel.BasicPublish("gps.government", txttrack.Text, null, bytearr);
                Console.WriteLine(govConnectionFactory.HostName + ":" + govConnectionFactory.Port + "---" + govConnectionFactory.Protocol + "---" + govConnectionFactory.Password + "-----" + govConnectionFactory.VirtualHost + "----" + govConnectionFactory.UserName);
            }
            catch (Exception ex)
            {
                StartConnectRabbit();
            }





        }

        private void btnconnect_Click(object sender, EventArgs e)
        {
            StartConnectRabbit();

        }
    }
}
