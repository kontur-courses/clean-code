using System.Collections.Generic;
using Markdown.Automaton.Interfaces;
using System;

namespace Markdown.Automaton
{
    public class TransitionFunction : ITransitionFunction
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
