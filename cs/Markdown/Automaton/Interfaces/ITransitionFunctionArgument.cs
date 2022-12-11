namespace Markdown.Automaton.Interfaces
{
    public interface ITransitionFunctionArgument
    {
        int CurrentState { get; }
        string InputToken { get; }
        string? StackTop { get; set; }
    }
}
