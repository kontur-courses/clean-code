using System.Collections.Generic;

namespace Markdown.RenderUtilities
{
    public interface ITokenProcessor
    {
        List<TokenType> AcceptedTokenTypes { get; }

        void ProcessTokens(List<Token> tokens);
        bool TryGetRenderedTokenText(List<Token> tokens, int tokenIndex, out string tokenString);
    }
}
