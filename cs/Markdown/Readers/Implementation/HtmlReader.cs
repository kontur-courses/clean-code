using System.Text;
using Markdown.Helpers;
using Markdown.Tags;

namespace Markdown.Readers.Implementation;

public class HtmlReader : IReader
{
    private readonly List<ITag> tags;

    public HtmlReader(params ITag[] tags)
    {
        this.tags = tags.ToList();
    }

    public HtmlReader()
    {
        tags = TagHelper.GetAllTags<ITag>()!.ToList()!;
    }
    
    public string Reader(string markdownInput)
    {
        foreach (var tag in tags)
        {
            var translatorValue = TagHelper.GetOpenClosePattern(tag.TranslateName);
            var htmlValue = TagHelper.GetOpenClosePattern(tag.ResultName);
            markdownInput = markdownInput.Replace(translatorValue.start, htmlValue.start);
            markdownInput = markdownInput.Replace(translatorValue.end, htmlValue.end);

            var substringFrom = markdownInput.IndexOf(htmlValue.start, StringComparison.Ordinal);
            var substringTo = markdownInput.IndexOf(htmlValue.end, StringComparison.Ordinal);

            if (substringFrom < 0 || substringTo < 0)
                continue;

            substringFrom += htmlValue.start.Length;
            var substring = markdownInput.Substring(substringFrom, substringTo - substringFrom);
            markdownInput = markdownInput.Replace(substring, tag.MakeTransformations(substring));
        }

        return markdownInput;
    }
}