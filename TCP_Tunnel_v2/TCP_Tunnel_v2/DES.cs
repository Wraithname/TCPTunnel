using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace TCP_Tunnel_v2
{
    public class DES
    {
        public string GenerateKey()
        {
            // Create an instance of Symetric Algorithm. Key and IV is generated automatically.
            DESCryptoServiceProvider desCrypto = (DESCryptoServiceProvider)DESCryptoServiceProvider.Create();
            // Use the Automatically generated key for Encryption. 
            return ASCIIEncoding.ASCII.GetString(desCrypto.Key);

        }
        public void EncryptFile(string source, string destination, string sKey)
        {
            FileStream fsInput = new FileStream(source,
                FileMode.Open,
                FileAccess.Read);

            FileStream fsEncrypted = new FileStream(destination,
                            FileMode.Create,
                            FileAccess.Write);

            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
            DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            ICryptoTransform desencrypt = DES.CreateEncryptor();
            CryptoStream cryptostream = new CryptoStream(fsEncrypted,
                                desencrypt,
                                CryptoStreamMode.Write);
            byte[] bytearrayinput = new byte[fsInput.Length - 1];
            fsInput.Read(bytearrayinput, 0, bytearrayinput.Length);
            cryptostream.Write(bytearrayinput, 0, bytearrayinput.Length);
            cryptostream.Close();
            fsInput.Close();
            fsEncrypted.Close();
        }
        public void DecryptFile(string source, string destination, string sKey)
        {
            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
            //A 64 bit key and IV is required for this provider.
            //Set secret key For DES algorithm.
            DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            //Set initialization vector.
            DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);

            //Create a file stream to read the encrypted file back.
            FileStream fsread = new FileStream(source,
                                           FileMode.Open,
                                           FileAccess.Read);
            //Create a DES decryptor from the DES instance.
            ICryptoTransform desdecrypt = DES.CreateDecryptor();
            //Create crypto stream set to read and do a 
            //DES decryption transform on incoming bytes.
            CryptoStream cryptostreamDecr = new CryptoStream(fsread,
                                                         desdecrypt,
                                                         CryptoStreamMode.Read);
            destination = destination.Substring(0, destination.Length - 4);
            FileStream fsOut = new FileStream(destination, FileMode.Create);
            
            int read;
            byte[] buffer = new byte[1048576];

            try
            {
                while ((read = cryptostreamDecr.Read(buffer, 0, buffer.Length)) > 0)
                {
                    fsOut.Write(buffer, 0, read);
                }
            }
            catch (Exception ex)
            {
            }

            try
            {
                cryptostreamDecr.Close();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                fsOut.Close();
                fsread.Close();
            }
            ////Print the contents of the decrypted file.
            //StreamWriter fsDecrypted = new StreamWriter(destination);
            //fsDecrypted.Write(new StreamReader(cryptostreamDecr).ReadToEnd());
            //fsDecrypted.Flush();
            //fsDecrypted.Close();
        }
    }
}
