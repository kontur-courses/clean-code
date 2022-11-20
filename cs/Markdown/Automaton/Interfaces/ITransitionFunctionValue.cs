using Markdown.Token;

namespace Markdown.PushdownAutomaton.Interfaces
{
    internal interface ITransitionFunctionValue
    {
        IToken NewCondition { get; }
        IToken NewStackElements { get; }
    }
}
