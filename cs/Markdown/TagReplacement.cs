namespace Markdown
{
    class TagReplacement
    {
        public readonly int Index;
        public readonly int OldTagLength;
        public readonly string OldTag;
        public readonly string NewTag;

        public TagReplacement(int index, string oldTag, string newTag)
        {
            Index = index;
            OldTag = oldTag;
            OldTagLength = OldTag.Length;
            NewTag = newTag;
        }
    }
}
