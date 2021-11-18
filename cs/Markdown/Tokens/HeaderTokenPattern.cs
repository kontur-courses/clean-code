using System.Collections.Generic;
using Markdown.Models;

namespace Markdown.Tokens
{
    public class HeaderTokenPattern : ITokenPattern
    {
        public int StartTagLength { get; } = 2;
        public int EndTagLength { get; } = 0;
        public bool LastCloseSucceed { get; private set; }
        public IEnumerable<TagType> ForbiddenChildren { get; } = new List<TagType>();

        public bool TrySetStart(Context context) => context.Text.StartsWith("# ") && context.Index == 0;

        public bool TryContinue(Context context)
        {
            var isLineEnd = context.Index == context.Text.Length;
            if (isLineEnd)
            {
                LastCloseSucceed = true;
                return false;
            }

            var symbol = context.Text[context.Index];
            var isTerminateSymbol = symbol is '\n' or '\r';
            LastCloseSucceed = isTerminateSymbol;
            return !isTerminateSymbol;
        }
    }
}