using Markdown.Automaton.Interfaces;

namespace Markdown.Automaton
{
    public class TransitionFunctionValue : ITransitionFunctionValue
    {
        public int NewState { get; }
        public string[] NewStackElements { get; }

        public TransitionFunctionValue(int newState, string[] newStackElements)
        {
            NewState = newState;
            NewStackElements = newStackElements;
        }

        public TransitionFunctionValue(int newState, string newStackElement)
        {
            NewState = newState;
            NewStackElements = new[] {newStackElement};
        }
    }
}
