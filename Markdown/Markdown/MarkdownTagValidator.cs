namespace Markdown;

public class MarkdownTagValidator
{
    private string _markdown;

    public MarkdownTagValidator(string markdown)
    {
        this._markdown = markdown;
    }

    public bool IsTagPartCorrect(int start, bool isOpeningTag, int length)
    {
        var isTagScreened = (start > 0 && _markdown[start - 1] == '\\')
                            && (start > 1 && _markdown[start - 2] != '\\');
        if (isOpeningTag)
            return !isTagScreened && _markdown[start + length] != ' ';
        return !isTagScreened && _markdown[start - 1] != ' ';
    }

    public bool IsTagPartsSplittingWord(int start, int end)
    {
        var isStartSplittingWord =  start > 0 && char.IsLetter(_markdown[start - 1]);
        var isEndSplittingWord = end < _markdown.Length - 1 && char.IsLetter(_markdown[end + 1]);
        if (!isStartSplittingWord || !isEndSplittingWord)
            return false;
        return _markdown.Substring(start, end - start).Contains(' ');
    }

    public bool HasTagContentInside(int start, int end)
    {
        return !string.IsNullOrEmpty(_markdown.Substring(start, end - start + 1));
    }

    public bool HasTagDigitsInside(int start, int end)
    {
        return _markdown.Substring(start, end - start + 1).Any(char.IsDigit);
    }

    public bool IsNewLineStarted(int index)
    {
        return index == 0 || _markdown[index - 1] == '\n';
    }

    public bool IsListOpening(int index)
    {
        return _markdown[index] == '*' && IsNewLineStarted(index);
    }
}