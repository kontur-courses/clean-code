using Markdown.Tags;

namespace Markdown.Contracts;

public interface ITag
{
    public TagStatus Status { get; set; }
    public TagType Type { get; }
    public TagInfo Info { get; }
    public char PreviousChar => Context.Text[Context.Position - 1];
    public char NextChar => Context.Text[Context.Position + Info.GlobalMark.Length];
    public ContextInfo Context { get; set; }
    public void ChangeStatusIfBroken();

    public void SetTagStatus()
    {
        switch (Type)
        {
            case TagType.Bold:
            case TagType.Italic:
            {
                if (char.IsWhiteSpace(PreviousChar) && !char.IsWhiteSpace(NextChar))
                    Status = TagStatus.Open;
                else if (!char.IsWhiteSpace(PreviousChar) && char.IsWhiteSpace(NextChar))
                    Status = TagStatus.Close;
                break;
            }
            case TagType.Header:
                Status = TagStatus.Open;
                break;
            case TagType.Newline:
                Status = TagStatus.Undefined;
                break;
            default:
                throw new Exception("Tag type is unknown");
        }
    }

    public bool IsClosingFor(ITag another)
    {
        return another.Type == Type;
    }
}