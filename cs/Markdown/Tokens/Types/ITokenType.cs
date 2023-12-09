namespace Markdown.Tokens.Types;

public interface ITokenType
{
    public string Representation(bool isClosingTag);
}