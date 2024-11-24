namespace Markdown.Tokens;

public class MdEscapeToken : IToken
{
    public string Value => isEscaped ? "" : "\\";
    public int Length => 1;
    private bool isEscaped;

    public void Escape(IToken token)
    {
        isEscaped = true;
        //TODO
    }
}