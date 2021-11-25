using System;
using System.Collections.Generic;

namespace Markdown
{
    public class State
    {
        public readonly Dictionary<char, State> Transitions;
        public State Fallback { get; private set; }
        public Action<string, int> OnEntry { get; private set; }

        private State()
        {
            Transitions = new Dictionary<char, State>();
            OnEntry = (s, i) => { };
        }

        public static State Create() => new State();

        public State AddTransition(char symbol, State nextState)
        {
            if (Transitions.ContainsKey(symbol))
                throw new ArgumentException("This symbol is already there");
            Transitions.Add(symbol, nextState);
            return this;
        }

        public State SetFallback(State fallbackState)
        {
            Fallback = fallbackState;
            return this;
        }

        public State SetOnEntry(Action<string, int> action)
        {
            OnEntry = action;
            return this;
        }
    }
}