using System.Collections.Generic;

namespace Markdown.Languages
{
    public class MarkDown : ILanguage
    {
        public LanguageTagDict Tags { get; }

        public MarkDown()
        {
            Tags = new LanguageTagDict(new Dictionary<TagType, Tag>
            {
                {TagType.Strong, new Tag("__", "__", new TagType[] {TagType.Em})},
                {TagType.Em, new Tag("_", "_", new TagType[] { })}
            });
        }

        public bool IsOpenTag(string line, int i, string tag)
        {
            return i + tag.Length < line.Length && line[i + tag.Length] != ' ' && !char.IsNumber(line, i + tag.Length);
        }

        public bool IsCloseTag(string line, int i)
        {
            return i > 0 && line[i - 1] != ' ' && !char.IsNumber(line, i - 1);
        }

        public bool IsTag(string line, int i, string tag)
        {
            return i + tag.Length <= line.Length && tag == line.Substring(i, tag.Length)
                                                 && (i + tag.Length >= line.Length ||
                                                     line[i + tag.Length] != tag[tag.Length - 1]);
        }
    }
}