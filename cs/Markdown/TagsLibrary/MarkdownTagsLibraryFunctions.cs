using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown.TagsLibrary
{
    public static partial class MarkdownTagsLibrary
    {
        public static bool TryToGetUsableTagInAssociations(string text, int startIndex, out TagElement resultTag)
        {
            if(text == null)
                throw new NullReferenceException("text can't be null");

            if(startIndex < 0 || startIndex >= text.Length)
                throw new ArgumentOutOfRangeException($"{nameof(startIndex)} not included in the array bounds");

            var parseVariants = new List<(TagElement tag, int priority)>();
            var substrLength = 1;

            while(substrLength <= MaximumTagLength && substrLength + startIndex <= text.Length)
            {
                var key = text.Substring(startIndex, substrLength);
                if (TagAssociations.ContainsKey(key))
                {
                    var (type, priority) = TagAssociations[key];
                    var tagUsability = GetTagUsability(text, startIndex, startIndex + substrLength - 1, type);
                    
                    if(tagUsability != TagUsability.None)
                        parseVariants.Add((new TagElement(type, tagUsability, startIndex, substrLength), priority));
                }
                substrLength++;
            }

            if (parseVariants.Count == 0)
            {
                resultTag = null;
                return false;
            }

            resultTag = parseVariants.OrderByDescending(x => x.priority).First().tag;
            return true;
        }

        public static TagUsability GetTagUsability(string text, int startIndex, int endIndex, TagType type)
        {
            if (text == null)
                throw new NullReferenceException();

            if (startIndex < 0 || startIndex >= text.Length)
                throw new ArgumentOutOfRangeException($"{nameof(startIndex)} not included in the array bounds");

            if (endIndex < 0 || endIndex >= text.Length)
                throw new ArgumentOutOfRangeException($"{nameof(endIndex)} is out of bounds");

            if (startIndex > endIndex)
                throw new ArgumentException($"{nameof(startIndex)} can't be more than {nameof(endIndex)}");

            var leftChar = text.TryGetChar(startIndex - 1);
            var rightChar = text.TryGetChar(endIndex + 1);

            return GetUsabilityDependsOnSideSymbols(leftChar, rightChar, type);
        }

        private static TagUsability GetUsabilityDependsOnSideSymbols(char? leftSymbol, char? rightSymbol, TagType type)
        {
            var canBeStart = StartTagRules[type](leftSymbol, rightSymbol);
            var canBeEnd = EndTagRules[type](leftSymbol, rightSymbol);

            if (canBeStart && canBeEnd)
                return TagUsability.All;

            return canBeStart ? TagUsability.Start : 
                canBeEnd ? TagUsability.End : TagUsability.None;
        }
    }
}