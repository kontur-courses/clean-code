using System.Collections.Generic;

namespace Markdown
{
    public interface IRule
    {
        List<Token> Apply(List<Token> symbolsMap);
    }
}