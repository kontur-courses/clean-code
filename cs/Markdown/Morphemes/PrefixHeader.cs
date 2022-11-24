using Markdown.Interfaces;

namespace Markdown.Morphemes
{
    public class PrefixHeader : IMorpheme
    {
        public string View => "#";

        public Tags Tag => Tags.Header;
        
        public TagType TagType => TagType.Open;

        public MorphemeType MorphemeType => MorphemeType.Prefix;

        public bool CheckForCompliance(string textContext, int position)
        {
            return position == 0 && textContext.Length == 1 && textContext[position] == '#';
        }
    }
}