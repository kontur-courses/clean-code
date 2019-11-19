using Markdown.Converters;
using Markdown.SyntaxTrees;
using Markdown.Tokens;

namespace Markdown.Core
{
    public class MarkdownProcessorFactory
    {
        private static readonly IConverter DefaultConverter =
            new ConverterUsingSyntaxTree(new MarkdownSyntaxTreeBuilder(new MarkdownRules()),
                new SyntaxTreeConverter(), new MarkdownToHtmlSeparatorConverter());
        private static readonly ITokenReader DefaultTokenReader = new TokenReader(new MarkdownTokenReaderConfiguration());

        public static MarkdownProcessor Create()
        {
            return Create(DefaultConverter, DefaultTokenReader);
        }

        public static MarkdownProcessor Create(IConverter converter, ITokenReader tokenReader)
        {
            return new MarkdownProcessor(converter, tokenReader);
        }
    }
}