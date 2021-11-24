using Markdown.Tags;

namespace Markdown.Parser
{
    public class OpenTag
    {
        public Tag Tag { get; }
        public int OpenIndex { get; }

        public OpenTag(Tag tag, int openIndex)
        {
            Tag = tag;
            OpenIndex = openIndex;
        }
    }
}