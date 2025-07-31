using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Core.Exceptions;
using Core.Helpers;

namespace Core.Extensions
{
    public static class StringExtensions
    {
        private const int NationalCodeLength = 10;
        public static string SubstringToMaxLength(this string text, int startIndex, int maxLength)
        {
            var sb = new StringBuilder();
            for (int i = startIndex; i < text.Length - 1; i++)
            {
                sb.Append(text[i]);

                if (sb.Length >= maxLength) break;
            }
            return sb.ToString();
        }

        public static T FromJsonString<T>(this string jsonString) =>
            JsonHelper.FromJsonString<T>(jsonString);

        public static string Remove(this string text, string textToRemove) =>
            text.Replace(textToRemove, "");

        public static bool TryGetSqlInjectionFreeText(this string orderByText, out string sqlInjectionFreeText)
        {
            sqlInjectionFreeText = string.Empty;

            if (string.IsNullOrWhiteSpace(orderByText)) return false;

            var match = Regex.Match(orderByText, @"^([a-zA-Z0-9\[\]_\.]){2,}(\s+(\b(asc|desc)\b)+){0,1}", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            if (!match.Success) return false;

            sqlInjectionFreeText = match.Value;

            return true;
        }

        public static bool TryGetSqlInjectionFreeText(this string orderByText, out (string ColumnName, bool IsAscending) sqlInjectionFreeText)
        {
            sqlInjectionFreeText = (string.Empty, false);
            if (string.IsNullOrWhiteSpace(orderByText)) return false;

            var isValid = TryGetSqlInjectionFreeText(orderByText, out string sqlInjectionFreeTextString);
            if (!isValid)
                return false;

            var splittedText = sqlInjectionFreeTextString.Split(' ');
            var isDescending = false;

            if (splittedText.Length > 1)
                isDescending = splittedText[1].Equals("desc", StringComparison.InvariantCultureIgnoreCase);

            sqlInjectionFreeText = (splittedText[0], !isDescending);

            return isValid;
        }

        public static Type ToType(this string typeName)
        {
            if (string.IsNullOrWhiteSpace(typeName))
                throw new CustomException(Enums.ExceptionCodeEnum.InternalServerError, $"{nameof(typeName)} is not valid .");
            
            return Type.GetType(typeName, true, true);
        }

        public static DateTime ToMiladiDate(this string shamsiDate)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(shamsiDate);

            DateTime dateTime;
            shamsiDate = shamsiDate.Replace("/", "");

            System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();
            dateTime = pc.ToDateTime(
                int.Parse(shamsiDate.Substring(0, 4)),
                int.Parse(shamsiDate.Substring(4, 2)),
                int.Parse(shamsiDate.Substring(6, 2)),
                1, 1, 1, 1);

            return dateTime;
        }

        public static string FixArabicAlphabet(this string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return input;
            
            return input
                .Replace("ي", "ی")
                .Replace('ئ', 'ی')
                .Replace("ك", "ک")
                .Replace(char.ConvertFromUtf32(8204), " ")
                .Replace("  ", " ")
                .Trim();
        }

        public static string FixArabicNumeric(this string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return input;

            return input
                .Replace("۱", "1")
                .Replace('۲', '2')
                .Replace('۳', '3')
                .Replace('۴', '4')
                .Replace('۵', '5')
                .Replace('۶', '6')
                .Replace('۷', '7')
                .Replace('۸', '8')
                .Replace('۹', '9')
                .Replace('۰', '0')
                .Replace(char.ConvertFromUtf32(8204), " ")
                .Replace("  ", " ")
                .Trim();
        }

        public static string ToFarsi(this string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return input;

            if (!input.All(char.IsDigit)) throw new ArgumentException("input is invalid.");

            return AmountToFarsiEquivalentHelper.ConvertToHoruf(input);
        }

        public static bool IsValidIranianNationalCode(this string input, bool isLegalPerson = false)
        {
            input = input.Trim();
            int allowedLength = isLegalPerson ? NationalCodeLength + 1 : NationalCodeLength;

            return input.Length == allowedLength
                   && input.All(char.IsDigit)
                   && input.Distinct().Count() > 1;
        }
    }
}
