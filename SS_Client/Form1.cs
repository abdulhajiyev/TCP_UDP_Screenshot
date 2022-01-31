using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SS_Client
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var client = new TcpClient();
            client.Connect(IPAddress.Loopback, 9290);

            var UdpClient = new UdpClient(9590);
            var ep = new IPEndPoint(IPAddress.Loopback, 0);

            for (int i = 0; i < 10; i++)
            {
                var bytes = UdpClient.Receive(ref ep);
                Image img;

                using (var ms = new MemoryStream(bytes))
                {
                    img = Image.FromStream(ms);
                }
                pictureBox1.Image = img;
            }
        }
    }
}
