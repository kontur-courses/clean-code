using System.Collections.Generic;
using Markdown.Models;

namespace Markdown.Tokens
{
    public class PairedTokenPattern : ITokenPattern
    {
        public int TagLength => tag.Length;
        public bool LastCloseSucceed { get; private set; }
        public IEnumerable<TagType> ForbiddenChildren { get; init; } = new List<TagType>();

        private readonly string tag;
        private bool isStartInWord;

        public PairedTokenPattern(string tag)
        {
            this.tag = tag;
        }

        public bool TrySetStart(Context context)
        {
            var canStart = IsCanStart(context);
            if (canStart)
                isStartInWord = IsLetterBeforeTag(context) && IsLetterAfterTag(context);

            return canStart;
        }

        public bool TryContinue(Context context)
        {
            if (char.IsWhiteSpace(context.Text[context.Index]) && isStartInWord)
            {
                LastCloseSucceed = false;
                return false;
            }


            if (IsEnd(context))
            {
                LastCloseSucceed = true;
                return false;
            }

            ;

            return true;
        }

        private bool IsCanStart(Context context) =>
            IsStartsWithTag(context)
            && IsLetterAfterTag(context)
            && (IsStringBeginning(context) || IsLetterBeforeTag(context) || IsWhiteSpaceBeforeTag(context));

        private bool IsEnd(Context context) =>
            IsStartsWithTag(context)
            && IsLetterBeforeTag(context)
            && (IsStringEnd(context) || IsLetterAfterTag(context) || IsWhiteSpaceAfterTag(context));

        private bool IsStartsWithTag(Context context) => context.Text[context.Index..].StartsWith(tag);

        private bool IsLetterAfterTag(Context context) =>
            context.Text.Length > context.Index + tag.Length
            && char.IsLetter(context.Text[context.Index + tag.Length]);

        private bool IsWhiteSpaceAfterTag(Context context) =>
            context.Text.Length > context.Index + tag.Length
            && char.IsWhiteSpace(context.Text[context.Index + tag.Length]);

        private bool IsStringEnd(Context context) =>
            context.Index + tag.Length == context.Text.Length;

        private static bool IsStringBeginning(Context context) =>
            context.Index == 0;

        private static bool IsLetterBeforeTag(Context context) =>
            context.Index - 1 >= 0
            && char.IsLetter(context.Text[context.Index - 1]);

        private static bool IsWhiteSpaceBeforeTag(Context context) =>
            context.Index - 1 >= 0
            && char.IsWhiteSpace(context.Text[context.Index - 1]);
    }
}