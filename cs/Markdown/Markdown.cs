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
            return translator.Translate(treeBuilder.BuildTree(parser.GetTokens(markdownText)));
        }
    }
}