using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public static partial class MarkdownTagsLibrary
    {
        public static bool TryToGetUsableTagInAssociations(string text, int startIndex, out TagElement resultTag)
        {
            if(text == null)
                throw new NullReferenceException();

            if(startIndex < 0 || startIndex >= text.Length)
                throw new ArgumentOutOfRangeException();

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

            if (startIndex < 0 || startIndex >= text.Length || 
                endIndex < 0 || endIndex >= text.Length || 
                endIndex < startIndex)
                throw new ArgumentOutOfRangeException();

            var leftChar = startIndex - 1 > 0 ? text[startIndex - 1] : (char?)null;
            var rightChar = endIndex + 1 < text.Length ? text[endIndex + 1] : (char?)null;

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