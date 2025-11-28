using System.Collections.Generic;

namespace Markdown
{
    internal interface ITokenProcessor
    {
        List<Token> Process(List<Token> tokens);
    }
}
