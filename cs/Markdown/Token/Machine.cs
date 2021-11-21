using System;
using System.Collections.Generic;

namespace Markdown
{
    public class Machine
    {
        private State currentState;
        private readonly string input;

        private Machine(string input, State startState)
        {
            currentState = startState;
            this.input = input;
        }

        public static Machine CreateMachine(string input, State startState)
        {
            return new Machine(input, startState);
        }

        public void Run()
        {
            for (var index = 0; index < input.Length; index++)
            {
                var nextState = GetNextState(input[index]);
                if (!nextState.Equals(currentState))
                {
                    currentState.OnExit(input, index);
                    currentState = nextState;
                    currentState.OnEntry(input, index);
                }
                else
                {
                    currentState.OnStay(input, index);
                }
            }
            currentState.OnExit(input, input.Length - 1);
        }

        private State GetNextState(char symbol)
        {
            return !currentState.Transitions.ContainsKey(symbol)
                ? currentState.Fallback
                : currentState.Transitions[symbol];
        }
    }
}