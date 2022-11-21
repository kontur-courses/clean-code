using System.Collections.Generic;

namespace Markdown.Automaton.Interfaces
{
    public interface IPushdownAutomaton
    {
        TransitionFunction TransitionFunction { get; }
        private static Stack<string> stack;

        public bool Run(char[] tokens);
    }
}
