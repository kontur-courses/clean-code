using System;
using System.Collections.Generic;

namespace Markdown.Properties.Implementations
{
    public class MarkDownFiniteDetermentationAutomaton : FiniteDerminationAutomaton<char>
    {
        private readonly HashSet<char> notTrashSymbols;
        
        public MarkDownFiniteDetermentationAutomaton() : base()
        {
           
            notTrashSymbols = new HashSet<char>(new[] {' ', '\n', '\\', '\t', '\0'});

            var whiteSpaceCollectorState = new State<char>(initialState);
            var trashCollectorState = new State<char>(initialState);
            var slashEcranationState = new State<char>(trashCollectorState);

            Func<char, bool> whiteSpaceCollectorCondition = ch => char.IsWhiteSpace(ch);
            Func<char, bool> trashCollectorCondition = ch => !notTrashSymbols.Contains(ch);

            whiteSpaceCollectorState.AddTransition(whiteSpaceCollectorCondition, whiteSpaceCollectorState);
            initialState.AddTransition(whiteSpaceCollectorCondition, whiteSpaceCollectorState);
            initialState.Add('\\', slashEcranationState);
            slashEcranationState.AddTransition(whiteSpaceCollectorCondition, whiteSpaceCollectorState);
            initialState.AddTransition(trashCollectorCondition, trashCollectorState);
            trashCollectorState.AddTransition(trashCollectorCondition, trashCollectorState);
        }
        
        public State<char> AddNewTag(string tag)
        {
            var currentState = initialState;
            if (tag.Length != 0)
                notTrashSymbols.Add(tag[0]);
            foreach (var symbol in tag)
            {
                if (!currentState.ContainsKey(symbol))
                    currentState.Add(symbol, new State<char>(initialState));
                currentState = currentState.GetNextState(symbol);
            }

            return currentState;
        }
    }
}