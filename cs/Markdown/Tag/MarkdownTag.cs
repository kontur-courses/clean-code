using System;
using System.Linq;

namespace Markdown.Tag
{
    public class MarkdownTag
    {
        public string Tag { get; }
        public int Length => Tag.Length;
        public string OpenTagTranslation { get; }
        public string CloseTagTranslation { get; }
        private readonly Type[] possibleInnerTags;

        protected MarkdownTag(string tag, string translation, params Type[] possibleInnerTags)
        {
            translation = translation.ToLower();
            Tag = tag.ToLower();
            OpenTagTranslation = $"<{translation}>";
            CloseTagTranslation = $"</{translation}>";
            this.possibleInnerTags = possibleInnerTags;
        }

        public bool StartsWith(char letter)
        {
            return Tag.StartsWith(letter.ToString());
        }

        public bool CanContain(MarkdownTag tag)
        {
            return possibleInnerTags.Contains(tag.GetType());
        }
    }
}