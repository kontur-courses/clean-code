using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown.Models.Tags.HtmlTags
{
    internal class Strong : Tag
    {
        public override string Opening => "<strong>";
        public override string Closing => "</strong>";
    }
}
