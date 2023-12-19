namespace Markdown.Tokens.Types;

public class BulletedToken : ITokenType
{
    public static readonly TagPair BulletedTokenOuterTag = new ("<ul>", "</ul>");
    public string Value => "* ";
    public bool SupportsClosingTag => false;
    public bool HasLineBeginningSemantics => true;
    public bool HasPredefinedValue => true;
    public TagPair OuterTag => BulletedTokenOuterTag;
    public string Representation(bool isClosingTag) => isClosingTag ? "</li>" : "<li>";
}