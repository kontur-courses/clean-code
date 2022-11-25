using System.Text;
using Markdown.Helpers;
using Markdown.Tags;

namespace Markdown.Translators.Implementation;

public class MarkdownTranslator : ITranslator
{
    private static readonly List<char>? StartCharOfTags = new();
    private Stack<TagWithIndex> stackOfTags;
    private StringBuilder textWithTranslate;
    private int index;
    private int deleteScreenSymbolsCount;
    private readonly Dictionary<string, ITag?> tags;

    private readonly Dictionary<ITag, (int startCount, int endCount)> tagsOpenCloseCounter = new();

    public MarkdownTranslator()
    {
        tags = new Dictionary<string, ITag?>();
        textWithTranslate = new StringBuilder();

        foreach (var item in TagHelper.GetAllTags<ITag>()!)
        {
            tags.Add(item!.SourceName, item);
            tagsOpenCloseCounter.Add(item, (0, 0));
            StartCharOfTags?.Add(item.SourceName[0]);
        }

        stackOfTags = new Stack<TagWithIndex>();
    }

    public string Translate(string input)
    {
        textWithTranslate = new StringBuilder();
        stackOfTags = new Stack<TagWithIndex>();
        while (index < input.Length)
        {
            var token = GetNextToken(input);
            index += token.Length;

            textWithTranslate.Append(ParseToken(token, input));
        }

        foreach (var tag in stackOfTags)
        {
            if (tagsOpenCloseCounter[tag.Tag!].startCount == tagsOpenCloseCounter[tag.Tag!].endCount)
                ReplaceMdTag(tag);
            else
                tagsOpenCloseCounter[tag.Tag!] = (tagsOpenCloseCounter[tag.Tag!].startCount - 1,
                    tagsOpenCloseCounter[tag.Tag!].endCount);
        }

        return textWithTranslate.ToString();
    }

    private string ParseToken(string token, string text)
    {
        var startIndex = index - token.Length;
        var previousTag = default(ITag);
        if (IsLetter(text[startIndex]) || IsSpecialSymbol(text[startIndex]))
            return token;

        var hasTag = tags.TryGetValue(token, out var tag);
        if (!hasTag)
            return token;

        var isStart = TagMostBeStart(tag);
        if (stackOfTags.Count > 0)
            previousTag = stackOfTags.Peek().Tag;
        
        if (IsCorrectStart(tag, index, text) && isStart && !PreviousValueIsScreen(startIndex, text))
        {
            stackOfTags.Push(new TagWithIndex(tag, startIndex, true));
            tagsOpenCloseCounter[tag!] =
                (tagsOpenCloseCounter[tag!].startCount + 1, tagsOpenCloseCounter[tag!].endCount);
        }
        else if (IsCorrectEnding(tag, index, text) && !isStart && !CheckIntersections(tag!, previousTag, index, text) &&
                 !PreviousValueIsScreen(startIndex, text)) 
        {
            stackOfTags.Push(new TagWithIndex(tag, startIndex, false));
            tagsOpenCloseCounter[tag!] =
                (tagsOpenCloseCounter[tag!].startCount, tagsOpenCloseCounter[tag!].endCount + 1);
        }
        else if (CheckIntersections(tag!, previousTag, index, text) && !isStart)
        {
            tagsOpenCloseCounter!.Reboot();
            stackOfTags.Clear();
        }
        else if (PreviousValueIsScreen(startIndex, text))
        {
            RemoveScreenValue(startIndex - 1);
        }

        return token;
    }

    private void RemoveScreenValue(int indexToRemove) =>
        textWithTranslate.Remove(indexToRemove - deleteScreenSymbolsCount++, 1);

    private static bool PreviousValueIsScreen(int currentIndex, string text) =>
        currentIndex - 1 >= 0 && text[currentIndex - 1] == '\\';

    private bool TagMostBeStart(ITag? tag) =>
        tagsOpenCloseCounter[tag!].startCount <= tagsOpenCloseCounter[tag!].endCount;

    private string GetNextToken(string text)
    {
        if (IsLetter(text[index]))
            return ReadForNow(IsLetter, text);

        return IsTag(text[index], index) ? ReadForNow(IsTag, index, text) : ReadForNow(IsSpecialSymbol, text);
    }

    private static bool IsLetter(char symbol) => char.IsLetter(symbol);

    private static bool IsSpecialSymbol(char symbol) => !IsLetter(symbol) && !IsTag(symbol, 0);

