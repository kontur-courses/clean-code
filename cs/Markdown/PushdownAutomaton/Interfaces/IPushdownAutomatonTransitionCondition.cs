using Markdown.Token;

namespace Markdown.PushdownAutomaton.Interfaces
{
    public interface IPushdownAutomatonTransitionCondition
    {
        IToken InputValue { get; }
        IToken StackTop { get; }
        IToken NewStackElements { get; }
    }
}
