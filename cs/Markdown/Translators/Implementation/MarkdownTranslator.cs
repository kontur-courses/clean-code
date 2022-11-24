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
    private StringBuilder sb;
    private readonly List<TagWithIndex> tagsInLine;
    private string text;
    private int point;

    public MarkdownTranslator()
    {
        tags = TagHelper.GetAllTags<ITag>()!.ToList();
        sb = new StringBuilder();
        tagsInLine = new List<TagWithIndex>();
        stackOfTags = new Stack<TagWithIndex>();
        text = string.Empty;
    }

    public string Translate(string input)
    {
        stackOfTags = new Stack<TagWithIndex>();
        text = input;
        return GetTranslate();
    }

    private string GetTranslate()
    {
        sb = new StringBuilder();
        while (point < text.Length)
        {
            sb.Append(GetToken());
        }

        if (stackOfTags.Count != 0)
            PasteSourceNames(stackOfTags, null);

        return sb.ToString();
    }

    private string GetToken()
    {
        var token = IsLetter(point) ? ReadForNow(IsLetter) : ReadForNow(IsTag);
        if (token == "" && text[point] != '\\')
            return text[point++].ToString();
        if (text[point] == '\\')
        {
            token = ReadForNow(IsTag, point + 1, text);
            point += token.Length + 1;
            if (token == "")
                token = "\\";

            return token;
        }

        point += token.Length;

        var tag = tags.FirstOrDefault(tag => tag!.SourceName == token);
        if (tag is not null && stackOfTags.Count == 0 && IsCorrectStart(stackOfTags, tag, point) ||
            tag is not null && stackOfTags.All(tagWith => tagWith.Tag != tag) &&
            IsCorrectStart(stackOfTags, tag, point))
        {
            stackOfTags.Push(new TagWithIndex(tag, point - tag.SourceName.Length));
            return string.Empty;
        }

        if (tag is not null && stackOfTags.Any(tagWith => tagWith.Tag == tag) &&
            IsCorrectEnding(tag, point))
        {
            var previewTag = stackOfTags.First(tagWith => tagWith.Tag == tag);
            ReplaceByIndex(previewTag.Tag, previewTag.Index);
            return string.Empty;
        }

        return token;
    }

    private bool IsLetter(int index) =>
        char.GetUnicodeCategory(text[index]) == UnicodeCategory.UppercaseLetter ||
        char.GetUnicodeCategory(text[index]) == UnicodeCategory.LowercaseLetter;

    private bool IsTag(int textIndexFrom, int textIndexTo, int tagIndexTo, string myText) =>
        tags.Where(tag => tag?.SourceName.Length >= tagIndexTo + 1)
            .Count(tag => tag?.SourceName[..(tagIndexTo + 1)] == myText.Substring(textIndexFrom, tagIndexTo + 1)) > 0;

    private bool IsNumber(int index) =>
        char.GetUnicodeCategory(text[index]) == UnicodeCategory.LetterNumber;

    private static string ReadForNow(Func<int, bool> func, int index, string currentText)
    {
        var symbols = new StringBuilder();
        var currentPoint = index;
        while (currentPoint < currentText.Length)
        {
            if (!func(currentPoint))
                return symbols.ToString();
            symbols.Append(currentText[currentPoint]);
            currentPoint++;
        }

        return symbols.ToString();
    }

    private string ReadForNow(Func<int, bool> func) => ReadForNow(func, point, text);

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

    private string ReadForNow(Func<int, int, int, string, bool> func) => ReadForNow(func, point, text);

    private void ReplaceByIndex(ITag? tag, int index)
    {
        var insertIndex = index;
        if (stackOfTags.Peek().Tag == tag)
        {
            insertIndex = stackOfTags.Where(tagWith => tagWith.Tag != tag).Aggregate(insertIndex,
                (current, previousTag) => current - previousTag.Tag!.SourceName.Length);

            stackOfTags.Pop();
        }
        else if (CheckIntersections(tag!, point))
        {
            PasteSourceNames(stackOfTags, null, true);
            return;
        }
        else
            PasteSourceNames(stackOfTags, tag);

        var translateName = TagHelper.GetHtmlFormat(tag!.TranslateName);
        var indexPreviewItems = tagsInLine
            .Where(tagWithIndex => tagWithIndex.Index < index)
            .Sum(item => item.Tag!.TranslateName.Length * 2 + OpenCloseTagLength - item.Tag.SourceName.Length * 2);

        sb.Insert(insertIndex + indexPreviewItems, translateName.start);
        sb.Append(translateName.end);
        tagsInLine.Add(new TagWithIndex(tag, index));
    }

    private void PasteSourceNames(IEnumerable<TagWithIndex> tagsWith, ITag? tag, bool needAppendTag = false)
    {
        var index = tagsInLine.Sum(tagWith => tagWith.Tag is not null ? tagWith.Tag.TranslateName.Length : default);
        if (tag is not null)
            index += tag.SourceName.Length;

        var tagWithIndices = tagsWith.ToList();
        foreach (var item in tagWithIndices.Where(tagWith => tagWith.Tag != tag).Reverse())
            sb.Insert(item.Index - index, item.Tag?.SourceName);

        if (needAppendTag)
            sb.Append(tagWithIndices.Last().Tag?.SourceName);

        stackOfTags = new Stack<TagWithIndex>();
    }

    private bool IsCorrectStart(Stack<TagWithIndex> tagsStack, ITag? tag, int index)
    {
        if (index < text.Length && IsLetter(index))
        {
            var startIndex = index - 1;
            var lettersNext = ReadForNow(IsLetter, index, text);
            index += lettersNext.Length;
            var isNeedsTag = ReadForNow(IsTag, index, text);

            if ((isNeedsTag == tag?.SourceName || isNeedsTag == "" && startIndex - 1 < 0 ||
                 startIndex - 1 >= 0 && !IsLetter(startIndex - 1)) && (tagsStack.Count <= 0 ||
                                                                       tagsStack.Peek().Tag!.SourceName != "_" ||
                                                                       tag?.SourceName != "__" || index - 1 < 0 ||
                                                                       IsNumber(index - 1) || index + 1 > text.Length ||
                                                                       IsNumber(index + 1)))
                return true;
        }
        else
            return false;

        return false;
    }

    private bool IsCorrectEnding(ITag? tag, int index) =>
        index - (tag!.SourceName.Length + 1) >= 0 && IsLetter(index - (tag.SourceName.Length + 1));

    private bool CheckIntersections(ITag tag, int currentIndex) =>
        (from tagWithIndex in stackOfTags
            where tagWithIndex.Tag != tag
            let subString = text.Substring(currentIndex, text.Length - currentIndex)
            let tagStartedFrom = subString.IndexOf(tagWithIndex.Tag!.SourceName, StringComparison.Ordinal)
            where ReadForNow(IsTag, tagStartedFrom, subString) == tagWithIndex.Tag.SourceName
            select tagWithIndex).Any();
}