using System.Linq;

namespace Markdown.Tag
{
    public class MarkdownTag
    {
        public string Value { get; }
        public int Length => Value.Length;
        public readonly string Translation;
        private readonly string[] uncombinableTags;

        protected MarkdownTag(string value, string translation, params MarkdownTag[] uncombinableTags)
        {
            Translation = translation.ToLower();
            Value = value;
            this.uncombinableTags = uncombinableTags
                .Select(t => t.Value)
                .ToArray();
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
            return uncombinableTags.Any(t => t == tag.Value);
        }
    }
}