using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class TokenReader
    {
        private readonly string text;
        private int position = 0;
        private readonly Dictionary<TokenType, TokenDescription> tokenTypeToDescription;
        private readonly List<TokenDescription> tokenDescriptions;

        public TokenReader(string text, IEnumerable<TokenDescription> tokenDescriptions)
        {
            this.text = text;
            position = 0;
            this.tokenDescriptions = tokenDescriptions
                .OrderByDescending(descr => descr.marker.Length)
                .ToList();
            tokenTypeToDescription = this.tokenDescriptions.ToDictionary(descr => descr.tokenType);
        }

        public List<Token> TokenizeText()
        {
            var tokenList = new List<Token>();
            var position = 0;
            while(position < text.Length)
            {
                var rawTextToken = ReadRawTextToken();
                if (rawTextToken != null) {
                    tokenList.Add(rawTextToken);
                    position += rawTextToken.length;
                }
                if (position == text.Length)
                    break;
                var token = ReadToken();
                tokenList.Add(token);
                position += token.length;
            }

            return tokenList;
        }
   
        // читаем, пока не встретилось начало какого-нибудь токена или не закончился текст
        public Token ReadRawTextToken()
        {
            throw new NotImplementedException();
        }

        // читаем какой-то другой токен, внутри обращаемся к одному из TokenDescription
        public Token ReadToken()
        {
            throw new NotImplementedException();
        }
    }
}
