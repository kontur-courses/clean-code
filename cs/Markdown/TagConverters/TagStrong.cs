using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown.TagConverters
{
    internal class TagStrong : TagConverter
    {
        public override TagHtml Html => TagHtml.strong;

        public override TagMd Md => TagMd.__;

        public override StringOfset Convert(string text, int position)
        {
            throw new NotImplementedException();
        }
    }
}
