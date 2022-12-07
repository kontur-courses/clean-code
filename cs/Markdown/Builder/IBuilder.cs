using System.Collections.Generic;

namespace Markdown
{
    public interface IBuilder
    {
        public string Build(IEnumerable<IToken> tokens);
    }
}
