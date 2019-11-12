using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public static class MarkdownUtilities
    {
        public static List<TokenDescription> GetMarkdownTokenDescriptions()
        {
            var tokenDescription = new List<TokenDescription>() {
                new TokenDescription(TokenType.Eol, "\n", (text, i) => text[i] == '\n' ? 1 : 0),
                new TokenDescription(TokenType.Escape, "\\", (text, i) => text[i] == '\\' ? 2 : 0),
                new TokenDescription(TokenType.Emphasis, "_", (text, i) => text[i] == '_' ? 1 : 0),
                new TokenDescription(TokenType.Strong, "__", (text, i) =>
                                                             text.Substring(i, 2) == "__" ? 2: 0),
                new TokenDescription(TokenType.Digits, "1", (text, i) =>
                {
                    var length = 0;
                    while(i + length < text.Length && Char.IsDigit(text[i+length]))
                        length++;
                    return length;
                }),
                new TokenDescription(TokenType.WhiteSpace, " ", (text, i) =>
                {
                    var length = 0;
                    while(i + length < text.Length && Char.IsWhiteSpace(text[i+length]))
                        length++;
                    return length;
                }),
            };

            return tokenDescription;
        }
    }
}
