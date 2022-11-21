using System.Collections.Generic;
using Markdown.Automaton.Interfaces;
using System;

namespace Markdown.Automaton
{
    public class TransitionFunction : ITransitionFunction
    {
        public Dictionary<TransitionFunctionArgument, TransitionFunctionValue> Transitions { get; }

        public ITransitionFunctionValue GetFunctionValue(ITransitionFunctionArgument argument)
        {
            throw new NotImplementedException();
        }

        public TransitionFunction(
            Dictionary<TransitionFunctionArgument, TransitionFunctionValue> transitions)
        {
            Transitions = transitions;
        }

    }
}
