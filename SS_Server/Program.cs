using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Sockets;


namespace SS_Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var listener = new TcpListener(IPAddress.Loopback, 9290);

            listener.Start(100);
            Console.WriteLine("Listening on {0}", listener.LocalEndpoint);

            while (true)
            {
                listener.AcceptTcpClient();
                Console.WriteLine("Client connected");

                using var bitmap = new Bitmap(1920, 1080);
                using (var g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(0, 0, 0, 0,
                        bitmap.Size, CopyPixelOperation.SourceCopy);
                }
                bitmap.Save("screenshot.jpg", ImageFormat.Jpeg);

                UdpClient udpClient = new UdpClient();
                try
                {
                    udpClient.Connect(IPAddress.Loopback, 9590);

                    // Sends a message to the host to which you have connected.

                    byte[] buffer = File.ReadAllBytes("screenshot.jpg");
                    udpClient.Send(buffer, buffer.Length);

                    udpClient.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }

                //bitmap.Save("screenshot.jpg", ImageFormat.Jpeg);

            }
        }
    }
}
