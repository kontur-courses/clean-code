using Markdown.Infrastructure.Blocks;

namespace Markdown.Infrastructure.Parsers.Tags
{
    public class Tag
    {
        public Style Style;

        public Tag(Style style)
        {
            Style = style;
        }
    }
}