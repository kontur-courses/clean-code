using System.Collections.Generic;
using MarkdownTask.Tags;

namespace MarkdownTask.Styles
{
    public class MdStyleKeeper
    {
        private static readonly TagStyleInfo Italic =
            new("_", "_", TagType.Italic);

        private static readonly TagStyleInfo Strong =
            new("__", "__", TagType.Strong);

        private static readonly TagStyleInfo Header =
            new("# ", "", TagType.Header);

        public static readonly Dictionary<TagType, TagStyleInfo> Styles = new()
        {
            [TagType.Italic] = Italic,
            [TagType.Strong] = Strong,
            [TagType.Header] = Header
        };
    }
}