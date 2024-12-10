namespace Markdown.Tokens.HtmlTokens
{
    internal abstract class BasicMarkdownToken(IRenderable innerItem)
    {
        public IRenderable InnerItem { get; init; }
            = innerItem ?? throw new ArgumentNullException(nameof(innerItem),
                "Значение не может быть null.");
    }
}
