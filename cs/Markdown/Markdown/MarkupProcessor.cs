using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public class MarkupProcessor
    {
        public readonly Dictionary<MarkupType, string> MarkupToHtmlTag;
        public readonly Dictionary<MarkupType, string> MarkupToMdTag;
        public readonly HashSet<string> SingleTags;
        public readonly HashSet<string> StartingTags;
        public readonly HashSet<string> AllTags;

        public MarkupProcessor(
            Dictionary<MarkupType, string> markupToHtmlTag,
            Dictionary<MarkupType, string> markupToMdTag,
            HashSet<string> singleTags,
            HashSet<string> startingTags
        )
        {
            MarkupToHtmlTag = markupToHtmlTag;
            MarkupToMdTag = markupToMdTag;
            SingleTags = singleTags;
            StartingTags = startingTags;
            AllTags = new HashSet<string>();
            foreach (var mdTag in markupToMdTag.Values)
            {
                AllTags.Add(mdTag);
            }

            CheckCorrectnessOfTags();
        }

        public bool IsSingleTag(MarkupType markupType)
        {
            return IsSingleTag(MarkupToMdTag[markupType]);
        }

        public bool IsStartingTag(string tag)
        {
            return StartingTags.Contains(tag);
        }

        public bool IsSingleTag(string tag)
        {
            return SingleTags.Contains(tag);
        }

        public MarkupType GetMarkupType(string mdTag)
        {
            switch (mdTag)
            {
                case null:
                    throw new ArgumentNullException($"{nameof(mdTag)} is null");
                case "":
                    throw new ArgumentNullException($"{nameof(mdTag)} is empty string");
            }

            foreach (var (markupType, tag) in MarkupToMdTag)
            {
                if (tag == mdTag)
                    return markupType;
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
            return GetOpeningTag(GetMarkupType(text.GetTokenText(token)));
        }

        public string GetClosingTag(MarkupType markupType)
        {
            return $@"\</{MarkupToHtmlTag[markupType]}>";
        }

        public string GetClosingTag(string text, Token token)
        {
            return GetClosingTag(GetMarkupType(text.GetTokenText(token)));
        }

        private void CheckCorrectnessOfTags()
        {
            if (!SingleTags.IsSubsetOf(AllTags) || !StartingTags.IsSubsetOf(AllTags))
                throw new Exception("AllTags does not contain all tags");
            if (MarkupToHtmlTag.Count != MarkupToMdTag.Count)
                throw new Exception($"{nameof(MarkupToHtmlTag)} should have the same size as {nameof(MarkupToMdTag)}");
            if (!MarkupToHtmlTag.Keys.SequenceEqual(MarkupToMdTag.Keys))
                throw new Exception($"{nameof(MarkupToHtmlTag)} should have the same keys as {nameof(MarkupToMdTag)}");
        }
    }
}