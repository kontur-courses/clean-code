using System;
using System.Collections.Generic;

namespace Markdown
{
    public abstract class Tag
    {
        public int InitialIndex { get; set; }
        public string End { get; private protected set; }
        public string Start { get; private protected set; }
        public Func<string, string, string, bool> IsAgreedStart { get; private protected set; }
        public Func<string, string, string, bool> IsAgreedEnd { get; private protected set; }
        private protected static Dictionary<string, Func<Tag>> Tags = new Dictionary<string, Func<Tag>>();

        static Tag()
        {
            Tags.Add("_", () => new TagEm());
            Tags.Add("__", () => new TagStrong());
            Tags.Add("[", () => new TagA());
        }

        public static bool TryOpenTag(string previousToken, string token, string nextToken, out Tag tag)
        {
            if (!Tags.ContainsKey(token))
            {
                tag = null;
                return false;
            }

            var possibleTag = Tags[token]();

            if (!possibleTag.IsAgreedStart(token, previousToken, nextToken))
            {
                tag = null;
                return false;
            }

            tag = possibleTag;
            return true;
        }

        public static bool TryCloseTag(string previousToken, string token, string nextToken, Tag lastOpenTag)
        {
            if (lastOpenTag.End != token || !lastOpenTag.IsAgreedEnd(token, previousToken, nextToken))
                return false;
            if (!(lastOpenTag is IPairTag tag)) return true;
            return true;
        }
    }
}
