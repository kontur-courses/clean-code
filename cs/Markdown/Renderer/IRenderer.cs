using System.Collections.Generic;

namespace Markdown
{
    public interface IRenderer<TTag>
    {
        public IToken<TTag>[] Render(IEnumerable<IRule> rules);
    }
}
