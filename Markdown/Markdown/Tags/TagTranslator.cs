using System;
using System.Collections.Generic;

namespace Markdown
{
    internal class TagTranslator : ITagTranslator
    {
        private readonly Dictionary<Tag, Tag> tagsRelations = new();
        
        internal TagTranslator(){}

        public void SetReference(Tag from, Tag to)
        {
            MdExceptionHelper.ThrowArgumentNullExceptionIf(
                new ExceptionCheckObject(nameof(from), from),
                new ExceptionCheckObject(nameof(to), to));
            if (tagsRelations.ContainsKey(from))
                throw new ArgumentException($"tag {from.Start} already translating to tag {tagsRelations[from].Start}");

            tagsRelations[from] = to;
        }

        public Tag Translate(Tag tag)
        {
            MdExceptionHelper.ThrowArgumentNullExceptionIf(new ExceptionCheckObject(nameof(tag), tag));
            return !tagsRelations.ContainsKey(tag) ? tag : tagsRelations[tag];
        }
    }
}