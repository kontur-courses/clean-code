using System;
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
            Func<string, int, int, bool> additionalCheckForBold
                = delegate(string data, int startPos, int endPos)
                {
                    if (data.Length > endPos + 2 && data[endPos + 2] == '_')
                        return false;
                    return true;
                };

            var strongTagData = new StyleTagData(
                new TagBorder("__", "__"),
                new TagBorder(@"\<strong>", @"\</strong>"),
                additionalValidationCheck:additionalCheckForBold
            );
            
            Func<string, int, int, bool> additionalCheckForItalic
                = delegate(string data, int startPos, int endPos)
                {
                    if (data[endPos - 1] == '_')
                        return false;
                    
                    var underscoreCount = 0;
                    while (data.Length > endPos + underscoreCount + 1)
                    {
                        if (data[endPos + underscoreCount + 1] != '_')
                            break;
                        underscoreCount += 1;
                    }
                    return underscoreCount == 0 || underscoreCount == 2;
                };
            
            var italicTagData = new StyleTagData(
                new TagBorder("_", "_"), 
                new TagBorder(@"\<em>",@"\</em>"),
                additionalValidationCheck:additionalCheckForItalic,
                notAllowedNestedTags:strongTagData);
            
            var firstHeaderTagData = new HeaderTagData(
                new TagBorder("# ", ""), 
                new TagBorder(@"\<h1>",@"\</h1>")
            );
            
            // Добавил заголовок 2 уровня для демонстрации
            var secondHeaderTagData = new HeaderTagData(
                new TagBorder("## ", ""), 
                new TagBorder(@"\<h2>",@"\</h2>")
            );
            
            var bulletedListTagData = new TagData(
                new TagBorder("", ""),
                new TagBorder("\\<ul>\n","\n\\</ul>"), 
                EndOfLineAction.ContinueAndCompleteAtEOF);
            
            var bulletedLineTagData = new HeaderTagData(
                new TagBorder("+ ", ""), 
                new TagBorder(@"\<li>",@"\</li>"),
                parenTagData:bulletedListTagData
            );
            
            return new ITagData[]{italicTagData, strongTagData, 
                firstHeaderTagData, secondHeaderTagData, bulletedLineTagData};
        }
    }
}