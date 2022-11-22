using System.Text;
using Markdown.Helpers;
using Markdown.Tags;

namespace Markdown.Translators.Implementation;

public class MarkdownTranslator : ITranslator
{
    private List<ITag?> tags;
    private Stack<TagWithIndex> stackOfTags;
    private StringBuilder sb;
    private int tagsLength = 0;

    public MarkdownTranslator()
    {
        tags = TagHelper.GetAllTags<ITag>()!.ToList();
        stackOfTags = new Stack<TagWithIndex>();
        sb = new StringBuilder();
    }
    
    public string Translate(string input)
    {
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
                ReplaceByIndex(previewTag.Tag, previewTag.Index);
            }
            else
                sb.Append(input[index]);
        }

        return sb.ToString();
    }

    private void ReplaceByIndex(ITag tag, int index)
    {
        var sbLength = sb.Length;
        var translateName = TagHelper.GetHtmlFormat(tag.TranslateName);
        sb.Insert(index + tagsLength, translateName.start);
        sb.Append(translateName.end);
        tagsLength += sb.Length - sbLength - 2;
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
