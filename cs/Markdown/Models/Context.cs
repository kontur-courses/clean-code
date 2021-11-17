namespace Markdown.Models
{
    public class Context
    {
        public string Text { get; }
        public int Index { get; set; }
        public TagType ParentTag { get; set; }

        public Context(string text, int index = 0)
        {
            Text = text;
            Index = index;
        }
    }
}