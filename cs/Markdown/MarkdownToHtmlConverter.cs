using Markdown.Generators;
using Markdown.Parsers;
using Markdown.Tags;

namespace Markdown
{
    public class MarkdownToHtmlConverter
    {
        private readonly IGenerator htmlGenerator;
        private readonly IParser markdownParser;
        private readonly Dictionary<string, TagType> supportedMarkdownTags = new Dictionary<string, TagType>
        {
            {"_", TagType.Italic},
            {"__", TagType.Strong},
            {"# ", TagType.Header},
            {"", TagType.None}
        };
        private readonly Dictionary<TagType, string> supportedHtmlTags = new Dictionary<TagType, string>
        {
            {TagType.None, ""},
            {TagType.Strong, "strong"},
            {TagType.Italic, "em"},
            {TagType.Header, "h1"}
        };


        public MarkdownToHtmlConverter()
        {
            markdownParser = new MarkdownParser(supportedMarkdownTags);
            htmlGenerator = new HtmlGenerator(supportedHtmlTags);
        }

        public string Convert(string text)
        {
            throw new NotImplementedException();
        }
    }
}
