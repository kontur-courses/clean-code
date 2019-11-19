using System.Collections.Generic;

namespace Markdown.TagsDataBase
{
    class TagsDB
    {
        private static readonly Dictionary<string, string> mdTags = new Dictionary<string, string>
        {
            { "open_", "_" },
            { "close_", "_" },
            { "open__", "__" },
            { "close__", "__" },
            { "open>", ">" },
            { "close>", "\n" }
        };

        private static readonly Dictionary<string, string> htmlTags = new Dictionary<string, string>
        {
            { "em", "<em>" },
            { "/em", "</em>" },
            { "strong", "<strong>" },
            { "/strong", "</strong>" },
            { "blockquote", "<blockquote>" },
            { "/blockquote", "</blockquote>" }
        };

        public static Tag GetMdTagById(string mdTagId) => GetTagByIdFromTable(mdTagId, mdTags);
        public static Tag GetHtmlTagById(string htmlTagId) => GetTagByIdFromTable(htmlTagId, htmlTags);

        private static Tag GetTagByIdFromTable(string tagId, Dictionary<string, string> table) => 
            new Tag { Id = tagId, Value = table[tagId] };
    }
}