using System.Collections.Generic;

namespace Markdown
{
    public interface IConverter
    {
        string ConvertTokens(List<Token> tokens);
    }
}