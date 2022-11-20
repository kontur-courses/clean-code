using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown.Markdown;
 

namespace Markdown.Tokens
{
    public static class TokenParser
    {


        public static List<Token> GetTokens(List<Token> tokenList, int endIndex)
        {
            var tokens = new List<Token>();
            var firstToken = new Token(0, 0).FromGetTokenTo(tokenList[0]);
            if (firstToken.Length > 0)
                tokens.Add(firstToken);
            var token = tokenList[0];
            for (var i = 0; i < tokenList.Count; i++)
            {
                AddDefaultToken(i, tokenList, tokens);
                token = CreateToken(tokenList, i, tokens);
            }
            tokens.Add(token.FromGetTokenTo(new Token(endIndex, 0)));
            return tokens;
        }

        private static void AddDefaultToken(int i, List<Token> tokenList, List<Token> tokens)
        {
            if (i < 1) return;
            var textToken = tokenList[i - 1].FromGetTokenTo(tokenList[i]);
            if (textToken.Length > 0)
                tokens.Add(textToken);
        }

        private static Token CreateToken(List<Token> tokenList, int i, List<Token> tokens)
        {
            var token = tokenList[i];
            tokens.Add(token);
            return token;
        }

    }
}

