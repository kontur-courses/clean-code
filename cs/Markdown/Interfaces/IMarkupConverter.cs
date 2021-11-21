using System.Collections.Generic;

namespace Markdown.Interfaces
{
    public interface IMarkupConverter
    {
        string Convert(IEnumerable<IToken> inputTagTokens);
    }
}