namespace Markdown
{
    public class TagElement
    {
        public readonly TagType Type;
        public readonly TagUsability TagUsability;
        public readonly int StartIndex;
        public readonly int EndIndex;
        public readonly int Length;
        
        public TagElement(TagType type, TagUsability tagUsability, int startIndex, int length)
        {
            Type = type;
            TagUsability = tagUsability;
            StartIndex = startIndex;
            EndIndex = startIndex + length - 1;
            Length = length;
        }

        public override string ToString()
        {
            return $"Type = {Type}, Can use in {TagUsability}, Length = {Length}, at text {StartIndex} - {EndIndex}";
        }
    }
}