using Markdown.Interfaces;

namespace Markdown.Morphemes
{
    public class Word : IMorpheme
    {
        public Word(string word)
        {
            View = word;
        }
        public string View { get; }
        
        public Tags Tag => Tags.None;

        public TagType TagType => TagType.None;

        public MorphemeType MorphemeType => MorphemeType.Word;

        public bool CheckForCompliance(string textContext, int position)
        {
            return char.IsLetter(textContext[position]);
        }
    }
}