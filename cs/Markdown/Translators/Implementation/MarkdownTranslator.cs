using System.Globalization;
using System.Text;
using Markdown.Helpers;
using Markdown.Tags;

namespace Markdown.Translators.Implementation;

public class MarkdownTranslator : ITranslator
{
    private List<ITag?> tags;
    private Stack<TagWithIndex> stackOfTags;
    private StringBuilder sb;
    private List<TagWithIndex> tagsInLine;
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
        point += token.Length;
        if (token == string.Empty)
            return text[point++].ToString();
        
        var tag = tags.FirstOrDefault(tag => tag!.SourceName == token);
        if (tag is not null && stackOfTags.Count == 0 && IsCorrectStart(stackOfTags, tag, point) ||
            tag is not null && stackOfTags.All(tagWith => tagWith.Tag != tag) && IsCorrectStart(stackOfTags, tag, point))
        {
            stackOfTags.Push(new TagWithIndex(tag, point - tag.SourceName.Length));
            return string.Empty;
        }
        if (tag is not null && stackOfTags.Any(tagWith => tagWith.Tag == tag) && IsCorrectEnding(stackOfTags, tag, point))
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

    // private bool IsTag(int index) =>
    //     tags.Any(tag => tag!.SourceName[0] == text[index]);

    private string ReadForNow(Func<int, bool> func)
    {
        var symbols = new StringBuilder();
        var currentPoint = point;
        while (currentPoint < text.Length)
        {
            if (!func(currentPoint))
                return symbols.ToString();
            symbols.Append(text[currentPoint]);
            currentPoint++;
        }

        return symbols.ToString();
    }

    private string ReadForNow(Func<int, int, int, string, bool> func, int index, string currentText)
    {
        if (index < 0)
            return string.Empty;
        
        var startIndex = index;
        var tagIndex = 0;
        var symbols = new StringBuilder();
        while (index < text.Length)
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
            foreach (var previousTag in stackOfTags.Where(tagWith => tagWith.Tag != tag))
                insertIndex -= previousTag.Tag!.SourceName.Length;
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
        // TODO: Change 5 to Const
        var indexPreviewItems = tagsInLine
            .Where(tagWithIndex => tagWithIndex.Index < index)
            .Sum(item => item.Tag!.TranslateName.Length * 2 + 5 - item.Tag.SourceName.Length * 2);

        sb.Insert(insertIndex + indexPreviewItems, translateName.start);
        sb.Append(translateName.end);
        tagsInLine.Add(new TagWithIndex(tag, index));
    }

    private void PasteSourceNames(IEnumerable<TagWithIndex> tags, ITag? tag, bool needAppendTag = false)
    {
        var index = tagsInLine.Sum(tagWith => tagWith.Tag is not null ? tagWith.Tag.TranslateName.Length : default);
        if (tag is not null)
            index += tag.SourceName.Length;
        
        foreach (var item in tags.Where(tagWith => tagWith.Tag != tag).Reverse())
            sb.Insert(item.Index - index, item.Tag?.SourceName);

        if (needAppendTag)
            sb.Append(tags.Last().Tag.SourceName);
        
        stackOfTags = new Stack<TagWithIndex>();
    }

    private bool IsCorrectStart(IEnumerable<TagWithIndex> tags, ITag? tag, int index)
    {
        if (index < text.Length && IsLetter(index))
            return true;
        return false;
    }

    private bool IsCorrectEnding(IEnumerable<TagWithIndex> tags, ITag? tag, int index)
    {
        if (index - (tag!.SourceName.Length + 1) >= 0 && IsLetter(index - (tag.SourceName.Length + 1)))
            return true;
        return false;
    }

    private bool CheckIntersections(ITag tag, int currentIndex)
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

public class TagWithIndex
{
    public ITag? Tag;
    public int Index;

    public TagWithIndex(ITag? tag, int index)
    {
        Tag = tag;
        Index = index;
    }
}
