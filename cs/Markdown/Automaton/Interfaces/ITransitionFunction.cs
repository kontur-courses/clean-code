using System.Collections.Generic;

namespace Markdown.Automaton.Interfaces
{
    public interface ITransitionFunction
    {
        Dictionary<ITransitionFunctionArgument, ITransitionFunctionValue> Transitions { get; }
        public ITransitionFunctionValue GetFunctionValue(ITransitionFunctionArgument argument);
    }
}
