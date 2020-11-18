using System.Collections.Generic;
using System.Text;
using System.Linq;
using Markdown.Constants;

namespace Markdown.TagConverters
{
    internal class H1TagConverter : TagConverterBase
    {
        public override bool IsSingleTag => true;
        public override string TagHtml => Constants.TagHtml.h1;

        public override string TagName => MarkdownElement.sharp;

        protected override HashSet<string> TagInside => TagsAssociation.tags
            .Where(t => t != new UlTagConverter().TagName)
            .ToHashSet();
        public override bool IsTag(string text, int pos) => pos == 0;

        public override bool CanOpen(StringBuilder text, int pos) => pos == 0;
    }
}
