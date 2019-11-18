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

        void InitHandle();
        void HandleToken(List<Token> tokens, int tokenIndex);
        void EndHandle();
        bool TryGetTokenString(List<Token> tokens, int tokenIndex, out string tokenString);
    }
}
