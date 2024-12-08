using System.Text;
using Markdown.Token;

namespace Markdown.MarkDownConverter.TagConverters;

public class LinkConverter : ITagConverter
{
    public bool CanHandle(TokenType type) => type == TokenType.Link;

    public void Handle(Token.Token token, Stack<TokenType> tagStack, StringBuilder result)
    {
        result.Append($"<a href=\"{token.Url}\">{System.Net.WebUtility.HtmlEncode(token.Text)}</a>");
    }
}