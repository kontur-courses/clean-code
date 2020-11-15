using System.Collections.Generic;

namespace Markdown.TokenConverters
{
    public interface ITokenConverter
    {
        string ConvertTokenToString(TextToken token, IReadOnlyCollection<ITokenConverter> tokenConverters);
    }
}