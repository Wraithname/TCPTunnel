using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Server
    {
        public static Socket ServerSocket;
        public static bool Work = true;
        public static List<SendFile> Files = new List<SendFile>();
        public struct SendFile
        {
            public int ID;
            public string FileName;
            public string From;
            public int FileSize;
            public byte[] fileBuffer;
            public string sk;
        }

        public static int CountUsers = 0;
        public static List<Users> UserList = new List<Users>();
        //Запуск сервера
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
                new Users(handle);

            }
            Console.WriteLine("Server closeing...");
        }
        public static SendFile GetFileByID(int ID)
        {
            int countFiles = Files.Count;
            for (int i = 0; i < countFiles; i++)
            {
                if (Files[i].ID == ID)
                    return Files[i];
            }
            return new SendFile() { ID = 0 };
        }

        //Добавление нового пользователя
        public static void NewUser(Users usr)
        {
            if (UserList.Contains(usr))
                return;
            UserList.Add(usr);
            UserConnected(usr.Username);
        }
        //удаление пользователя
        public static void EndUser(Users usr)
        {
            if (!UserList.Contains(usr))
                return;
            UserList.Remove(usr);
            usr.End();
            UserDisconnected(usr.Username);
        }
        //Подключение нового пользователя
        public static void UserDisconnected(string user)
        {
            Console.WriteLine($"User {user} disconnected.");
            CountUsers--;
            SendUserList();
        }
        //Отключение пользователя
        public static void UserConnected(string user)
        {
            Console.WriteLine($"User {user} connected.");
            CountUsers++;
            SendUserList();
        }
        //Отправка всех подключенных клиентов
        public static void SendUserList()
        {
            string userList = "#userlist|";

            for (int i = 0; i < CountUsers; i++)
            {
                userList += UserList[i].Username + ",";
            }

            SendAllUsers(userList);
        }
        //Отправка списка клиентов
        public static void SendAllUsers(string data)
        {
            for (int i = 0; i < CountUsers; i++)
            {
                UserList[i].Send(data);
            }
        }
        //Получение нужного пользователя
        public static Users GetUser(string Name)
        {
            for (int i = 0; i < CountUsers; i++)
            {
                if (UserList[i].Username == Name)
                    return UserList[i];
            }
            return null;
        }

    }
}
