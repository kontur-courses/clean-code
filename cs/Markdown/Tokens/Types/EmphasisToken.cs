namespace Markdown.Tokens.Types;

public class EmphasisToken : ITokenType
{
    public string Value => "_";
    public bool ValueSupportsClosingTag => true;
    public bool HasLineBeginningSemantics => false;
    public string Representation(bool isClosingTag) => isClosingTag ? "</em>" : "<em>";
}