using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown.Models.Tags.MdTags
{
    class Escaped : Tag
    {
        public override string Opening => "\\";
    }
}
