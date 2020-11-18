using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown.TagConverters
{
    class LiTagConverter : TagConverterBase
    {
        protected override HashSet<string> TagInside => new HashSet<string>();

        public override string TagHtml => Markdown.TagHtml.li;

        public override string TagName => MarkdownElement.empty;

    }
}
