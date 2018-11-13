using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class Md
    {
        private StateTransitionTable table;

        private State currentState;

        public Md(StateTransitionTable table)
        {
            this.table = table;
        }

        public string Render(string markdownString)
        {
            var resultString = new StringBuilder();
            foreach (var sym in markdownString)
            {
                currentState = table[currentState.Index, sym];
                currentState.Action.Invoke(sym, resultString);
            }
            return resultString.ToString();
        }

    }
}
