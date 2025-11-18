using Markdown.Tags;

namespace Markdown.Tokens;

internal class TagToken : IToken
{
    public ITag Tag { get; }
    
    public TagStatus Status { get; set; }
    public char Left { get; }
    public char Right { get; }
    public string Value => Status switch
    {
        TagStatus.Opened => Tag.HtmlOpenTag,
        TagStatus.Closed => Tag.HtmlCloseTag,
        TagStatus.Broken => Tag.MdTag,
        _ => throw new ArgumentOutOfRangeException($"Unknown tag status: {Status}")
    };
    
    public int Length => Tag.MdTag.Length;
    public bool IsInWord => char.IsLetterOrDigit(Left) && char.IsLetterOrDigit(Right);


    public TagToken(ITag tag, char leftChar, char rightChar)
    {
        Tag = tag;
        Left = leftChar;
        Right = rightChar;
    }
}