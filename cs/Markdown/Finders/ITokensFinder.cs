using System.Collections.Generic;

namespace Markdown
{
    public interface ITokensFinder
    {
        public IEnumerable<Token> Find();
    }
}