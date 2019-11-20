namespace Markdown
{
    public class TagInserter
    {
        public int Index { get; set; }
        public string Tag { get; set; }
        public string Mark { get; set; }

        public TagInserter(int index, string tag, string mark)
        {
            Index = index;
            Tag = tag;
            Mark = mark;
        }
    }
}