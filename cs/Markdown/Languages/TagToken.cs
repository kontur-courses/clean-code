namespace Markdown.Languages
{
    internal class TagToken
    {
        public readonly TagType Tagtype;
        public readonly bool IsOpen;
        public readonly int Position;


        public TagToken(TagType tagType, bool isOpen, int i)
        {
            Tagtype = tagType;
            IsOpen = isOpen;
            Position = i;
        }
    }
}