namespace Markdown.Nodes
{
    public class FirstHeaderTaggedNode : TaggedNode
    {
        private const string HtmlTag = "h1";
        private const string MarkdownTag = "# ";
        public FirstHeaderTaggedNode() : base(HtmlTag) {}

        public override bool ShouldBeClosed(Marking marking)
        {
            return marking.SymbolAfterMarking == null;
        }

        public override Marking TrimMarking(Marking initialMarking)
        {
            return new Marking(
                initialMarking.SymbolBeforeMarking,
                initialMarking.StringMarking[2..],
                initialMarking.SymbolAfterMarking
            );
        }

        public override string GetMarkdownOpening()
        {
            return MarkdownTag;
        }
    }
}