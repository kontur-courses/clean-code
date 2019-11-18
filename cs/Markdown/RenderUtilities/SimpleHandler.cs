using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.RenderUtilities
{
    public class SimpleHandler: ITokenHandler
    {
        private readonly List<SimpleTokenHandleDescription> handleDescriptions;
        private readonly Dictionary<TokenType, SimpleTokenHandleDescription> typeToHandler;

        public SimpleHandler(IEnumerable<SimpleTokenHandleDescription> handleDescriptions)
        {
            this.handleDescriptions = handleDescriptions.ToList();
            typeToHandler = this.handleDescriptions.ToDictionary(token => token.TokenType);
        }

        public List<TokenType> GetAcceptedTokenTypes()
        {
            return handleDescriptions.Select(token => token.TokenType).ToList();
        }

        public void HandleToken(List<Token> tokens, int tokenIndex)
        {
            return;
        }

        public bool TryGetTokenString(List<Token> tokens, int currentTokenIndex, out string tokenString)
        {
            tokenString = null;
            SimpleTokenHandleDescription handler = null;
            if (!typeToHandler.TryGetValue(tokens[currentTokenIndex].TokenType, out handler))
                return false;
            tokenString = handler.PrintToken(tokens, currentTokenIndex);
            return true;
        }
    }
}
