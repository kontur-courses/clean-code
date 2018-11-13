using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    public class StateTransitionTable
    {
        private readonly int[,] innerTable;
        private readonly State[] states;
        private readonly CharsGroup[] chars;

        public StateTransitionTable(State[] states, CharsGroup[] chars, int[,] stateTable)
        {
            if(states.Length != stateTable.GetLength(0) || chars.Length != stateTable.GetLength(1))
                throw new ArgumentException("Length states array or chars array not equals stateTable dimension");
            this.states = states;
            this.chars = chars;
            innerTable = stateTable;
        }

        public State this[int indexState, char sym]
        {
            get
            {
                var currentGroup = GetCharsGroupByChar(sym);
                var indexNewState = innerTable[indexState, currentGroup.Index];
                return GetStateByIndex(indexNewState);
            }
        }

        private CharsGroup GetCharsGroupByChar(char ch)
        {
            return chars.Where(group => group.Contains(ch)).SingleOrDefault();
        }

        private State GetStateByIndex(int index)
        {
            return states.Where(state => state.Index == index).SingleOrDefault();
        }
    }
}
