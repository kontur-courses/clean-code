using System.Collections.Generic;

namespace Markdown.Interfaces
{
    public interface IConvectorMarkup
    {
        string Convert(IEnumerable<TagToken> inputTagTypes);
    }
}
