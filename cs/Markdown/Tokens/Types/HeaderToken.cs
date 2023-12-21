namespace Markdown.Tokens.Types;

public class HeaderToken : ITokenType
{
    public string Value => "# ";
    public bool SupportsClosingTag => false;
    public bool HasLineBeginningSemantics => true;
    public bool HasPredefinedValue => true;
    public TagPair? OuterTag => null;
    public string Representation(bool isClosingTag) => isClosingTag ? "</h1>" : "<h1>";
}