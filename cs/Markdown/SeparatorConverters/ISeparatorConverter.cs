using System.Collections.Generic;

namespace Markdown.SeparatorConverters
{
    public interface ISeparatorConverter
    {
        List<string> GetTokensFormats(string separator, int tokensCount);
    }
}