using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.Core.Rules
{
    class RuleFactory
    {
        public static IRule[] CreateAllRules() => new IRule[] {new EmRule(), new StrongRule() };
    }
}
