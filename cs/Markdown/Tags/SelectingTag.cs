using Markdown.Extensions;

namespace Markdown.Tags
{
    public abstract class SelectingTag : Tag
    {
        public override bool IsCorrectOpenTag(string mdText, int position)
        {
            mdText.TryGetNextChars(position + OpenMdTag.Length - 1, 1, out char[] nextChars);
            return nextChars == null || !char.IsWhiteSpace(nextChars[0]);
        }

        public override bool IsCorrectCloseTag(string mdText, int position)
        {
            mdText.TryGetCharsBehind(position, 1, out char[] behindChars);
            return behindChars == null || !char.IsWhiteSpace(behindChars[0]);
        }
    }
}