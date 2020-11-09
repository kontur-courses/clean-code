using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class Tags
    {
        public List<int> EMIndexes;
        public List<int> StrongIndexes;
        public List<int> SharpIndexes;

        public Tags()
        {
            EMIndexes = new List<int>();
            StrongIndexes = new List<int>();
            SharpIndexes = new List<int>();
        }
    }
}
