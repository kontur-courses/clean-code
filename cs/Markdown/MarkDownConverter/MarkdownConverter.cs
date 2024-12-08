using System.Text;
using Markdown.interfaces;
using Markdown.MarkDownConverter.TagConverters;
using Markdown.Token;

namespace Markdown.MarkDownConverter;

public class MarkdownConverter : IMarkdownConverter
{
    private readonly IList<ITagConverter> tagConverters;

    public MarkdownConverter()
    {
        tagConverters = new List<ITagConverter>
        {
            new TextConverter(),
            new StrongConverter(),
            new ItalicConverter(),
            new HeaderConverter(),
            new LinkConverter()
        };
    }

    public string Convert(IEnumerable<Token.Token> tokens)
    {
        var result = new StringBuilder();
        var tagStack = new Stack<TokenType>();

        foreach (var token in tokens)
        {
            var converter = tagConverters.FirstOrDefault(c => c.CanHandle(token.Type));
            if (converter != null)
            {
                converter.Handle(token, tagStack, result);
            }
            else
            {
                result.Append(System.Net.WebUtility.HtmlEncode(token.Text));
            }
        }

        while (tagStack.Count > 0)
        {
            var openTag = tagStack.Pop();
            result.Append(GetClosingTag(openTag));
        }

        return result.ToString();
    }

    private string GetClosingTag(TokenType type)
    {
        return type switch
        {
            TokenType.Strong => "</strong>",
            TokenType.Italic => "</em>",
            TokenType.Header => "</h1>",
            _ => string.Empty
        };
    }
}