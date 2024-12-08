using System.Text;
using Markdown.Token;

namespace Markdown.MarkDownConverter.TagConverters;

public class ItalicConverter : ITagConverter
{
    public bool CanHandle(TokenType type) => type == TokenType.Italic;

    public void Handle(Token.Token token, Stack<TokenType> tagStack, StringBuilder result)
    {
        if (token.State == TagState.Open)
        {
            result.Append("<em>");
            tagStack.Push(token.Type);
        }
        else
        {
            if (tagStack.Count > 0 && tagStack.Peek() == token.Type)
            {
                result.Append("</em>");
                tagStack.Pop();
            }
            else
            {
                result.Append(System.Net.WebUtility.HtmlEncode(token.Text));
            }
        }
    }
}