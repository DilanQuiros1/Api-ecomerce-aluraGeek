using System.Security.Cryptography;

namespace ApiProyecto.Modelos
{
    public class Crypto
    {

        //genera la sal aleatoria de 1028 Bytes
        public string createRandomSalt()//BzdPPofOhR
        {
            RNGCryptoServiceProvider rgn = new RNGCryptoServiceProvider();
            byte[] bytes = new byte[64];
            rgn.GetBytes(bytes);

            var hashedInputBytesStringBuilder = new System.Text.StringBuilder(255);//pasa los 1024 bytes a 128 para que entre a la base de datos
            foreach (var b in bytes)
                hashedInputBytesStringBuilder.Append(b.ToString());

            return hashedInputBytesStringBuilder.ToString();
        }

        //create random password
        public string createRamdomPassword(int passwordLenght)
        {
            string _allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Byte[] bytes = new Byte[passwordLenght];
            char[] chars = new char[passwordLenght];
            int allowedCharCount = _allowedChars.Length;

            for (int i = 0; i < passwordLenght; i++)
            {
                Random randon = new Random();
                randon.NextBytes(bytes);
                chars[i] = _allowedChars[(int)bytes[i] % allowedCharCount];
            }
            return new string(chars);
        }

        //IRREVERSIBLE
        public string GetHash(string salt, string contra)
        {
            try
            {
                string input = salt + contra;
                var bytes = System.Text.Encoding.UTF8.GetBytes(input);//lo combierte a bytes porque SHA1 no acepta strings
                using (var hash = System.Security.Cryptography.SHA1.Create())
                {
                    var hashedInputBytes = hash.ComputeHash(bytes);//computehash es el que encripta
                    var hashedInputBytesStringBuilder = new System.Text.StringBuilder(128);
                    foreach (var b in hashedInputBytes)
                        hashedInputBytesStringBuilder.Append(b.ToString());

                    return hashedInputBytesStringBuilder.ToString();
                }
            }
            catch (Exception e)
            {
                throw new Exception("Problemas al generar el hash " + e.Message);
            }

        }

    }
}
