namespace Markdown.MdTags
{
    internal class SimpleTag: Tag
    {
        public SimpleTag((int lenght, string content) contentInfo) : base(contentInfo)
        { }

        public override bool CanClose(string tag) => false;
    }
}
