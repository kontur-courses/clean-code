using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.RenderUtilities
{
    public static class MarkdownRenderUtilities
    {
        public static List<SimpleTokenHandleDescription> GetSimpleTokenHandleDescriptions()
        {
            var handleDescriptions = new List<SimpleTokenHandleDescription>()
            {
                new SimpleTokenHandleDescription(TokenType.Text, (token) => token.Text),
                new SimpleTokenHandleDescription(TokenType.WhiteSpace, (token) => token.Text),
                new SimpleTokenHandleDescription(TokenType.Digits, (token) => token.Text),
                new SimpleTokenHandleDescription(TokenType.Escape, (token) => token.Text.Substring(1))
            };

            return handleDescriptions;
        }
    }
}
