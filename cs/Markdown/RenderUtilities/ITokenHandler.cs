using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.RenderUtilities
{
    public interface ITokenHandler
    {
        List<TokenType> GetAcceptedTokenTypes();

        void HandleToken(List<Token> tokens, int tokenIndex);
        bool TryGetTokenString(List<Token> tokens, int currentTokenIndex, out string tokenString);
    }
}
