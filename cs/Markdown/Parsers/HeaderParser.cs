using Markdown.Tags;
using Markdown.Tags.HeaderTag;

namespace Markdown.Parsers
{
    public static class HeaderParser
    {
        public static Tag[] ParseTags(string paragraph)
        {
            return paragraph[0] == '#' 
                ? new Tag[] {new OpenHeaderTag(0), new CloseHeaderTag(paragraph.Length)} 
                : new Tag[0];
        }
    }
}