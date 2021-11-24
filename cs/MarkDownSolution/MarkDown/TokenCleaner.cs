using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkDown
{
    public static class TokenCleaner
    {
        public static void CleanToken(Token token)
        {
            IfOpenedOrIncorrectAddNestedToFather(token);
        }
        
        private static void IfOpenedOrIncorrectAddNestedToFather(Token token)
        {
            var nested = token.GetNestedTokens();
            for (int i = 0; i < nested.Count; i++)
            {
                IfOpenedOrIncorrectAddNestedToFather(nested[i]);
            }
            if (!token.closed || token is BoldToken && token.fatherToken is ItalicToken)
            {
                foreach(var n in nested)
                {
                    token.fatherToken.AddNestedToken(n);
                }
                token.fatherToken.RemoveNestedToken(token);
            }
        }
    }
}
