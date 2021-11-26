using System.Collections.Generic;

namespace Markdown
{
    public interface IParser
    {
        public List<Token> Parse(string text);
    }
}