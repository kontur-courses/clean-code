using System;
using System.Collections.Generic;
using Markdown.Content;
using Markdown.MdTags;

namespace Markdown.Builder
{
    internal class TagBuilder
    {
        private static readonly Dictionary<string, Func<ContentFinder, string, int, Tag>> mdTags 
            = new Dictionary<string, Func<ContentFinder, string, int, Tag>>()
        {
            { "__", (finder, text, index) =>new StrongTag(finder.GetStrongContent(text, index)) },
            { "_", (finder, text, index) => new EmTag(finder.GetEmContent(text, index)) },
            { "**", (finder, text, index) => new StrongTag(finder.GetStrongContent(text, index)) },
            { "*", (finder, text, index) => new ListTag(finder.GetListContent(text, index)) },
            { "~", (finder, text, index) => new StrikeTag(finder.GetStrikeContent(text, index)) },
            { "`", (finder, text, index) => new CodeTag(finder.GetCodeContent(text, index)) },
            { "", (finder, text, index) => new SimpleTag(finder.GetSimpleContent(text, index)) },
            { "#", (finder, text, index) => new HeaderTag(finder.GetHeaderContent(text, index),"#" )},
            { "##", (finder, text, index) => new HeaderTag(finder.GetHeaderContent(text, index),"##") },
            { "###", (finder, text, index) => new HeaderTag(finder.GetHeaderContent(text, index),"###") },
            { "####", (finder, text, index) => new HeaderTag(finder.GetHeaderContent(text, index),"####") },
            { "#####", (finder, text, index) => new HeaderTag(finder.GetHeaderContent(text, index),"#####") },
            { "######", (finder, text, index) => new HeaderTag(finder.GetHeaderContent(text, index),"######") },
            { "***", (finder, text, index) => new HorizontalTag(finder.GetHorizontalContent(text, index)) },
            { "___", (finder, text, index) => new HorizontalTag(finder.GetHorizontalContent(text, index)) },
            { ">", (finder, text, index) => new BlockquoteTag(finder.GetBlockquoteContent(text, index)) }
        };

        private static readonly ContentFinder contentFinder = new ContentFinder();

        public static Tag BuildTag(string tag, string text, int offset)
            => mdTags[tag](contentFinder, text, offset);
    }
}
