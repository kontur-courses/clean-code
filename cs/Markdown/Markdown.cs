using Markdown.Converter;
using Markdown.Syntax;

namespace Markdown;

public class Markdown
{
    public string Render(string text, ISyntax syntax)
    {
        var processor = new Processor.Processor(text);
        var tagTokens = processor.ParseTags();
        var converter = new HtmlConverter(syntax);
        return converter.ConvertTags(tagTokens, text);
    }
}