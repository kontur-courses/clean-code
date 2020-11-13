namespace Markdown.Tags
{
    public class Tag
    {
        public Tag PairTag { get; protected set; }
        public string mdTag { get; protected set; }
        public string htmlTag { get; set; }
        public int position { get; protected set; }
    }
}