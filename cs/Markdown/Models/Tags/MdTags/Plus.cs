using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown.Models.Tags.MdTags
{
    internal class Plus : Tag
    {
        public override string Opening => "+";
        public override string Closing => "\n";
    }
}
