using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown.TagConverters
{
    class LiITagConverter : TagConverterBase
    {
        protected internal override bool IsSingleTag => false;

        protected override HashSet<string> TagInside => throw new NotImplementedException();

        protected override string Html => TagHtml.li;

        protected override string Md => MarkdownElement.empty;

        protected override bool CanClose(StringBuilder text, int pos) => false;

        protected override bool CanOpen(StringBuilder text, int pos) => false;

        protected internal override bool IsTag(string text, int pos) => false;
    }
}
