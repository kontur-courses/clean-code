using MarkdownTask.Tags;

namespace MarkdownTask.Styles
{
    public class TagStyleInfo
    {
        public readonly string TagAffix;
        public readonly string TagPrefix;
        public readonly TagType Type;

        public TagStyleInfo(string tagPrefix, string tagAffix, TagType type)
        {
            TagPrefix = tagPrefix;
            TagAffix = tagAffix;
            Type = type;
        }
    }
}