using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class State
    {
        public int Index { get; }
        public Action<char, StringBuilder> Action { get; }

        public State(int index, Action<char, StringBuilder> action)
        {
            Index = index;
            Action = action;
        }
    }
}
