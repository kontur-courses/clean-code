using System;
using System.Collections.Generic;

namespace Markdown
{
    public abstract class Tag
    {
        public TextSeparator StartSeparator { get; private set; }
        private protected static Dictionary<string, Func<Tag>> RegisterTags = new Dictionary<string, Func<Tag>>();

        static Tag()
        {
            // todo get Teg children (container)
            RegisterTags.Add("_", () => new TagEm());
            RegisterTags.Add("__", () => new TagStrong());
        }

        public static Tag CreateTagOnTextSeparator(TextSeparator textSeparator)
        {
            var tag = RegisterTags[textSeparator.Separator]();
            tag.StartSeparator = textSeparator;
            return tag;
        }
    }
}
