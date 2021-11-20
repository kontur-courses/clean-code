namespace Markdown.Nodes
{
    public class StrongTaggedNode: TaggedNode
    {
        private const string HtmlTag = "strong";
        private const string MarkdownTag = "__";


        public StrongTaggedNode() : base(HtmlTag) {}
        public override bool ShouldBeClosed(Marking marking)
        {
            return marking.SymbolBeforeMarking.HasValue &&
                   marking.StringMarking.StartsWith("__");
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