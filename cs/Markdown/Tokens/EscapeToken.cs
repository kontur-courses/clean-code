namespace Markdown.Tokens;

internal class EscapeToken : IToken
{
    public string Value => IsProcessed ? "" : "\\";
    public int Length => 1;
    private bool IsProcessed { get; set; }

    public void Escape(IToken token)
    {
        if (IsProcessed) 
            return;
        switch (token)
        {
            case TagToken tagToken:
                IsProcessed = true;
                tagToken.Status = TagStatus.Broken;
                break;
            case EscapeToken escapeToken:
                escapeToken.IsProcessed = true;
                break;
            
        }
    }
}