    private static bool IsTag(char symbol, int index) => StartCharOfTags!.Contains(symbol);

    private bool IsTag(int textIndexFrom, int textIndexTo, int tagIndexTo, string myText) => // TODO: Change
        tags.Where(tag => tag.Value?.SourceName.Length >= tagIndexTo + 1)
            .Count(tag =>
                tag.Value?.SourceName[..(tagIndexTo + 1)] == myText.Substring(textIndexFrom, tagIndexTo + 1)) > 0;

    private static string ReadForNow(Func<char, bool> func, int index, string currentText)
    {
        var symbols = new StringBuilder();
        while (index < currentText.Length)
        {
            if (!func(currentText[index]))
                return symbols.ToString();
            symbols.Append(currentText[index]);
            index++;
        }

        return symbols.ToString();
    }

    private string ReadForNow(Func<char, bool> func, string text) => ReadForNow(func, index, text);

    private static string ReadForNow(Func<int, int, int, string, bool> func, int index, string currentText)
    {
        if (index < 0)
            return string.Empty;

        var startIndex = index;
        var tagIndex = 0;
        var symbols = new StringBuilder();
        while (index < currentText.Length)
        {
            if (!func(startIndex, index, tagIndex, currentText))
                return symbols.ToString();
            symbols.Append(currentText[index]);
            index++;
            tagIndex++;
        }

        return symbols.ToString();
    }

    private void ReplaceMdTag(TagWithIndex tagWithIndex)
    {
        var translateName = TagHelper.GetHtmlFormat(tagWithIndex.Tag!.TranslateName);
        textWithTranslate.Replace(tagWithIndex.Tag.SourceName,
            tagWithIndex.IsStartedTag ? translateName.start : translateName.end,
            tagWithIndex.Index, tagWithIndex.Tag.SourceName.Length);
    }

    // private bool IsCorrectStart(Stack<TagWithIndex> tagsStack, ITag? tag, int currentIndex, string text)
    // {
    //     if (currentIndex < text.Length && IsLetter(text[currentIndex]))
    //     {
    //         var startIndex = currentIndex - 1;
    //         var lettersNext = ReadForNow(IsLetter, currentIndex, text);
    //         currentIndex += lettersNext.Length;
    //         var isNeedsTag = ReadForNow(IsTag, currentIndex, text);
    //
    //         if ((isNeedsTag == tag?.SourceName || isNeedsTag == "" && startIndex - 1 < 0 ||
    //              startIndex - 1 >= 0 && !IsLetter(text[startIndex - 1])) && (tagsStack.Count <= 0 ||
    //                                                                          tagsStack.Peek().Tag!.SourceName != "_" ||
    //                                                                          tagsStack.Peek().IsStartedTag is false ||
    //                                                                          tag?.SourceName != "__" ||
    //                                                                          currentIndex - 1 < 0 ||
    //                                                                          IsNumber(text[currentIndex - 1]) ||
    //                                                                          currentIndex + 1 > text.Length ||
    //                                                                          IsNumber(text[currentIndex + 1]))) 
    //             return true;
    //     }
    //
    //     return false;
    // }

    private static bool IsCorrectStart(ITag? tag, int currentIndex, string text)
    {
        if (currentIndex >= text.Length || !IsLetter(text[currentIndex])) 
            return false;
        
        var previousIndex = currentIndex - tag!.SourceName.Length - 1;
        var substringFromTag = text.Substring(currentIndex, text.Length - currentIndex);
        var substrings = substringFromTag.Split(" ");
            
        return substrings[0].Contains(tag.SourceName) || previousIndex == -1 ||
               previousIndex > 0 && !IsLetter(text[previousIndex]);
    }

    private static bool IsCorrectEnding(ITag? tag, int currentIndex, string text) =>
        currentIndex - (tag!.SourceName.Length + 1) >= 0 && IsLetter(text[currentIndex - (tag.SourceName.Length + 1)]);

    private bool CheckIntersections(ITag tag, ITag? previousTag, int currentIndex, string text)
    {
        if (previousTag is null || previousTag == tag)
            return false;

        var substring = text.Substring(currentIndex, text.Length - currentIndex);
        var nextTagStartedFrom = substring.IndexOf(previousTag.SourceName, StringComparison.Ordinal);
        return ReadForNow(IsTag, nextTagStartedFrom, substring) == previousTag.SourceName;
    }
}