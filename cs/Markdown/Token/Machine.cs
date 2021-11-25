using System;

namespace Markdown
{
    public class Machine
    {
        private State currentState;

        private Machine(State startState)
        {
            currentState = startState;
        }

        public static Machine Create(State startState)
        {
            return new Machine(startState);
        }

        public void Run(string input)
        {
            if (input is null)
                throw new ArgumentException("Input should be not null");
            if (input.Length == 0)
                throw new ArgumentException("Input shouldn't be empty");
            for (var index = 0; index < input.Length; index++)
            {
                var nextState = GetNextState(input[index]);
                if (nextState.Equals(currentState)) continue;
                currentState = nextState;
                currentState.OnEntry(input, index);
            }
        }

        private State GetNextState(char symbol)
        {
            return !currentState.Transitions.ContainsKey(symbol)
                ? currentState.Fallback
                : currentState.Transitions[symbol];
        }
    }
}