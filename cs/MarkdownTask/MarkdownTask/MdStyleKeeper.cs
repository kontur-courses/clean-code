using System.Collections.Generic;
using MarkdownTask.Tags;

namespace MarkdownTask
{
    public class MdStyleKeeper
    {
        private static readonly StyleInfo Italic =
            new("_", "_", TagType.Italic);

        private static readonly StyleInfo Strong =
            new("__", "__", TagType.Strong);

        private static readonly StyleInfo Header =
            new("# ", "", TagType.Header);

        public static readonly Dictionary<TagType, StyleInfo> Styles = new()
        {
            [TagType.Italic] = Italic,
            [TagType.Strong] = Strong,
            [TagType.Header] = Header
        };
    }
}