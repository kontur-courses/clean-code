using Markdown.Builders;
using Markdown.Parsers;

namespace Markdown
{
    class Md : ITranslator
    {
        private readonly ILanguageParser languageParser;
        private readonly ILanguageBuilder languageBuilder;

        public Md() : this(new MarkdownParser(), new HtmlBuilder())
        {
        }

        public Md(ILanguageParser languageParser, ILanguageBuilder languageBuilder)
        {
            this.languageParser = languageParser;
            this.languageBuilder = languageBuilder;
        }

        public string Render(string inputDocument)
        {
            return languageBuilder.BuildDocument(languageParser.GetParsedDocument(inputDocument));
        }
    }
}
