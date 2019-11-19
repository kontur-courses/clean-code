using System;
using System.Collections.Generic;

namespace Markdown.TagsDataBase
{
    class MdAndHtmlTags
    {
        private static readonly Dictionary<string, string> mdTagToHtmlTag = new Dictionary<string, string>
        {
            { "open_", "em" },
            { "close_", "/em" },
            { "open__", "strong" },
            { "close__", "/strong" },
            { "open>", "blockquote" },
            { "close>", "/blockquote" }
        };

        public static Tag GetHtmlTagByMdTag(Tag mdTag)
        {
            if (!mdTagToHtmlTag.TryGetValue(mdTag.Id, out var htmlTagId))
                throw new ArgumentException();
            return TagsDB.GetHtmlTagById(htmlTagId);
        }
    }
}