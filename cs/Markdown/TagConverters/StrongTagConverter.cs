using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown.TagConverters
{
    internal class StrongTagConverter : TagConverterBase
    {
        public override string TagHtml => Markdown.TagHtml.strong;

        public override string TagName => MarkdownElement.__;
        protected override HashSet<string> TagInside => new HashSet<string>() { new EmTagConverter().TagName };
    }
}
