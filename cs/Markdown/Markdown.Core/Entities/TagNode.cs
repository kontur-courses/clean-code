namespace Markdown.Core.Entities
{
    public class TagNode
    {
        public Tag Tag { get; }
        public List<TagNode> Childs { get; }

        public TagNode(Tag tag, IEnumerable<TagNode> child)
        {
            Tag = tag;
            Childs = child.ToList();
        }
    }
}