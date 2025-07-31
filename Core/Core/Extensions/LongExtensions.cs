using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Core.Helpers;

namespace Core.Extensions
{
    public static class LongExtensions
    {
        public static string ToCommaDelimited(this long number, string separator = ",", int separatedSize = 3) => ((decimal)number).ToCommaDelimited(separator, separatedSize);

        public static string ToFarsi(this long number)
        {
            if (number < 1) throw new ArgumentException("number is invalid.");
            
            var strNumber = number.ToString();
            if (strNumber.Length > 15) throw new ArgumentException("number length is not supported.");

            return AmountToFarsiEquivalentHelper.ConvertToHoruf(strNumber);
        }

    }
}
