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
            var parseTextData = markdownParser.Parse(rawText);
            return htmlMarkupBuilder.Build(parseTextData);
        }

        private ITagData[] CreateMdToHtmlTags()
        {
            var italicTagData = new StyleTagData(
                new TagBorder("_", "_"), 
                new TagBorder(@"\<em>",@"\</em>")
                );
            
            var strongTagData = new StyleTagData(
                new TagBorder("__", "__"), 
                new TagBorder(@"\<strong>",@"\</strong>")
                );

            /*var headerTagData = new HeaderTagData(
                new TagBorder("# ", ""), 
                new TagBorder(@"\<h1>","\\</h1>")
                );*/
            
            return new ITagData[]{italicTagData, strongTagData, /*headerTagData*/};
        }
    }
}