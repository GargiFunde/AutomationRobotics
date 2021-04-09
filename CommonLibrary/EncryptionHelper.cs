using System;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace CommonLibrary
{
    public class EncryptionHelper
    {
     

        public EncryptionHelper()
        {

           
        }

        
        private int BlockSize = 128;
        private byte[] IV = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
        public string Decrypt(String password, string passkey)
        {
            //Decrypt
            byte[] bytes = Convert.FromBase64String(password);
            #region oldCode
            //SymmetricAlgorithm crypt = Aes.Create();
            //HashAlgorithm hash = MD5.Create();
            //crypt.Key = hash.ComputeHash(Encoding.Unicode.GetBytes(passkey));
            //crypt.IV = IV;

            //using (MemoryStream memoryStream = new MemoryStream(bytes))
            //{
            //    using (CryptoStream cryptoStream =
            //       new CryptoStream(memoryStream, crypt.CreateDecryptor(), CryptoStreamMode.Read))
            //    {
            //        byte[] decryptedBytes = new byte[bytes.Length];
            //        cryptoStream.Read(decryptedBytes, 0, decryptedBytes.Length);
            //        string decryptedpassword = Encoding.Unicode.GetString(decryptedBytes);
            //        return decryptedpassword;
            //    }
            //} 
            #endregion
            using (SHA256 sha = new SHA256Managed())
            {
                using (SymmetricAlgorithm aes = Aes.Create())
                {
                    aes.Mode = CipherMode.CBC;
                    aes.BlockSize = BlockSize;
                    //aes.KeySize = 256;
                    aes.Key = sha.ComputeHash(Encoding.Unicode.GetBytes(passkey));
                    aes.IV = IV;
                   

                    using (MemoryStream memoryStream = new MemoryStream(bytes))
                    {
                        using (CryptoStream cryptoStream =
                           new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Read))
                        {
                            byte[] decryptedBytes = new byte[bytes.Length];
                            var count = cryptoStream.Read(decryptedBytes, 0, decryptedBytes.Length);
                            string decryptedPassword =  Encoding.Unicode.GetString(decryptedBytes,0,count);

                            return decryptedPassword;
                        }

                        
                    }


                }
            }
        



        }

        public string Encrypt(string plaintxt, string passkey)
        {
            #region oldCode

            //byte[] bytes = Encoding.Unicode.GetBytes(plaintxt);
            ////Encrypt
            //SymmetricAlgorithm crypt = Aes.Create();
            //HashAlgorithm hash = MD5.Create();
            //crypt.BlockSize = BlockSize;
            //crypt.Key = hash.ComputeHash(Encoding.Unicode.GetBytes(passkey));
            //crypt.IV = IV;

            //using (MemoryStream memoryStream = new MemoryStream())
            //{
            //    using (CryptoStream cryptoStream =
            //       new CryptoStream(memoryStream, crypt.CreateEncryptor(), CryptoStreamMode.Write))
            //    {
            //        cryptoStream.Write(bytes, 0, bytes.Length);
            //    }

            //    string encryptedpass = Convert.ToBase64String(memoryStream.ToArray());

            //    return encryptedpass;
            //} 
            #endregion

            string encryptedpass = string.Empty;

            byte[] plaintext = Encoding.Unicode.GetBytes(plaintxt);
            try
            {
                using (SHA256 sha = new SHA256Managed())
                {

                    
                    using (SymmetricAlgorithm aes = Aes.Create())
                    {
                      
                        
                        aes.BlockSize = BlockSize;
                        aes.Mode = CipherMode.CBC;
                        aes.Key = sha.ComputeHash(Encoding.Unicode.GetBytes(passkey));
                        aes.IV = IV;
                       

                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            using (CryptoStream cryptoStream =
                               new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                            {
                                cryptoStream.Write(plaintext, 0, plaintext.Length);
                            }

                              encryptedpass = Convert.ToBase64String(memoryStream.ToArray());

                              return encryptedpass;
                        }


                    }
                }




            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }


        }

        public string GenerateKey() {



            return null;
        }
    }
}
