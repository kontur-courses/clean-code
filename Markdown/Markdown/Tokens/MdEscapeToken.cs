namespace Markdown;

public class MdEscapeToken : IToken
{
    public string GetValue => "\\";
}