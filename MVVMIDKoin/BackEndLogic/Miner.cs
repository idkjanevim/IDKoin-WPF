
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace IDKoin.Properties
{
    public class Miner
    {
        static Random random = new Random();
        bool mining = false;
        Thread thread = new Thread(Mining);
        static MongoHandler mongoHandler = new MongoHandler();
        static string username;
        static Label _label;
        static int balance;
        public Miner()
        {

        }



        public static string generateHash()
        {
            return hash(RandomString(random.Next(5, 50)));
        }
        private static String hash(String value)
        {
            StringBuilder Sb = new StringBuilder();

            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                Byte[] result = hash.ComputeHash(enc.GetBytes(value));

                foreach (Byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }
        private static string RandomString(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] uintBuffer = new byte[sizeof(uint)];

                while (length-- > 0)
                {
                    rng.GetBytes(uintBuffer);
                    uint num = BitConverter.ToUInt32(uintBuffer, 0);
                    res.Append(valid[(int)(num % (uint)valid.Length)]);
                }
            }

            return res.ToString();
        }
        public void ToggleMine(string usr, Label label)
        {
            username = usr;

            _label = label;
            if (mining == false)
            {
                mining = true;
                try
                {
                    thread.Start();
                }
                catch (Exception)
                {
                    thread.Resume();
                }
            }
            else
            {
                mining = false;
                thread.Suspend();
            }
        }
        private static void Mining()
        {

            while (true)
            {

                string hash = generateHash();
                if (hash == null)
                    return;
                
                if (hash[0] == "6"[0] && hash[1] == "9"[0] && hash[2] == "6"[0] && hash[3] == "9"[0] && hash[4] == "0"[0])
                {
                    mongoHandler.AddKoins(username);
                    _label.Dispatcher.Invoke(new System.Action(() =>
                    {
                        Reset_Bal(mongoHandler.Balance(username));
                    }));
                }
            }
        }
        private static void Reset_Bal(int bal)
        {
            balance = bal;

            _label.Content = "Balance: " + balance;
        }
    }
}
