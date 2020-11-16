namespace Markdown
{
    class TagReplacement
    {
        public readonly TagSubstring OldTagSubstring;
        public readonly string NewTag;

        public TagReplacement(string newTag, TagSubstring oldTagSubstring)
        {
            OldTagSubstring = oldTagSubstring;
            NewTag = newTag;
        }
    }
}
