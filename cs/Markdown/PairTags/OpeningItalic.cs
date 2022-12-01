using Markdown.Interfaces;

namespace Markdown.PairTags
{
    public class OpeningItalic : IPairTag
    {
        public string ViewTag => "_";

        public Tag Tag => Tag.Italic;

        public TagType TagType => TagType.Open;


        public bool CheckForCompliance(string textContext, int position)
        {
            if (textContext[position] != '_' || position >= textContext.Length - 1)
                return false;

            if (char.IsWhiteSpace(textContext[position + 1]))
                return false;

            if (textContext[position + 1] == '_')
                return false;

            if (char.IsDigit(textContext[position + 1])
                || (position - 1 >= 0 && char.IsDigit(textContext[position - 1])))
                return false;

            return true;
        }
    }
}