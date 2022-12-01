using Markdown.Interfaces;

namespace Markdown
{
    public class Word : ITag
    {
        public Word(string word)
        {
            ViewTag = word;
        }

        public string ViewTag { get; }

        public Tag Tag => Tag.None;

        public TagType TagType => TagType.None;
    }
}