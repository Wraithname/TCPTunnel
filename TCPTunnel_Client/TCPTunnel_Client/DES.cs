using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace TCPTunnel_Client
{
    public static class DES
    {
        // Функция генерации 64-битного ключа
        static void GenerateKey(string Keyname)
        {
            // Создаем экземпляр симметричного алгоритма. Ключ и IV генерируются автоматически
             DESCryptoServiceProvider desCrypto =
            (DESCryptoServiceProvider)DESCryptoServiceProvider.Create();
            // Используем автоматически сгенерированный ключ для шифрования
            StreamWriter sw = new StreamWriter(Keyname);
            sw.WriteLine(ASCIIEncoding.ASCII.GetString(desCrypto.Key));
            sw.Close();
        }

        // Функция шифрования
        static void Encrypt(string IFilename, string OFilename, string Key)
        {
            FileStream fsInput = new FileStream(IFilename, FileMode.Open, FileAccess.Read);
            FileStream fsEncrypted = new FileStream(OFilename, FileMode.Create, FileAccess.Write);
            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
            DES.Key = ASCIIEncoding.ASCII.GetBytes(Key);
            DES.IV = ASCIIEncoding.ASCII.GetBytes(Key);
            ICryptoTransform desencrypt = DES.CreateEncryptor();
            CryptoStream cryptostream = new
           CryptoStream(fsEncrypted, desencrypt, CryptoStreamMode.Write);
            byte[] bytearrayinput = new byte[fsInput.Length];
            fsInput.Read(bytearrayinput, 0, bytearrayinput.Length);
            cryptostream.Write(bytearrayinput, 0, bytearrayinput.Length);
            cryptostream.Close();
            fsInput.Close();
            fsEncrypted.Close();

        }
        // Функция дешифрования
        static void Decrypt(string IFilename, string OFilename, string Key)
        {
            //Создать поток файлов, чтобы прочитать зашифрованный файл
            FileStream fsread = new FileStream(IFilename, FileMode.Open, FileAccess.Read);
            FileStream fsDecrypted = new FileStream(OFilename, FileMode.Create, FileAccess.Write);
            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
            //Для этого провайдера требуется 64-битный ключ и IV.
            //Установить секретный ключ для алгоритма DES
            DES.Key = ASCIIEncoding.ASCII.GetBytes(Key);
            //Установить вектор инициализации
            DES.IV = ASCIIEncoding.ASCII.GetBytes(Key);
            //Создать DES-расшифровщик из экземпляра DES
            ICryptoTransform desdecrypt = DES.CreateDecryptor();
            // Создаем криптовый поток, установленный для чтения и выполнения
            // Дешифрование DES для входящих байтов
            CryptoStream cryptostream = new CryptoStream(fsDecrypted, desdecrypt,
           CryptoStreamMode.Write);
            byte[] bytearrayinput = new byte[fsread.Length];
            fsread.Read(bytearrayinput, 0, bytearrayinput.Length);
            cryptostream.Write(bytearrayinput, 0, bytearrayinput.Length);
            cryptostream.Close();
            fsread.Close();
            fsDecrypted.Close();
        }

    }
}
