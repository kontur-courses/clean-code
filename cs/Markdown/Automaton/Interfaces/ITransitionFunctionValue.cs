namespace Markdown.Automaton.Interfaces
{
    public interface ITransitionFunctionValue
    {
        string NewState { get; }
        string[] NewStackElements { get; }
    }
}
