using System.Collections.Generic;

namespace Markdown
{
    public interface IHandler
    {
        public IEnumerable<IToken> Handle(IEnumerable<IToken> tokens);
    }
}
