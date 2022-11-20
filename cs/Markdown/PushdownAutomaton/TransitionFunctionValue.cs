using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown.PushdownAutomaton.Interfaces;
using Markdown.Token;

namespace Markdown.PushdownAutomaton
{
    internal class TransitionFunctionValue : ITransitionFunctionValue
    {
        public IToken NewCondition { get; }
        public IToken[] NewStackElements { get; }

        public TransitionFunctionValue(IToken newCondition, IToken[] newStackElements)
        {
            NewCondition = newCondition;
            NewStackElements = newStackElements;
        }
    }
}
