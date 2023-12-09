namespace Markdown.Tokens.Types;

public class EscapeToken : ITokenType
{
    public string Representation(bool isClosingTag) => @"\";
}