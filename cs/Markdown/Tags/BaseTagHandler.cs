using System.Runtime.InteropServices;

namespace Markdown.Tags;

public abstract class BaseTagHandler : ITagHandler
{
    protected abstract string Symbol { get; }
    protected abstract string HtmlTag { get; }

    public abstract bool IsTagStart(string text, int index);

    public virtual int ProcessTag(ref string text, int startIndex)
    {
        if (HelperFunctions.ContainsOnlyDash(text.Substring(startIndex)))
            return text.Length;
        if (HelperFunctions.ContainsUnderscore(text) && CheckTagIntersections(text))
            return text.Length;

        var endIndex = 0;
        if (startIndex + Symbol.Length < text.Length && char.IsDigit(text[startIndex + Symbol.Length]))
            endIndex = FindEndIndexForDigit(text, startIndex);
        else
            endIndex = FindEndIndex(text, startIndex);

        if (endIndex == -1)
            return startIndex + Symbol.Length;

        var content = ExtractContent(text, startIndex, endIndex);
        if (HelperFunctions.ContainsOnlySpases(content))
            return StringOnlySpaces(ref text, endIndex, Symbol.Length);
        if (!AreTagsCorrectlyPositioned(text, startIndex, endIndex, content))
            return startIndex + content.Length;

        content = ProcessNestedTag(ref content);
        var replacement = WrapWithHtmlTag(content);
        text = ReplaceText(text, startIndex, endIndex, replacement);

        return startIndex + replacement.Length;
    }

    private int FindEndIndexForDigit(string text, int startIndex)
    {
        var currentIndex = startIndex + Symbol.Length;

        while (currentIndex < text.Length)
        {
            currentIndex = text.IndexOf(Symbol, currentIndex);
            if (currentIndex == -1)
                return -1;

            if (currentIndex + Symbol.Length < text.Length && char.IsDigit(text[currentIndex + Symbol.Length]))
            {
                currentIndex += Symbol.Length;
                continue;
            }

            if (currentIndex + Symbol.Length == text.Length || char.IsWhiteSpace(text[currentIndex + Symbol.Length]))
                return currentIndex;

            currentIndex += Symbol.Length;
        }

        return -1;
    }

    protected virtual int FindEndIndex(string text, int startIndex)
    {
        var currentIndex = startIndex + Symbol.Length;

        while (currentIndex < text.Length)
        {
            currentIndex = text.IndexOf(Symbol, currentIndex);
            if (currentIndex == -1)
                return -1;

            if (currentIndex + Symbol.Length < text.Length && char.IsDigit(text[currentIndex + Symbol.Length]))
            {
                currentIndex += Symbol.Length;
                continue;
            }
            if (currentIndex > 0 && char.IsDigit(text[currentIndex - 1]))
            {
                if (currentIndex + Symbol.Length == text.Length ||
                    text.Substring(currentIndex + Symbol.Length).All(char.IsWhiteSpace))

                    return currentIndex;

                currentIndex += Symbol.Length;
                continue;
            }
            return currentIndex;
        }

        return -1;
    }

    private bool CheckTagIntersections(string text)
    {
        (List<int> singleUnderscoreIndexes, List<int> doubleUnderscoreIndexes) = HelperFunctions.GetUnderscoreIndexes(text);

        if (!HelperFunctions.HasUnpairedTags(singleUnderscoreIndexes, doubleUnderscoreIndexes))
            return false;

        for (int i = 0; i < singleUnderscoreIndexes.Count - 1; i++)
        {
            for (int j = 0; j < doubleUnderscoreIndexes.Count - 1; j++)
            {
                var segment1 = Tuple.Create(singleUnderscoreIndexes[i], singleUnderscoreIndexes[i + 1]);
                var segment2 = Tuple.Create(doubleUnderscoreIndexes[j], doubleUnderscoreIndexes[j + 1]);
                if (HelperFunctions.AreSegmentsIntersecting(segment1, segment2) &&
                    !HelperFunctions.AreSegmentsNested(segment1, segment2))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private static bool AreTagsCorrectlyPositioned(string text, int startIndex, int endIndex, string content)
    {
        if (!content.Contains(' '))
            return true;
        if (startIndex - 1 >= 0 && (char.IsLetter(text[startIndex - 1]) || char.IsDigit(text[startIndex - 1])))
            return false;
        if (endIndex + 1 < text.Length && (char.IsLetter(text[endIndex + 1]) || char.IsDigit(text[endIndex + 1])))
            return false;
        if (char.IsWhiteSpace(content.First()) || char.IsWhiteSpace(content.Last()))
            return false;
        return true;
    }

    protected virtual string ProcessNestedTag(ref string text)
    {
        return HelperFunctions.ProcessNestedTag(ref text);
    }

    protected virtual string ExtractContent(string text, int startIndex, int endIndex)
    {
        return text.Substring(startIndex + Symbol.Length, endIndex - startIndex - Symbol.Length);
    }

    protected virtual string WrapWithHtmlTag(string content)
    {
        return $"<{HtmlTag}>{content}</{HtmlTag}>";
    }

    protected virtual string ReplaceText(string text, int startIndex, int endIndex, string replacement)
    {
        return text.Substring(0, startIndex) + replacement + text.Substring(endIndex + Symbol.Length);
    }

    protected virtual int StringOnlySpaces(ref string text, int endIndex, int symbolLength)
    {
        text = text.Substring(endIndex + symbolLength);
        return endIndex;
    }
}