using System.Collections.Generic;
using Markdown.Models;

namespace Markdown.Tokens.Patterns
{
    public class ParagraphTokenPattern : ITokenPattern
    {
        public string StartTag { get; }
        public string EndTag => "";
        public bool LastEndingSucceed { get; private set; }
        public IEnumerable<TagType> ForbiddenChildren { get; } = new List<TagType>();

        public ParagraphTokenPattern(string startSymbol)
        {
            StartTag = startSymbol + " ";
        }

        public bool TrySetStart(Context context) => context.Text.StartsWith(StartTag) && context.Index == 0;

        public bool TryContinue(Context context)
        {
            var isLineEnd = context.Index == context.Text.Length;
            if (isLineEnd)
            {
                LastEndingSucceed = true;
                return false;
            }

            var isTerminateSymbol = context.CurrentSymbol is '\n' or '\r';
            LastEndingSucceed = isTerminateSymbol;
            return !isTerminateSymbol;
        }
    }
}