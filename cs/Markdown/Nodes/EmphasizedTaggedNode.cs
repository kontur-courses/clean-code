namespace Markdown.Nodes
{
    public class EmphasizedTaggedNode: TaggedNode
    {
        private const string HtmlTag = "em";
        private const string MarkdownTag = "_";
        public EmphasizedTaggedNode() : base(HtmlTag) {}

        public override bool ShouldBeClosed(Marking marking)
        {
            return marking.SymbolBeforeMarking.HasValue &&
                   marking.SymbolBeforeMarking.Value != ' ' &&
                   marking.StringMarking.StartsWith('_');
        }

        public override Marking TrimMarking(Marking initialMarking)
        {
            return new Marking(
                initialMarking.SymbolBeforeMarking,
                initialMarking.StringMarking[1..],
                initialMarking.SymbolAfterMarking
            );
        }

        public override string GetMarkdownOpening()
        {
            return MarkdownTag;
        }
    }
}