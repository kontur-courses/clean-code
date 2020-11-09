using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown.TagConverters
{
    internal abstract class TagConverter : ITagConverter
    {
        public abstract TagHtml Html { get; }
        public abstract TagMd Md { get; }
        public string StringMd => Md.ToString();
        public int LengthMd => StringMd.Length;

        public string StringHtml => Html.ToString();
        public string OpenTag() => string.Format("<{0}>", StringHtml);
        public string CloseTag() => string.Format(@"<\{0}>", StringHtml);
        public abstract StringOfset Convert(string text, int position);
    }
}
