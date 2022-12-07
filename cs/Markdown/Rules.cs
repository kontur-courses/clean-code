using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public static class Rules
    {
        private readonly static Dictionary<ITag, IEnumerable<ITag>> Replacement;

        static Rules()
        {
            Replacement = new Dictionary<ITag, IEnumerable<ITag>>
            {
                [MarkdownTag.Italic] = new[] { HTMLTag.Emphasys },
                [MarkdownTag.Bold] = new[] { HTMLTag.Strong },
                [MarkdownTag.Heading] = new[] { HTMLTag.Heading },
            };
        }

        public static TT GetReplacement<T, TT>(T tag)
            where T : ITag
            where TT : ITag
        {
            return (TT)Replacement[tag]
                .SingleOrDefault(t => t.GetType() == typeof(TT));
        }
    }
}
