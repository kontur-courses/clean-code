
using System;

namespace Markdown.Types
{
    public interface ITokenHandler
    {
        string TokenAssociation { get; set; }
        Func<char, bool> IsStopChar { get; set; }

        TypeToken GetNextTypeToken(string content, int position);
        bool IsStopToken(string content, int position);
        bool IsStartToken(string content, int position);
        bool IsNestedToken(string content, int position);
        ITokenHandler GetNextNestedToken(string content, int position);
    }
}