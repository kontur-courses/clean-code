using System;
using System.Collections.Generic;

namespace Markdown
{
    public class TagTranslator : ITagTranslator
    {
        private readonly Dictionary<Tag, Tag> tagsRelations = new();
        
        internal TagTranslator(){}

        public void SetReference(Tag from, Tag to)
        {
            if (from is null || to is null) throw new ArgumentNullException();
            if (tagsRelations.ContainsKey(from))
                throw new ArgumentException($"tag {from.Start} already translating to tag {tagsRelations[from].Start}");

            tagsRelations[from] = to;
        }

        public Tag Translate(Tag tag)
        {
            if (tag is null) throw new ArgumentNullException();
            if (!tagsRelations.ContainsKey(tag))
                throw new ArgumentException($"unknown tag {tag.Start} for translation");

            return tagsRelations[tag];
        }
    }
}