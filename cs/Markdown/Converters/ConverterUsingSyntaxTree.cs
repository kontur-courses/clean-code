using System.Collections.Generic;
using Markdown.SeparatorConverters;
using Markdown.SyntaxAnalysis.SyntaxTreeBuilders;
using Markdown.SyntaxAnalysis.SyntaxTreeConverters;
using Markdown.Tokenization;

namespace Markdown.Converters
{
    public class ConverterUsingSyntaxTree : IConverter
    {
        private readonly ISyntaxTreeBuilder syntaxTreeBuilder;
        private readonly ISyntaxTreeConverter syntaxTreeConverter;
        private readonly ISeparatorConverter separatorConverter;

        public ConverterUsingSyntaxTree(ISyntaxTreeBuilder syntaxTreeBuilder, ISyntaxTreeConverter syntaxTreeConverter,
            ISeparatorConverter separatorConverter)
        {
            this.syntaxTreeBuilder = syntaxTreeBuilder;
            this.syntaxTreeConverter = syntaxTreeConverter;
            this.separatorConverter = separatorConverter;
        }

        public string Convert(IEnumerable<Token> tokens, string text)
        {
            var syntaxTree = syntaxTreeBuilder.BuildSyntaxTree(tokens, text);
            var result = syntaxTreeConverter.Convert(syntaxTree, separatorConverter);

            return result;
        }
    }
}