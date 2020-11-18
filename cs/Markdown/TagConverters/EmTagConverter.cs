using System.Collections.Generic;
using Markdown.Constants;

namespace Markdown.TagConverters
{
    internal class EmTagConverter : TagConverterBase
    {
        public override string TagHtml => Constants.TagHtml.em;
        public override string TagName => MarkdownElement._;
        protected override HashSet<string> TagInside => new HashSet<string> ();
    }
}
