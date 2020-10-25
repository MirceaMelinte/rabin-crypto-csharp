namespace RabinCryptoSys.Infrastructure
{
    using System.Numerics;
    using System.Security.Cryptography;

    public static class Primality
    {
        public static bool IsLikelyPrime(this BigInteger src, int iterations, RNGCryptoServiceProvider cryptoService)
        {
            if (src == 2 || src == 3)
            {
                return true;
            }

            if (src < 2 || src % 2 == 0)
            {
                return false;
            }

            var d = src - 1;

            var s = 0;

            while (d % 2 == 0)
            {
                d /= 2;
                s += 1;
            }

            var byteArray = new byte[src.ToByteArray().LongLength];

            BigInteger? a;

            for (var i = 0; i < iterations; i++)
            {
                do
                {
                    cryptoService.GetBytes(byteArray);
                    a = new BigInteger(byteArray);
                }
                while (a < 2 || a >= src - 2);

                if (a.HasValue)
                {
                    BigInteger x = BigInteger.ModPow(a.Value, d, src);

                    if (x == 1 || x == src - 1)
                    {
                        continue;
                    }

                    for (var r = 1; r < s; r++)
                    {
                        x = BigInteger.ModPow(x, 2, src);

                        if (x == 1)
                        {
                            return false;
                        }

                        if (x == src - 1)
                        {
                            break;
                        }
                    }

                    if (x != src - 1)
                    {
                        return false;
                    }
                }

                return false;
            }

            return true;
        }
    }
}