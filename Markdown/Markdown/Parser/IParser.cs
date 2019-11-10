using System.Collections.Generic;

namespace Markdown.Parser
{
    public interface IParser<T>
    {
        List<T> Parse(string textToParse);
    }
}
