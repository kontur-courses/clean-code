using System;
using System.ComponentModel.Design;
using Markdown.Automaton.Interfaces;

namespace Markdown.Automaton
{
    public class Rule
    {
        public Func<ITransitionFunctionArgument, ITransitionFunctionValue>? func;
        public Rule(Func<ITransitionFunctionArgument, ITransitionFunctionValue> func)
        {
            this.func = func;
        }

        public ITransitionFunctionValue GetValue(ITransitionFunctionArgument argument)
        {
            return func(argument);
        }
    }
}
