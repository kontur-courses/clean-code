namespace Markdown.Tokens.Types;

public interface ITokenType
{
    public string Value { get; }
    public bool SupportsClosingTag { get; }
    public bool HasLineBeginningSemantics { get; }
    public string Representation(bool isClosingTag);
}