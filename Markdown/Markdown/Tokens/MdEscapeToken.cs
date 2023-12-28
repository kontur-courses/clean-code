namespace Markdown;

public class MdEscapeToken : IToken
{
    public string Value => isEscaped ? "" : "\\";
    public int Length => 1;
    private bool isEscaped;

    public void Escape(IToken token)
    {
        if (isEscaped) return;
        if (token is MdEscapeToken escapeToken)
            escapeToken.isEscaped = true;
        if (token is MdTagToken tagToken)
        {
            isEscaped = true;
            tagToken.Status = Status.Broken;
        }
    }
}