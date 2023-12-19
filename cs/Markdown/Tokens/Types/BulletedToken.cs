namespace Markdown.Tokens.Types;

public class BulletedToken : ITokenType
{
    public string Value => "* ";
    public bool SupportsClosingTag => false;
    public bool HasLineBeginningSemantics => true;
    public bool HasPredefinedValue => true;
    public string Representation(bool isClosingTag) => isClosingTag ? "</li>" : "<li>";
}