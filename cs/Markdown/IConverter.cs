using System.Collections.Generic;

namespace Markdown
{
    public interface IConverter
    {
        string ConvertTokensToHtml(List<Token> tokens);
    }
}