using System;
using System.Collections.Generic;

namespace Markdown.Properties
{
    public class FiniteDerminationAutomaton<Type>
    {
        protected readonly State<Type> initialState;
        

        public FiniteDerminationAutomaton()
        {
            initialState = new State<Type>(initialState);
        }


        public State<Type> GetInitialState()
        {
            return initialState;
        }
    }


    public class State<Type> : Dictionary<Type, State<Type>>
    {
        private readonly State<Type> defaultTransitionState;
        private readonly List<Tuple<Func<Type, bool>, State<Type>>> transitions;

        public State(State<Type> defaultTransitionState)
        {
            transitions = new List<Tuple<Func<Type, bool>, State<Type>>>();
            foreach (var transition in transitions)
                transitions.Add(transition);
            this.defaultTransitionState = defaultTransitionState;
        }

        public State<Type> GetNextState(Type ch)
        {
            if (ContainsKey(ch))
                return base[ch];

            foreach (var kvp in transitions)
                if (kvp.Item1(ch))
                    return kvp.Item2;
            return defaultTransitionState;
        }

        public void AddTransition(Func<Type, bool> condition, State<Type> state)
        {
            transitions.Add(Tuple.Create(condition, state));
        }
    }
}