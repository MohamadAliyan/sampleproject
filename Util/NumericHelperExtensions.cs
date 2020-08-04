using System.Globalization;

namespace Util
{
    public static class NumericHelperExtensions
    {

        private static readonly CultureInfo farsi = new CultureInfo("fa-IR");
        private static readonly CultureInfo latin = new CultureInfo("en-US");

        public static string ToPersian(this string input)
        {
            var arabicDigits = farsi.NumberFormat.NativeDigits;
            for (int i = 0; i < arabicDigits.Length; i++)
            {
                input = input.Replace(i.ToString(), arabicDigits[i]);
            }
            return input;
        }

        public static string ToLatin(this string input)
        {
            var latinDigits = latin.NumberFormat.NativeDigits;
            var arabicDigits = farsi.NumberFormat.NativeDigits;
            for (int i = 0; i < latinDigits.Length; i++)
            {
                input = input.Replace(arabicDigits[i], latinDigits[i]);
            }
            return input;
        }
    }
}
