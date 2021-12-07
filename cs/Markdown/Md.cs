using Markdown.TagEvents;
using Markdown.TagParsers;

namespace Markdown
{
    public class Md
    {
        public string Render(string inputMarkdown)
        {
            var tagEvents = new Taginizer(inputMarkdown).Taginize();
            new EscapingTagParser(tagEvents).Parse();
            new UnderlineTagParser(tagEvents, TagName.Underliner).Parse();
            new UnderlineTagParser(tagEvents, TagName.DoubleUnderliner).Parse();
            new UnderlineParserCorrector(tagEvents).Parse();
            new TagInteractionParser(tagEvents).Parse();

            var htmlResult = new MarkdownToHtmlTranslator(tagEvents).Translate();
            return htmlResult;
        }
    }
}
