using Markdown.PushdownAutomaton.Interfaces;

namespace Markdown.PushdownAutomaton
{
    internal class PushdownAutomatonStateTransitionTable : IPushdownAutomatonStateTransitionTable
    {
        public IPushdownAutomatonTransitionCondition[][] TransitionTable { get; }

        public PushdownAutomatonStateTransitionTable(
            IPushdownAutomatonTransitionCondition[][] conditions)
        {
            TransitionTable = conditions;
        }
    }
}
