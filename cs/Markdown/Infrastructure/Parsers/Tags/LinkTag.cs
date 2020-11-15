using Markdown.Infrastructure.Blocks;

namespace Markdown.Infrastructure.Parsers.Tags
{
    public class LinkTag : Tag
    {
        public readonly string Link;

        public LinkTag(string link) : base(Style.Media)
        {
            Link = link;
        }
    }
}