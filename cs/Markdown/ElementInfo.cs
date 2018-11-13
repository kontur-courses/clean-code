using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class ElementInfo
    {
        public int OpeningIndex { get; set; }
        public int ClosingIndex { get; set; }
        public int Length => ClosingIndex + 1 - OpeningIndex;

        public ElementInfo(int openingIndex, int closingIndex)
        {
            OpeningIndex = openingIndex;
            ClosingIndex = closingIndex;
        }
    }
}
