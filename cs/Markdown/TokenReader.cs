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
        private readonly List<TokenDescription> tokenDescriptions;

        public TokenReader(IEnumerable<TokenDescription> tokenDescriptions)
        { 
            this.tokenDescriptions = tokenDescriptions
                .OrderByDescending(descr => descr.TokenMarker.Length)
                .ToList();
        }

        public List<Token> TokenizeText(string text)
        {
            if (text == null)
                throw new ArgumentNullException("text");
            var tokenList = new List<Token>();
            var position = 0;
            while(position < text.Length)
            {
                Token token = null;
                var prevPosition = position;
                while (position < text.Length && !TryReadToken(text, position, out token))
                    position++;
                if (prevPosition < position)
                    tokenList.Add(new Token(text, prevPosition, TokenType.Text, position - prevPosition));
                if (token != null)
                {
                    tokenList.Add(token);
                    position += token.Length;
                }
            }

            tokenList.Add(new Token(text, text.Length, TokenType.Eof));

            return tokenList;
        }

        public bool TryReadToken(string text, int position, out Token token)
        {
            token = null;
            foreach(var tokenDescription in tokenDescriptions)
            {
                if (tokenDescription.TryReadToken(text, position, out token))
                    return true;
            }
            return false;
        }
    }
}
