using System;
using System.Collections.Generic;

namespace Markdown
{
    public class MarkupProcessor
    {
        public Dictionary<MarkupType, string> markupToTag;

        public MarkupProcessor(Dictionary<MarkupType, string> markupToTag)
        {
            this.markupToTag = markupToTag;
        }

        public MarkupType GetMarkupType(string tag)
        {
            foreach (var markupTagPair in markupToTag)
            {
                if (markupTagPair.Value == tag)
                    return markupTagPair.Key;
            }

            throw new ArgumentException($"Tag {tag} not found");
        }

        public MarkupType GetMarkupType(string text, Token token)
        {
            return GetMarkupType(text.Substring(token.Start, token.Length));
        }

        public string GetOpeningTag(MarkupType markupType)
        {
            return $@"\<{markupToTag[markupType]}>";
        }

        public string GetOpeningTag(string text, Token token)
        {
            return GetOpeningTag(GetMarkupType(text.Substring(token)));
        }

        public string GetClosingTag(MarkupType markupType)
        {
            return $@"\</{markupToTag[markupType]}>";
        }

        public string GetClosingTag(string text, Token token)
        {
            return GetClosingTag(GetMarkupType(text.Substring(token)));
        }
    }
}