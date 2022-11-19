namespace Markdown.PushdownAutomaton.Interfaces
{
    public interface IPushdownAutomatonStateTransitionTable
    {
        IPushdownAutomatonTransitionCondition[][] TransitionTable { get; }
    }
}
