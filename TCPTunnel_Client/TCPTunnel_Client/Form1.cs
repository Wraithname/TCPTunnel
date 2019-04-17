using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace TCPTunnel_Client
{
    public partial class Settings : Form
    {
        private Socket _serverSocket;
        private string _host ;
        private int _port ;
        public Settings()
        {
            InitializeComponent();
        }
        private void Connect_Click(object sender, EventArgs e)
        {
            _host = textBox2.Text.ToString();
            _port = Convert.ToInt32(textBox1.Text);
            IPAddress temp = IPAddress.Parse(_host);
            _serverSocket = new Socket(temp.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _serverSocket.Connect(new IPEndPoint(temp, _port));
            if (_serverSocket.Connected)
            {
                TCP_Tunnel work = new TCP_Tunnel();
                MessageBox.Show("Связь с сервером установлена.", "Успешно",MessageBoxButtons.OK,MessageBoxIcon.Information);
                this.Hide();
                work._port = _port;
                work._host = _host;
                work.Show();
            }
            else
                MessageBox.Show("Связь с сервером не установлена.", "Ошибка подключения", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
