using System;
using System.Collections.Generic;

namespace Markdown.Token
{
    public class Machine
    {
        private readonly HashSet<State> states;
        private State currentState;
        private readonly string input;

        private Machine(string input, State startState)
        {
            states = new HashSet<State>();
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
        
        public Machine AddState(State state)
        {
            if (state.Fallback is null)
                throw new ArgumentException("State should have fallback!");
            states.Add(state);
            return this;
        }

        private State GetNextState(char symbol)
        {
            return !currentState.Transitions.ContainsKey(symbol)
                ? currentState.Fallback
                : currentState.Transitions[symbol];
        }
    }
}