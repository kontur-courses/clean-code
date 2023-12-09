namespace Markdown.Tokens.Types;

public class StrongToken : ITokenType
{
    public string Representation(bool isClosingTag) => isClosingTag ? "</strong>" : "<strong>";
}