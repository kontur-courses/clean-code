using System.Collections.Generic;
using Markdown.Models;

namespace Markdown.Tokens
{
    public interface ITokenPattern
    {
        string StartTag { get; }
        string EndTag { get; }
        bool LastEndingSucceed { get; }
        IEnumerable<TagType> ForbiddenChildren { get; }
        bool TrySetStart(Context context);
        bool TryContinue(Context context);
    }
}