using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.RenderUtilities
{
    public class SimpleTokenHandleDescription
    {
        public readonly TokenType TokenType;
        private readonly Func<Token, string> printToken;

        public SimpleTokenHandleDescription(TokenType tokenType, Func<Token, string> printToken)
        {
            TokenType = tokenType;
            this.printToken = printToken;
        }
        
        public string PrintToken(Token token)
        {
            return printToken(token);
        }
    }
}
