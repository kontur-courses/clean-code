using System;
using System.Collections.Generic;
using System.Linq;
using Markdown.TokenModels;

namespace Markdown.Core
{
    public static class Tokenizer
    {
        private static readonly Dictionary<string, string> Tags = new Dictionary<string, string>
        {
            {"__", "__"},
            {"_", "_"},
            {"#", "\n"}
        };

        public static IEnumerable<IToken> ParseIntoTokens(string mdText)
        {
            var tokens = new List<IToken>();
            var stringTokenValue = "";
            for (var index = 0; index < mdText.Length;)
            {
                if (!Tags.ContainsKey(mdText[index].ToString()))
                {
                    stringTokenValue += mdText[index++];
                    continue;
                }

                if (stringTokenValue != "")
                {
                    tokens.Add(new StringToken(stringTokenValue));
                    stringTokenValue = "";
                }

                var closingTag = Tags[mdText[index] + (mdText[index + 1] is '_' ? "_" : "")];
                var startIndex = index + closingTag.Length;
                
                if (closingTag == "\n" && !mdText.Contains("\n"))
                    mdText += "\n";
                
                var tokenValue = GetTokenValue(startIndex, closingTag, mdText);
                var newToken = GetToken(closingTag, tokenValue);
                
                tokens.Add(newToken);
                index += closingTag.Length + tokenValue.Length + closingTag.Length;
            }

            if (stringTokenValue != "") 
                tokens.Add(new StringToken(stringTokenValue));

            return tokens;
        }

        private static IToken GetToken(string tagType, string tokenValue) => tagType switch
        {
            "\n" => HeaderToken.Create(tokenValue),
            "_" => ItalicToken.Create(tokenValue),
            "__" => BoldToken.Create(tokenValue),
            _ => throw new ArgumentException()
        };


        private static string GetTokenValue(int startIndex, string closedTag, string mdText)
        {
            var length = mdText.Substring(startIndex).IndexOf(closedTag, StringComparison.Ordinal);
            return mdText.Substring(startIndex, length);
        }
    }
}