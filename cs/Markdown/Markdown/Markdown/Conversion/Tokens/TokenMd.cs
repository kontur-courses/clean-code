using System.Collections.Generic;

namespace Markdown
{
    public class TokenMd
    {
        public string Token { get; set; }
        public string textWithoutMark { get; set; }

        public (string First, string Last) markSymbols { get; set; }
        
        public List<TokenMd> InnerTokens { get; set; }
    }
}