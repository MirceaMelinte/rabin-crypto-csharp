namespace RabinCryptoSys.Services
{
    using System.Numerics;
    using System.Security.Cryptography;
    using Infrastructure;

    public static class RabinCrypto
    {
        public static (BigInteger p, BigInteger q, BigInteger n) GetKeys(int bitLength, RNGCryptoServiceProvider cryptoService)
        {
            var p = Utils.GeneratePrime(bitLength, cryptoService);
            var q = Utils.GeneratePrime(bitLength, cryptoService);

            return (p, q, p * q);
        }

        public static BigInteger Encryption(BigInteger m, BigInteger n)
            => BigInteger.ModPow(m, 2, n);

        public static (BigInteger d1, BigInteger d2, BigInteger d3, BigInteger d4) Decryption(BigInteger c, BigInteger p, BigInteger q)
        {
            var n = p * q;

            var p1 = BigInteger.ModPow(c, (p + 1) / 4, p);
            var p2 = p - p1;

            var q1 = BigInteger.ModPow(c, (q + 1) / 4, q);
            var q2 = q - q1;

            var (yP, yQ) = Utils.ExtendedEuclidean(p, q);

            return (
                (yP * p * q1 + yQ * q * p1) % n,
                (yP * p * q2 + yQ * q * p1) % n,
                (yP * p * q1 + yQ * q * p2) % n,
                (yP * p * q2 + yQ * q * p2) % n);
        }
    }
}