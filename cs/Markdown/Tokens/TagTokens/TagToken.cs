namespace Markdown
{
    public abstract class TagToken : Token
    {
        protected abstract string HtmlValue { get; }

        public override string GetValue()
        {
            return $"<{HtmlValue}>";
        }
    }
}
