using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class MdBoldStyleFinder : MdEmphasisStyleFinder
    {
        public MdBoldStyleFinder(Style mdStyle, string text) : base(mdStyle, text)
        {
        }
    }
}
