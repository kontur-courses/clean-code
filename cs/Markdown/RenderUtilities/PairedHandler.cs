using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.RenderUtilities
{
    public class PairedHandler : ITokenHandler
    {
        private readonly List<PairedTokenHandleDescription> handleDescriptions;
        private readonly Dictionary<TokenType, PairedTokenHandleDescription> typeToHandler;

        public PairedHandler(IEnumerable<PairedTokenHandleDescription> handleDescriptions)
        {
            this.handleDescriptions = handleDescriptions.ToList();
            typeToHandler = this.handleDescriptions.ToDictionary(token => token.TokenType);
        }

        public List<TokenType> GetAcceptedTokenTypes()
        {
            return handleDescriptions.Select(token => token.TokenType).ToList();
        }

        private List<Token> openingTokens;
        private Dictionary<Token, PairedTokenDescription> tokenToPairedDescription;

        public void InitHandle()
        {
            openingTokens = new List<Token>();
            tokenToPairedDescription = new Dictionary<Token, PairedTokenDescription>();
        }

        public void HandleToken(List<Token> tokens, int tokenIndex)
        {
            var token = tokens[tokenIndex];
            if (!GetAcceptedTokenTypes().Contains(token.TokenType))
                return;
            var handler = typeToHandler[token.TokenType];
            if (handler.IsClosing(tokens, tokenIndex))
            {
                var openingToken = openingTokens.Where(tkn => tkn.TokenType == token.TokenType).LastOrDefault();
                if(openingToken != null)
                {
                    openingTokens.Remove(openingToken);
                    var newPair = new PairedTokenDescription(openingToken, token);
                    tokenToPairedDescription[openingToken] = newPair;
                    tokenToPairedDescription[token] = newPair;
                    return;
                }
            }
            if (handler.IsOpening(tokens, tokenIndex))
                openingTokens.Add(token);
        }

        public void EndHandle()
        {
            return;
        }

        public bool TryGetTokenString(List<Token> tokens, int tokenIndex, out string tokenString)
        {
            tokenString = null;
            var token = tokens[tokenIndex];
            if (!GetAcceptedTokenTypes().Contains(token.TokenType))
                return false;
            var handler = typeToHandler[token.TokenType];
            if (!tokenToPairedDescription.ContainsKey(token))
                tokenString = handler.PrintToken(token, false, false);
            else
                tokenString = handler.PrintToken(token, true, tokenToPairedDescription[token].CloseToken == token);
            return true;
        }
    }
}
