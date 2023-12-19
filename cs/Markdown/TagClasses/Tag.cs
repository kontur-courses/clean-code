namespace Markdown.TagClasses;

public abstract class Tag
{
    public abstract string Name { get; }
    public abstract string MarkdownOpening { get; }
    public abstract string MarkdownClosing { get; }
    public abstract string HtmlTagOpen { get; }
    public abstract string HtmlTagClose { get; }
    public abstract bool ShouldHavePair { get; }

    public virtual bool IsMarkdownOpening(string markdownText, int startIndex)
    {
        if (InNumber(markdownText, startIndex))
            return false;

        var leftSpace = startIndex < 1 || char.IsWhiteSpace(markdownText[startIndex - 1]);
        var rightNotWhiteSpace = startIndex < markdownText.Length - MarkdownOpening.Length 
                                 && !char.IsWhiteSpace(markdownText[startIndex + MarkdownOpening.Length]);

        return leftSpace && rightNotWhiteSpace;
    }

    public virtual bool IsMarkdownClosing(string markdownText, int endIndex)
    {
        if (InNumber(markdownText, endIndex))
            return false;

        var leftNotWhiteSpace = endIndex > MarkdownClosing.Length - 1 
                                && !char.IsWhiteSpace(markdownText[endIndex - 1]);
        var rightSpace = endIndex >= markdownText.Length - MarkdownClosing.Length 
                         || char.IsWhiteSpace(markdownText[endIndex + MarkdownClosing.Length]);

        return leftNotWhiteSpace && rightSpace;
    }

    public virtual bool CanBeOpened(string markdownText, int startIndex)
    {
        return IsMarkdownOpening(markdownText, startIndex) || InWord(markdownText, startIndex);
    }

    public virtual bool CanBePairedWith(string markdownText, int currentTagStartIndex, Tag? otherTag,
        int otherTagStartIndex)
    {
        if (otherTag.GetType() != this.GetType())
            return false;


        var insideStart = currentTagStartIndex + MarkdownOpening.Length;

        var betweenTags = markdownText.Substring(insideStart, otherTagStartIndex - insideStart);

        if (otherTag.InWord(markdownText, otherTagStartIndex))
        {
            var checkForSpaces = betweenTags.Contains(' ');
            return !checkForSpaces;
        }

        if (betweenTags.Length < 1)
            return false;

        return otherTag.IsMarkdownClosing(markdownText, otherTagStartIndex);
    }

    public abstract bool CantBeInsideTags(IEnumerable<Tag> tagsContext);

    public bool InWord(string markdownText, int startIndex)
    {
        return startIndex > 0
               && startIndex < markdownText.Length - MarkdownOpening.Length
               && (char.IsLetter(markdownText[startIndex - 1]) || char.IsPunctuation(markdownText[startIndex - 1]))
               && (char.IsLetter(markdownText[startIndex + MarkdownOpening.Length]) || char.IsPunctuation(markdownText[startIndex + MarkdownOpening.Length]));
    }

    public bool InNumber(string markdownText, int startIndex)
    {
        var leftNumber = startIndex > 0 && char.IsNumber(markdownText[startIndex - 1]);
        var leftLetter = startIndex > 0 && char.IsLetter(markdownText[startIndex - 1]);
        var rightNumber = startIndex < markdownText.Length - MarkdownOpening.Length
                          && char.IsNumber(markdownText[startIndex + MarkdownOpening.Length]);
        var rightLetter = startIndex < markdownText.Length - MarkdownOpening.Length
                          && char.IsLetter(markdownText[startIndex + MarkdownOpening.Length]);

        return ((leftLetter && rightNumber)
                   || (leftNumber && rightLetter)
                   || (leftNumber && rightNumber));
    }

    public override string ToString()
    {
        return Name;
    }
}