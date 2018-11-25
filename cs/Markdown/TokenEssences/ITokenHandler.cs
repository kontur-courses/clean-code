
using System;

namespace Markdown.TokenEssences
{
    public interface ITokenHandler
    {
        string TokenAssociation { get; }
        char StopChar { get; }
        bool IsNestedToken { get; set; }

        TypeToken GetNextTypeToken(string content, int position);
        bool IsStopToken(string content, int position);
        bool IsStartToken(string content, int position);
        bool ContainsNestedToken(string content, int position);
        ITokenHandler GetNextNestedToken(string content, int position);
    }
}