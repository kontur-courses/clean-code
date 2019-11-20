namespace Markdown
{
    internal enum TagType
    {
        Opening,
        Closing,
    }

    internal class Tag
    {
        public readonly string Designations;
        public readonly TagType Type;
        public readonly int Priority;
        public readonly int Index;


        public Tag(string designations, int index, TagType type, int priority)
        {
            Designations = designations;
            Type = type;
            Priority = priority;
            Index = index;
        }
    }
}
