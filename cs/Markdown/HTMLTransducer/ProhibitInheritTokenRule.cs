namespace Markdown.HTMLTransducer
{
    public class ProhibitInheritTokenRule
    {
        private readonly Token parentToken;
        private readonly Token childToken;

        public ProhibitInheritTokenRule(Token parentToken,
            Token childToken)
        {
            this.parentToken = parentToken;
            this.childToken = childToken;
        }

        public bool For(Token parentToken, Token childToken) =>
            Equals(this.parentToken, parentToken) &&
            Equals(this.childToken, childToken);
    }
}