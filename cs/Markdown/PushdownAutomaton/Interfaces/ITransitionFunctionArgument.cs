using Markdown.Token;

namespace Markdown.PushdownAutomaton.Interfaces
{
    internal interface ITransitionFunctionArgument
    {
        IToken CurrentState { get; }
        IToken InputToken { get; }
        IToken StackTop { get; }
    }
}
