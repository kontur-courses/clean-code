namespace Markdown.Automaton.Interfaces
{
    public interface ITransitionFunctionArgument
    {
        string CurrentState { get; }
        string InputToken { get; }
        string StackTop { get; }
    }
}
