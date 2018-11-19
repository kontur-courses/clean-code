
using System;

namespace Markdown.Types
{
    public interface IToken
    {
        int Position { get; set; }
        int Length { get; set; }
        string Value { get; set; }
        TypeToken TypeToken { get; set; }
        string TokenAssociation { get; set; }
        Func<char, bool> IsStopChar { get; set; }

        TypeToken GetNextTypeToken(string content, int position);
        bool IsStopToken(string content, int position);
        bool IsStartToken(string content, int position);
        bool IsNestedToken(string content, int position);
        IToken GetNextNestedToken(string content, int position);
    }
}