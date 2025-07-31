using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Helpers
{
    public static class RandomTextHelper
    {
        const string alphabeticChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string numericChars = "0123456789";

        private static readonly Random random = new Random();

        public static string GetAlphabeticString(int length)
        {
            return new string(Enumerable.Repeat(alphabeticChars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string GetNumericString(int length)
        {
            return new string(Enumerable.Repeat(numericChars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string GetAlphaNumericString(int length)
        {
            string chars = alphabeticChars + numericChars;
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
