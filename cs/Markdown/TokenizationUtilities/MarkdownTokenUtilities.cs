using System;
using System.Collections.Generic;

namespace Markdown
{
    public static class MarkdownTokenUtilities
    {
        public static List<TokenDescription> GetMarkdownTokenDescriptions()
        {
            var tokenDescription = new List<TokenDescription>() {
                new TokenDescription(TokenType.WhiteSpace, (text, i) =>
                {
                    var length = 0;
                    while(i + length < text.Length && Char.IsWhiteSpace(text[i+length]))
                        length++;
                    return length;
                }),
                new TokenDescription(TokenType.Digits, (text, i) =>
                {
                    var length = 0;
                    while(i + length < text.Length && Char.IsDigit(text[i+length]))
                        length++;
                    return length;
                }),
                new TokenDescription(TokenType.Strong, (text, i) => i + 2 <= text.Length &&
                                                        text.Substring(i, 2) == "__" ? 2: 0),
                new TokenDescription(TokenType.Emphasis, (text, i) => i + 1 <= text.Length &&
                                                        text[i] == '_' ? 1 : 0),
                new TokenDescription(TokenType.Escape, (text, i) => i + 2 <= text.Length &&
                                                        text[i] == '\\' ? 2 : 0),
                new TokenDescription(TokenType.Text, (text, i) =>
                {
                    var specialChars = new List<char>(){'_', '\\'};
                    var length = 0;
                    while(i + length < text.Length && !Char.IsWhiteSpace(text[i+length]) 
                        && !Char.IsDigit(text[i+length]) && !specialChars.Contains(text[i+length]))
                        length++;
                    return length;
                })
            };

            return tokenDescription;
        }
    }
}
