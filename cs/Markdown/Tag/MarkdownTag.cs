using System.Collections.Generic;

namespace Markdown.Tag
{
    public class MarkdownTag
    {
        public string Value { get; }
        public int Length => Value.Length;
        public readonly string Translation;
        private readonly HashSet<string> uncombinableTags;

        protected MarkdownTag(string value, string translation, params MarkdownTag[] uncombinableTags)
        {
            Translation = translation.ToLower();
            Value = value;
            this.uncombinableTags = new HashSet<string>();
            foreach (var uncombinableTag in uncombinableTags)
                this.uncombinableTags.Add(uncombinableTag.Value);
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
            return !uncombinableTags.Contains(tag.Value);
        }
    }
}