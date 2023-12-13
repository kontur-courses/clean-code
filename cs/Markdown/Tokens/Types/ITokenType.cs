namespace Markdown.Tokens.Types;

public interface ITokenType
{
    public bool ValueSupportsClosingTag { get; }
    public bool HasLineBeginningSemantics { get; }
    public string Representation(bool isClosingTag);
}