using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace TCPTunnel_Client
{
    public partial class TCP_Tunnel : Form
    {
        private Socket _serverSocket;
        private Thread listenThread;
        public string _host;
        public int _port;
        public TCP_Tunnel()
        {
            InitializeComponent();
        }

        private void TCP_Tunnel_Load(object sender, EventArgs e)
        {
            IPAddress temp = IPAddress.Parse(_host);
            _serverSocket = new Socket(temp.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _serverSocket.Connect(new IPEndPoint(temp, _port));
            listenThread = new Thread(listner);
            listenThread.IsBackground = true;
            listenThread.Start();
        }
        public void Send(byte[] buffer)
        {
            try
            {
                _serverSocket.Send(buffer);
            }
            catch { }
        }
        public void Send(string Buffer)
        {
            try
            {
                _serverSocket.Send(Encoding.Unicode.GetBytes(Buffer));
            }
            catch { }
        }
        public void handleCommand(string cmd)
        {
            string[] commands = cmd.Split('#');
            int countCommands = commands.Length;
            for (int i = 0; i < countCommands; i++)
            {
                try
                {
                    string currentCommand = commands[i];
                    if (string.IsNullOrEmpty(currentCommand))
                        continue;
                    if (currentCommand.Contains("setnamefailed"))
                    {
                        MessageBox.Show("Неверный ник!","Ошибка",MessageBoxButtons.OK,MessageBoxIcon.Error);
                        continue;
                    }
                    if (currentCommand.Contains("userlist"))
                    {
                        string[] Users = currentCommand.Split('|')[1].Split(',');
                        int countUsers = Users.Length;
                        userList.Invoke((MethodInvoker)delegate { userList.Items.Clear(); });
                        for (int j = 0; j < countUsers; j++)
                        {
                            userList.Invoke((MethodInvoker)delegate { userList.Items.Add(Users[j]); });
                        }
                        continue;
                    }
                    if (currentCommand.Contains("gfile"))
                    {
                        string[] Arguments = currentCommand.Split('|');
                        string fileName = Arguments[1];
                        string FromName = Arguments[2];
                        string FileSize = Arguments[3];
                        string idFile = Arguments[4];
                        DialogResult Result = MessageBox.Show($"Вы хотите принять файл {fileName} размером {FileSize} от {FromName}", "Файл", MessageBoxButtons.YesNo);
                        if (Result == DialogResult.Yes)
                        {
                            Thread.Sleep(1000);
                            Send("#yy|" + idFile);
                            byte[] fileBuffer = new byte[int.Parse(FileSize)];
                            _serverSocket.Receive(fileBuffer);
                            File.WriteAllBytes(fileName, fileBuffer);
                            MessageBox.Show($"Файл {fileName} принят.");
                        }
                        else
                            Send("nn");
                        continue;
                    }

                }
                catch (Exception exp) { Console.WriteLine("Error with handleCommand: " + exp.Message); }
            }
        }
        public void listner()
        {
            try
            {
                while (_serverSocket.Connected)
                {
                    byte[] buffer = new byte[2048];
                    int bytesReceive = _serverSocket.Receive(buffer);
                    handleCommand(Encoding.Unicode.GetString(buffer, 0, bytesReceive));
                }
            }
            catch
            {
                MessageBox.Show("Связь с сервером прервана");
                Application.Exit();
            }
        }
        private void TCP_Tunnel_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_serverSocket.Connected)
                Send("#endsession");
        }

        private void userList_Click(object sender, EventArgs e)
        {
            label5.Visible = true;
            label5.Text = sender.ToString();
            File_Show.Visible = true;
            Sending.Visible = true;
        }

        private void File_Show_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofp = new OpenFileDialog();
            ofp.ShowDialog();
            if (!File.Exists(ofp.FileName))
            {
                MessageBox.Show($"Файл {ofp.FileName} не найден!");
                return;
            }
            FileInfo fi = new FileInfo(ofp.FileName);
            byte[] buffer = File.ReadAllBytes(ofp.FileName);
            Send(buffer);
        }

        private void Sending_Click(object sender, EventArgs e)
        {

        }
    }
}
