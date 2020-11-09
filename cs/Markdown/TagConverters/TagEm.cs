using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown.TagConverters
{
    internal class TagEm : TagConverter
    {
        public override TagHtml Html => TagHtml.em;
        public override TagMd Md => TagMd._;
        public override StringOfset Convert(string text, int position)
        {
            var result = new StringBuilder();
            result.Append(OpenTag());
            int pos;
            for(pos = position + 1; text[pos].ToString() != StringMd; pos++)
            {
                result.Append(text[pos].ToString());
            }
            result.Append(CloseTag());
            return new StringOfset(result.ToString(), pos - position + 1);
        }
    }
}
