using Markdown.Models;

namespace Markdown.Tokens
{
    public class PairedTokenPattern : ITokenPattern
    {
        private readonly string tag;

        public PairedTokenPattern(string tag)
        {
            this.tag = tag;
        }

        public bool IsStart(Context context)
        {
            return IsStartsWithTag(context) && HasLetterAfterTag(context);
        }

        public bool IsEnd(Context context)
        {
            return IsStartsWithTag(context) && HasLetterBeforeTag(context);
        }

        private bool IsStartsWithTag(Context context) => context.Text[context.Index..].StartsWith(tag);

        private bool HasLetterAfterTag(Context context) =>
            context.Text.Length > context.Index + tag.Length
            && char.IsLetter(context.Text[context.Index + tag.Length]);

        private static bool HasLetterBeforeTag(Context context) =>
            context.Index - 1 >= 0
            && char.IsLetter(context.Text[context.Index - 1]);
    }
}