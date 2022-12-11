using Markdown.Automaton.Interfaces;

namespace Markdown.Automaton
{
    internal class AutomatonState : IAutomatonState
    {
        public int IndexNumber { get; }
        public bool IsFinalState { get; }
        public ITransitionFunction TransitionFunction { get; set; }

        public AutomatonState(int indexNumber, TransitionFunction transitionFunction, bool isFinalState = false)
        {
            IsFinalState = isFinalState;
            IndexNumber = indexNumber;
            TransitionFunction = transitionFunction;
        }
    }
}
