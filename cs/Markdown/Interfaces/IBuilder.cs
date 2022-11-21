using System.Collections.Generic;

namespace Markdown.Interfaces
{
    public interface IBuilder
    {
        public string Build(IEnumerable<Token> tokens);
    }
}