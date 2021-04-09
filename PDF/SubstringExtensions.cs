using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;


namespace InternetExplorer

{
    static class SubstringExtensions
    {
        public static string Between(this string value, string a, string b)
        {
            int posA = value.IndexOf(a);
            int posB = value.LastIndexOf(b);
            if (posA == -1)
            {
                return "";
            }
            if (posB == -1)
            {
                return "";
            }
            int adjustedPosA = posA + a.Length;
            if (adjustedPosA >= posB)
            {
                return "";
            }
            return value.Substring(adjustedPosA, posB - adjustedPosA);
        }

        /// <summary>
        /// Get string value after [first] a.
        /// </summary>
        public static string Before(this string value, string a)
        {
            int posA = value.IndexOf(a);
            if (posA == -1)
            {
                return "";
            }
            return value.Substring(0, posA);
        }

        /// <summary>
        /// Get string value after [last] a.
        /// </summary>
        public static string After(this string value, string a)
        {
            int posA = value.LastIndexOf(a);
            if (posA == -1)
            {
                return "";
            }
            int adjustedPosA = posA + a.Length;
            if (adjustedPosA >= value.Length)
            {
                return "";
            }
            char[] MyChar = { ':', '.', ':', '\\', ' ' };
            string result = "";
            // string NewString = MyString.TrimStart(MyChar);
            int index = value.IndexOf(" ");
            string after = value.Substring(adjustedPosA);

            if (after.Contains(' '))
            {
                int index1 = after.IndexOf("\n");
                result = after.Substring(0, index1);
                string[] lines = Regex.Split(result, "\r\n");

                Console.WriteLine("result: " + lines[0].TrimStart(MyChar));
                string news = result.TrimStart(MyChar) + " " + "Indigen Technologies..";
                int i = 0;
                if (a.Trim().Equals("Place OF Supply") || a.Trim().Equals("YOUR TERM & CONDITION OF SALE"))
                {
                     i = news.IndexOf("\r");
                }
                else
                {
                     i = news.IndexOf(" ");
                }
                
                result = news.Substring(0, i);
                Console.WriteLine("result new.........: " + result.TrimStart(MyChar));
            }

            String searchVal = result.TrimStart(MyChar);
            if (searchVal == "")
            {
                if (!String.IsNullOrWhiteSpace(after))
                {
                    String stopAt = "\n";
                    int charLocation = after.IndexOf(stopAt, StringComparison.Ordinal);

                    if (charLocation > 0)
                    {
                        return after.Substring(0, charLocation).TrimStart(MyChar);
                    }
                }
            }
            return searchVal;
        }
    }
}
