using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Markdown.Languages
{
    public class LanguageTagDict
    {
        private readonly Dictionary<TagType, Tag> dictionary;
        public Dictionary<TagType, Tag>.KeyCollection Keys => dictionary.Keys;
        public Dictionary<TagType, Tag>.ValueCollection Values => dictionary.Values;
        public int Count => dictionary.Count;

        public LanguageTagDict(Dictionary<TagType, Tag> dictionary)
        {
            this.dictionary = dictionary;
        }

        public Tag this[TagType tagType]
        {
            get
            {
                if (dictionary.ContainsKey(tagType))
                {
                    return dictionary[tagType];
                }
                else
                {
                    var child = ((TagType[]) Enum.GetValues(typeof(TagType))).ToList();
                    return new Tag("", "", child);
                }
            }
        }
    }
}