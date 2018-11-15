using System;

namespace Markdown
{
    public class TegRule
    {
        public Func<Teg, bool> Check { get; }
        public TegRule(Func<Teg, bool> rule) => Check = rule;
    }
}
