using System.Collections.Generic;

namespace Markdown
{
    public interface IConverter
    {
        string CovertTokensToHtml(List<Token> tokens);
    }
}