using Markdown.TagClasses.TagModels;

namespace Markdown.TagClasses;

public abstract class Tag
{
    public readonly TagModel Model;

    public Tag(TagModel model)
    {
        Model = model;
    }

    public virtual bool IsMarkdownOpening(string markdownText, int startIndex)
    {
        if (InNumber(markdownText, startIndex))
            return false;

        var leftSpace = startIndex < 1 || char.IsWhiteSpace(markdownText[startIndex - 1]);
        var rightNotWhiteSpace = startIndex < markdownText.Length - Model.MarkdownOpening.Length
                                 && !char.IsWhiteSpace(markdownText[startIndex + Model.MarkdownOpening.Length]);

        return leftSpace && rightNotWhiteSpace;
    }

    public virtual bool IsMarkdownClosing(string markdownText, int endIndex)
    {
        if (InNumber(markdownText, endIndex))
            return false;

        var leftNotWhiteSpace = endIndex > Model.MarkdownClosing.Length - 1
                                && !char.IsWhiteSpace(markdownText[endIndex - Model.MarkdownClosing.Length]);
        var rightSpace = endIndex <= markdownText.Length - 1
                         || char.IsWhiteSpace(markdownText[endIndex + 1]);

        return leftNotWhiteSpace && rightSpace;
    }

    public virtual bool CanBeOpened(string markdownText, int startIndex)
    {
        return IsMarkdownOpening(markdownText, startIndex) || InWord(markdownText, startIndex);
    }

    public virtual bool CanBePairedWith(string markdownText, int currentTagStartIndex, Tag? otherTag,
        int otherTagEndIndex)
    {
        if (otherTag.GetType() != this.GetType())
            return false;


        var insideStart = currentTagStartIndex + Model.MarkdownOpening.Length;
        var insideEnd = otherTagEndIndex - Model.MarkdownClosing.Length + 1;

        var betweenTags = markdownText.Substring(insideStart, insideEnd - insideStart);

        if (otherTag.InWord(markdownText, insideEnd))
        {
            var checkForSpaces = betweenTags.Contains(' ');
            return !checkForSpaces;
        }

        if (betweenTags.Length < 1)
            return false;

        return otherTag.IsMarkdownClosing(markdownText, otherTagEndIndex);
    }

    public virtual bool CantBeInsideTags(IEnumerable<Tag> tagsContext)
    {
        return false;
    }

    public bool InWord(string markdownText, int startIndex)
    {
        return startIndex > 0
               && startIndex < markdownText.Length - Model.MarkdownOpening.Length
               && (char.IsLetter(markdownText[startIndex - 1]) || markdownText[startIndex - 1] == '\\')
               && (char.IsLetter(markdownText[startIndex + Model.MarkdownOpening.Length])
                   || markdownText[startIndex + Model.MarkdownOpening.Length] == '\\');
    }

    public bool InNumber(string markdownText, int startIndex)
    {
        var leftNumber = startIndex > 0 && char.IsNumber(markdownText[startIndex - 1]);
        var leftLetter = startIndex > 0 && char.IsLetter(markdownText[startIndex - 1]);
        var rightNumber = startIndex < markdownText.Length - Model.MarkdownOpening.Length
                          && char.IsNumber(markdownText[startIndex + Model.MarkdownOpening.Length]);
        var rightLetter = startIndex < markdownText.Length - Model.MarkdownOpening.Length
                          && char.IsLetter(markdownText[startIndex + Model.MarkdownOpening.Length]);

        return ((leftLetter && rightNumber)
                   || (leftNumber && rightLetter)
                   || (leftNumber && rightNumber));
    }

    public override string ToString()
    {
        return Model.ToString();
    }
}