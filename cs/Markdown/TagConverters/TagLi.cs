using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown.TagConverters
{
    class TagLi : TagConverterBase
    {
        public override bool IsSingleTag => false;

        public override HashSet<string> TagInside => throw new NotImplementedException();

        public override string Html => TagHtml.li;

        public override string Md => MarkdownElement.empty;

        public override bool CanClose(StringBuilder text, int pos) => false;

        public override bool CanOpen(StringBuilder text, int pos) => false;

        public override bool IsTag(string text, int pos) => false;
    }
}
