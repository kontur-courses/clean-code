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
        for (var index = 0; index < input.Length; index++)
        {
            var currentTag = tags.FirstOrDefault(tag => tag!.SourceName == input[index].ToString());
            if (currentTag is not null && stackOfTags.Count == 0 ||
                currentTag is not null && stackOfTags.Peek().Tag != currentTag) 
            {
                stackOfTags.Push(new TagWithIndex(currentTag, index));
            }
            else if (currentTag is not null && stackOfTags.Peek().Tag == currentTag)
            {
                var previewTag = stackOfTags.Pop();
                ReplaceByIndex(stackOfTags, previewTag.Tag, previewTag.Index);
            }
            else
                sb.Append(input[index]);
        }

        return sb.ToString();
    }

    private string GetTranslate()
    {
        sb = new StringBuilder();
        while (point < text.Length)
        {
            sb.Append(GetToken());
        }

        return sb.ToString();
    }

    private string GetToken()
    {
        if (text[point] == ' ')
            return text[point++].ToString();
        
        var token = IsLetter(point) ? ReadForNow(IsLetter) : ReadForNow(IsTag);
        var tag = tags.FirstOrDefault(tag => tag!.SourceName == token);
        if (tag is not null && stackOfTags.Count == 0 ||
            tag is not null && stackOfTags.Peek().Tag != tag)
        {
            stackOfTags.Push(new TagWithIndex(tag, point - tag.SourceName.Length));
            return string.Empty;
        }
        if (tag is not null && stackOfTags.Peek().Tag == tag)
        {
            var previewTag = stackOfTags.Pop();
            ReplaceByIndex(stackOfTags, previewTag.Tag, previewTag.Index);
            return string.Empty;
        }

        return token;
    }

    private bool IsLetter(int index) =>
        char.GetUnicodeCategory(text[index]) == UnicodeCategory.UppercaseLetter ||
               char.GetUnicodeCategory(text[index]) == UnicodeCategory.LowercaseLetter;

    private bool IsTag(int index) =>
        tags.Any(tag => tag!.SourceName[0] == text[index]);

    private string ReadForNow(Func<int, bool> func)
    {
        var symbols = new StringBuilder();
        while (point < text.Length)
        {
            if (!func(point))
                return symbols.ToString();
            symbols.Append(text[point]);
            point++;
        }

        return symbols.ToString();
    }

    private void ReplaceByIndex(IEnumerable<TagWithIndex> tags, ITag tag, int index)
    {
        var insertIndex = index;
        foreach (var previousTag in tags)
            insertIndex -= previousTag.Tag.SourceName.Length;
        
        var translateName = TagHelper.GetHtmlFormat(tag.TranslateName);
        // TODO: Change 5 to Const
        var indexPreviewItems = tagsInLine
            .Where(tagWithIndex => tagWithIndex.Index < index)
            .Sum(item => item.Tag.TranslateName.Length * 2 + 5 - item.Tag.SourceName.Length * 2);
        
        sb.Insert(insertIndex + indexPreviewItems, translateName.start);
        sb.Append(translateName.end);
        tagsInLine.Add(new TagWithIndex(tag, index));
    }
}

public class TagWithIndex
{
    public ITag Tag;
    public int Index;

    public TagWithIndex(ITag tag, int index)
    {
        Tag = tag;
        Index = index;
    }
}
