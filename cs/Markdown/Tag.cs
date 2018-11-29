using System;
using System.Collections.Generic;

namespace Markdown
{
    public abstract class Tag
    {
        public int InitialIndex { get; set; }
        public string End { get; private protected set; }
        public string Start { get; private protected set; }
        public Func<string, string, string, bool> FindRule { get; private protected set; }
        public Func<string, string, string, bool> CloseRule { get; private protected set; }
        private protected static Dictionary<string, Func<Tag>> RegisterTags = new Dictionary<string, Func<Tag>>();

        static Tag()
        {
            RegisterTags.Add("_", () => new TagEm());
            RegisterTags.Add("__", () => new TagStrong());
            RegisterTags.Add("[", () => new TagA());
        }

        public static bool TryOpenTag(string previousToken, string token, string nextToken, out Tag tag)
        {
            if (!RegisterTags.ContainsKey(token))
            {
                tag = null;
                return false;
            }

            var possibleTag = RegisterTags[token]();

            if (!possibleTag.FindRule(token, previousToken, nextToken))
            {
                tag = null;
                return false;
            }

            tag = possibleTag;
            return true;
        }

        public static bool TryCloseTag(string previousToken, string token, string nextToken, Tag lastOpenTag)
        {
            if (lastOpenTag.End != token || !lastOpenTag.CloseRule(token, previousToken, nextToken))
                return false;
            if (!(lastOpenTag is IPairTag tag)) return true;
            return true;
        }
    }
}
