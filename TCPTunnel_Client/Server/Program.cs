using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Введите ip сервера: ");
            string Host = Console.ReadLine();
            Console.Write("Введите порт сервера: ");
            int Port = Convert.ToInt32(Console.ReadLine());
            IPAddress address = IPAddress.Parse(Host);
            Server.ServerSocket = new Socket(address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            Server.ServerSocket.Bind(new IPEndPoint(address, Port));
            Server.ServerSocket.Listen(100);
            Console.WriteLine($"Server has been started on {Host}:{Port}");
            Console.WriteLine("Waiting connections...");
            while (Server.Work)
            {
                Socket handle = Server.ServerSocket.Accept();
                Console.WriteLine($"New connection: {handle.RemoteEndPoint.ToString()}");
                new User(handle);

            }
            Console.WriteLine("Server closeing...");
        }

    }
}
