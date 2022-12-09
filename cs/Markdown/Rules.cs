using Markdown.Tag;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public static class Rules
    {
        public static IList<Func<string, char, char, bool>> Interactions { get; }

        private static readonly IDictionary<ITag, IEnumerable<ITag>> Replacement;

        static Rules()
        {
            Replacement = new Dictionary<ITag, IEnumerable<ITag>>
            {
                [MarkdownTag.Italic] = new[] { HtmlTag.Emphasis },
                [MarkdownTag.Bold] = new[] { HtmlTag.Strong },
                [MarkdownTag.Heading] = new[] { HtmlTag.Heading }
            };

            Interactions = new List<Func<string, char, char, bool>>
            {
                (nesting, prev, next) => nesting == string.Empty,
                (nesting, prev, next) => nesting.All(char.IsDigit),
                (nesting, prev, next) => char.IsWhiteSpace(nesting.First()),
                (nesting, prev, next) => char.IsWhiteSpace(nesting.Last()),
                (nesting, prev, next) => (char.IsLetter(prev) || char.IsLetter(next)) &&
                                         nesting.Contains(' ')
            };
        }

        public static TOut GetReplacement<TIn, TOut>(TIn tag)
            where TIn : ITag
            where TOut : ITag
        {
            return (TOut)Replacement[tag]
                .SingleOrDefault(t => t.GetType() == typeof(TOut));
        }
    }
}
