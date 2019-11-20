using Markdown.RenderUtilities.TokenHandleDescriptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.RenderUtilities
{
    public static class MarkdownRenderUtilities
    {
        public static Renderer GetMarkdownRenderer()
        {
            return new Renderer(new List<ITokenHandler>()
            {
                 new SimpleHandler(GetSimpleTokenHandleDescriptions()),
                 new PairedHandler(GetPairedTokenHandleDescriptions())
            });
        }

        public static List<MarkdownSimpleTokenHandleDescription> GetSimpleTokenHandleDescriptions()
        {
            var handleDescriptions = new List<MarkdownSimpleTokenHandleDescription>()
            {
                new MarkdownSimpleTokenHandleDescription(TokenType.Text),
                new MarkdownSimpleTokenHandleDescription(TokenType.WhiteSpace),
                new MarkdownSimpleTokenHandleDescription(TokenType.Digits),
                new MarkdownEscapeTokenHandleDescription()
            };

            return handleDescriptions;
        }

        public static List<MarkdownPairedTokenHandleDescription> GetPairedTokenHandleDescriptions()
        {
            var handleDescriptions = new List<MarkdownPairedTokenHandleDescription>()
            {
                new MarkdownPairedTokenHandleDescription(TokenType.Emphasis, "em"),
                new MarkdownPairedTokenHandleDescription(TokenType.Strong, "strong")
            };

            return handleDescriptions;
        }
    }
}
