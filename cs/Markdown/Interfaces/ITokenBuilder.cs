using Markdown.Enums;
using Markdown.Tokens;

namespace Markdown.Interfaces;

public interface ITokenBuilder
{
    public Tag GetTag(int start, int end, TokenType type);
    public Text GetText(int start, string value);
}