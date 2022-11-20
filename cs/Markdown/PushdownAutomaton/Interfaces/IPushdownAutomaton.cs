using Markdown.Token;

namespace Markdown.PushdownAutomaton.Interfaces
{
    public interface IPushdownAutomaton
    {
        ITransitionFunction TransitionFunction { get; }
        private static Stack<IToken> stack = null!;

        public bool Run(IToken[] tokens);
    }
}
