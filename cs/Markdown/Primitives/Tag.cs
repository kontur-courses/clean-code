using Markdown.Primitives.TagHelper;

namespace Markdown.Primitives
{
    public class Tag
    {
        public TagTypes Type { get; }
        public string StartTag { get; }
        public string EndTag { get; }

        public Tag(TagTypes type, string startTag, string endTag)
        {
            Type = type;
            StartTag = startTag;
            EndTag = endTag;
        }
    }
}
