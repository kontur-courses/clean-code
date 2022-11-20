using System;
using System.Collections.Generic;

namespace Markdown
{
    public class DefaultRenderer<TTag> : IRenderer<TTag>
    {
        public IToken<TTag>[] Render(IEnumerable<IRule> rules)
        {
            throw new NotImplementedException();
        }
    }
}
