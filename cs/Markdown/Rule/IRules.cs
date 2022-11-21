using System.Collections.Generic;

namespace Markdown
{
    public interface IRules
    {
        public IDictionary<ITag, IRule> Rules { get; }
    }
}
