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
        private Dictionary<ITransitionFunctionArgument, ITransitionFunctionValue> Transitions { get; }
    }
}
