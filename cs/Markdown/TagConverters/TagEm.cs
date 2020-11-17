using System.Collections.Generic;
using System.Text;

namespace Markdown.TagConverters
{
    internal class TagEm : TagConverterBase
    {
        public override string Html => TagHtml.em;
        public override string Md => MarkdownElement._;

        public override bool CanClose(StringBuilder text, int pos) => CanCloseBase(text, pos);
        public override bool CanOpen(StringBuilder text, int pos) => CanOpenBase(text, pos);
        public override bool IsTag(string text, int pos) => IsTagBase(text, pos);

        public override bool IsSingleTag => false;

        public override HashSet<string> TagInside => new HashSet<string> ();
    }
}
