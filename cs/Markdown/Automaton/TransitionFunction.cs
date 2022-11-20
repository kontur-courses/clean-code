using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown.PushdownAutomaton.Interfaces;

namespace Markdown.PushdownAutomaton
{
    internal class TransitionFunction : ITransitionFunction
    {
        public Dictionary<ITransitionFunctionArgument, ITransitionFunctionValue> Transitions { get; }

        public ITransitionFunctionValue GetFunctionValue(ITransitionFunctionArgument argument)
        {
            throw new NotImplementedException();
        }

        public TransitionFunction(
            Dictionary<ITransitionFunctionArgument, ITransitionFunctionValue> transitions)
        {
            Transitions = transitions;
        }

    }
}
