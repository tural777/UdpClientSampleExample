using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace UdpClientSampleExample
{
    class Udp
    {
        static int RemotePort { get; set; }
        static int LocalPort { get; set; }
        static IPAddress RemoteIPAddress { get; set; }

        static void Main(string[] args)
        {
            Console.SetWindowSize(60, 25);
            Console.Title = "BabatChat";
            Console.WriteLine("Enter Remote IP : ");
            RemoteIPAddress = IPAddress.Parse(Console.ReadLine());
            Console.WriteLine("Enter Remote Port : ");
            RemotePort = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter Local Port : ");
            LocalPort = int.Parse(Console.ReadLine());

            Task.Factory.StartNew(() => { Listener(); }, TaskCreationOptions.LongRunning);


            Console.ForegroundColor = ConsoleColor.Red;

            while (true)
            {
                Client(Console.ReadLine());
            }
        }

        static void Listener()
        {
            try
            {
                while (true)
                {
                    UdpClient udpClient = new UdpClient(LocalPort);
                    IPEndPoint ep = null;

                    var response = udpClient.Receive(ref ep);
                    var data = Encoding.UTF8.GetString(response);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"{ep.Address} : {data}");
                    Console.ForegroundColor = ConsoleColor.Red;
                    udpClient.Close();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        static void Client(string data)
        {
            using (UdpClient client = new UdpClient())
            {
                try
                {
                    IPEndPoint ep = new IPEndPoint(RemoteIPAddress, RemotePort);
                    var bytes = Encoding.UTF8.GetBytes(data);
                    client.Send(bytes, bytes.Length, ep);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
