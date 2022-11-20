using Markdown.PushdownAutomaton.Interfaces;
using Markdown.Token;

namespace Markdown.PushdownAutomaton
{
    internal class TransitionFunctionArgument : ITransitionFunctionArgument
    {
        public IToken CurrentState { get; }
        public IToken InputToken { get; }
        public IToken StackTop { get; }

        public TransitionFunctionArgument(IToken currentState, IToken inputToken, IToken stackTop)
        {
            CurrentState = currentState;
            InputToken = inputToken;
            StackTop = stackTop;
        }
    }
}
