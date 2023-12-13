namespace Markdown.Tokens.Types;

public class HeaderToken : ITokenType
{
    public bool ValueSupportsClosingTag => false;
    public bool HasLineBeginningSemantics => true;
    public string Representation(bool isClosingTag) => isClosingTag ? "</h1>" : "<h1>";
}