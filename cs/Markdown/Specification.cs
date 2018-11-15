using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    static class Specification
    {
        public static HashSet<string> Delimiters = new HashSet<string>() {"_", "__", "\\"};

        public static Dictionary<TokenType, HashSet<TokenType>> possibleNesting =
            new Dictionary<TokenType, HashSet<TokenType>>
            {
                {TokenType.italic, new HashSet<TokenType>() {TokenType.italic, TokenType.escaped,TokenType.text}},
                {TokenType.text, new HashSet<TokenType>() {TokenType.italic, TokenType.escaped,TokenType.text, TokenType.bold}},
                {TokenType.bold, new HashSet<TokenType>() {TokenType.bold, TokenType.italic,TokenType.text,TokenType.escaped}},
                
            };

        public static Dictionary<string, TokenType> MdToTokenTypes = new Dictionary<string, TokenType>()
        {
            {"_", TokenType.italic},
            {"__", TokenType.bold},
            {" ", TokenType.text},
            {"", TokenType.text},
            {"\\", TokenType.escaped},
        };

        public static Dictionary<TokenType, string> TokenTypeToHTML = new Dictionary<TokenType, string>
        {
            {TokenType.italic, "em"},
            {TokenType.bold, "strong"},
            {TokenType.escaped, ""}
        };

        public static bool CanBeStarting(Substring substring, string markdownInput)
        {
            return substring.Index + substring.Length < markdownInput.Length &&
                   markdownInput[substring.Index + substring.Length] != ' ';
        }

        public static bool CanBeClosing(Substring substring, string markdownInput)
        {
            return substring.Value == "\\" || substring.Index != 0 && markdownInput[substring.Index - 1] != ' ';
        }

        public static bool DelimetersCanBePair(Delimiter startDelimiter, Delimiter closingDelimiter)
        {
            if (startDelimiter.delimiter == "\\" && closingDelimiter.index - startDelimiter.index == 1)
                return true;

            if ((startDelimiter.delimiter == "" || startDelimiter.delimiter == " ") &&
                (closingDelimiter.delimiter == "" || closingDelimiter.delimiter == " "))
                return true;

            return startDelimiter.delimiter == closingDelimiter.delimiter;
        }
    }
}