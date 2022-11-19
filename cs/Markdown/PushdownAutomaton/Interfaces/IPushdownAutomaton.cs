using Markdown.Token;

namespace Markdown.PushdownAutomaton.Interfaces
{
    public interface IPushdownAutomaton
    {
        IPushdownAutomatonStateTransitionTable transitionTable { get; }
        private static Stack<IToken> stack = null!;

        public bool Run(IToken[] tokens);
    }
}
