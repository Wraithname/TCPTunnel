using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    class Users
    {
        private Thread _userThread;
        private string _userName;
        private Socket _userHandle;
        private bool AuthSuccess = false;
        //Инициализация сокета
        public Users(Socket handle)
        {
            _userHandle = handle;
            _userThread = new Thread(listner);
            _userThread.IsBackground = true;
            _userThread.Start();
        }
        //Получение ника пользователя
        public string Username
        {
            get { return _userName; }
        }
        //Проверка ника ползователя
        private bool setName(string Name)
        {
            _userName = Name;
            for(int i=0;i<Server.UserList.Count();i++)
            {
                if(Server.UserList[i].Username== _userName)
                {
                    AuthSuccess = false;
                    return false;
                }
            }
            Server.NewUser(this);
            AuthSuccess = true;
            return true;
        }
        //Прослушка
        private void listner()
        {
            try
            {
                while (_userHandle.Connected)
                {
                    byte[] buffer = new byte[2048];
                    int bytesReceive = _userHandle.Receive(buffer);

                    handleCommand(Encoding.Unicode.GetString(buffer, 0, bytesReceive));
                }
            }
            catch { Server.EndUser(this); }
        }
        //Вспомогательные команды серверной части
        private void handleCommand(string cmd)
        {
            try
            {
                string[] commands = cmd.Split('#');
                int countCommands = commands.Length;
                for (int i = 0; i < countCommands; i++)
                {
                    string currentCommand = commands[i];
                    if (string.IsNullOrEmpty(currentCommand))
                        continue;
                    if (!AuthSuccess)
                    {
                        if (currentCommand.Contains("setname"))
                        {
                            if (setName(currentCommand.Split('|')[1]))
                                Send("#setnamesuccess");
                            else
                                Send("#setnamefailed");
                        }
                        continue;
                    }
                    if (currentCommand.Contains("endsession"))
                    {
                        Server.EndUser(this);
                        return;
                    }
                    if (currentCommand.Contains("success"))
                    {
                        string id = currentCommand.Split('|')[1];
                        Server.SendFile file = Server.GetFileByID(int.Parse(id));
                        if (file.ID == 0)
                        {
                            continue;
                        }
                        Send(file.fileBuffer);
                        Server.Files.Remove(file);
                    }
                    if (currentCommand.Contains("sendfileto"))
                    {
                        string[] Arguments = currentCommand.Split('|');
                        string TargetName = Arguments[1];
                        int FileSize = Convert.ToInt32(Arguments[2]);
                        string FileName = Arguments[3];
                        string ske= Arguments[4];
                        byte[] fileBuffer = new byte[FileSize];
                        _userHandle.Receive(fileBuffer);
                        Users targetUser = Server.GetUser(TargetName);
                        if (targetUser == null)
                        {
                            //Ошибка что пользователь вышел
                            continue;
                        }
                        Server.SendFile newFile = new Server.SendFile()
                        {
                            ID = Server.Files.Count + 1,
                            FileName = FileName,
                            From = Username,
                            fileBuffer = fileBuffer,
                            FileSize = fileBuffer.Length,
                            sk = ske
                            
                        };
                        Server.Files.Add(newFile);
                        targetUser.SendFile(newFile, targetUser);
                    }
                    if (currentCommand.Contains("faild"))
                    {
                        string id = currentCommand.Split('|')[1];
                        Server.SendFile file = Server.GetFileByID(int.Parse(id));
                        if (file.ID == 0)
                        {
                            //ошибка если файл отсутствует
                            continue;
                        }
                        Server.Files.Remove(file);
                    }
                }
            }
            catch { }
        }
        //Закрытие сокета
        public void End()
        {
            try
            {
                _userHandle.Close();
            }
            catch { }

        }
        //Отправка строки
        public void Send(string Buffer)
        {
            try
            {
                _userHandle.Send(Encoding.Unicode.GetBytes(Buffer));
            }
            catch { }
        }
        public void SendFile(Server.SendFile Buffer, Users To)
        {
            try
            {
                    Console.WriteLine($"Sending {Buffer.FileName} from {Buffer.From} to {To.Username}");
                    To.Send("requesttoresivefile|" + Buffer.FileName + "|" + Buffer.From + "|" + Buffer.ID + "|" + Buffer.FileSize + "|" +Buffer.sk);
            }
            catch { }
        }
        public void Send(byte[] Buffer)
        {
            try
            {
                _userHandle.Send(Buffer);
            }
            catch { }
        }
    }
}
