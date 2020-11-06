using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown.TagConverters
{
    internal class TagH1 : TagConverter
    {
        public override TagHtml Html => TagHtml.h1;

        public override TagMd Md => TagMd.sharp;

        public new string StringMd => "#";

        public override StringOfset Convert(string text, int position)
        {
            throw new NotImplementedException();
        }
    }
}
