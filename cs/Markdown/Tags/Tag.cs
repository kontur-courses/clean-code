using Markdown.Extensions;
using System;

namespace Markdown
{
    public abstract class Tag
    {
        public abstract string OpenHTMLTag { get; }
        public abstract string CloseHTMLTag { get; }
        public abstract string OpenMdTag { get; }
        public abstract string CloseMdTag { get; }
        public abstract bool AllowNesting { get; }
        public abstract Func<string, int, bool> IsCorrectOpenTag { get; }
        public abstract Func<string, int, string, bool> IsCorrectCloseTag { get; }
    }

    public abstract class SelectingTag : Tag
    {
        public override Func<string, int, bool> IsCorrectOpenTag => (mdText, position) =>
        {
            mdText.TryGetNextChars(position, 1, out char[] nextChars);
            if (nextChars != null && char.IsWhiteSpace(nextChars[0]))
                return false;
            return true;
        };

        public override Func<string, int, string, bool> IsCorrectCloseTag => (mdText, position, mdTag) =>
        {
            position = position - mdTag.Length + 1;
            mdText.TryGetCharsBehind(position, 1, out char[] behindChars);
            if (behindChars != null && char.IsWhiteSpace(behindChars[0]))
                return false;
            return true;
        };
    }
}
