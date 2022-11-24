using Markdown.Interfaces;

namespace Markdown.Morphemes
{
    public class PrefixItalic : IMorpheme
    {
        public string View => "_";
        
        public Tags Tag => Tags.Italic;

        public TagType TagType => TagType.Open;

        public MorphemeType MorphemeType => MorphemeType.Prefix;

        public bool CheckForCompliance(string textContext, int position)
        {
            if (textContext[position] != '_' || position >= textContext.Length - 1)
                return false;

            if (char.IsWhiteSpace(textContext[position + 1]))
                return false;

            if (textContext[position + 1] == '_')
                return false;

            if (char.IsDigit(textContext[position + 1]) || position - 1 >= 0
                && char.IsDigit(textContext[position - 1]))
                return false;

            return true;
        }
    }
}