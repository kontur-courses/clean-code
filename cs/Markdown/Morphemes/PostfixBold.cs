using Markdown.Interfaces;

namespace Markdown.Morphemes
{
    public class PostfixBold : IMorpheme
    {
        public string View => "__";

        public Tags Tag => Tags.Bold;
        public TagType TagType => TagType.Close;

        public MorphemeType MorphemeType => MorphemeType.Postfix;

        public bool CheckForCompliance(string textContext, int position)
        {
            if (position == 0 || position + 1 >= textContext.Length)
                return false;

            if (textContext[position] != '_' || textContext[position + 1] != '_')
                return false;

            if (char.IsWhiteSpace(textContext[position - 1]))
                return false;

            if (position + 2 < textContext.Length
                && char.IsDigit(textContext[position + 2]) || (char.IsDigit(textContext[position - 1])))
                return false;

            return true;
        }
    }
}