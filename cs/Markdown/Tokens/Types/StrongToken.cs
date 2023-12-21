namespace Markdown.Tokens.Types;

public class StrongToken : ITokenType
{
    public string Value => "__";
    public bool SupportsClosingTag => true;
    public bool HasLineBeginningSemantics => false;
    public bool HasPredefinedValue => true;
    public TagPair? OuterTag => null;
    public string Representation(bool isClosingTag) => isClosingTag ? "</strong>" : "<strong>";
}