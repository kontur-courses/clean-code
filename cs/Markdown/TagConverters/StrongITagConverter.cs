using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown.TagConverters
{
    internal class StrongITagConverter : TagConverterBase
    {
        protected override string Html => TagHtml.strong;

        protected override string Md => MarkdownElement.__;
        protected internal override bool IsSingleTag => false;

        protected internal override bool IsTag(string text, int pos) => IsTagBase(text, pos);
        protected override bool CanClose(StringBuilder text, int pos) => CanCloseBase(text, pos);
        protected override bool CanOpen(StringBuilder text, int pos) => CanOpenBase(text, pos);
        protected override HashSet<string> TagInside => new HashSet<string>() { new EmITagConverter().StringMd };
    }
}
