namespace Markdown.Tokens.Types;

public class HeaderToken : ITokenType
{
    public string Representation(bool isClosingTag) => isClosingTag ? "</h1>" : "<h1>";
}