using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class Delimiter
    {
        public readonly bool canBeClosing;
        public readonly bool canBeStarting;
        public int index;
        public string delimiter;

        public Delimiter(string delimiter, int index, bool canBeClosing, bool canBeStarting)
        {
            this.delimiter = delimiter;
            this.index = index;
            this.canBeClosing = canBeClosing;
            this.canBeStarting = canBeStarting;
        }
    }
}