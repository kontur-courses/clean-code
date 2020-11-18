using System.Collections.Generic;
using System.Text;

namespace Markdown.TagConverters
{
    internal class EmTagConverter : TagConverterBase
    {
        public override string TagHtml => Markdown.TagHtml.em;
        public override string TagName => MarkdownElement._;
        protected override HashSet<string> TagInside => new HashSet<string> ();
    }
}
