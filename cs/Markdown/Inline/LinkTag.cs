using System.Collections.Generic;

namespace Markdown.Inline
{
    public class LinkTag : IInlineTag
    {
        public List<IInlineTag> Content { get; }
        public string Url { get; }
        public LinkTag(List<IInlineTag> content, string url)
        {
            Content = content;
            Url = url;
        }
    }
}
