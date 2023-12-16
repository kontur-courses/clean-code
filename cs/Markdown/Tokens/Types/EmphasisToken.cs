namespace Markdown.Tokens.Types;

public class EmphasisToken : ITokenType
{
    public string Value => "_";
    public bool SupportsClosingTag => true;
    public bool HasLineBeginningSemantics => false;
    public bool HasPredefinedValue => true;
    public string Representation(bool isClosingTag) => isClosingTag ? "</em>" : "<em>";
}