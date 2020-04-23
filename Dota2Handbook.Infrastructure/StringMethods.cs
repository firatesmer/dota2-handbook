using System;

namespace Dota2Handbook.Infrastructure
{
    public static class StringMethods
    {
        public static string TrimEnd(this string input, string suffixToRemove)
        {
            if (!string.IsNullOrEmpty(suffixToRemove) && input.EndsWith(suffixToRemove))
                return input.Substring(0, input.Length - suffixToRemove.Length);

            else return input;
        }
    }
}