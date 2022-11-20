using System;

namespace Markdown
{
    public class Rule : IRule
    {
        public Func<ITag, ITag> Function { get; }

        public Rule(Func<ITag, ITag> function)
        {
            Function = function;
        }
    }
}
