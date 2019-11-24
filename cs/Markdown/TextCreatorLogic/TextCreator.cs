using System;
using System.Text;

namespace Markdown
{
    internal static class TextCreator
    { 
        private static Random random = new Random();
        internal static string CreateText(int countSubstring)
        {
            
            
            var result = new StringBuilder();

            for (int i = 0; i < countSubstring; i++)
            {
                var randomIndex = random.Next(TextCreatorData.Substrings.Length);
                result.Append(TextCreatorData.Substrings[randomIndex]);
            }

            return result.ToString();
        }
    }
}