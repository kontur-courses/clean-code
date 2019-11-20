using Markdown.RenderUtilities.TokenProcessingDescriptions;
using System.Collections.Generic;
using System.Linq;

namespace Markdown.RenderUtilities
{
    public class PairedProcessor : ITokenProcessor
    {
        private readonly List<MarkdownPairedTokenProcessingDescription> processingDescriptions;
        private readonly Dictionary<TokenType, MarkdownPairedTokenProcessingDescription> typeToDescription;

        private List<Token> openingTokens;
        private Dictionary<Token, PairedTokenDescription> tokenToPairedDescription;

        public PairedProcessor(IEnumerable<MarkdownPairedTokenProcessingDescription> processingDescriptions)
        {
            this.processingDescriptions = processingDescriptions.ToList();
            typeToDescription = this.processingDescriptions.ToDictionary(token => token.TokenType);

            openingTokens = new List<Token>();
            tokenToPairedDescription = new Dictionary<Token, PairedTokenDescription>();
        }

        public List<TokenType> AcceptedTokenTypes => 
            processingDescriptions.Select(token => token.TokenType).ToList();

        public void ProcessTokens(List<Token> tokens)
        {
            for (var i = 0; i < tokens.Count; i++)
                ProcessToken(tokens, i);
        }

        private void ProcessToken(List<Token> tokens, int tokenIndex)
        {
            var token = tokens[tokenIndex];
            if (!AcceptedTokenTypes.Contains(token.TokenType))
                return;
            var processingDescription = typeToDescription[token.TokenType];
            if (processingDescription.IsClosing(tokens, tokenIndex))
            {
                var openingToken = openingTokens
                                   .LastOrDefault(tkn => tkn.TokenType == token.TokenType);
                if(openingToken != null)
                {
                    var openingTokenIndex = openingTokens.LastIndexOf(openingToken);
                    openingTokens.RemoveRange(openingTokenIndex, openingTokens.Count - openingTokenIndex);
                    var newPair = new PairedTokenDescription(openingToken, token);
                    tokenToPairedDescription[openingToken] = newPair;
                    tokenToPairedDescription[token] = newPair;
                    return;
                }
            }
            if (processingDescription.IsOpening(tokens, tokenIndex))
                openingTokens.Add(token);
        }

        public bool TryGetRenderedTokenText(List<Token> tokens, int tokenIndex, out string tokenString)
        {
            tokenString = null;
            var token = tokens[tokenIndex];
            if (!AcceptedTokenTypes.Contains(token.TokenType))
                return false;
            var processingDescription = typeToDescription[token.TokenType];
            if (!tokenToPairedDescription.ContainsKey(token))
                tokenString = processingDescription.GetRenderedTokenText(token, false, false);
            else
                tokenString = processingDescription.GetRenderedTokenText(
                    token, true, tokenToPairedDescription[token].CloseToken == token);
            return true;
        }
    }
}
