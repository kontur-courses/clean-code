using Markdown.Tags;
using Markdown.Enums;


namespace Markdown.Interfaces;

public interface IMarkdownTag
{
    public TagStatus Status { get; set; }
    public TagType Type { get; }
    public MarkdownTagInfo Info { get; }
    public MarkdownContext MarkdownContext { get; set; }
    public void ChangeStatusIfBroken();

    char GetPreviousChar()
    {
        int position = MarkdownContext.Position;
        return position > 0 ? MarkdownContext.Text[position - 1] : '\0';
    }

    char GetNextChar()
    {
        int position = MarkdownContext.Position;
        int endPosition = position + Info.GlobalTag.Length;
        return endPosition < MarkdownContext.Text.Length ? MarkdownContext.Text[endPosition] : '\0';
    }

    public void SetTagStatus()
    {
        if (Type == TagType.Bold || Type == TagType.Italic || Type == TagType.ListItem)
        {
            if (char.IsWhiteSpace(GetPreviousChar()) && !char.IsWhiteSpace(GetNextChar()))
                Status = TagStatus.Open;
            else if (!char.IsWhiteSpace(GetPreviousChar()) && char.IsWhiteSpace(GetNextChar()))
                Status = TagStatus.Close;
            else if (GetPreviousChar() == '+' && GetNextChar() != '+')
                Status = TagStatus.Open;
            else if (GetPreviousChar() != '+' && GetNextChar() == '+')
                Status = TagStatus.Close;
        }
        else if (Type == TagType.Header)
        {
            Status = TagStatus.Open;
        }
        else if (Type == TagType.Newline)
        {
            Status = TagStatus.Undefined;
        }
        else
        {
            throw new Exception("Неизвестный тип тега");
        }
    }

    public bool IsClosingFor(IMarkdownTag another)
    {
        return another.Type == Type;
    }
}