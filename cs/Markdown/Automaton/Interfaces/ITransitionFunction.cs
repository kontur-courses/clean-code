using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.PushdownAutomaton.Interfaces
{
    public interface ITransitionFunction
    {
        Dictionary<ITransitionFunctionArgument, ITransitionFunctionValue> Transitions { get; }
        public ITransitionFunctionValue GetFunctionValue(ITransitionFunctionArgument argument);
    }
}
