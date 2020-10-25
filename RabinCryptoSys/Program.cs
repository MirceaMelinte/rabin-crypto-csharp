namespace RabinCryptoSys
{
    using System;
    using System.Numerics;
    using System.Security.Cryptography;
    using System.Text;
    using Services;

    static class Program
    {
        readonly static RNGCryptoServiceProvider cryptoService = new RNGCryptoServiceProvider();
        readonly static HMAC hmacService = HMAC.Create("HMACSHA1");

        readonly static int bitLength = 128;
        readonly static string message = "helloworld";

        static void Main(string[] _)
        {
            var (p, q, n) = RabinCrypto.GetKeys(bitLength, cryptoService);

            Console.WriteLine($"Private key:\tp={p}, q={q}");
            Console.WriteLine($"Public key:\tN={n}");

            Console.WriteLine($"[1] Message before encryption:\t{message}");
            
            var messageBytes = Encoding.ASCII.GetBytes(message);
            var messageNumber = new BigInteger(messageBytes);

            var cipherText = RabinCrypto.Encryption(messageNumber, n);
            
            Console.WriteLine($"[2] Message after encryption:\t{cipherText}");

            // compute message digest
            hmacService.Key = n.ToByteArray();
            var messageDigest = hmacService.ComputeHash(messageBytes);

            // send cipherText, messageDigest & (p, q)

            var (d1, d2, d3, d4) = RabinCrypto.Decryption(cipherText, p, q);
            var candidates = new BigInteger[] { d1, d2, d3, d4 };

            Console.WriteLine($"\n[3] Messages after decryption (plaintext candidates):");

            foreach (var candidate in candidates)
            {
                Console.WriteLine(Encoding.ASCII.GetString(candidate.ToByteArray()));
            }

            // select the correct plaintext

            Console.WriteLine($"\n[4] Correct plaintext message:");

            foreach (var candidate in candidates)
            {
                if (Encoding.ASCII.GetString(hmacService.ComputeHash(candidate.ToByteArray())) == Encoding.ASCII.GetString(messageDigest))
                {
                    Console.WriteLine(Encoding.ASCII.GetString(candidate.ToByteArray()));
                }
            }

            Console.ReadKey();
        }
    }
}