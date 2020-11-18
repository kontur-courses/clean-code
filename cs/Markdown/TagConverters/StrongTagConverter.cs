using System.Collections.Generic;
using Markdown.Constants;

namespace Markdown.TagConverters
{
    internal class StrongTagConverter : TagConverterBase
    {
        public override string TagHtml => Constants.TagHtml.strong;
        public override string TagName => MarkdownElement.__;
        protected override HashSet<string> TagInside => new HashSet<string>() { new EmTagConverter().TagName };
    }
}
