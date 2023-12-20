using Markdown.Converter;
using Markdown.Syntax;
using Markdown.Token;

namespace Markdown;

public class Markdown
{
    public string Render(string text, ISyntax syntax)
    {
        var processor = new Processor.AnySyntaxParser(text, syntax);
        var tagTokens = processor.ParseTokens();
        var converter = new HtmlConverter(syntax);
        return converter.ConvertTags(tagTokens, text);
    }
}