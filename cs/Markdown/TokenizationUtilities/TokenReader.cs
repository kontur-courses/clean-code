﻿using System;
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
