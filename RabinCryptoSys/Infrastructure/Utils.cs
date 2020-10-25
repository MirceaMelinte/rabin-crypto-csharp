namespace RabinCryptoSys.Infrastructure
{
    using System.Numerics;
    using System.Security.Cryptography;

    public static class Utils
    {
        public static BigInteger GeneratePrime(int bitLength, RNGCryptoServiceProvider cryptoService)
        {
            BigInteger? p;

            do
            {
                var byteArray = new byte[bitLength];

                cryptoService.GetBytes(byteArray);

                p = new BigInteger(byteArray, true);
            }
            while (!(p.HasValue && p.Value.IsLikelyPrime(bitLength / 2, cryptoService) && p.Value % 4 == 3));

            return p.Value;
        }

        public static (BigInteger sCoeff, BigInteger tCoeff) ExtendedEuclidean(BigInteger a, BigInteger b)
        {
            var s = BigInteger.Parse("0");
            var oldS = BigInteger.Parse("1");

            var t = BigInteger.Parse("1");
            var oldT = BigInteger.Parse("0");

            var r = b;
            var oldR = a;

            while (r != 0)
            {
                var q = oldR / r;

                var tr = r;

                r = oldR - q * r;
                oldR = tr;

                var ts = s;

                s = oldS - q * s;
                oldS = ts;

                var tt = t;

                t = oldT - q * t;
                oldT = tt;
            }

            return (oldS, oldT);
        }
    }
}