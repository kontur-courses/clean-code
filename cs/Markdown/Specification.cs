using System.Collections.Generic;

namespace Markdown
{
    static class Specification
    {
        public static HashSet<string> KeyWords = new HashSet<string>() {"_", "__", "\\"};
        public static HashSet<string> Digits = new HashSet<string> {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9"};


        public static Dictionary<TokenType, HashSet<TokenType>> ImpossibleNesting =
            new Dictionary<TokenType, HashSet<TokenType>>
            {
                {TokenType.Italic, new HashSet<TokenType>() {TokenType.Bold}}
            };

        public static Dictionary<string, TokenType> MdToTokenTypes = new Dictionary<string, TokenType>()
        {
            {"_", TokenType.Italic},
            {"__", TokenType.Bold},
        };

        public static Dictionary<TokenType, string> TokenTypeToHTML = new Dictionary<TokenType, string>
        {
            {TokenType.Italic, "em"},
            {TokenType.Bold, "strong"}
        };

        public static bool CanBeStarting(Substring substring, string markdownInput)
        {
            return substring.Index + substring.Length < markdownInput.Length &&
                   markdownInput[substring.Index + substring.Length] != ' ';
        }

        public static bool CanBeClosing(Substring substring, string markdownInput)
        {
            return substring.Index != 0 && markdownInput[substring.Index - 1] != ' ';
        }

        public static bool DelimitersCanBePair(Delimiter startDelimiter, Delimiter closingDelimiter)
        {
            return startDelimiter.Value == closingDelimiter.Value;
        }
    }
}