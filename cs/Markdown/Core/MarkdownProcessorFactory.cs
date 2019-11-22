using Markdown.Converters;
using Markdown.Rules;
using Markdown.SeparatorConverters;
using Markdown.SyntaxAnalysis.SyntaxTreeBuilders;
using Markdown.SyntaxAnalysis.SyntaxTreeConverters;
using Markdown.Tokenization;

namespace Markdown.Core
{
    public class MarkdownProcessorFactory
    {
        public static MarkdownProcessor CreateMarkdownToHtmlProcessor()
        {
            var converter = new ConverterUsingSyntaxTree(new MarkdownSyntaxTreeBuilder(new MarkdownRules()),
                new SyntaxTreeConverter(), new MarkdownToHtmlSeparatorConverter());
            var tokenReader = new TokenReader(new MarkdownTokenReaderConfiguration());
            return Create(converter, tokenReader);
        }

        public static MarkdownProcessor Create(IConverter converter, ITokenReader tokenReader)
        {
            return new MarkdownProcessor(converter, tokenReader);
        }
    }
}