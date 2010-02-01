using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace ManxAds
{
    internal class Password
    {
        private const int generateLength = 10;
        private const string charString = "abcdefghijkmnopqrstuvwxyz0123456789ABCDEFGHJKLMNOPQRSTUVWXYZ";

        private string decrypted;

        /// <summary>
        /// Gets a one-way encryption of the password.
        /// </summary>
        public string Encrypted
        {
            get
            {
                if (String.IsNullOrEmpty(decrypted))
                {
                    return null;
                }

                SHA1 sha1 = new SHA1Managed();
                byte[] decryptHash = Encoding.ASCII.GetBytes(decrypted);
                byte[] encryptHash = sha1.ComputeHash(decryptHash);
                string encrypted = BitConverter.ToString(encryptHash);
                return encrypted.Replace("-", String.Empty).ToLower();
            }
        }

        /// <summary>
        /// Gets or sets the password before encryption.
        /// </summary>
        public string Decrypted
        {
            get { return decrypted; }
            set { decrypted = value; }
        }

        public Password(string decrypted)
        {
            this.decrypted = decrypted;
        }

        public static string Generate()
        {
            Random random = new Random(Environment.TickCount);
            char[] output = new char[generateLength];
            char[] characters = charString.ToCharArray();

            for (int i = 0; i < generateLength; i++)
            {
                output[i] = characters[random.Next(0, characters.Length - 1)];
            }

            return new string(output);
        }

        public static implicit operator Password(string password)
        {
            return new Password(password);
        }
    }
}
