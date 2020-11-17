using System.Collections.Generic;
using System.Text;

namespace Markdown.TagConverters
{
    internal class EmITagConverter : TagConverterBase
    {
        protected override string Html => TagHtml.em;
        protected override string Md => MarkdownElement._;

        protected override bool CanClose(StringBuilder text, int pos) => CanCloseBase(text, pos);
        protected override bool CanOpen(StringBuilder text, int pos) => CanOpenBase(text, pos);
        protected internal override bool IsTag(string text, int pos) => IsTagBase(text, pos);

        protected internal override bool IsSingleTag => false;

        protected override HashSet<string> TagInside => new HashSet<string> ();
    }
}
