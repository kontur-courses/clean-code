using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Markdown.ConverterInTokens;
using Markdown.ConverterTokens;
using Markdown.Tokens;

namespace Markdown.CoreParser
{
    public class Parser: IParser
    {
        private readonly HashSet<IConverterInToken> iConverterTokens = new HashSet<IConverterInToken>();  
        public void register(IConverterInToken converterInToken)
        {
            iConverterTokens.Add(converterInToken);
        }

        public IToken[] tokenize(string str)
        {
            var tokens = new List<IToken>();
            var pastEscapeCharacter = false;
            for ( var currentIndex = 0; currentIndex < str.Length; currentIndex++)
            {
                if (str[currentIndex] == '\\')
                {
                    currentIndex = pastEscapeCharacter ? currentIndex : currentIndex + 1;
                    pastEscapeCharacter = !pastEscapeCharacter;
                    continue;
                }

                var tempTokens = new List<IToken>();
                foreach (var iConverterToken in iConverterTokens)
                {
                    var token = iConverterToken.MakeConverter(str, currentIndex);
                    if (token == null) continue;
                    tempTokens.Add(token);
                }
                if  (tempTokens.Count == 0) continue;
                var currentToken = tempTokens.Max((t) => t);
                tokens.Add(currentToken);
                currentIndex += currentToken.Length;
            }

            return tokens.ToArray();
        }
    }
}