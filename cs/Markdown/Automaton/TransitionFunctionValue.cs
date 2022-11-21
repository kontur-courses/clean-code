using Markdown.Automaton.Interfaces;

namespace Markdown.Automaton
{
    public class TransitionFunctionValue : ITransitionFunctionValue
    {
        public string NewState { get; }
        public string[] NewStackElements { get; }

        public TransitionFunctionValue(string newState, string[] newStackElements)
        {
            NewState = newState;
            NewStackElements = newStackElements;
        }
    }
}
