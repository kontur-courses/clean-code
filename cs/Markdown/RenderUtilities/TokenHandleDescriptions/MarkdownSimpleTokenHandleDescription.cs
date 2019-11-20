using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.RenderUtilities.TokenHandleDescriptions
{
    public class MarkdownSimpleTokenHandleDescription : TokenHandleDescription
    {
        private readonly TokenType tokenType;

        public override TokenType TokenType => tokenType;

        public MarkdownSimpleTokenHandleDescription(TokenType tokenType)
        {
            this.tokenType = tokenType;
        }

        public virtual string GetRenderedTokenText(Token token)
        {
            return token.Text;
        }
    }
}
