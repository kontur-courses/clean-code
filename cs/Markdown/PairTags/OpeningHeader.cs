using Markdown.Interfaces;

namespace Markdown.PairTags
{
    public class OpeningHeader : IPairTag
    {
        public string ViewTag => "#";

        public Tag Tag => Tag.Header;

        public TagType TagType => TagType.Open;


        public bool CheckForCompliance(string textContext, int position)
        {
            return position == 0 && textContext.Length == 1 && textContext[position] == '#';
        }
    }
}