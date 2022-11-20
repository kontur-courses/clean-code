using Markdown.Tokens;

namespace Markdown.Automaton.Interfaces
{
    public interface IPushdownAutomaton
    {
        ITransitionFunction TransitionFunction { get; }
        private static Stack<IToken> stack = null!;

        public bool Run(IToken[] tokens);
    }
}
