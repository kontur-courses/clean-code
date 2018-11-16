using System;
using Markdown.TokenParser;
using Markdown.TreeBuilder;
using Markdown.TreeTranslator;

namespace Markdown
{
    public class Markdown
    {
        private readonly ITokenParser parser;
        private readonly ITokenTreeBuilder treeBuilder;
        private readonly ITokenTreeTranslator translator;

        public Markdown(ITokenParser parser, ITokenTreeTranslator translator, ITokenTreeBuilder treeBuilder)
        {
            this.parser = parser;
            this.translator = translator;
            this.treeBuilder = treeBuilder;
        }

        public string Render(string markdownText)
        {
            if (markdownText == null)
                throw new ArgumentNullException(nameof(markdownText));
            if (markdownText.Contains("\n\n"))
                throw new ArgumentException("Input string can't contain \\n\\n");
            return translator.Translate(treeBuilder.BuildTree(parser.GetTokens(markdownText)));
        }
    }
}