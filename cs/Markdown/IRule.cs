using System.Collections.Generic;

namespace Markdown
{
    public interface IRule
    {
        SortedList<int, Token> Apply(SortedList<int, Token> symbolsMap);
    }
}