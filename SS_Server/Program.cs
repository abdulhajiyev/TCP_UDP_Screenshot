using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace SS_Server
{
    class Program
    {
        static void Main(string[] args)
        {
            //var listener = new TcpListener(IPAddress.Loopback, 9290);
            //listener.Start(100);
            //Console.WriteLine("Listening on {0}", listener.LocalEndpoint);

            var ip = IPAddress.Loopback;
            var TcpServer = new TcpListener(ip, 9290);

            Console.WriteLine("Litsening ", ip);

            TcpServer.Start(100);

            UdpClient udpClient = new UdpClient();
            var ep = new IPEndPoint(ip, 9590);

            while (true)
            {
                try
                {
                    udpClient.Connect(ep);
                }
                catch (Exception e)
                {

                    Console.WriteLine(e.Message);
                }

                var client = TcpServer.AcceptTcpClient();
                Console.WriteLine($"{client.Client.RemoteEndPoint} Client Connected");
                for (int i = 0; i < 10; i++)
                {
                    Thread.Sleep(50);
                    using var bitmap = new Bitmap(1920, 1080);
                    var encoder = ImageCodecInfo.GetImageEncoders().First(c => c.FormatID == ImageFormat.Jpeg.Guid);
                    var encParams = new EncoderParameters() { Param = new[] { new EncoderParameter(Encoder.Quality, 10L) } };

                    using (var g = Graphics.FromImage(bitmap))
                    {
                        g.CopyFromScreen(0, 0, 0, 0, bitmap.Size, CopyPixelOperation.SourceCopy);
                    }

                    try
                    {
                        bitmap.Save("ss.jpg", encoder, encParams);
                    }
                    catch (Exception)
                    {
                    }
                    //bitmap.Save("screenshot.jpg", ImageFormat.Jpeg);


                    //Image img = bitmap;
                    Image img = Image.FromFile("ss.jpg");
                    //Console.WriteLine(img.Width);

                    byte[] bytes = (byte[])(new ImageConverter()).ConvertTo(img, typeof(byte[]));



                    udpClient.Send(bytes, bytes.Length);
                }
                udpClient.Dispose();


            }

            //while (true)
            //{
            //    listener.AcceptTcpClient();
            //    Console.WriteLine("Client connected");

            //    using var bitmap = new Bitmap(800, 600);
            //    using (var g = Graphics.FromImage(bitmap))
            //    {
            //        g.CopyFromScreen(0, 0, 0, 0,
            //            bitmap.Size, CopyPixelOperation.SourceCopy);
            //    }
            //    bitmap.Save("screenshot.jpg", ImageFormat.Jpeg);

            //    UdpClient udpClient = new UdpClient();
            //    try
            //    {
            //        udpClient.Connect(IPAddress.Loopback, 9590);

            //        byte[] buffer = File.ReadAllBytes("screenshot.jpg");
            //        udpClient.Send(buffer, buffer.Length);

            //        udpClient.Close();
            //    }
            //    catch (Exception e)
            //    {
            //        Console.WriteLine(e.ToString());
            //    }

            //    //bitmap.Save("screenshot.jpg", ImageFormat.Jpeg);

            //}
        }
    }
}
