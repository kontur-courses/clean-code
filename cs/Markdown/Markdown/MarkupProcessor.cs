using System;
using System.Collections.Generic;

namespace Markdown
{
    public class MarkupProcessor
    {
        public readonly Dictionary<MarkupType, string> MarkupToHtmlTag;
        public readonly Dictionary<MarkupType, string> MarkupToMdTag;
        public readonly HashSet<string> SingleTags;
        public readonly HashSet<string> AllTags;

        public MarkupProcessor(
            Dictionary<MarkupType, string> markupToHtmlTag,
            Dictionary<MarkupType, string> markupToMdTag,
            HashSet<string> singleTags
        )
        {
            MarkupToHtmlTag = markupToHtmlTag;
            MarkupToMdTag = markupToMdTag;
            SingleTags = singleTags;
            AllTags = new HashSet<string>();
            foreach (var mdTag in markupToMdTag.Values)
            {
                AllTags.Add(mdTag);
            }
        }

        public bool IsSingleTag(MarkupType markupType)
        {
            return IsSingleTag(MarkupToMdTag[markupType]);
        }

        public bool IsSingleTag(string tag)
        {
            return SingleTags.Contains(tag);
        }

        public MarkupType GetMarkupType(string mdTag)
        {
            foreach (var markupTagPair in MarkupToMdTag)
            {
                if (markupTagPair.Value == mdTag)
                    return markupTagPair.Key;
            }

            throw new ArgumentException($"Tag {mdTag} not found");
        }

        public MarkupType GetMarkupType(string text, Token token)
        {
            return GetMarkupType(text.Substring(token.Start, token.Length));
        }

        public string GetOpeningTag(MarkupType markupType)
        {
            return $@"\<{MarkupToHtmlTag[markupType]}>";
        }

        public string GetOpeningTag(string text, Token token)
        {
            return GetOpeningTag(GetMarkupType(text.Substring(token)));
        }

        public string GetClosingTag(MarkupType markupType)
        {
            return $@"\</{MarkupToHtmlTag[markupType]}>";
        }

        public string GetClosingTag(string text, Token token)
        {
            return GetClosingTag(GetMarkupType(text.Substring(token)));
        }
    }
}