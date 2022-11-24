using Markdown.Interfaces;

namespace Markdown.Morphemes
{
    public class PostfixHeader : IMorpheme
    {
        public string View => "\n\r";

        public Tags Tag => Tags.Header;

        public TagType TagType => TagType.Close;

        public MorphemeType MorphemeType => MorphemeType.Prefix;
        
        public bool CheckForCompliance(string textContext, int position)
        {
            return position + 1 < textContext.Length
                   && textContext[position] == '\n' && textContext[position + 1] == '\r';
        }
    }
}