using System;
using System.Collections.Generic;

namespace Markdown
{
    public class Parser
    {
        private List<(string FirstSymbol,string LastSymbol)> markSymbols;

        public Parser(List<Mark> marks)
        {
            //получаем индетифицирующий символ и последний символ
        }
        public List<TokenMd> GetTokens(string text)
        {
            var tokens = new List<TokenMd>();
            var index = 0;
            var newIndex = 0;
            
            while (index<text.Length)
            {
                //примерный вид
                var token = GetToken(index, out newIndex);
                tokens.Add(token);
                index = newIndex;
            }
            throw new NotImplementedException();
        }

        private TokenMd GetToken(int index, out int finalIndex)
        {
            throw new NotImplementedException();
        }
    }
}