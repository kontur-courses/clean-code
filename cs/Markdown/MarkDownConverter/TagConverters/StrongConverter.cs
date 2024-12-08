using System.Text;
using Markdown.Token;

namespace Markdown.MarkDownConverter.TagConverters;

public class StrongConverter : ITagConverter
{
    public bool CanHandle(TokenType type) => type == TokenType.Strong;

    public void Handle(Token.Token token, Stack<TokenType> tagStack, StringBuilder result)
    {
        if (token.State == TagState.Open)
        {
            result.Append("<strong>");
            tagStack.Push(token.Type);
        }
        else
        {
            if (tagStack.Count > 0 && tagStack.Peek() == token.Type)
            {
                result.Append("</strong>");
                tagStack.Pop();
            }
            else
            {
                result.Append(System.Net.WebUtility.HtmlEncode(token.Text));
            }
        }
    }
}