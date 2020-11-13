using System.Collections.Generic;

namespace Markdown
{
    public class TokenMd
    {
        private string Token;
        private string textWithoutMark;

        private (string First, string Last) markSymbols;
        
        private List<TokenMd> InnerTokens;
    }
}