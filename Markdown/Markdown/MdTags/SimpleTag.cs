namespace Markdown.MdTags
{
    internal class SimpleTag: Tag
    {
        public SimpleTag(string content = "") { Content = content; }

        public override bool CanClose(string tag) => false;
    }
}
