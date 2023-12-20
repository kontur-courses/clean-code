using System.Text;
using Markdown.Syntax;
using Markdown.Token;

namespace Markdown.Converter;

public class HtmlConverter : IConverter
{
    private ISyntax syntax;
    
    public HtmlConverter(ISyntax syntax)
    {
        this.syntax = syntax;
    }
    public string ConvertTags(IList<IToken> tags, string source)
    {
        var result = new StringBuilder();
        var i = 0;

        while (i < source.Length)
        {
            
        }
    }
}