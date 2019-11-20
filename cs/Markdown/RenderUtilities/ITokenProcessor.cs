using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.RenderUtilities
{
    public interface ITokenProcessor
    {
        List<TokenType> AcceptedTokenTypes { get; }

        void ProcessTokens(List<Token> tokens);
        bool TryGetRenderedTokenText(List<Token> tokens, int tokenIndex, out string tokenString);
    }
}
