using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class ReplaceInfo
    {
        public int StartIndex { get; }
        public Mark MarkToReplace { get; }

        public ReplaceInfo(int startIndex, Mark markToReplace)
        {
            StartIndex = startIndex;
            MarkToReplace = markToReplace;
        }
    }
}
