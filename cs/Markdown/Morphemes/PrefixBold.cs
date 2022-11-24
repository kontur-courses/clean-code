using Markdown.Interfaces;

namespace Markdown.Morphemes
{
    public class PrefixBold : IMorpheme
    {
        public string View => "__";
        
        public Tags Tag => Tags.Bold;
        public TagType TagType => TagType.Open;

        public MorphemeType MorphemeType => MorphemeType.Prefix;

        public  bool CheckForCompliance(string textContext, int position)
        {
            if (position + 2 > textContext.Length - 1)
                return false;

            if (textContext[position] != '_' 
                || textContext[position + 1] != '_' 
                || char.IsWhiteSpace(textContext[position + 2]))
                return false;

            if (position -1 >= 0 && char.IsDigit(textContext[position - 1]) || char.IsDigit(textContext[position + 2]))
                return false;

            return true;
        }
    }
}