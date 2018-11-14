using Markdown.Data.TagsInfo;

namespace Markdown.Data
{
    public class Tag
    {
        public readonly ITagInfo Info;
        public readonly string Translation;

        public Tag(ITagInfo info, string translation)
        {
            Translation = translation;
            Info = info;
        }
    }
}