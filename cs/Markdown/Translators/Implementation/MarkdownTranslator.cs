using System.Globalization;
using System.Text;
using Markdown.Helpers;
using Markdown.Tags;

namespace Markdown.Translators.Implementation;

public class MarkdownTranslator : ITranslator
{
    private static readonly List<char>? StartCharOfTags = new();
    private const int OpenCloseTagLength = 5;
    private Stack<TagWithIndex> stackOfTags;
    private StringBuilder textWithTranslate;
    private readonly List<TagWithIndex> tagsInLine;
    private int index;
    private Dictionary<string, ITag?> tags;

    public MarkdownTranslator()
    {
        tags = new Dictionary<string, ITag?>();
        
        foreach (var item in TagHelper.GetAllTags<ITag>()!)
        {
            tags.Add(item!.SourceName, item);
            StartCharOfTags?.Add(item.SourceName[0]);
        }
        
        tagsInLine = new List<TagWithIndex>();
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
        //
        // if (stackOfTags.Count != 0)
        //     PasteSourceNames(stackOfTags, null);

        return textWithTranslate.ToString();
    }

    private string ParseToken(string token, string text)
    {
        var startIndex = index - token.Length;
        if (IsLetter(text[startIndex]) || IsSpecialSymbol(text[startIndex]))
            return token;

        var hasTag = tags.TryGetValue(token, out var tag);
        if (!hasTag)
            return token;

        if (IsCorrectStart(stackOfTags, tag, index, text) &&
            (stackOfTags.Count == 0 || stackOfTags.All(tagWith => tagWith.Tag != tag))) // TODO: Stack
        {
            stackOfTags.Push(new TagWithIndex(tag, startIndex));
            return token;
        }
        if (!IsCorrectEnding(tag, index, text) || stackOfTags.All(tagWith => tagWith.Tag != tag))  // TODO: Stack
            return token;
        
        var currentTag = stackOfTags.First(tagWith => tagWith.Tag == tag);
        ReplaceByIndex(currentTag.Tag, index, currentTag.Index, text);
        return string.Empty;
    }

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

    private static bool IsNumber(char symbol) => char.IsNumber(symbol);

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

    private void ReplaceByIndex(ITag? tag, int currentIndex, int tagIndex, string text)
    {
        if (stackOfTags.Peek().Tag == tag)
            stackOfTags.Pop();
        else if (CheckIntersections(tag!, currentIndex, text))
        {
            textWithTranslate.Append(tag.SourceName);
            stackOfTags.Clear();
            return;
        }

        var translateName = TagHelper.GetHtmlFormat(tag!.TranslateName);
        var indexPreviewItems = tagsInLine
            .Where(tagWithIndex => tagWithIndex.Index < tagIndex)
            .Sum(item => item.Tag!.TranslateName.Length * 2 + OpenCloseTagLength - item.Tag.SourceName.Length * 2);

        textWithTranslate.Replace(tag.SourceName, translateName.start, tagIndex + indexPreviewItems, tag.SourceName.Length);
        textWithTranslate.Append(translateName.end);
        tagsInLine.Add(new TagWithIndex(tag, currentIndex));
    }

    private bool IsCorrectStart(Stack<TagWithIndex> tagsStack, ITag? tag, int currentIndex, string text)
    {
        if (currentIndex < text.Length && IsLetter(text[currentIndex]))
        {
            var startIndex = currentIndex - 1;
            var lettersNext = ReadForNow(IsLetter, currentIndex, text);
            currentIndex += lettersNext.Length;
            var isNeedsTag = ReadForNow(IsTag, currentIndex, text);

            if ((isNeedsTag == tag?.SourceName || isNeedsTag == "" && startIndex - 1 < 0 ||
                 startIndex - 1 >= 0 && !IsLetter(text[startIndex - 1])) && (tagsStack.Count <= 0 ||
                                                                       tagsStack.Peek().Tag!.SourceName != "_" ||
                                                                       tag?.SourceName != "__" || currentIndex - 1 < 0 ||
                                                                       IsNumber(text[currentIndex - 1]) || currentIndex + 1 > text.Length ||
                                                                       IsNumber(text[currentIndex + 1])))
                return true;
        }

        return false;
    }

    private bool IsCorrectEnding(ITag? tag, int currentIndex, string text) =>
        currentIndex - (tag!.SourceName.Length + 1) >= 0 && IsLetter(text[currentIndex - (tag.SourceName.Length + 1)]);

    private bool CheckIntersections(ITag tag, int currentIndex, string text)
    {
        foreach (var tagWithIndex in stackOfTags)
        {
            if (tagWithIndex.Tag == tag)
                continue;

            var subString = text.Substring(currentIndex, text.Length - currentIndex);
            var tagStartedFrom = subString.IndexOf(tagWithIndex.Tag!.SourceName, StringComparison.Ordinal);
            if (ReadForNow(IsTag, tagStartedFrom, subString) == tagWithIndex.Tag.SourceName)
                return true;
        }

        return false;
    }
}