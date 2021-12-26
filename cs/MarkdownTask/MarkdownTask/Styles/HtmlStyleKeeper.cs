using System.Collections.Generic;
using MarkdownTask.Tags;

namespace MarkdownTask.Styles
{
    public class HtmlStyleKeeper
    {
        private static readonly StyleInfo Italic =
            new(@"<em>", @"</em>", TagType.Italic);

        private static readonly StyleInfo Strong =
            new(@"<strong>", @"</strong>", TagType.Strong);

        private static readonly StyleInfo Header =
            new(@"<h1>", @"</h1>", TagType.Header);

        public static readonly Dictionary<TagType, StyleInfo> Styles = new()
        {
            [TagType.Italic] = Italic,
            [TagType.Strong] = Strong,
            [TagType.Header] = Header
        };
    }
}