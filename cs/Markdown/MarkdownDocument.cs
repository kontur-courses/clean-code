namespace Markdown
{
    public class MarkdownDocument
    {
        private RootToken RootToken { get; }

        public MarkdownDocument(RootToken rootToken)
        {
            RootToken = rootToken;
        }

        public string ToHtml()
        {
            return RootToken.ToHtml();
        }
    }
}