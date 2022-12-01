using Markdown.Interfaces;

namespace Markdown.PairTags
{
    public class ClosingBold : IPairTag
    {
        public string ViewTag => "__";

        public Tag Tag => Tag.Bold;
        public TagType TagType => TagType.Close;


        public bool CheckForCompliance(string textContext, int position)
        {
            if (position == 0 || position + 1 >= textContext.Length)
                return false;

            if (textContext[position] != '_' || textContext[position + 1] != '_')
                return false;

            if (char.IsWhiteSpace(textContext[position - 1]))
                return false;

            if ((position + 2 < textContext.Length
                 && char.IsDigit(textContext[position + 2])) || char.IsDigit(textContext[position - 1]))
                return false;

            return true;
        }
    }
}