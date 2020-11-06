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
            throw new NotImplementedException();
        }
    }
}
