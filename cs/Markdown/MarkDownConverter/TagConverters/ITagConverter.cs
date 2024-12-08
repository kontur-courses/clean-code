using System.Text;
using Markdown.Token;

namespace Markdown.MarkDownConverter.TagConverters;

public interface ITagConverter
{
    bool CanHandle(TokenType type);
    void Handle(Token.Token token, Stack<TokenType> tagStack, StringBuilder result);
}