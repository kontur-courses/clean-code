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

        public TokenReader(List<TokenDescription> tokenDescriptions)
        {

            this.tokenDescriptions = tokenDescriptions.ToList();
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
                if (!TryReadToken(text, position, out token))
                    break;
                tokenList.Add(token);
                position += token.Length;
            }

            tokenList.Add(new Token(text, text.Length, TokenType.Eof));

            return tokenList;
        }

        public bool TryReadToken(string text, int position, out Token token)
        {
            token = null;

            var offset = 0;
            for(offset = 0;  position + offset < text.Length; offset++)
                foreach(var tokenDescription in tokenDescriptions)
                {
                    if (tokenDescription.TryReadToken(text, position + offset, out token))
                    {
                        if (offset != 0)
                            token = new Token(text, position, TokenType.Text, offset);
                        return true;
                    }
                }

            if (offset != 0)
                token = new Token(text, position, TokenType.Text, offset);
            return token != null;
        }
    }
}
