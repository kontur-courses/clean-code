﻿using System;
using System.Collections.Generic;

namespace Markdown
{
    internal class TagTranslator : ITagTranslator
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
            if (!tagsRelations.ContainsKey(tag)) return tag;

            return tagsRelations[tag];
        }
    }
}