using System.Collections.Generic;

namespace Markdown.DataStructures
{
    public interface IToken
    {
        ITag Tag { get; }
        Token Parent { get; set; }
        List<Token> Children { get; }
        int StartIndex { get; }
        int EndIndex { get; set; }
        bool StartsInsideWord { get; }
    }
}