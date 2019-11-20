using Markdown.RenderUtilities.TokenProcessingDescriptions;
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
            return new Renderer(GetMarkdownProcessors);
        }

        public static List<ITokenProcessor> GetMarkdownProcessors()
        {
            return new List<ITokenProcessor>()
            {
                 new SimpleProcessor(GetSimpleTokenProcessingDescriptions()),
                 new PairedProcessor(GetPairedTokenProcessingDescriptions())
            };
        }

        public static List<MarkdownSimpleTokenProcessingDescription> GetSimpleTokenProcessingDescriptions()
        {
            var processingDescriptions = new List<MarkdownSimpleTokenProcessingDescription>()
            {
                new MarkdownSimpleTokenProcessingDescription(TokenType.Text),
                new MarkdownSimpleTokenProcessingDescription(TokenType.WhiteSpace),
                new MarkdownSimpleTokenProcessingDescription(TokenType.Digits),
                new MarkdownEscapeTokenProcessingDescription()
            };

            return processingDescriptions;
        }

        public static List<MarkdownPairedTokenProcessingDescription> GetPairedTokenProcessingDescriptions()
        {
            var processingDescriptions = new List<MarkdownPairedTokenProcessingDescription>()
            {
                new MarkdownPairedTokenProcessingDescription(TokenType.Emphasis, "em"),
                new MarkdownPairedTokenProcessingDescription(TokenType.Strong, "strong")
            };

            return processingDescriptions;
        }
    }
}
