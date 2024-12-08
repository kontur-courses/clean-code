using System.Text;
using Markdown.Token;

namespace Markdown.MarkDownConverter.TagConverters;

public class TextConverter : ITagConverter
{
    public bool CanHandle(TokenType type) => type == TokenType.Text;

    public void Handle(Token.Token token, Stack<TokenType> tagStack, StringBuilder result)
    {
        result.Append(System.Net.WebUtility.HtmlEncode(token.Text));
    }
}