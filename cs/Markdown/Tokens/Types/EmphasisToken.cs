namespace Markdown.Tokens.Types;

public class EmphasisToken : ITokenType
{
    public string Representation(bool isClosingTag) => isClosingTag ? "</em>" : "<em>";
}