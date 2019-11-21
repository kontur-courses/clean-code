using System.Collections.Generic;
using System.Linq;
using Markdown.CoreParser.ConverterInTokens;
using Markdown.Tokens;

namespace Markdown.CoreParser
{
    public class Parser: IParser
    {
        private readonly HashSet<IConverterInToken> iConverterTokens = new HashSet<IConverterInToken>();  
        public void Register(IConverterInToken converterInToken)
        {
            iConverterTokens.Add(converterInToken);
        }

        public IToken[] Tokenize(string str)
        {
            var tokens = new List<IToken>();
            var pastEscapeCharacter = false;
            for (var currentIndex = 0; currentIndex < str.Length; currentIndex++)
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
                    var token = iConverterToken.SelectTokenInString(str, currentIndex);
                    if (token == null) continue;
                    tempTokens.Add(token);
                }

                if (tempTokens.Count == 0) continue;

                tempTokens.Sort((t1, t2) => t1.Length.CompareTo(t2.Length) * -1);
                var currentToken = tempTokens.First();
                tokens.Add(currentToken);
                currentIndex += currentToken.Length;
            }

            return tokens.ToArray();
        }
    }
}