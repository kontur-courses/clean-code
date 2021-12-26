using MarkdownTask.Tags;

namespace MarkdownTask
{
    public class StyleInfo
    {
        public readonly string TagAffix;
        public readonly string TagPrefix;
        public readonly TagType Type;

        public StyleInfo(string tagPrefix, string tagAffix, TagType type)
        {
            TagPrefix = tagPrefix;
            TagAffix = tagAffix;
            Type = type;
        }
    }
}