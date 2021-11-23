using System.Collections.Generic;
using Markdown.Models;

namespace Markdown.Tokens.Patterns
{
    public class PairedTokenPattern : ITokenPattern
    {
        public string StartTag => tag;
        public string EndTag => tag;
        public bool LastEndingSucceed { get; private set; }
        public IEnumerable<TagType> ForbiddenChildren { get; init; } = new List<TagType>();

        private readonly string tag;
        private bool isStartInWord;

        public PairedTokenPattern(string tag)
        {
            this.tag = tag;
        }

        public bool TrySetStart(Context context)
        {
            var canStart = CanStart(context);
            if (canStart)
                isStartInWord = context.CharBefore(char.IsLetter)
                                && context.CharAfter(StartTag, char.IsLetter);

            return canStart;
        }

        public bool TryContinue(Context context)
        {
            if (context.Skip(-1).IsStringEnding() || char.IsWhiteSpace(context.CurrentSymbol) && isStartInWord)
            {
                LastEndingSucceed = false;
                return false;
            }

            if (IsPatternEnd(context))
            {
                LastEndingSucceed = true;
                return false;
            }

            return true;
        }

        private bool CanStart(Context context) =>
            context.StartsWith(StartTag)
            && context.CharAfter(StartTag, char.IsLetter)
            && (context.IsStringBeginning()
                || context.CharBefore(char.IsLetter)
                || context.CharBefore(char.IsWhiteSpace));

        private bool IsPatternEnd(Context context) =>
            context.StartsWith(EndTag)
            && context.CharBefore(char.IsLetter)
            && (context.Skip(EndTag.Length - 1).IsStringEnding()
                || context.CharAfter(EndTag, char.IsLetter)
                || context.CharAfter(EndTag, char.IsWhiteSpace));
    }
}