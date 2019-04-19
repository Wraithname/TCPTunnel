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
        public static List<FileD> Files = new List<FileD>();
        public struct FileD
        {
            public int ID;
            public string FileName;
            public string From;
            public int FileSize;
            public byte[] fileBuffer;
        }
        public static int CountUsers = 0;
        public static List<User> UserList = new List<User>();
        public static Socket ServerSocket;
        public static bool Work = true;
        static void Main(string[] args)
        {
            Console.Write("Введите ip сервера: ");
            string Host = Console.ReadLine();
            Console.Write("Введите порт сервера: ");
            int Port = Convert.ToInt32(Console.ReadLine());
            IPAddress address = IPAddress.Parse(Host);
            ServerSocket = new Socket(address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            ServerSocket.Bind(new IPEndPoint(address, Port));
            ServerSocket.Listen(100);
            Console.WriteLine($"Server has been started on {Host}:{Port}");
            Console.WriteLine("Waiting connections...");
            while (Work)
            {
                Socket handle = ServerSocket.Accept();
                Console.WriteLine($"New connection: {handle.RemoteEndPoint.ToString()}");
                new User(handle);

            }
            Console.WriteLine("Server closeing...");
        }
        public static void NewUser(User usr)
        {
            if (UserList.Contains(usr))
                return;
            UserList.Add(usr);
        }
        public static void EndUser(User usr)
        {
            if (!UserList.Contains(usr))
                return;
            UserList.Remove(usr);
            usr.End();
        }

        public static User GetUser(string Name)
        {
            for (int i = 0; i < CountUsers; i++)
            {
                if (UserList[i].Username == Name)
                    return UserList[i];
            }
            return null;
        }
        public static void SendUserList()
        {
            string userList = "#userlist|";

            for (int i = 0; i < CountUsers; i++)
            {
                userList += UserList[i].Username + ",";
            }

            SendAllUsers(userList);
        }
        public static void SendAllUsers(byte[] data)
        {
            for (int i = 0; i < CountUsers; i++)
            {
                UserList[i].Send(data);
            }
        }
        public static void SendAllUsers(string data)
        {
            for (int i = 0; i < CountUsers; i++)
            {
                UserList[i].Send(data);
            }
        }

    }
}
