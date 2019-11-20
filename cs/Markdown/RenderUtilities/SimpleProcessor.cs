using Markdown.RenderUtilities.TokenProcessingDescriptions;
using System.Collections.Generic;
using System.Linq;

namespace Markdown.RenderUtilities
{
    public class SimpleProcessor: ITokenProcessor
    {
        private readonly List<MarkdownSimpleTokenProcessingDescription> processingDescriptions;
        private readonly Dictionary<TokenType, MarkdownSimpleTokenProcessingDescription> typeToDescription;

        public List<TokenType> AcceptedTokenTypes => 
            processingDescriptions.Select(token => token.TokenType).ToList();

        public SimpleProcessor(IEnumerable<MarkdownSimpleTokenProcessingDescription> processingDescriptions)
        {
            this.processingDescriptions = processingDescriptions.ToList();
            typeToDescription = this.processingDescriptions.ToDictionary(token => token.TokenType);
        }

        public void ProcessTokens(List<Token> tokens)
        {
            return;
        }

        public bool TryGetRenderedTokenText(List<Token> tokens, 
            int tokenIndex, out string tokenString)
        {
            tokenString = null;
            if (!typeToDescription.TryGetValue(
                tokens[tokenIndex].TokenType, out var processingDescription))
                return false;
            tokenString = processingDescription.GetRenderedTokenText(tokens[tokenIndex]);
            return true;
        }
    }
}
