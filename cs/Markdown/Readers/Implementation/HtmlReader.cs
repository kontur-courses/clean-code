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
        tags = TagHelper.GetAllTags<ITag>();
    }
    
    public string Reader(string markdownInput)
    {
        foreach (var tag in tags)
        {
            var translatorValue = TagHelper.GetHtmlFormat(tag.TranslateName);
            var htmlValue = TagHelper.GetHtmlFormat(tag.HtmlName);
            markdownInput = markdownInput.Replace(translatorValue.start, htmlValue.start);
            markdownInput = markdownInput.Replace(translatorValue.end, htmlValue.end);
        }

        return markdownInput;
    }
}