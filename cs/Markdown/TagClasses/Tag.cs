namespace Markdown.TagClasses;

public abstract class Tag
{
    public abstract string Name { get; }
    public abstract string MarkdownOpening { get; }
    public abstract string MarkdownClosing { get; }
    public abstract bool ShouldHavePair { get; }
    public virtual string HtmlTagOpen => $"<{Name}>";
    public virtual string HtmlTagClose => $"</{Name}>";

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
                                && !char.IsWhiteSpace(markdownText[endIndex - MarkdownClosing.Length]);
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


        var insideStart = currentTagStartIndex + MarkdownOpening.Length;
        var insideEnd = otherTagEndIndex - MarkdownClosing.Length + 1;

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

    public abstract bool CantBeInsideTags(IEnumerable<Tag> tagsContext);

    public bool InWord(string markdownText, int startIndex)
    {
        return startIndex > 0
               && startIndex < markdownText.Length - MarkdownOpening.Length
               && (char.IsLetter(markdownText[startIndex - 1]) || markdownText[startIndex - 1] == '\\')
               && (char.IsLetter(markdownText[startIndex + MarkdownOpening.Length])
                   || markdownText[startIndex + MarkdownOpening.Length] == '\\');
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