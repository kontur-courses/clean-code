using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    class Substring
    {
        public Substring(int index, string value)
        {
            Index = index;
            Value = value;
        }

        public int Index { get; private set; }

        public string Value { get; private set; }

        public int Length => Value.Length;
    }
}