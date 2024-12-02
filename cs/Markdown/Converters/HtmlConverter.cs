using System.Text;
using Markdown.Tokenizers;

namespace Markdown.Converters;

public class HtmlConverter : IConverter
{
    private static readonly Dictionary<TokenType, string> HtmlTag = new()
    {
        { TokenType.Italic, "em" },
        { TokenType.Strong, "strong" },
    };
    
    public string Convert(IEnumerable<Token> tokens)
    {
        var html = new StringBuilder();
        var isClosed = Enum.GetValues<TokenType>().ToDictionary(type => type, type => true);

        foreach (var token in tokens)
        {
            html.Append(token.Type switch
            {
                TokenType.Italic or TokenType.Strong => 
                    isClosed[token.Type] ? Tag.Open(HtmlTag[token.Type]) : Tag.Close(HtmlTag[token.Type]),
                _ => token.Content
            });

            isClosed[token.Type] = !isClosed[token.Type];
        }

        return html.ToString();
    }
}