using System;

namespace Markdown
{
    public class Rule
    {
        public Func<Teg, bool> Check;
        public Rule(Func<Teg, bool> rule)
        {
            Check = rule;
        }
    }
}
