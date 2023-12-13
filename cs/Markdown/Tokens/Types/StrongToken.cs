namespace Markdown.Tokens.Types;

public class StrongToken : ITokenType
{
    public bool ValueSupportsClosingTag => true;
    public bool HasLineBeginningSemantics => false;
    public string Representation(bool isClosingTag) => isClosingTag ? "</strong>" : "<strong>";
}