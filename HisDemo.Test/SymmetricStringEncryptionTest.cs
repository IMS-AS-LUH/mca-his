using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using HisDemo.Crypt;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HisDemo.Test
{
    [TestClass]
    public class SymmetricStringEncryptionTest
    {
        [TestMethod]
        public void TestBounceBack()
        {
            string a = "Hello there this is an awesome text!";
            Random rnd = new Random(7918);
            byte[] key = new byte[32];
            rnd.NextBytes(key);

            SymmetricStringEncryption dut = new SymmetricStringEncryption(key);

            string b = dut.Encrypt(a);
            string c = dut.Decrypt(b);
            Assert.AreEqual(a, c);
        }

        [TestMethod]
        public void MultRandomBounce()
        {
            char[] pool = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789áéíóúàèìòùöäüÖÄÜ#*~+'-_.:,;0=}][{³²!\"§$%&/()=?`´\\".ToCharArray();

            SymmetricStringEncryption dut = new SymmetricStringEncryption();
            Random rnd = new Random(869594);
            string compare = "foo";
            foreach (int keySize in new int[] { 16, 24, 32 })
            {
                byte[] key = new byte[keySize];
                rnd.NextBytes(key);
                char[] rndString = new char[rnd.Next(4000)];
                for(int i = 0; i < rndString.Length; i++)
                {
                    rndString[i] = pool[rnd.Next(pool.Length)];
                }
                string a = new string(rndString);

                string b = dut.Encrypt(key, a);
                string c = dut.Decrypt(key, b);
                Assert.AreEqual(a, c);
                Assert.AreNotEqual(b, compare);
                compare = b;
            }
        }

        [TestMethod]
        public void OpenSSHCompare()
        {
            // Generate the data:
            // printf "%s" "----------------This is a quick brown fox jumping around in circles. E=m*c²" 
            //  | openssl enc -aes-128-cbc -K e839ffffe68e3616d4a3cbb2aafad0d1 -iv 4d92e772210c13bc4b9dc0878038a7ee | base64
            byte[] key = {
                0xe8, 0x39, 0xff, 0xff, 0xe6, 0x8e, 0x36, 0x16, 0xd4, 0xa3, 0xcb, 0xb2, 0xaa, 0xfa, 0xd0, 0xd1,
            };
            string a = "This is a quick brown fox jumping around in circles. E=m*c²";
            string b = "XQUUYhcggNlfgsdwYu+Tzlze9DOc64XLZlFPiTaJLCqiLue70QTBW5B1cDANikIWzyfrg0mgkMDDMyY8QP7y28QW4269Xi3B9doU949jGuU=";
            SymmetricStringEncryption dut = new SymmetricStringEncryption();
            string c = dut.Decrypt(key, b);
            Assert.AreEqual(a, c);
        }

        [TestMethod]
        public void OpenSSHCompare2()
        {
            byte[] key = {
                0xe8, 0x39, 0xff, 0xff, 0xe6, 0x8e, 0x36, 0x16, 0xd4, 0xa3, 0xcb, 0xb2, 0xaa, 0xfa, 0xd0, 0xd1,
            };
            string a = "An AES key, and an IV for symmetric encryption, are just bunchs of random bytes. So any cryptographically strong random number generator will do the trick. OpenSSL provides such a random number generator (which itself feeds on whatever the operating system provides, e.g. CryptGenRandom() on Windows or /dev/random and /dev/urandom on Linux).";
            string b = "uO5V9crdCuk1ZUDxFZMBVmrvAaZbQ9F26QkBj3f8aOnvouAcd0t55ko3apHv2sQGSiDCbgPOBgEBG5PB32M0gWR4zgJ8dIra9Jnx6QBdoYxI6GMH0bbApvd6kotwFJ2JwLbQ0ihjhXtLJOCapMUPc3wZhSCTuvLwBm8fRVitG+LTlcdUrS3Qvt7i7SSyTTrVa0+ALRhZQf3rsHFKimca/Nic6MwsHW0I6dHdzfgi8bW1j//60yIYUSOVgNMa5AP+9yIUsTYQW+8sd2PKjlEMoVEsfMio2Dc0jjDESSdrSX0xGyDGrsroGiWcoAcYqIvVNlngMYBBt65txakVv1d8/JiHxEeuV53zDH1ZV8jYEpHNWqdRUH5xMAeaSJUf2Qazjp6thrUv6Wic8H0wlz8M1D6D5qv6hg0T12rHTp/S9lYvy+h11Ztv1FsrgWO37t++TtCirulcrIJt9kLmmV/10XyxtIuxja8zVXO5POzQQ74=";
            SymmetricStringEncryption dut = new SymmetricStringEncryption();
            string c = dut.Decrypt(key, b);
            Assert.AreEqual(a, c);
        }

        // Note inverse openSSH
        // cat <FILE> | base64 -d | openssl enc -d -aes-128-cbc -K e839ffffe68e3616d4a3cbb2aafad0d1 -iv 00000000000000000000000000000000 | tail -c +17
    }
}
