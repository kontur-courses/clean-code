using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class TagBold : ITag

    {
        public bool IsAtTheBeginning { get; set; }
        public bool IsClosed { get; set; }

        public string HtmlTagAnalog
        {
            get
            {
                if (IsClosed)
                    return (IsStartTag) ? "<strong>" : "</strong>";
                return SimpleChar;
            }
        }

        public string SimpleChar => "__";

        public bool IsStartTag { get; set; }

        public string Content => HtmlTagAnalog;

        public bool IsPrevent { get; set; }

        public bool IsTag => true;
    }
}
