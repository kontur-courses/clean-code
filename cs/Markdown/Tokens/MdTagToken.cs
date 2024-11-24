using Markdown.Tags;

namespace Markdown.Tokens;

public class MdTagToken(ITag tag) : IToken
{
    public ITag Tag { get; } = tag;
    public Status Status { get; set; }
    public string Value => GetValueByStatus();
    public int Length => Tag.MdTag.Length;
    public (char left, char right) AdjacentChars { get; private set; }
    public bool IsInsideWord => char.IsLetterOrDigit(AdjacentChars.left) && 
                                char.IsLetterOrDigit(AdjacentChars.right);

    public void SetContext(char left, char right)
    {
        AdjacentChars = (left, right);
    }
    
    private string GetValueByStatus()
    {
        throw new NotImplementedException();
    }
}