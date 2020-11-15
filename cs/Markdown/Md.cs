using Markdown.Tag;
using Markdown.Parser;
using Markdown.Builder;
using Markdown.Tag.SpecificTags;

namespace Markdown
{
    public class Md
    {
        private MarkupParser markdownParser;
        private MarkupBuilder htmlMarkupBuilder;

        public Md()
        {
            var tags = CreateMdToHtmlTags();
            markdownParser = new MarkupParser(tags);
            htmlMarkupBuilder = new MarkupBuilder(tags);
        }
        
        public string Render(string rawText)
        {
            var textTokens = markdownParser.Parse(rawText);
            return htmlMarkupBuilder.Build(textTokens);
        }

        private ITagData[] CreateMdToHtmlTags()
        {
            var italicTagData = new StyleTagData(FormattingState.Strong,
                new TagBorder("_", "_"), 
                new TagBorder(@"\<em>",@"\</em>"),
                FormattingState.Header);
            
            var strongTagData = new StyleTagData(FormattingState.Strong,
                new TagBorder("__", "__"), 
                new TagBorder(@"\<strong>",@"\</strong>"),
                FormattingState.Italic, FormattingState.Header);

            /*var headerTagData = new HeaderTagData(FormattingState.Header,
                new TagBorder("# ", ""), 
                new TagBorder(@"\<h1>","\\</h1>"));*/
            
            return new ITagData[]{italicTagData, strongTagData, /*headerTagData*/};
        }
    }
}