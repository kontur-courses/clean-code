using System.Collections.Generic;
using MarkdownTask.Tags;

namespace MarkdownTask.Styles
{
    public class HtmlStyleKeeper
    {
        private static readonly TagStyleInfo Italic =
            new(@"<em>", @"</em>", TagType.Italic);

        private static readonly TagStyleInfo Strong =
            new(@"<strong>", @"</strong>", TagType.Strong);

        private static readonly TagStyleInfo Header =
            new(@"<h1>", @"</h1>", TagType.Header);

        public static readonly Dictionary<TagType, TagStyleInfo> Styles = new()
        {
            [TagType.Italic] = Italic,
            [TagType.Strong] = Strong,
            [TagType.Header] = Header
        };
    }
}