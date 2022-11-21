using System.Collections.Generic;

namespace Markdown.Automaton.Interfaces
{
    public interface ITransitionFunction
    {
        Dictionary<TransitionFunctionArgument, TransitionFunctionValue> Transitions { get; }
        public ITransitionFunctionValue GetFunctionValue(ITransitionFunctionArgument argument);
    }
}
