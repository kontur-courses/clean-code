using System.Linq;

namespace Markdown.Tag
{
    public class MarkdownTag
    {
        public string Value { get; }
        public int Length => Value.Length;
        public readonly string Translation;
        private readonly MarkdownTag[] possibleInnerTags;

        protected MarkdownTag(string value, string translation, params MarkdownTag[] possibleInnerTags)
        {
            Translation = translation.ToLower();
            Value = value;
            this.possibleInnerTags = possibleInnerTags;
        }

        public string GetTranslation()
        {
            return $"<{Translation}>";
        }

        public string GetTranslationWithBackslash()
        {
            return $"</{Translation}>";
        }

        public bool CanContain(MarkdownTag tag)
        {
            return possibleInnerTags.Any(t => t.Value == tag.Value);
        }

        public bool IsInnerTagOf(MarkdownTag outerTag)
        {
            return this != outerTag && outerTag.CanContain(this);
        }
    }
}