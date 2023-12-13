namespace Markdown.Tokens.Types;

public class EmphasisToken : ITokenType
{
    public bool ValueSupportsClosingTag => true;
    public string Representation(bool isClosingTag) => isClosingTag ? "</em>" : "<em>";
}