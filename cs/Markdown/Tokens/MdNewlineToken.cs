namespace Markdown.Tokens;

public class MdNewlineToken : IToken
{
    public string Value => "\n";
    public int Length => 1;
}