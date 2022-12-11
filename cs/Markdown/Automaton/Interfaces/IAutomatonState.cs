namespace Markdown.Automaton.Interfaces
{
    public interface IAutomatonState
    {
        public int IndexNumber { get; }
        public bool IsFinalState { get; }
        public ITransitionFunction TransitionFunction { get; set; }
    }
}
