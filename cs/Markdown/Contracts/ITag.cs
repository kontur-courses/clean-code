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
}