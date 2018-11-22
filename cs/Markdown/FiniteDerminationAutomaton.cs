using System;
using System.Collections.Generic;

namespace Markdown.Properties
{
    public class FiniteDerminationAutomaton
    {
        private readonly State initialState;
        private readonly HashSet<char> notTrashSymbols;

        public FiniteDerminationAutomaton()
        {
            notTrashSymbols = new HashSet<char>(new[] {' ', '\n', '\\', '\t', '\0'});
            initialState = new State(initialState);

            var whiteSpaceCollectorState = new State(initialState);
            var trashCollectorState = new State(initialState);
            var slashEcranationState = new State(trashCollectorState);

            Func<char, bool> whiteSpaceCollectorCondition = ch => char.IsWhiteSpace(ch);
            Func<char, bool> trashCollectorCondition = ch => !notTrashSymbols.Contains(ch);

            whiteSpaceCollectorState.AddTransition(whiteSpaceCollectorCondition, whiteSpaceCollectorState);
            initialState.AddTransition(whiteSpaceCollectorCondition, whiteSpaceCollectorState);
            initialState.Add('\\', slashEcranationState);
            slashEcranationState.AddTransition(whiteSpaceCollectorCondition, whiteSpaceCollectorState);
            initialState.AddTransition(trashCollectorCondition, trashCollectorState);
            trashCollectorState.AddTransition(trashCollectorCondition, trashCollectorState);
        }

        public State AddNewTag(string tag)
        {
            var currentState = initialState;
            if (tag.Length != 0)
                notTrashSymbols.Add(tag[0]);
            foreach (var symbol in tag)
            {
                if (!currentState.ContainsKey(symbol))
                    currentState.Add(symbol, new State(initialState));
                currentState = currentState.GetNextState(symbol);
            }

            return currentState;
        }

        public State GetInitialState()
        {
            return initialState;
        }
    }


    public class State : Dictionary<char, State>
    {
        private readonly State dropOut;
        private readonly List<Tuple<Func<char, bool>, State>> transitions;

        public State(State dropOut)
        {
            transitions = new List<Tuple<Func<char, bool>, State>>();
            foreach (var transition in transitions)
                transitions.Add(transition);
            this.dropOut = dropOut;
        }

        public State GetNextState(char ch)
        {
            if (ContainsKey(ch))
                return base[ch];

            foreach (var kvp in transitions)
                if (kvp.Item1(ch))
                    return kvp.Item2;
            return dropOut;
        }

        public void AddTransition(Func<char, bool> condition, State state)
        {
            transitions.Add(Tuple.Create(condition, state));
        }
    }
}