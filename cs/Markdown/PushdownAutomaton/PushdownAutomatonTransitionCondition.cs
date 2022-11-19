using Markdown.PushdownAutomaton.Interfaces;
using Markdown.Token;

namespace Markdown.PushdownAutomaton
{
    internal class PushdownAutomatonTransitionCondition : IPushdownAutomatonTransitionCondition
    {
        public IToken InputValue { get; }
        public IToken StackTop { get; }
        public IToken NewStackElements { get; }

        public PushdownAutomatonTransitionCondition(Token.Token value, Token.Token stackTop, Token.Token newStackElements)
        {
            InputValue = value;
            StackTop = stackTop;
            NewStackElements = newStackElements;
        }
    }
}
