using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Foyer.Tests.Utilities
{
    internal static class Utilities
    {
        private static readonly Random Random = new Random();

        private static readonly char[] Chars = Enumerable
            .Range(char.MinValue, char.MaxValue)
            .Select(x => (char)x)
            .Where(c => !char.IsControl(c))
            .ToArray();

        private static char RandomChar() => Chars[Random.Next(Chars.Length)];

        public static string GenerateRandomString(int length)
        {
            var chars = new char[length];

            for (int i = 0; i < length; i++)
            {
                chars[i] = RandomChar();
            }

            return new string(chars);
        }

        // Other methode, not tested
        private static string GenerateRandomStringUsingBytes(int size)
        {
            var b = new byte[size];
            new RNGCryptoServiceProvider().GetBytes(b);
            return Encoding.ASCII.GetString(b);
        }
    }
}
