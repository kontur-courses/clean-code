using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Markdown.TagConverters
{
    internal class H1ITagConverter : TagConverterBase
    {
        protected internal override bool IsSingleTag => true;
        protected override string Html => TagHtml.h1;

        protected override string Md => MarkdownElement.sharp;

        protected override HashSet<string> TagInside => TagsAssociation.tags
            .Where(t => t != new UlITagConverter().StringMd)
            .ToHashSet();
        protected internal override bool IsTag(string text, int pos) => pos == 0;

        protected override bool CanClose(StringBuilder text, int pos) => pos == 0;
        protected override bool CanOpen(StringBuilder text, int pos) => CanCloseBase(text, pos);
    }
}
