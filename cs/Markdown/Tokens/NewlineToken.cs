namespace Markdown.Tokens;

internal class NewlineToken : IToken
{
    public string Value => "\n";
    public int Length => 1;
}