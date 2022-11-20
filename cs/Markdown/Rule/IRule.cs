using System;

namespace Markdown
{
    public interface IRule
    {
        public Func<ITag, ITag> Function { get; }
    }
}
