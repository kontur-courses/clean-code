using System.Collections.Generic;

namespace Markdown
{
    internal interface IHtmlCreator
    {
        string CreateFromTokens(IEnumerable<Token> tokens);
    }


}