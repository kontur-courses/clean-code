using System.Collections.Generic;

namespace Markdown
{
    public interface IConverter
    {
        string GetHtml(IReadOnlyCollection<TextToken> textTokens);
    }
}