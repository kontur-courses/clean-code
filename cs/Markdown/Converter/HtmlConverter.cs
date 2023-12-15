using Markdown.Syntax;
using Markdown.Token;

namespace Markdown.Converter;

public class HtmlConverter
{
    private ISyntax syntax;
    
    public HtmlConverter(ISyntax syntax)
    {
        this.syntax = syntax;
    }
    public string ConvertTags(IList<IToken> tags, string source)
    {
        throw new NotImplementedException();
    }
}