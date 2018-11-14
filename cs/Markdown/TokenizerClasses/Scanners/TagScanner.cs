using System.Collections.Generic;

namespace Markdown.TokenizerClasses.Scanners
{
    public class TagScanner
    {
        public static readonly Dictionary<string, string> TokenType = new Dictionary<string, string>
        {
            {"_", "UNDERSCORE"},
            {"\\", "ESCAPE"},
            {"\n", "NEWLINE"}
        };

        public Token Scan(string text)
        {
            if (string.IsNullOrEmpty(text))
                return null;

            var token = text[0].ToString();

            return !TokenType.ContainsKey(token) ? null : new Token(TokenType[token], token);
        }
    }
}
