using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class Tag
    {
        public readonly string Opening;
        public readonly string Ending;

        public Tag(string name)
        {
            Opening = $"<{name}>";
            Ending = $"</{name}>";
        }
    }
}
