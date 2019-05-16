using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace TCP_Tunnel_v2
{
    public partial class Tunnel : Form
    {
        private string _host;
        private int _port;
        private string nickname;
        private Socket _userSocket;
        private Thread listenThread;
        AES gt = new AES();
        DES dt = new DES();

        public Tunnel()
        {
            InitializeComponent();

        }

        //Подключение к серверу
        private void Connect_Click(object sender, EventArgs e)
        {
            _host = textBox1.Text;
            _port = Convert.ToInt32(textBox2.Text);
            if (string.IsNullOrEmpty(textBox3.Text))
            {
                MessageBox.Show("Введите ник", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
                nickname = textBox3.Text;
            IPAddress temp = IPAddress.Parse(_host);
            _userSocket = new Socket(temp.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _userSocket.Connect(new IPEndPoint(temp, _port));
            if (_userSocket.Connected)
            {
                MessageBox.Show("Связь с сервером установлена.", "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);
                listenThread = new Thread(listner);
                listenThread.IsBackground = true;
                listenThread.Start();
                Send($"#setname|{nickname}");
            }
            else
                MessageBox.Show("Связь с сервером не установлена.", "Ошибка подключения", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        //Прослушка
        public void listner()
        {
            try
            {
                while (_userSocket.Connected)
                {
                    byte[] buffer = new byte[2048];
                    int bytesReceive = _userSocket.Receive(buffer);
                    handleCommand(Encoding.Unicode.GetString(buffer, 0, bytesReceive));
                }
            }
            catch
            {
                MessageBox.Show("Связь с сервером прервана", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }

        //!!!!Вспомогательные команды на клиентской части
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
                    if (currentCommand.Contains("setnamesuccess"))
                    {
                        MessageBox.Show("Пользователь " + nickname + " подключен. Доступ разрешен", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Invoke((MethodInvoker)delegate
                        {
                            Connect.Enabled = false;
                            textBox1.ReadOnly = true;
                            textBox2.ReadOnly = true;
                            textBox3.ReadOnly = true;
                        });

                        continue;
                    }
                    if (currentCommand.Contains("setnamefailed"))
                    {
                        MessageBox.Show("Ник уже существует. Доступ запрещен.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    if (currentCommand.Contains("requesttoresivefile"))
                    {
                        string[] Arguments = currentCommand.Split('|');
                        string fileName = Arguments[1];
                        string From = Arguments[2];
                        string id = Arguments[3];
                        string sk= Arguments[5];
                        int FileSize = Convert.ToInt32(Arguments[4]);
                        DialogResult Result = MessageBox.Show($"Вы хотите принять файл {fileName} размером {FileSize} от {From}", "Файл", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                        if (Result == DialogResult.Yes)
                        {
                            string path = "";
                            Invoke((MethodInvoker)delegate
                            {
                                FolderBrowserDialog DirDialog = new FolderBrowserDialog();
                                DirDialog.Description = "Выбор директории";
                                DirDialog.SelectedPath = @"C:\";

                                if (DirDialog.ShowDialog() == DialogResult.OK)
                                {
                                    path = DirDialog.SelectedPath;
                                }
                            });
                            if(sk =="")
                            {
                                Thread.Sleep(1000);
                                Send("#success|" + id);
                                byte[] fileBuffer = new byte[FileSize];
                                _userSocket.Receive(fileBuffer);
                                File.WriteAllBytes(path + "\\" + fileName, fileBuffer);
                                gt.Decrypt(path + "\\" + fileName, "Network");
                                MessageBox.Show($"Файл {fileName} принят.");
                                File.Delete(path + "\\" + fileName);
                            }
                            else
                            {
                                Thread.Sleep(1000);
                                Send("#success|" + id);
                                byte[] fileBuffer = new byte[FileSize];
                                _userSocket.Receive(fileBuffer);
                                File.WriteAllBytes(path + "\\" + fileName, fileBuffer);
                                dt.DecryptFile(path + "\\" + fileName, path + "\\" + fileName, sk);
                                MessageBox.Show($"Файл {fileName} принят.");
                                File.Delete(path + "\\" + fileName);
                            } 
                        }
                        else
                            Send("#faild|" + id);
                        continue;
                    }
                }
                catch (Exception exp) { MessageBox.Show("Ошибка с командой: " + exp.Message + ".", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
        }
        //Отправка строки
        public void Send(string Buffer)
        {
            try
            {
                _userSocket.Send(Encoding.Unicode.GetBytes(Buffer));
            }
            catch { }
        }
        //закрытие формы
        private void TCP_Tunnel_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_userSocket.Connected)
                Send("#endsession");
            Application.Exit();
        }
        //Выбор файла
        private void FileChoose_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                label7.Text = dlg.SafeFileName;
                SendFileName = dlg.FileName;
            }
        }
        public string SendFileNameAes = null;
        public string SendFileNameDes = null;
        public string SendFileName = null;
        //Событие кнопки отправления запроса
        private void Sending_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(label6.Text) != true && String.IsNullOrEmpty(label7.Text) != true)
            {
                if (radioButton1.Checked == true)
                {
                    string sKey = dt.GenerateKey();
                    SendFileNameDes = SendFileName + ".des";
                    dt.EncryptFile(SendFileName, SendFileNameDes, sKey);
                    byte[] buffer = File.ReadAllBytes(SendFileNameDes);
                    string SendFileNameDesforDel = SendFileNameDes;
                    SendFileNameDes = label7.Text + ".des";
                    Send($"#sendfileto|{userList.SelectedItem}|{buffer.Length}|{SendFileNameDes}|{sKey}");//g 
                    Send(buffer);
                    File.Delete(SendFileNameDesforDel);
                }
                if (radioButton2.Checked == true)
                {
                    gt.Encrypt(SendFileName, "Network");
                    SendFileNameAes = SendFileName + ".aes";
                    byte[] buffer = File.ReadAllBytes(SendFileNameAes);
                    string SendFileNameAesforDel = SendFileNameAes;
                    SendFileNameAes = label7.Text + ".aes";
                    Send($"#sendfileto|{userList.SelectedItem}|{buffer.Length}|{SendFileNameAes}");//g 
                    Send(buffer);
                    File.Delete(SendFileNameAesforDel);
                }
            }
            else
            {
                MessageBox.Show("Выберете пользователя и файл", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void userList_Click(object sender, EventArgs e)
        {
            label6.Text = userList.SelectedItem.ToString();
        }
        public void Send(byte[] Buffer)
        {
            try
            {
                _userSocket.Send(Buffer);
            }
            catch { }
        }
    }
}
