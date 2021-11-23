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
            IfOpenedAddNestedToFather(token);
        }
        
        private static void IfOpenedAddNestedToFather(Token token)
        {
            var nested = token.GetNestedTokens();
            for (int i = 0; i < nested.Count; i++)
            {
                IfOpenedAddNestedToFather(nested[i]);
            }
            if (!token.closed)
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
