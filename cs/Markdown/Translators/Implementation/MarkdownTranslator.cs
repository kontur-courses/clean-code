using System.Globalization;
using System.Text;
using Markdown.Helpers;
using Markdown.Tags;

namespace Markdown.Translators.Implementation;

public class MarkdownTranslator : ITranslator
{
    private const int OpenCloseTagLength = 5;
    private readonly List<ITag?> tags;
    private Stack<TagWithIndex> stackOfTags;
    private StringBuilder textWithTranslate;
    private readonly List<TagWithIndex> tagsInLine;
    private int index;
    private static List<char> startCharOfTags;

    public MarkdownTranslator()
    {
        tags = TagHelper.GetAllTags<ITag>()!.ToList();
        tagsInLine = new List<TagWithIndex>();
        stackOfTags = new Stack<TagWithIndex>();
        startCharOfTags = tags.Select(tag => tag!.SourceName[0]).ToList();
    }

    public string Translate(string input)
    {
        textWithTranslate = new StringBuilder();
        stackOfTags = new Stack<TagWithIndex>();
        while (index < input.Length)
        {
            var token = GetNextToken(input);
            index += token.Length;
            
            textWithTranslate.Append(ParseToken(token));
        }

        if (stackOfTags.Count != 0)
            PasteSourceNames(stackOfTags, null);

        return textWithTranslate.ToString();
    }

    private string ParseToken(string token)
    {
        return token;
    }

    private string GetNextToken(string text)
    {
        // var token = IsLetter(text[index]) ? ReadForNow(IsLetter, text) : ReadForNow(IsTag, text);
        
        if (IsLetter(text[index]))
            return ReadForNow(IsLetter, text);
        if (IsTag(text[index], index))
            return ReadForNow(IsTag, index, text);
        return ReadForNow(IsSpecialSymbol, text);

        // if (token == "" && text[index] != '\\')
        //     return text[index++].ToString();
        // if (text[index] == '\\')
        // {
        //     token = ReadForNow(IsTag, index + 1, text);
        //     index += token.Length + 1;
        //     if (token == "")
        //         token = "\\";
        //
        //     return token;
        // }

        // index += token.Length;

        // var tag = tags.FirstOrDefault(tag => tag!.SourceName == token);
        // if (tag is not null && stackOfTags.Count == 0 && IsCorrectStart(stackOfTags, tag, index) ||
        //     tag is not null && stackOfTags.All(tagWith => tagWith.Tag != tag) &&
        //     IsCorrectStart(stackOfTags, tag, index))
        // {
        //     stackOfTags.Push(new TagWithIndex(tag, index - tag.SourceName.Length));
        //     return string.Empty;
        // }
        //
        // if (tag is not null && stackOfTags.Any(tagWith => tagWith.Tag == tag) &&
        //     IsCorrectEnding(tag, index))
        // {
        //     var previewTag = stackOfTags.First(tagWith => tagWith.Tag == tag);
        //     ReplaceByIndex(previewTag.Tag, previewTag.Index);
        //     return string.Empty;
        // }
        //
        // return token;
    }

    private static bool IsLetter(char symbol) => char.IsLetter(symbol);

    private static bool IsSpecialSymbol(char symbol) => !IsLetter(symbol) && !IsTag(symbol, 0);

    private static bool IsTag(char symbol, int index) => startCharOfTags.Contains(symbol);

    private bool IsTag(int textIndexFrom, int textIndexTo, int tagIndexTo, string myText) =>
        tags.Where(tag => tag?.SourceName.Length >= tagIndexTo + 1)
            .Count(tag => tag?.SourceName[..(tagIndexTo + 1)] == myText.Substring(textIndexFrom, tagIndexTo + 1)) > 0;

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

    private void ReplaceByIndex(ITag? tag, int currentIndex, string text)
    {
        var insertIndex = currentIndex;
        if (stackOfTags.Peek().Tag == tag)
        {
            insertIndex = stackOfTags.Where(tagWith => tagWith.Tag != tag).Aggregate(insertIndex,
                (current, previousTag) => current - previousTag.Tag!.SourceName.Length);

            stackOfTags.Pop();
        }
        else if (CheckIntersections(tag!, currentIndex, text))
        {
            PasteSourceNames(stackOfTags, null, true);
            return;
        }
        else
            PasteSourceNames(stackOfTags, tag);

        var translateName = TagHelper.GetHtmlFormat(tag!.TranslateName);
        var indexPreviewItems = tagsInLine
            .Where(tagWithIndex => tagWithIndex.Index < currentIndex)
            .Sum(item => item.Tag!.TranslateName.Length * 2 + OpenCloseTagLength - item.Tag.SourceName.Length * 2);

        textWithTranslate.Insert(insertIndex + indexPreviewItems, translateName.start);
        textWithTranslate.Append(translateName.end);
        tagsInLine.Add(new TagWithIndex(tag, currentIndex));
    }

    private void PasteSourceNames(IEnumerable<TagWithIndex> tagsWith, ITag? tag, bool needAppendTag = false)
    {
        var sumOfIndex = tagsInLine.Sum(tagWith => tagWith.Tag is not null ? tagWith.Tag.TranslateName.Length : default);
        if (tag is not null)
            sumOfIndex += tag.SourceName.Length;

        var tagWithIndices = tagsWith.ToList();
        foreach (var item in tagWithIndices.Where(tagWith => tagWith.Tag != tag).Reverse())
            textWithTranslate.Insert(item.Index - sumOfIndex, item.Tag?.SourceName);

        if (needAppendTag)
            textWithTranslate.Append(tagWithIndices.Last().Tag?.SourceName);

        stackOfTags = new Stack<TagWithIndex>();
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
        else
            return false;

        return false;
    }

    private bool IsCorrectEnding(ITag? tag, int currentIndex, string text) =>
        currentIndex - (tag!.SourceName.Length + 1) >= 0 && IsLetter(text[currentIndex - (tag.SourceName.Length + 1)]);

    private bool CheckIntersections(ITag tag, int currentIndex, string text) =>
        (from tagWithIndex in stackOfTags
            where tagWithIndex.Tag != tag
            let subString = text.Substring(currentIndex, text.Length - currentIndex)
            let tagStartedFrom = subString.IndexOf(tagWithIndex.Tag!.SourceName, StringComparison.Ordinal)
            where ReadForNow(IsTag, tagStartedFrom, subString) == tagWithIndex.Tag.SourceName
            select tagWithIndex).Any();
}